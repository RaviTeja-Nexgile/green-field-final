import { NextResponse } from "next/server";
import bcrypt from "bcryptjs";
import { prisma } from "@/lib/db";
import { createSession, setSessionCookie } from "@/lib/auth";

export async function POST(request: Request) {
  let body: { email?: string; name?: string; password?: string };
  try {
    body = await request.json();
  } catch {
    return NextResponse.json({ error: "Invalid request body." }, { status: 400 });
  }

  const email = typeof body.email === "string" ? body.email.trim().toLowerCase() : "";
  const name = typeof body.name === "string" ? body.name.trim() : "";
  const password = typeof body.password === "string" ? body.password : "";

  if (!email || !name || !password) {
    return NextResponse.json(
      { error: "Email, name and password are required." },
      { status: 400 },
    );
  }

  const existing = await prisma.user.findUnique({ where: { email } });
  if (existing) {
    return NextResponse.json(
      { error: "This email is already in use." },
      { status: 409 },
    );
  }

  const passwordHash = await bcrypt.hash(password, 10);
  const user = await prisma.user.create({
    data: { email, name, passwordHash, role: "USER", approved: false },
  });

  const token = await createSession(user.id);
  setSessionCookie(token);

  return NextResponse.json(
    { user: { id: user.id, email: user.email, name: user.name, role: user.role } },
    { status: 201 },
  );
}
