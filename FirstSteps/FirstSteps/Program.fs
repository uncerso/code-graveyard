// Learn more about F# at http://fsharp.org

open System

let read = bigint.Parse << Console.ReadLine
let readInt = Int32.Parse << Console.ReadLine

//====================<task 1>====================
let rec factorial (n:bigint) : bigint =
    if (n < 2I) then 1I else n * factorial(n - 1I)
//====================</task 1>===================

//====================<task 2>====================
let rec fib n minOfTheNum maxOfTheNum : bigint =
    if (n = 1I) then minOfTheNum  else fib (n - 1I) maxOfTheNum (minOfTheNum + maxOfTheNum)

let fibonacci n =
    assert(n > 0I)
    fib n 0I 1I
//====================</task 2>===================

//====================<task 3>====================
let revers ls =
    List.fold (fun acc el -> List.append [el;] acc) [] ls
//====================</task 3>===================

//====================<task 4>====================
let gen n m = 
    List.init m (fun index -> 1I <<< (n + index))
//====================</task 4>===================

(*_*)
[<EntryPoint>]
let main argv =
    (*task 1*)
    printf "Enter the number for factorial: "
    printfn "%s\n" <| factorial(read()).ToString()
    (*task 2*)
    printf "Enter the number for fibonacci (the sequence is 0 1 1 2 ...): "
    printfn "%s\n" <| fibonacci(read()).ToString()
    (*task 3*)
    printf "Enter size of the list: "
    printfn "%A" (revers [1..readInt()])
    (*task 4*)
    printf "Enter n: "
    let n = readInt();
    printf "Enter m: "
    let m = readInt();
    printfn "%A" (gen n m)
    0 // return an integer exit code
