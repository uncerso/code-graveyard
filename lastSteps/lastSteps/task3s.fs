// Learn more about F# at http://fsharp.org
module hw

open System
open System.IO
open System.Net
open System.Text.RegularExpressions
open System.Timers

let read = bigint.Parse << Console.ReadLine
let readInt = Int32.Parse << Console.ReadLine
let readStr = Console.ReadLine
let rnd = System.Random()

type Unordered_set<'a when 'a : equality> (hash:'a -> int) = 
    let mutable max_depth = 5
    let mutable size = 32
    let mutable storage = Array.init size (fun _ -> [])
    let get_pos el = (%) << abs <| hash el <| size
    let rec find_handle el ls = 
            match ls with
            | [] -> false
            | value::tail -> if value = el then true else find_handle el tail

    let rehash () =
        let rec rehashList (newStorage: array<list<'a>> ref) ls = 
            match ls with
            | [] -> ()
            | value::tail -> 
                let pos = get_pos value
                newStorage.Value.[pos] <- value::newStorage.Value.[pos]
                rehashList newStorage tail
        let rec handle pos newStorage = 
            if pos < storage.Length then
                rehashList newStorage storage.[pos]
                handle (pos + 1) newStorage
        size <- size <<< 1
        let newStorage = ref <| Array.init size (fun _ -> [])
        handle 0 newStorage
        storage <- !newStorage

    member v.Find (el : 'a) = 
        let pos = get_pos el
        find_handle el storage.[pos]
        
    member v.Insert (el : 'a) = 
        let mutable pos = get_pos el
        if (find_handle el storage.[pos])
            then ()
        else
            while storage.[pos].Length = max_depth do
                rehash ()
                pos <- get_pos el
            storage.[pos] <- el::storage.[pos]

    member v.Erase (el : 'a) = 
        let rec handle el ls = 
            match ls with
            | [] -> []
            | value::tail -> if value = el then tail else value::handle el tail
        let pos = get_pos el
        storage.[pos] <- handle el storage.[pos]

(*_*)
[<EntryPoint>]
let main argv =
    
    0 // return an integer exit code
