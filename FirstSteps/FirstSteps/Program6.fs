// Learn more about F# at http://fsharp.org
module hw

open System
open System.IO

let read = bigint.Parse << Console.ReadLine
let readInt = Int32.Parse << Console.ReadLine
let readStr = Console.ReadLine
let rnd = System.Random()

//=======================<task1>=======================
let fold ls =
    let rec handle func ls acc =
        match ls with
        | [] -> acc
        | x::tail -> handle func tail (func x acc)
    (handle (fun el acc -> acc + sin(el) ) ls 0.0) / double (ls.Length)
//=======================</task1>======================

//=======================<task2>=======================
//=======================</task2>======================

//=======================<task3>=======================
//=======================</task3>======================
(*_*)
[<EntryPoint>]
let main argv =
    0 // return an integer exit code
