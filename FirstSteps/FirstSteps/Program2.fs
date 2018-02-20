// Learn more about F# at http://fsharp.org

open System

let read = bigint.Parse << Console.ReadLine
let readInt = Int32.Parse << Console.ReadLine
let readStr = Console.ReadLine
let rnd = System.Random()

//====================<task 1>====================
let rec productOfDigitsOfNumber x =
    if (abs(x) < 10I) then abs(x) else (x % 10I) * productOfDigitsOfNumber(x / 10I)
//====================</task 1>===================

//====================<task 2>====================
let findAndGetFirstIndexes ls pattern =
    max -1 (-List.fold (fun acc el -> if acc < 0 then acc elif pattern = el then -acc else (acc + 1)) 0 ls)
//====================</task 2>===================

//====================<task 3>====================
let palindromeChecker (s:string) =
    let rec check (s:string) left right : bool =
        if (left >= right)
            then true
        elif (s.Chars(left) = s.Chars(right))
            then (check s (left + 1) (right - 1)) 
        else false
    check s 0 (s.Length - 1)
//====================</task 3>===================

//====================<task 4>====================
let rec mergeSort (ls:List<int>) =
    let rec merge (left:List<int>) (right:List<int>) = 
        if (left.IsEmpty) 
            then right
        elif (right.IsEmpty) 
            then left
        elif (left.Head < right.Head)
            then left.Head::merge left.Tail right
        else right.Head::merge left right.Tail
    let split ls =
        let sp el acc = 
            let a, b, c, d = acc
            if (c < d) then (el::a, b, c - 1, d) else (a, el::b, c - 1, d)
        let a, b, c, d = List.foldBack sp ls ([], [], ls.Length - 1, ls.Length / 2)
        (a, b)
    if (ls.Length < 2) 
        then ls
    else
        let lists = split ls
        merge (mergeSort (fst lists)) (mergeSort (snd lists))
//====================</task 4>===================

(*_*)
[<EntryPoint>]
let main argv =
    (*task 1*)
    printf "Enter a number: "
    printfn "%O" <| productOfDigitsOfNumber(read())
    (*task 2*)
    printf "Enter size of a list which will be generated with random numbers from 0 to 9: "
    let n = readInt()
    let ls = List.init n (fun _ -> (rnd.Next() % 10))
    printfn "%A\n%A\nEnter your number from 0 to 9" <| [0..n-1] <| ls
    printfn "%d" <| findAndGetFirstIndexes ls (readInt())
    (*task 3*)
    printfn "Enter string to check: "
    printfn "%b" <| palindromeChecker(readStr())
    (*task 4*)
    printf "Enter size of a list: "
    let n = readInt()
    let ls = List.init n (fun _ -> (10 + rnd.Next() % 40)) // random from 10..59
    printfn "%A\n%A" <| ls <| mergeSort(ls)
    0 // return an integer exit code
