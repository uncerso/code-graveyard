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

let nextInt t =
    t + 1UL

let nextList (ls:list<int>) =
    let rec foo (ls:list<int>) x = 
        let cntOfChars = 26
        match ls with
        | [t] -> ([(x + t) % cntOfChars], (x + t) / cntOfChars)
        | t::tail -> 
            let (newStr,shift) = foo tail x
            if (shift = 0) then 
                (t::newStr, shift)
            else
                (((shift + t) % cntOfChars)::newStr, (shift + t) / cntOfChars)
        | [] -> raise <| Errors("This should not have happened.")
    if (ls.Length = 0) then [0]
    else
        let (newStr,shift) = foo ls 1
        if (shift = 0) 
        then newStr
        else 0::newStr

let toStr (x:obj) = 
    match x with
    | :? list<int> as ls -> List.fold (fun s c -> s+string(char(c + int('a')))) "" ls
    | t -> t.ToString()

let reduction tm = 
    let nextIntNumber = ref 0UL//1000000000000000UL
    let nextNormalName = ref []
    let occupiedNames = ref Set.empty
    let rec rename names next needUpdateOccupiedNames  (mp:Map<string, string>) tm =
            let rec selectNextName names =
                names := next(!names)
                if (Set.contains (toStr !names) !occupiedNames) then selectNextName names else ()
            
            match tm with
            | Terms([]) -> raise <| Errors("You can't use Terms []")
            | Var(x) -> if (Map.containsKey x mp) then Var (Map.find x mp) 
                        else 
                            if(needUpdateOccupiedNames) 
                                then occupiedNames := Set.add x !occupiedNames
                            Var x
            | Lambda(x, y) -> 
                let (accArgue, accMap) = 
                    List.foldBack 
                        (fun el (accArgue, accMap) -> 
                            selectNextName names
                            (toStr(!names)::accArgue, Map.add el (toStr(!names)) accMap))
                        x 
                        ([], mp) 
                Lambda(accArgue, rename names next needUpdateOccupiedNames accMap y)
            | Terms(x) -> Terms (List.foldBack (fun el acc -> (rename names next needUpdateOccupiedNames mp el)::acc) x [])
             
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
    normalize (rename nextNormalName nextList false Map.empty <| apply (rename nextIntNumber nextInt true Map.empty tm))

(*_*)
[<EntryPoint>]
let main argv =
    0 // return an integer exit code
