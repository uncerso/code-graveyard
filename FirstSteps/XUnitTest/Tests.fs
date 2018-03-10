module Tests

open Xunit
open FsUnit
open hw 

[<Fact>]
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

    let tm = Terms [Lambda(["x"], Var "y"); Terms[Lambda(["z"], Terms [Var "z"; Var "z"]);Lambda(["z"], Terms [Var "z"; Var "z"])]]
    let ans = Var "y"
    reduction tm |> should equal ans

    let tm = Terms [Terms[Lambda(["x"], Var "y")]; Terms[Lambda(["z"], Terms [Var "z"; Var "z"]);Lambda(["z"], Terms [Var "z"; Var "z"])]]
    let ans = Var "y"
    reduction tm |> should equal ans

    let tm = Terms [Terms[Terms[Lambda(["x"], Var "y")]]; Terms[Lambda(["z"], Terms [Var "z"; Var "z"]);Lambda(["z"], Terms [Var "z"; Var "z"])]]
    let ans = Var "y"
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
