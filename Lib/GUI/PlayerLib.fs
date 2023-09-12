namespace SharpBeat.Lib.GUI

module PlayerLib =
    open LibVLCSharp.Shared
    open System

    let getMediaFromUri (source: Uri) =
        use libvlc = new LibVLC()
        new Media(libvlc, source)

    let getEmptyPlayer =
        use libvlc = new LibVLC()
        new MediaPlayer(libvlc)
