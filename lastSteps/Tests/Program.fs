open FsUnit
open hw 
open FsCheck

let test ()= 
//=======================<task1>=======================
    checkBrackets "" |> should be True
    checkBrackets "[" |> should be False
    checkBrackets "}" |> should be False
    checkBrackets "}{" |> should be False
    checkBrackets "(}{)" |> should be False
    checkBrackets "[[[]]]" |> should be True
    checkBrackets "{[()]}" |> should be True
    checkBrackets "{[((]}" |> should be False
    checkBrackets "{[([]}" |> should be False
    checkBrackets "[][][]" |> should be True
//=======================</task1>======================
//=======================<task2>=======================
    func 9 ([]) |> should equal <| []
    func -1 (-100::0::100::[]) |> should equal <| 100::0::-100::[]
    func 0 (1::2::3::4::5::[]) |> should equal <| 0::0::0::0::0::[]
    func 5 (1::2::3::4::5::[]) |> should equal <| 5::10::15::20::25::[]
    func 10 (1::2::3::4::5::[]) |> should equal <| 10::20::30::40::50::[]
    let check (x : int) (ls : list<int>) = func x ls = List.map (fun el -> el*x) ls
    Check.Quick check
//=======================</task2>======================


[<EntryPoint>]
let main argv =
    test()
    0 // return an integer exit code
    