import { redirect } from "next/navigation";
import { getCurrentUser } from "@/lib/auth";
import LoginPage from "./(auth)/login/page";

export default async function Home() {
  const user = await getCurrentUser();
  if (user) {
    if (user.role === "ADMIN") {
      redirect("/admin");
    }
    redirect("/home");
  }
  return <LoginPage />;
}
