# Requirements: Music Stream

A Spotify-like music streaming app where Users browse, play and manage music, and Admins manage the catalogue.

## Requirement 1: Registration and login

**User Story:** As a User, I want to register and sign in, so that I can keep my playlists and favourites.

_Source: F3, F5_

### Acceptance Criteria

1.1 WHEN a visitor opens the app at `/` THE SYSTEM SHALL show the sign-in page with a link to register.

1.2 WHEN a visitor submits the registration form with an email address, a user name and a password THE SYSTEM SHALL create the account and sign the visitor in.

1.3 IF a visitor submits a registration form with an email address that is already registered THEN THE SYSTEM SHALL show a message that the email is already in use and keep the visitor on the registration form.

1.4 WHEN a visitor submits the sign-in form with a registered email address and the matching password THE SYSTEM SHALL sign the visitor in and open the homepage.

1.5 IF a visitor submits the sign-in form with a wrong password THEN THE SYSTEM SHALL show a message that the email or password is wrong and keep the visitor on the sign-in page.

1.6 WHEN a signed-in User opens the app THE SYSTEM SHALL open the homepage, not the sign-in page.

1.7 WHEN a signed-in Admin opens the app THE SYSTEM SHALL open the admin dashboard.

1.8 WHEN a signed-in Approved artist opens the app THE SYSTEM SHALL open the homepage.

## Requirement 2: Homepage and personalised recommendations

**User Story:** As a User, I want a homepage that shows me music I am likely to enjoy, so that I can find something to play quickly.

_Source: F3, F6_

### Acceptance Criteria

2.1 WHEN a signed-in User opens the homepage THE SYSTEM SHALL show a personalised recommendations section first.

2.2 WHEN a signed-in User opens the homepage THE SYSTEM SHALL show sections for songs, albums, artists, genres and playlists.

2.3 WHEN a signed-in User opens the homepage THE SYSTEM SHALL show a search field.

2.4 WHEN a signed-in User opens the homepage THE SYSTEM SHALL show a link to their favourites.

## Requirement 3: Music search

**User Story:** As a User, I want to search the catalogue, so that I can find a song, album, artist or genre by name.

_Source: F3_

### Acceptance Criteria

3.1 WHEN a User types a search term and submits it THE SYSTEM SHALL show matching songs, albums, artists and genres.

3.2 WHEN a User submits a search that matches nothing THE SYSTEM SHALL show a message that no results were found.

3.3 WHEN a User clicks a search result THE SYSTEM SHALL open the page for that song, album, artist or genre.

## Requirement 4: Songs, albums, artists and genres

**User Story:** As a User, I want to browse songs, albums, artists and genres, so that I can explore the catalogue.

_Source: F3, E1, E2, E3, E5_

### Acceptance Criteria

4.1 WHEN a User opens a song page THE SYSTEM SHALL show the song's title, artist, album, genre and cover image.

4.2 WHEN a User opens an album page THE SYSTEM SHALL show the album's title, cover image, artist and its songs.

4.3 WHEN a User opens an artist page THE SYSTEM SHALL show the artist's name, details, image and their songs and albums.

4.4 WHEN a User opens a genre page THE SYSTEM SHALL show the genre's name and the songs in that genre.

4.5 WHEN a User clicks a song anywhere in the app THE SYSTEM SHALL start playing that song in the player.

## Requirement 5: Playlists

**User Story:** As a User, I want to create and manage playlists, so that I can group songs I like.

_Source: F3, F8, E4_

### Acceptance Criteria

5.1 WHEN a User creates a playlist with a name THE SYSTEM SHALL add the playlist to their playlists.

5.2 WHEN a User adds a song to a playlist THE SYSTEM SHALL add the song to that playlist.

5.3 WHEN a User opens a playlist THE SYSTEM SHALL show the playlist's name and its songs.

5.4 WHEN a User removes a song from a playlist THE SYSTEM SHALL remove the song from that playlist.

5.5 WHEN a User deletes a playlist THE SYSTEM SHALL remove the playlist from their playlists.

## Requirement 6: Favourites

**User Story:** As a User, I want to mark songs as favourites, so that I can find them again easily.

_Source: F3_

### Acceptance Criteria

6.1 WHEN a User marks a song as a favourite THE SYSTEM SHALL add the song to their favourites.

6.2 WHEN a User opens their favourites THE SYSTEM SHALL show the songs they marked as favourites.

6.3 WHEN a User unmarks a favourite song THE SYSTEM SHALL remove the song from their favourites.

## Requirement 7: Music player

**User Story:** As a User, I want a complete music player, so that I can control playback of a song.

_Source: F4, W2_

### Acceptance Criteria

7.1 WHEN a User plays a song THE SYSTEM SHALL play the song's audio.

7.2 WHEN a User clicks pause THE SYSTEM SHALL pause the song.

7.3 WHEN a User clicks play while paused THE SYSTEM SHALL resume the song.

7.4 WHEN a User clicks next THE SYSTEM SHALL play the next song in the current list.

7.5 WHEN a User clicks previous THE SYSTEM SHALL play the previous song in the current list.

7.6 WHEN a User turns on shuffle THE SYSTEM SHALL play the current list in a random order.

7.7 WHEN a User turns on repeat THE SYSTEM SHALL repeat the current list after it ends.

7.8 WHEN a User moves the volume control THE SYSTEM SHALL change the playback volume.

7.9 WHEN a song plays THE SYSTEM SHALL show a progress bar that moves with the song and lets the User seek to a position.

7.10 WHEN a User navigates to another page while a song plays THE SYSTEM SHALL keep playing the song in the background.

## Requirement 8: Upload and manage music

**User Story:** As an Admin or Approved artist, I want to upload and manage songs, audio files, cover images, artist details and albums, so that the catalogue stays up to date.

_Source: G2, W1, E1, E2, E3, E5, F7_

### Acceptance Criteria

8.1 WHEN an Admin or Approved artist chooses to upload a song THE SYSTEM SHALL show a form to choose an audio file, add a title, artist, album and genre, and upload a cover image.

8.2 WHEN an Admin or Approved artist saves the song THE SYSTEM SHALL add the song to the catalogue.

8.3 WHEN an Admin or Approved artist saves the song THE SYSTEM SHALL make the song appear in the catalogue at once, with no approval step.

8.4 WHEN an Admin creates an album THE SYSTEM SHALL add the album with its title, cover image, artist and songs.

8.5 WHEN an Admin creates an artist THE SYSTEM SHALL add the artist with its name, details and image.

8.6 WHEN an Admin creates a genre THE SYSTEM SHALL add the genre with its name.

8.7 WHEN an Admin edits a song, album, artist or genre THE SYSTEM SHALL save the changes.

8.8 WHEN an Admin deletes a song, album, artist or genre THE SYSTEM SHALL remove it from the catalogue.

8.9 IF a User who is not an Admin or Approved artist tries to upload a song THEN THE SYSTEM SHALL not allow the upload.

## Requirement 9: Admin dashboard

**User Story:** As an Admin, I want a dashboard to manage the catalogue, so that I can keep the app's music organised.

_Source: F5, G2_

### Acceptance Criteria

9.1 WHEN an Admin opens the admin dashboard THE SYSTEM SHALL show the songs, albums, artists and genres in the catalogue.

9.2 WHEN an Admin opens the admin dashboard THE SYSTEM SHALL show controls to add, edit and delete songs, albums, artists and genres.

9.3 WHEN an Admin opens the admin dashboard THE SYSTEM SHALL show controls to approve an artist.

9.4 WHEN an Admin approves an artist THE SYSTEM SHALL mark the artist as an Approved artist who may upload music.

## Later releases

- None.

## Assumptions

- A User's favourites are a single list of songs, not a list per album or artist.
- The player's "current list" is the list the User started playing from (a playlist, an album, a genre, search results or the homepage).
- An Approved artist can upload and manage only their own songs and albums; an Admin can manage the whole catalogue.
- The app keeps data in SQLite, as the brief states, and stores audio files and cover images on the server's file storage.
