// Learn more about F# at http://fsharp.org
module hw

open System
open FsCheck

let read = bigint.Parse << Console.ReadLine
let readInt = Int32.Parse << Console.ReadLine
let readStr = Console.ReadLine
let rnd = System.Random()

//=======================<task1>=======================
let checkBranches ls =
    let rec check ls =
        match ls with
        | [] -> []
        | [x] -> [x]
        | t::others ->
            let res = check others
            match res with
            | [] -> [t]
            | x::tail ->
                match t with
                | '[' -> if (x = ']') then tail else t::res
                | '{' -> if (x = '}') then tail else t::res
                | '(' -> if (x = ')') then tail else t::res
                | _ -> t::res
    (check ls).Length = 0
//=======================</task1>======================

//=======================<task2>=======================
let func : int -> list<int> -> list<int> = 
    List.map << (*)
//=======================</task2>======================

//=======================<task3>=======================
//=======================</task3>======================
(*_*)
[<EntryPoint>]
let main argv =
    0 // return an integer exit code
