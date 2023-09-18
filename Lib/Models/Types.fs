namespace SharpBeat.Lib.Models

module Types =
    type PlayDirection =
        | Next
        | Previous

    type PlayState =
        | Play
        | Pause
        | Stop
