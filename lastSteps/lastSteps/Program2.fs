// Learn more about F# at http://fsharp.org
module hw

open System
open System.IO

let read = bigint.Parse << Console.ReadLine
let readInt = Int32.Parse << Console.ReadLine
let readStr = Console.ReadLine
let rnd = System.Random()

//=======================<task1>=======================
type Rounding (accuracy : int) =
    member this.Bind(x : double, f) =
        f <| Math.Round (x, accuracy)
    member this.Return(x : double) =
        Math.Round (x, accuracy)


//=======================</task1>======================

//=======================<task2>=======================
type Calculate () =
    member this.Bind(x, f) =
        try 
            f <| Int32.Parse x
        with
            | :? FormatException -> None
    member this.Return(x) =
        Some x
//=======================</task2>======================
(*_*)
[<EntryPoint>]
let main argv =

    0 // return an integer exit code
