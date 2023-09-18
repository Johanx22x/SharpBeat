module SharpBeat.Lib.DB
// This module is used to handle playlists data in the database.

open System.Data.SQLite
open System.Data

module Playlist =
    let private connection() =
        new SQLiteConnection("Data Source=SharpBeat.db;Version=3;")

    type Playlist = {
        name: string
        songs: string list
    }

    let createDatabase () =
        let db = connection()
        db.Open()

        // Create a table for playlists with id and name 
        let createPlaylistsTable = "CREATE TABLE IF NOT EXISTS playlists (name TEXT PRIMARY KEY)"
        let createSongsTable = "CREATE TABLE IF NOT EXISTS songs (hash TEXT PRIMARY KEY, name TEXT NOT NULL)"
        let createPlaylistSongsTable = "CREATE TABLE IF NOT EXISTS playlist_songs (playlist TEXT NOT NULL, song TEXT NOT NULL, FOREIGN KEY(playlist) REFERENCES playlists(name), FOREIGN KEY(song) REFERENCES songs(hash))"

        let cmd = new SQLiteCommand(db)
        cmd.CommandText <- createPlaylistsTable
        cmd.ExecuteNonQuery() |> ignore

        cmd.CommandText <- createSongsTable 
        cmd.ExecuteNonQuery() |> ignore

        cmd.CommandText <- createPlaylistSongsTable
        cmd.ExecuteNonQuery() |> ignore

        db.Close()

    let getPlaylistSongs (name) : string list =
        let db = connection()
        db.Open()

        let cmd = new SQLiteCommand(db)
        cmd.CommandText <- "SELECT song FROM playlist_songs WHERE playlist = @name"
        cmd.Parameters.Add("@name", DbType.String).Value <- name

        let reader = cmd.ExecuteReader()

        let rec readSongs (songs: string list) =
            if reader.Read() then
                let song = reader.GetString(0)
                readSongs (song :: songs)
            else
                songs

        let songs = readSongs []

        db.Close()

        songs

    let getPlaylists () : Playlist list =
        let db = connection()
        db.Open()

        let cmd = new SQLiteCommand(db)
        cmd.CommandText <- "SELECT * FROM playlists"
        let reader = cmd.ExecuteReader()

        let rec readPlaylists (playlists: Playlist list) =
            if reader.Read() then
                let name = reader.GetString(0)
                let songs = getPlaylistSongs name
                readPlaylists ({ name = name; songs = songs } :: playlists)
            else
                playlists

        let playlists = readPlaylists []

        db.Close()

        playlists

    let getSongs () : (string * string) list =
        let db = connection()
        db.Open()

        let cmd = new SQLiteCommand(db)
        cmd.CommandText <- "SELECT * FROM songs"
        let reader = cmd.ExecuteReader()

        let rec readSongs (songs: (string * string) list) =
            if reader.Read() then
                let hash = reader.GetString(0)
                let name = reader.GetString(1)
                readSongs ((hash, name) :: songs)
            else
                songs

        let songs = readSongs []

        db.Close()

        songs

    let addPlaylist (name) =
        let db = connection()
        db.Open()

        let cmd = new SQLiteCommand(db)
        cmd.CommandText <- "INSERT INTO playlists (name) VALUES (@name)"
        cmd.Parameters.Add("@name", DbType.String).Value <- name
        cmd.ExecuteNonQuery() |> ignore

        db.Close()

    let addSong (hash, name) =
        let db = connection()
        db.Open()

        let cmd = new SQLiteCommand(db)
        cmd.CommandText <- "INSERT INTO songs (hash, name) VALUES (@hash, @name)"
        cmd.Parameters.Add("@hash", DbType.String).Value <- hash
        cmd.Parameters.Add("@name", DbType.String).Value <- name
        cmd.ExecuteNonQuery() |> ignore

        db.Close()

    let addPlaylistSong (playlist, song) =
        let db = connection()
        db.Open()

        let cmd = new SQLiteCommand(db)
        cmd.CommandText <- "INSERT INTO playlist_songs (playlist, song) VALUES (@playlist, @song)"
        cmd.Parameters.Add("@playlist", DbType.String).Value <- playlist
        cmd.Parameters.Add("@song", DbType.String).Value <- song
        cmd.ExecuteNonQuery() |> ignore

        db.Close()
    
    let removePlaylist (name) =
        let db = connection()
        db.Open()

        let cmd = new SQLiteCommand(db)
        cmd.CommandText <- "DELETE FROM playlists WHERE name = @name"
        cmd.Parameters.Add("@name", DbType.String).Value <- name
        cmd.ExecuteNonQuery() |> ignore

        db.Close()

    let removeSong (hash) =
        let db = connection()
        db.Open()

        let cmd = new SQLiteCommand(db)
        cmd.CommandText <- "DELETE FROM songs WHERE hash = @hash"
        cmd.Parameters.Add("@hash", DbType.String).Value <- hash
        cmd.ExecuteNonQuery() |> ignore

        db.Close()

    let removePlaylistSong (playlist, song) =
        let db = connection()
        db.Open()

        let cmd = new SQLiteCommand(db)
        cmd.CommandText <- "DELETE FROM playlist_songs WHERE playlist = @playlist AND song = @song"
        cmd.Parameters.Add("@playlist", DbType.String).Value <- playlist
        cmd.Parameters.Add("@song", DbType.String).Value <- song
        cmd.ExecuteNonQuery() |> ignore

        db.Close()

    let removePlaylistSongs (playlist) =
        let db = connection()
        db.Open()

        let cmd = new SQLiteCommand(db)
        cmd.CommandText <- "DELETE FROM playlist_songs WHERE playlist = @playlist"
        cmd.Parameters.Add("@playlist", DbType.String).Value <- playlist
        cmd.ExecuteNonQuery() |> ignore

        db.Close()

    let addSampleData() =
        addPlaylist "Favorites"
        addPlaylist "Workout"
        addPlaylist "Chill"
        addPlaylist "JPOP"

    let checkDBState() =
        if not <| System.IO.File.Exists("SharpBeat.db") then
            createDatabase()
            addSampleData()
