open FsUnit
open hw 
open FsCheck

let test ()= 
    let q = PrimaryQueue<int>()
    q.Insert (5 , 7)
    q.Front() |> should equal (5, 7)

    q.Insert (2 , 1)
    q.Front() |> should equal (5, 7)
    
    q.Insert (12 , 5)
    q.Front() |> should equal (12, 5)
    
    q.Insert (7 , 3)
    q.Front() |> should equal (12, 5)

    q.Pop()
    q.Front() |> should equal (7, 3)

    q.Pop()
    q.Front() |> should equal (5, 7)

    q.Insert (0, 0)
    q.Front() |> should equal (5, 7)

    q.Pop()
    q.Front() |> should equal (2, 1)

    q.Insert (3, 4)
    q.Front() |> should equal (3, 4)

    q.Pop()
    q.Front() |> should equal (2, 1)

[<EntryPoint>]
let main argv =
    test()
    0 // return an integer exit code
    