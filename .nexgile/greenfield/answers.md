# Requirements brief — Music Stream
## Profile
A Spotify-like music streaming app where users browse, play and manage music, and admins manage the catalogue. · web app · music streaming · size: business · depth: standard · personalData, publicUsers
- **Has a user interface?** Yes (The description asks for a responsive UI for web and mobile.)
- **Runs locally?** Yes (The app serves pages to a browser.)
## The app
A Spotify-like music streaming app where users browse, play and manage music, and admins manage the catalogue.
## Goals and success
- G1 Provide a complete, fully functional Spotify-like music streaming application. (description)
- G2 Let users and admins upload and manage songs, audio files, cover images, artist details and albums. (description)
## Roles
- R1 User (description)
- R2 Admin (description)
- R3 Approved artist (Q1)
## Permissions
- R2 Admin may upload music (Q1)
- R3 Approved artist may upload music (Q1)
- R1 User may play a song, add a song to a playlist; may not upload music (Q1, Q4)
## Workflows
### W1 Upload a song (release 1) — actors: R1 User, R2 Admin; trigger: A user or admin chooses to upload a song; outcome: The song is available in the catalogue (description, Q3)
1. Choose audio file
2. Add title, artist, album, genre
3. Upload cover image
4. Save song
5. The song appears in the catalogue at once
### W2 Play a song (release 1) — actors: R1 User; trigger: A user selects a song; outcome: The user hears the song (description)
1. Open player
2. Play song
3. Control playback with play, pause, next, previous, shuffle, repeat, volume, progress bar
## Records
- E1 Song. fields: title, audio file, cover image, artist, album, genre; owner: Admin (description)
- E2 Album. fields: title, cover image, artist, songs; owner: Admin (description)
- E3 Artist. fields: name, details, image; owner: Admin (description)
- E4 Playlist. fields: name, songs, owner; owner: User (description)
- E5 Genre. fields: name; owner: Admin (description)
## Decisions
- **Stack:** The stack the design part of the spec recommends for this app (decided by Greenfield) (Q6)
- **Deploy target:** No deploy for now (decided by Greenfield) (Q7)
## Other facts
- F1 The database is SQLite. [stack] (description)
- F2 The app has a responsive UI for web and mobile. [devices] (description)
- F3 The app includes user registration, login, homepage, music search, songs, albums, artists, genres, playlists, favourites and personalised recommendations. [scope] (description)
- F4 The app includes a complete music player with play, pause, next, previous, shuffle, repeat, volume control, progress bar and background playback. [scope] (description)
- F5 The app includes a complete frontend, backend, APIs, authentication and admin dashboard. [scope] (description)
- F6 The homepage shows personalised recommendations first. [workflows] (Q2)
- F7 Uploaded songs appear in the catalogue at once, with no approval step. [workflows] (Q3)
- F8 A user can play a song and add it to playlists. [scope] (Q4)
## The interview
- Q1 [permissions · owner] Who can upload music? → Admins and approved artists
- Q2 [workflows · owner] What should the homepage show first? → Personalised recommendations
- Q3 [workflows · owner] Should songs need approval before they appear? → No, they appear at once
- Q4 [scope · owner] What can a user do with a song? → Play it and add it to playlists
- Q5 [success · owner] When you picture this music streaming app working well a year from now, what would tell you it has succeeded? → Decide for me
## Decided by Greenfield (Enough, decide the rest)
- Q6 Which languages and frameworks should the app use? → Decide for me (settled: The stack the design part of the spec recommends for this app)
- Q7 Where should the live app run? → Decide for me (settled: No deploy for now)
