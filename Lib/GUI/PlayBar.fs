namespace SharpBeat.Lib.GUI

module PlayBar =
    open Avalonia.FuncUI
    open Avalonia.Controls
    open Avalonia.FuncUI.DSL
    open Avalonia.Layout
    open SharpBeat.Lib.GUI
    open SharpBeat.Lib.Models.Song
    open LibVLCSharp.Shared
    open LibVLCSharp
    open PlayerLib
    open System

    open LibVLCSharp.Avalonia

    let mediaButtons = 
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
                ]
                Button.create [
                    Button.content Icons.play
                    Button.width 100.0
                    Button.horizontalAlignment HorizontalAlignment.Center
                ]
                Button.create [
                    Button.content Icons.next
                    Button.width 100.0
                    Button.horizontalAlignment HorizontalAlignment.Center
                ]
            ]
        ]


    let progressBar min max curr = 
        StackPanel.create [
            StackPanel.verticalAlignment VerticalAlignment.Bottom
            StackPanel.horizontalAlignment HorizontalAlignment.Center
            StackPanel.orientation Orientation.Horizontal
            StackPanel.dock Dock.Bottom

            StackPanel.children [
                Slider.create [
                    Slider.minimum min
                    Slider.maximum max
                    Slider.value curr
                    Slider.width 428.0
                    Slider.horizontalAlignment HorizontalAlignment.Center
                ]
            ]
        ]

    let playBar (song: IWritable<Option<Song>>) = 

        // XXX: uncomment this, this works for playing audio, apparently there's a media player component available from
        // the vlcsharp avalonia library, we just need to dig deeper lmao
        // let player = getEmptyPlayer
        // let media = song.Current |>
        //              function 
        //              | Some(song) -> getMediaFromUri(new Uri(song.url()))
        //              | None -> getMediaFromUri(new Uri("file://"))
    
        // TODO: yk
        // player.Media <- media
        // player.Play() |> ignore

        StackPanel.create [
            StackPanel.verticalAlignment VerticalAlignment.Bottom
            StackPanel.horizontalAlignment HorizontalAlignment.Center
            StackPanel.orientation Orientation.Vertical
            StackPanel.dock Dock.Right
            StackPanel.children [
                mediaButtons
                // NOTE: I'm planning on somwhow getting these values from the player
                progressBar 0 100 46
            ]
        ]
