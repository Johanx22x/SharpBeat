namespace SharpBeat.Lib.Models

module Song =
    type Song = {
        Id :  int64
        Title : string
        Artist : string
        Hash : string
        Duration : int16
        Genre : string
    } with
        override this.ToString () : string =
            sprintf "%s - %s" this.Artist this.Title
        member this.url () : string =
            sprintf "http://localhost:8080/%s/outputlist.m3u8" this.Hash
