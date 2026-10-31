import { PrismaClient } from "@prisma/client";
import bcrypt from "bcryptjs";

const prisma = new PrismaClient();

const accounts = [
  { email: "admin@musicstream.local", password: "admin123", name: "Admin", role: "ADMIN" as const, approved: true },
  { email: "artist@musicstream.local", password: "artist123", name: "Approved Artist", role: "ARTIST" as const, approved: true },
  { email: "user@musicstream.local", password: "user123", name: "User", role: "USER" as const, approved: false },
];

async function main() {
  for (const account of accounts) {
    const passwordHash = await bcrypt.hash(account.password, 10);
    await prisma.user.upsert({
      where: { email: account.email },
      update: {
        name: account.name,
        role: account.role,
        approved: account.approved,
        passwordHash,
      },
      create: {
        email: account.email,
        name: account.name,
        role: account.role,
        approved: account.approved,
        passwordHash,
      },
    });
  }
  console.log("Seeded test logins: admin@musicstream.local, artist@musicstream.local, user@musicstream.local");
}

main()
  .catch((e) => {
    console.error(e);
    process.exit(1);
  })
  .finally(async () => {
    await prisma.$disconnect();
  });
