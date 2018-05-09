open FsUnit
open hw 
open FsCheck

let test () = 
    let us = Unordered_set<int> id
    us.Find 3 |> should be False
    us.Erase 3
    us.Find 3 |> should be False
    us.Insert 3
    us.Find 3 |> should be True
    us.Insert 3
    us.Find 3 |> should be True
    us.Erase 3
    us.Find 3 |> should be False
    
    for i in 1..19 do
        us.Insert i
    for i in 1..19 do
        us.Find i |> should be True
    us.Find 0 |> should be False
    us.Find 20 |> should be False
    us.Find 123 |> should be False
    us.Find -23 |> should be False
    us.Find -15 |> should be False
    
    for i in 30..2000000 do
        us.Insert i
    for i in 1..19 do
        us.Find i |> should be True
    for i in 30..2000000 do
        us.Find i |> should be True
    us.Find 0 |> should be False
    us.Find 20 |> should be False
    us.Find -123 |> should be False
    us.Find -23 |> should be False
    us.Find -15 |> should be False
    us.Find -32 |> should be False
    us.Find 30 |> should be True
    us.Erase 31415
    us.Find 31415 |> should be False
    for i in 1..19 do
        us.Find i |> should be True
    for i in 30..2000000 do
        if i <> 31415 then 
            us.Find i |> should be True
[<EntryPoint>]
let main argv =
    test()
    0 // return an integer exit code
    