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

let rec normalize term = 
    match term with 
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
    | Terms t -> Terms <| List.foldBack (fun el acc -> (normalize el)::acc) t []
    | o -> o

let nextInt t =
    t + 1UL


//returns next element in the sequence: a, b, .. z, aa, ab, .. az, ba, bb, ... (you need to add the code of 'a')
let nextList ls =
    let rec add ls carry = 
        let cntOfChars = 26
        match ls with
        | [currentChar] -> ([(carry + currentChar) % cntOfChars], (carry + currentChar) / cntOfChars)
        | currentChar::tail -> 
            let (newStr,newCarry) = add tail carry
            if (newCarry = 0) then 
                (currentChar::newStr, newCarry)
            else
                (((newCarry + currentChar) % cntOfChars)::newStr, (newCarry + currentChar) / cntOfChars)
        | [] -> raise <| Errors("This should not have happened.")
    if (List.length ls = 0) then [0]
    else
        let (newStr,newCarry) = add ls 1
        if (newCarry = 0) 
        then newStr
        else 0::newStr //this is required to be included in the sequence: 0, 0*, 0**, ...

let toStr (x:obj) = 
    match x with
    | :? list<int> as ls -> List.fold (fun s c -> s + string(char(c + int('a')))) "" ls
    | t -> t.ToString()

let reduction term = 
    let rec rename occupiedNames name next needUpdateOccupiedNames map term =
            let rec selectNextName occupiedNames name =
                let newName = next(name)
                if (Set.contains (toStr newName) occupiedNames) then selectNextName occupiedNames newName else newName
            
            match term with
            | Terms([]) -> raise <| Errors("You can't use Terms []")
            | Var(x) -> let newVar = if (Map.containsKey x map) then Map.find x map else x
                        (Var newVar, name, if(needUpdateOccupiedNames) then Set.add newVar occupiedNames else occupiedNames)
            | Lambda(x, y) -> 
                let (accArgue, accMap, accName) = 
                    List.foldBack 
                        (fun el (accArgue, accMap, accName) -> 
                            let newName = selectNextName occupiedNames accName
                            (toStr(newName)::accArgue, Map.add el (toStr(newName)) accMap, newName))
                        x 
                        ([], map, name) 
                let (term, newName, newOccupiedNames) = rename occupiedNames accName next needUpdateOccupiedNames accMap y
                (Lambda(accArgue, term), newName, newOccupiedNames)
            | Terms(x) -> 
                let (term, newName, newOccupiedNames) = 
                    List.foldBack 
                        (fun el (accTerms, accName, accOccupiedNames) -> 
                            let (term, newName, newOccupiedNames) = rename accOccupiedNames accName next needUpdateOccupiedNames map el
                            (term::accTerms, newName, newOccupiedNames)) 
                        x 
                        ([], name, occupiedNames)
                (Terms term, newName, newOccupiedNames)
             
    let rec apply term =
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
            | Terms t -> Terms <| List.foldBack (fun el acc -> (replace a b el)::acc) t []
            
        match term with 
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
            | o ->Terms <| (List.foldBack (fun el acc -> (apply el)::acc) (o::tail) [])
    
    let (term, nextName, occupiedNames) = rename Set.empty 1000000000000000UL nextInt true Map.empty term 
    let (term, nextName, occupiedNames) = rename occupiedNames [] nextList false Map.empty <| apply term
    normalize term

(*_*)
[<EntryPoint>]
let main argv =
    0 // return an integer exit code
