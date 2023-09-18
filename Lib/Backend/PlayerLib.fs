namespace SharpBeat.Lib.Backend

module PlayerLib =
    open LibVLCSharp.Shared
    open System

    let getMediaFromUri (source: Uri) =
        use libvlc = new LibVLC()
        new Media(libvlc, source)

    let getEmptyPlayer : MediaPlayer =
        use libvlc = new LibVLC()
        new MediaPlayer(libvlc)
