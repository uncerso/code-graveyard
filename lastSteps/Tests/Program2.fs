open FsUnit
open hw 
open FsCheck

let test ()= 
//=======================<task1>=======================
    let ans =
        Rounding 3 {
            let! a = 2.0 / 12.0
            let! b = 3.5
            return a / b
        }
    ans |> should equal 0.048
//=======================</task1>======================
//=======================<task2>=======================
    let ans = 
        Calculate() {
            let! x = "1"
            let! y = "2"
            let z = x + y
            return z
        }
    ans |> should equal <| Some 3
    
    let ans = 
        Calculate() {
            let! x = "1"
            let! y = "ololo"
            let z = x + y
            return z
        }
    ans |> should equal <| None
//=======================</task2>======================


[<EntryPoint>]
let main argv =
    test()
    0 // return an integer exit code
    