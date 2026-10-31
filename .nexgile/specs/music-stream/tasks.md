# Tasks: Music Stream

- [x] 1. Project setup
  - _Requirements: 1.1_
  - _Files: package.json, tsconfig.json, next.config.mjs, prisma/schema.prisma, prisma/seed.ts, .env.example, .gitignore, README.md, src/app/layout.tsx, src/app/globals.css, src/app/api/health/route.ts, src/lib/db.ts, src/lib/auth.ts, e2e/.gitkeep, vitest.config.ts_
  - _Verify: npm install && npm test_
  - _Verify: npx prisma validate_
  - _Issue: #1_

- [ ] 2. Registration and login
  - _Requirements: 1.1, 1.2, 1.3, 1.4, 1.5, 1.6, 1.7, 1.8_
  - _Depends: 1_
  - _Files: src/app/(auth)/login/page.tsx, src/app/(auth)/register/page.tsx, src/app/api/auth/register/route.ts, src/app/api/auth/login/route.ts, src/app/api/auth/logout/route.ts, src/app/api/auth/me/route.ts, src/app/(auth)/auth.module.css, src/app/(auth)/__tests__/auth.test.ts_
  - _Verify: npm test -- auth_
  - _Issue: #2_

- [ ] 3. Homepage and personalised recommendations
  - _Requirements: 2.1, 2.2, 2.3, 2.4_
  - _Depends: 1, 2_
  - _Files: src/app/home/page.tsx, src/app/api/home/route.ts, src/app/home/home.module.css, src/app/home/__tests__/home.test.ts_
  - _Verify: npm test -- home_
  - _Issue: #3_

- [ ] 4. Music search
  - _Requirements: 3.1, 3.2, 3.3_
  - _Depends: 1, 2_
  - _Files: src/app/search/page.tsx, src/app/api/search/route.ts, src/app/search/search.module.css, src/app/search/__tests__/search.test.ts_
  - _Verify: npm test -- search_
  - _Issue: #4_

- [ ] 5. Songs, albums, artists and genres pages
  - _Requirements: 4.1, 4.2, 4.3, 4.4, 4.5_
  - _Depends: 1, 2_
  - _Files: src/app/songs/[id]/page.tsx, src/app/albums/[id]/page.tsx, src/app/artists/[id]/page.tsx, src/app/genres/[id]/page.tsx, src/app/api/songs/[id]/route.ts, src/app/api/albums/[id]/route.ts, src/app/api/artists/[id]/route.ts, src/app/api/genres/[id]/route.ts, src/app/catalogue/catalogue.module.css, src/app/catalogue/__tests__/catalogue.test.ts_
  - _Verify: npm test -- catalogue_
  - _Issue: #5_

- [ ] 6. Playlists
  - _Requirements: 5.1, 5.2, 5.3, 5.4, 5.5_
  - _Depends: 1, 2_
  - _Files: src/app/playlists/page.tsx, src/app/playlists/[id]/page.tsx, src/app/api/playlists/route.ts, src/app/api/playlists/[id]/route.ts, src/app/api/playlists/[id]/songs/route.ts, src/app/playlists/playlists.module.css, src/app/playlists/__tests__/playlists.test.ts_
  - _Verify: npm test -- playlists_
  - _Issue: #6_

- [ ] 7. Favourites
  - _Requirements: 6.1, 6.2, 6.3_
  - _Depends: 1, 2_
  - _Files: src/app/favorites/page.tsx, src/app/api/favorites/route.ts, src/app/api/favorites/[songId]/route.ts, src/app/favorites/favorites.module.css, src/app/favorites/__tests__/favorites.test.ts_
  - _Verify: npm test -- favorites_
  - _Issue: #7_

- [ ] 8. Music player
  - _Requirements: 7.1, 7.2, 7.3, 7.4, 7.5, 7.6, 7.7, 7.8, 7.9, 7.10_
  - _Depends: 1, 2, 5_
  - _Files: src/components/player/Player.tsx, src/components/player/player.module.css, src/app/api/stream/[songId]/route.ts, src/components/player/__tests__/player.test.tsx_
  - _Verify: npm test -- player_
  - _Issue: #8_

- [ ] 9. Upload and manage music
  - _Requirements: 8.1, 8.2, 8.3, 8.4, 8.5, 8.6, 8.7, 8.8, 8.9_
  - _Depends: 1, 2, 5_
  - _Files: src/app/upload/page.tsx, src/app/api/songs/route.ts, src/app/api/songs/[id]/route.ts, src/app/api/albums/route.ts, src/app/api/albums/[id]/route.ts, src/app/api/artists/route.ts, src/app/api/artists/[id]/route.ts, src/app/api/genres/route.ts, src/app/api/genres/[id]/route.ts, src/app/upload/upload.module.css, src/app/upload/__tests__/upload.test.ts_
  - _Verify: npm test -- upload_
  - _Issue: #9_

- [ ] 10. Admin dashboard
  - _Requirements: 9.1, 9.2, 9.3, 9.4_
  - _Depends: 1, 2, 9_
  - _Files: src/app/admin/page.tsx, src/app/api/admin/route.ts, src/app/api/admin/artists/[id]/approve/route.ts, src/app/admin/admin.module.css, src/app/admin/__tests__/admin.test.ts_
  - _Verify: npm test -- admin_
  - _Issue: #10_
