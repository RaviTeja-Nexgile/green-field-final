import { describe, it, expect, beforeAll, afterAll, beforeEach, vi } from "vitest";
import { PrismaClient } from "@prisma/client";
import bcrypt from "bcryptjs";
import path from "node:path";
import fs from "node:fs";
import os from "node:os";

// A throwaway SQLite database in a temp folder, so tests never touch dev.db.
const tmpDir = fs.mkdtempSync(path.join(os.tmpdir(), "music-stream-auth-"));
const dbPath = path.join(tmpDir, "test.db");

// `vi.mock` factories are hoisted above imports, so they cannot close over
// top-level variables. `vi.hoisted` runs even earlier and can create the shared
// Prisma client that both the mock factory and the test body use.
const { prisma } = vi.hoisted(() => {
  const { PrismaClient: PC } = require("@prisma/client") as typeof import("@prisma/client");
  const p = require("node:path") as typeof import("node:path");
  const f = require("node:fs") as typeof import("node:fs");
  const o = require("node:os") as typeof import("node:os");
  const dir = f.mkdtempSync(p.join(o.tmpdir(), "music-stream-auth-"));
  const client = new PC({ datasourceUrl: `file:${p.join(dir, "test.db")}` });
  return { prisma: client, tmpDir: dir };
});

// The auth routes import `prisma` from "@/lib/db"; point it at the throwaway DB.
vi.mock("@/lib/db", () => ({
  prisma,
}));

// The auth routes use `cookies()` from next/headers, which only works inside a
// request scope. Mock it with an in-memory store.
const cookieStore = new Map<string, string>();
vi.mock("next/headers", () => ({
  cookies: () => ({
    get: (name: string) =>
      cookieStore.has(name) ? { name, value: cookieStore.get(name)! } : undefined,
    set: (name: string, value: string) => {
      cookieStore.set(name, value);
    },
  }),
}));

import { POST as register } from "../../api/auth/register/route";
import { POST as login } from "../../api/auth/login/route";
import { POST as logout } from "../../api/auth/logout/route";
import { GET as me } from "../../api/auth/me/route";

function jsonRequest(body: unknown): Request {
  return new Request("http://localhost/api/auth", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(body),
  });
}

beforeAll(async () => {
  // Create the schema in the throwaway database.
  await prisma.$executeRawUnsafe(
    `CREATE TABLE IF NOT EXISTS "User" (
      "id" TEXT NOT NULL PRIMARY KEY,
      "email" TEXT NOT NULL,
      "name" TEXT NOT NULL,
      "passwordHash" TEXT NOT NULL,
      "role" TEXT NOT NULL DEFAULT 'USER',
      "approved" BOOLEAN NOT NULL DEFAULT false,
      "createdAt" DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
    );`,
  );
  await prisma.$executeRawUnsafe(
    `CREATE UNIQUE INDEX IF NOT EXISTS "User_email_key" ON "User"("email");`,
  );
});

beforeEach(async () => {
  cookieStore.clear();
  await prisma.user.deleteMany();
});

afterAll(async () => {
  await prisma.$disconnect();
  fs.rmSync(tmpDir, { recursive: true, force: true });
});

describe("register", () => {
  it("creates an account and signs the visitor in", async () => {
    const res = await register(
      jsonRequest({
        email: "new@example.com",
        name: "New User",
        password: "secret123",
      }),
    );
    expect(res.status).toBe(201);
    const data = await res.json();
    expect(data.user.email).toBe("new@example.com");
    expect(data.user.role).toBe("USER");

    const created = await prisma.user.findUnique({
      where: { email: "new@example.com" },
    });
    expect(created).not.toBeNull();
    expect(created!.name).toBe("New User");
    expect(await bcrypt.compare("secret123", created!.passwordHash)).toBe(true);

    // A session cookie was set.
    expect(cookieStore.has("session")).toBe(true);
  });

  it("rejects a duplicate email with 409", async () => {
    await prisma.user.create({
      data: {
        email: "dup@example.com",
        name: "Existing",
        passwordHash: await bcrypt.hash("secret123", 10),
        role: "USER",
      },
    });
    const res = await register(
      jsonRequest({
        email: "dup@example.com",
        name: "Another",
        password: "secret123",
      }),
    );
    expect(res.status).toBe(409);
    const data = await res.json();
    expect(data.error).toMatch(/already in use/i);
  });

  it("rejects missing fields with 400", async () => {
    const res = await register(jsonRequest({ email: "x@example.com" }));
    expect(res.status).toBe(400);
  });
});

describe("login", () => {
  it("signs in with a matching password", async () => {
    await prisma.user.create({
      data: {
        email: "user@example.com",
        name: "User",
        passwordHash: await bcrypt.hash("secret123", 10),
        role: "USER",
      },
    });
    const res = await login(
      jsonRequest({ email: "user@example.com", password: "secret123" }),
    );
    expect(res.status).toBe(200);
    const data = await res.json();
    expect(data.user.email).toBe("user@example.com");
    expect(cookieStore.has("session")).toBe(true);
  });

  it("rejects a wrong password with 401", async () => {
    await prisma.user.create({
      data: {
        email: "user@example.com",
        name: "User",
        passwordHash: await bcrypt.hash("secret123", 10),
        role: "USER",
      },
    });
    const res = await login(
      jsonRequest({ email: "user@example.com", password: "wrong" }),
    );
    expect(res.status).toBe(401);
    const data = await res.json();
    expect(data.error).toMatch(/wrong/i);
  });

  it("rejects an unknown email with 401", async () => {
    const res = await login(
      jsonRequest({ email: "nobody@example.com", password: "secret123" }),
    );
    expect(res.status).toBe(401);
  });
});

describe("logout", () => {
  it("clears the session cookie", async () => {
    cookieStore.set("session", "some-token");
    const res = await logout();
    expect(res.status).toBe(200);
    // The cookie is cleared by setting an empty value.
    expect(cookieStore.get("session")).toBe("");
  });
});

describe("me", () => {
  it("returns 401 when there is no session", async () => {
    const res = await me();
    expect(res.status).toBe(401);
  });
});
