module Tests

open Xunit
open FsUnit
open hw 

[<Fact>]
let test = 
//=======================<task1>=======================
    checkBranches [] |> should be True
    checkBranches ('['::[]) |> should be False
    checkBranches ('}'::[]) |> should be False
    checkBranches ('}'::'{'::[]) |> should be False
    checkBranches ('('::'}'::'{'::')'::[]) |> should be False
    checkBranches ('['::'['::'['::']'::']'::']'::[]) |> should be True
    checkBranches ('{'::'['::'('::')'::']'::'}'::[]) |> should be True
    checkBranches ('{'::'['::'('::'('::']'::'}'::[]) |> should be False
    checkBranches ('{'::'['::'('::'['::']'::'}'::[]) |> should be False
    checkBranches ('['::']'::'['::']'::'['::']'::[]) |> should be True
//=======================</task1>======================
//=======================<task2>=======================
    func 9 ([]) |> should equal <| []
    func -1 (-100::0::100::[]) |> should equal <| 100::0::-100::[]
    func 0 (1::2::3::4::5::[]) |> should equal <| 0::0::0::0::0::[]
    func 5 (1::2::3::4::5::[]) |> should equal <| 5::10::15::20::25::[]
    func 10 (1::2::3::4::5::[]) |> should equal <| 10::20::30::40::50::[]
//=======================</task2>======================
//=======================<task3>=======================
//=======================</task3>======================
