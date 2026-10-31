# Music Stream

A Spotify-like music streaming web app. Users register and sign in, browse a catalogue of songs, albums, artists and genres, search it, build playlists and favourites, and play music in a persistent player. Admins and Approved artists upload and manage the catalogue, and Admins manage it from a dashboard.

## Stack

- **Next.js** (App Router) with TypeScript — serves the UI and the JSON API from one server.
- **Prisma** ORM over **SQLite**.
- **bcrypt** password hashing and signed HTTP-only session cookies for authentication.
- **Vitest** for unit/integration tests.

## Getting started

```bash
npm install
npm run dev
```

`npm run dev` seeds the database with the test logins below before the app starts, so they work in a fresh copy.

Open http://localhost:3000 (or the port in `PORT`).

## Test logins

The seed script creates these accounts in a fresh local copy (never in production):

| Role            | Email                     | Password   |
| --------------- | ------------------------- | ---------- |
| Admin           | `admin@musicstream.local` | `admin123` |
| Approved artist | `artist@musicstream.local`| `artist123`|
| User            | `user@musicstream.local`  | `user123`  |

## Scripts

- `npm run dev` — start the dev server (seeds the database first).
- `npm test` — run the Vitest suite.
- `npm run build` — production build.
- `npm run seed` — seed the test logins.
- `npm run prisma:push` — apply the schema to the SQLite database.

## Environment

Copy `.env.example` to `.env` and adjust as needed. The SQLite database path is fixed to `prisma/dev.db` in the schema, so no `DATABASE_URL` is required. The app reads `SESSION_SECRET` (session cookie signing) and `PORT` (dev server port) from the environment.
