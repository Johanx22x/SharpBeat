namespace SharpBeat.Lib.GUI

module PlayBar =
    open Avalonia
    open Avalonia.Controls
    open Avalonia.FuncUI.DSL
    open Avalonia.Layout
    open Avalonia.Media
    open SharpBeat.Lib.GUI
    open SharpBeat.Lib.Models.Types
    open LibVLCSharp.Shared
    open LibVLCSharp.Shared.Structures
    open System

    let mediaButtons (
        playerState: PlayState,
        onPlayStateChange,
        onRequestPlay,
        onShuffleRequested
    ) =
        StackPanel.create [
            StackPanel.verticalAlignment VerticalAlignment.Bottom
            StackPanel.horizontalAlignment HorizontalAlignment.Center
            StackPanel.orientation Orientation.Horizontal
            StackPanel.dock Dock.Bottom

            StackPanel.children [
                Button.create [
                    Button.content Icons.previous
                    Button.width 100.0
                    Button.horizontalAlignment HorizontalAlignment.Center
                    Button.classes [ "mediabtn" ]
                    Button.onClick (fun _ -> onRequestPlay PlayDirection.Previous)
                ]
                match playerState with 
                | PlayState.Play ->
                    Button.create [
                        Button.content Icons.pause
                        Button.width 100.0
                        Button.horizontalAlignment HorizontalAlignment.Center
                        Button.classes [ "mediabtn" ]
                        Button.onClick (fun _ -> onPlayStateChange PlayState.Pause)
                    ]
                | _ ->
                    Button.create [
                        Button.content Icons.play
                        Button.width 100.0
                        Button.horizontalAlignment HorizontalAlignment.Center
                        Button.classes [ "mediabtn" ]
                        Button.onClick (fun _ -> onPlayStateChange PlayState.Pause)
                    ]
                Button.create [
                    Button.content Icons.next
                    Button.width 100.0
                    Button.horizontalAlignment HorizontalAlignment.Center
                    Button.classes [ "mediabtn" ]
                    Button.onClick (fun _ -> onRequestPlay PlayDirection.Next)
                ]
                Button.create [
                    Button.content Icons.shuffle
                    Button.width 100.0
                    Button.horizontalAlignment HorizontalAlignment.Center
                    Button.classes [ "mediabtn" ]
                    Button.onClick (fun _ -> onShuffleRequested())
                ]
            ]
            StackPanel.height 100.0
        ]


    let progressBar (curr: int) (player: MediaPlayer) (songDuration: int) =
        StackPanel.create [
            StackPanel.verticalAlignment VerticalAlignment.Bottom
            StackPanel.horizontalAlignment HorizontalAlignment.Center
            StackPanel.orientation Orientation.Vertical
            StackPanel.dock Dock.Bottom

            StackPanel.children [
                // Duration TextBlock
                TextBlock.create [
                    let songLength = TimeSpan.FromSeconds (float songDuration)
                    let actualPosition = TimeSpan.FromSeconds (float (player.Position * float32 songDuration))
                    TextBlock.text (if player.Length > 0L then actualPosition.ToString(@"mm\:ss") + " / " + songLength.ToString(@"mm\:ss") else "00:00 / 00:00")
                    TextBlock.horizontalAlignment HorizontalAlignment.Center
                    TextBlock.fontSize 15.
                    TextBlock.margin (Thickness (0., 0., 0., 0.))
                ]

                Slider.create [
                    Slider.minimum 0.0
                    Slider.maximum 100.0
                    Slider.value curr
                    Slider.width 428.0
                    Slider.horizontalAlignment HorizontalAlignment.Center

                    Slider.onValueChanged (fun value ->
                        if (int value) <> (int (float player.Position * 100.0)) then
                            player.Position <- (float value / 100.0) |> float32
                    )
                ]
            ]
        ]

    let playBar ( // (song: IWritable<Option<Song>>) = 
        songName: string,
        songArtist: string,
        songDuration: int,
        playerState: PlayState,
        progress: int,
        player: MediaPlayer,
        onPlayStateChange,
        onRequestPlay,
        onShuffleRequested
    ) =

        StackPanel.create [
            StackPanel.verticalAlignment VerticalAlignment.Bottom
            StackPanel.horizontalAlignment HorizontalAlignment.Center
            StackPanel.orientation Orientation.Vertical
            StackPanel.children [
                TextBlock.create [
                    TextBlock.text songName
                    TextBlock.horizontalAlignment HorizontalAlignment.Center
                    TextBlock.fontWeight FontWeight.Bold
                    TextBlock.fontSize 25.
                    TextBlock.margin (Thickness (0., 20., 0., 0.))
                ]

                TextBlock.create [
                    TextBlock.text songArtist
                    TextBlock.horizontalAlignment HorizontalAlignment.Center
                    TextBlock.fontSize 15.
                    TextBlock.margin (Thickness (0., 0., 0., 0.))
                ]

                mediaButtons (
                    playerState,
                    onPlayStateChange,
                    onRequestPlay,
                    onShuffleRequested
                )

                progressBar progress player songDuration
            ]
        ]
