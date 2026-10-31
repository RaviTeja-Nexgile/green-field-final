import type { Metadata } from "next";
import "./globals.css";

export const metadata: Metadata = {
  title: "Music Stream",
  description: "A Spotify-like music streaming app.",
};

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang="en">
      <body>{children}</body>
    </html>
  );
}
