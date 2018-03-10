// Learn more about F# at http://fsharp.org
module hw

open System

let read = bigint.Parse << Console.ReadLine
let readInt = Int32.Parse << Console.ReadLine
let readStr = Console.ReadLine
let rnd = System.Random()

type Term =
| Var of string
| Terms of list<Term>
| Lambda of list<string> * Term

exception Errors of string

let rec normalize tm = 
    match tm with 
    | Terms [] -> raise <| Errors("You can't use Terms []")
    | Lambda (s, t) ->
        match normalize t with
        | Lambda(x, y) -> Lambda(List.append s x, y)
        | o -> Lambda(s, o)
    | Terms [t] -> 
        match normalize t with
        | Terms([]) -> raise <| Errors("This should not have happened.")
        | Terms [o] -> o
        | o -> o
    | Terms t -> Terms <| List.foldBack (fun el acc -> (normalize el) ::acc) t []
    | o -> o

let reduction tm = 
    let rec apply tm =
        let rec replace a b tm =
            match tm with
            | Terms([]) -> raise <| Errors("You can't use Terms []")
            | Var(x) -> if (x = a) then b else Var x
            | Lambda(x, y) -> Lambda(x, replace a b y)
            | Terms([x]) -> 
                match replace a b x with
                | Terms([]) -> raise <| Errors("This should not have happened.")
                | Terms [o] -> o
                | o -> o
            | Terms t -> Terms <| List.foldBack (fun el acc -> (replace a b el) ::acc) t []
            
        match tm with 
        | Terms([]) -> raise <| Errors("You can't use Terms []")
        | Var(x) -> Var x
        | Lambda(s, t) -> Lambda (s, apply t)
        | Terms([t]) -> apply t
        | Terms(Lambda(a::others, t)::b::tail) -> 
            match others with
            | [] -> apply (Terms ((replace a b t)::tail))
            | x ->  apply (Terms (Lambda(x, replace a b t)::tail))
        | Terms (t::tail) -> 
            match t with
            | Terms (x) -> apply <| Terms (List.append x tail)
            | o ->Terms <| (List.foldBack (fun el acc -> (apply el) ::acc) (o::tail) [])

    normalize <| apply tm    

(*_*)
[<EntryPoint>]
let main argv =
    0 // return an integer exit code
