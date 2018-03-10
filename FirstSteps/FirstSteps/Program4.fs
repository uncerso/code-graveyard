// Learn more about F# at http://fsharp.org

open System
open NUnit.Framework
open FsUnit
open NUnit.Framework.Internal

let read = bigint.Parse << Console.ReadLine
let readInt = Int32.Parse << Console.ReadLine
let readStr = Console.ReadLine
let rnd = System.Random()

type term =
| Var of string
| Terms of list<term>
| Lambda of list<string> * term

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
        let rec replace a b tm=
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
            match apply t with
            | Lambda(x, y) -> apply <| Terms(Lambda(x, y)::tail)
            | o ->Terms <| o::(List.foldBack (fun el acc -> (apply el) ::acc) tail [])

    normalize <| apply tm    

[<Test>]
let test = 
    let tm = Terms[Terms [Terms[Terms [Var "a"; Var "b"]]; Terms [Var "c"; Var "d"]]]
    let ans = Terms[Terms [Var "a"; Var "b"]; Terms [Var "c"; Var "d"]]
    normalize tm |> should equal ans

    let tm = Terms [Var "a"]
    let ans = Var "a"
    normalize tm |> should equal ans

    let tm = Terms [Terms [Var "a"]]
    let ans = Var "a"
    normalize tm |> should equal ans

    let tm = Lambda (["a"], Lambda (["b"], Var "t"))
    let ans = Lambda (["a"; "b"], Var "t")
    normalize tm |> should equal ans

    let tm = Terms[Terms[Lambda (["a"], Lambda (["b"], Var "t"))]; Lambda (["a'"], Lambda (["b'"], Var "t'"))]
    let ans = Terms[Lambda (["a"; "b"], Var "t"); Lambda (["a'"; "b'"], Var "t'")]
    normalize tm |> should equal ans

    let tm = Lambda(["b"], Terms [Lambda(["b'"; "t"; "f"], Terms [Var "b'"; Var "t"; Var "f"]); Var "b"; Lambda(["x"; "y"], Var "y"); Lambda(["x"; "y"], Var "x")])
    let ans = Lambda (["b"], Terms [Var "b"; Lambda (["x"; "y"] ,Var "y"); Lambda (["x"; "y"], Var "x")])
    reduction tm |> should equal ans
    
    let tm = Terms [Lambda(["x"; "y"], Var "x"); Lambda(["x"], Var "x")]
    let ans = Lambda (["y"; "x"], Var "x")
    reduction tm |> should equal ans
    
    let tm = Terms [Lambda(["x"; "y"; "z"], Terms [Var "x"; Var "z"; Terms[Var "y"; Var "z"]]); Lambda(["x"; "y"], Var "x"); Lambda(["x"; "y"], Var "x")]
    let ans = Lambda (["z"], Var "z")
    reduction tm |> should equal ans

    let PAIR = Lambda (["x"; "y"; "f"], Terms([Var "f"; Var "x"; Var "y"]))
    let ans = Lambda (["f"], Terms[Var "f"; Var "a"; Var "b"])
    let tm = Terms[PAIR; Var "a"; Var "b"]
    reduction tm |> should equal ans
    
    
    let TRUE = Lambda(["x"; "y"], Var "x")
    let FALSE = Lambda(["x"; "y"], Var "y")
    let FST = Lambda (["p"], Terms [Var "p"; TRUE])
    let SND = Lambda (["p"], Terms [Var "p"; FALSE])
    
    let tm = Terms[FST; Terms[PAIR; Var "a"; Var "b"]]
    let ans = Var "a"
    reduction tm |> should equal ans

    let tm = Terms[SND; Terms[PAIR; Var "a"; Var "b"]]
    let ans = Var "b"
    reduction tm |> should equal ans

(*_*)
[<EntryPoint>]
let main argv =
    0 // return an integer exit code
