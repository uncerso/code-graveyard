// Learn more about F# at http://fsharp.org
module hw

open System

let read = bigint.Parse << Console.ReadLine
let readInt = Int32.Parse << Console.ReadLine
let readStr = Console.ReadLine
let rnd = System.Random()

exception Errors of string

type State<'a> = 
    | Empty
    | Ref of (int * 'a) * State<'a>

// Insert: O(n); Front, Pop: O(1)
type PrimaryQueue<'a> () = 
    let mutable value = Empty

    member v.Insert (el : int * 'a) = 
        let rec handle el queue =
            match queue with
            | Empty -> Ref(el, Empty)
            | Ref (x, tail) as tmp -> 
                if fst x <= fst el then
                    Ref (el, tmp)
                else 
                    Ref(x, handle el tail)
        value <- handle el value

    member v.Front () = 
        match value with
        | Empty -> raise <| Errors("Queue is empty!")
        | Ref (x, _) -> x

    member v.Pop () = 
        match value with
        | Empty -> raise <| Errors("Queue is empty!")
        | Ref (_, tail) -> value <- tail

(*_*)
[<EntryPoint>]
let main argv =

    0 // return an integer exit code
