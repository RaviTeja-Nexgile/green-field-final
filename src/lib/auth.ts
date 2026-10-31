import { cookies } from "next/headers";
import { prisma } from "./db";

export const SESSION_COOKIE = "session";

/**
 * A signed session token. In this setup the token is the user id signed with
 * the SESSION_SECRET using HMAC-SHA256, so it cannot be forged without the
 * secret. The cookie is HTTP-only and SameSite=Lax.
 */
export async function createSession(userId: string): Promise<string> {
  const secret = process.env.SESSION_SECRET ?? "dev-secret";
  const payload = `${userId}.${Date.now()}`;
  const signature = await sign(payload, secret);
  return `${payload}.${signature}`;
}

export async function verifySession(token: string): Promise<string | null> {
  const secret = process.env.SESSION_SECRET ?? "dev-secret";
  const parts = token.split(".");
  if (parts.length !== 3) return null;
  const [userId, timestamp, signature] = parts;
  const expected = await sign(`${userId}.${timestamp}`, secret);
  if (signature !== expected) return null;
  return userId;
}

async function sign(data: string, secret: string): Promise<string> {
  const encoder = new TextEncoder();
  const key = await crypto.subtle.importKey(
    "raw",
    encoder.encode(secret),
    { name: "HMAC", hash: "SHA-256" },
    false,
    ["sign"],
  );
  const sig = await crypto.subtle.sign("HMAC", key, encoder.encode(data));
  return Buffer.from(sig).toString("base64url");
}

/** Returns the signed-in user, or null when there is no valid session. */
export async function getCurrentUser() {
  const token = cookies().get(SESSION_COOKIE)?.value;
  if (!token) return null;
  const userId = await verifySession(token);
  if (!userId) return null;
  return prisma.user.findUnique({ where: { id: userId } });
}

export function setSessionCookie(token: string) {
  cookies().set(SESSION_COOKIE, token, {
    httpOnly: true,
    sameSite: "lax",
    secure: process.env.NODE_ENV === "production",
    path: "/",
    maxAge: 60 * 60 * 24 * 30, // 30 days
  });
}

export function clearSessionCookie() {
  cookies().set(SESSION_COOKIE, "", {
    httpOnly: true,
    sameSite: "lax",
    secure: process.env.NODE_ENV === "production",
    path: "/",
    maxAge: 0,
  });
}
