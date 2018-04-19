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
type Tree<'a> =
| Leaf of 'a
| Tree of 'a * Tree<'a> * Tree<'a>

let apply tree =
    let rec handle tree depth bestDepth =
        match tree with
        | Leaf(_) -> depth + 1
        | Tree (_, l, r) ->
            let lDepth = handle l (depth + 1) bestDepth
            let rDepth = handle r (depth + 1) bestDepth
            max bestDepth (max lDepth rDepth)
    handle tree 0 0

//=======================</task2>======================

//=======================<task3>=======================
exception Errors of string

type Queue<'a> = 
| Empty
| Ref of 'a * Queue<'a>
with
    member v.push el = 
        let rec handle el queue =
            match queue with
            | Empty -> Ref(el, Empty)
            | Ref (x, tail) -> Ref(x, handle el tail)
        handle el v
    member v.front () = 
        match v with
        | Empty -> raise <| Errors("Queue is empty!")
        | Ref (x, _) -> x
    member v.pop () = 
        match v with
        | Empty -> raise <| Errors("Queue is empty!")
        | Ref (_, tail) -> tail
    

//=======================</task3>======================
(*_*)
[<EntryPoint>]
let main argv =
    0 // return an integer exit code
