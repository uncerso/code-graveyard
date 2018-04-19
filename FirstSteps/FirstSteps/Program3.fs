// Learn more about F# at http://fsharp.org

open System
open NUnit.Framework
open FsUnit

let read = bigint.Parse << Console.ReadLine
let readInt = Int32.Parse << Console.ReadLine
let readStr = Console.ReadLine
let rnd = System.Random()

//====================<task 1>====================
let amountOfEvenNumbersWithFold ls = 
    List.fold (fun acc el -> acc + 1 - (el &&& 1)) 0 ls
let amountOfEvenNumbersWithMap ls = 
    List.sum (List.map (fun el -> 1 - (el &&& 1)) ls)
let amountOfEvenNumbersWithFilter ls = 
    List.length (List.filter (fun el -> (el &&& 1) = 0) ls)

let task1checker ls ans =
    amountOfEvenNumbersWithFold  ls |> should equal ans;
    amountOfEvenNumbersWithMap ls |> should equal ans;
    amountOfEvenNumbersWithFilter ls |> should equal ans;

[<Test>]
let task1Tests =
    let ls = [1; 2; 3; 4; 5; 6; 7;]
    let ans = 3
    task1checker ls ans

    let ls = []
    let ans = 0    
    task1checker ls ans

    let ls = [1; 1; 1;]
    let ans = 0    
    task1checker ls ans

    let ls = [-1; -1; 1;]
    let ans = 0    
    task1checker ls ans

    let ls = [2; 2; 2;]
    let ans = 3   
    task1checker ls ans

    let ls = [-2; -2; -2;]
    let ans = 3   
    task1checker ls ans

    printfn "task 1: ok"
//====================</task 1>===================

//====================<task 2>====================
type Tree<'T> =
    | Tree of 'T * Tree<'T> * Tree<'T>
    | Tip of 'T

let rec print tr =
    match tr with
    | Tree(x, l, r) -> 
            print(l); 
            printf "%d " x; 
            print(r)
    | Tip(x) -> 
            printf "%d " x;

let rec map (foo: 'a -> 'b) (tr:Tree<'a>) : Tree<'b>=
    match tr with
    | Tree(x, l, r) -> Tree((foo x), (map foo l), (map foo r))
    | Tip(x) -> Tip(foo x)

let rec addFrontToList tr acc=
    match tr with
    | Tree(x, l, r) -> addFrontToList l (x::(addFrontToList r acc));
    | Tip(x) -> x::acc;


[<Test>]
let task2Tests =
    let tree = Tree (7, Tree(3, Tip 20, Tip -6), Tip 9)
    addFrontToList tree [] |> should equal [20; 3; -6; 7; 9]
    addFrontToList (map (fun x -> x * 2) tree) [] |> should equal [40; 6; -12; 14; 18]

    addFrontToList (Tip 5) [] |> should equal [5]
    addFrontToList (map (fun x -> x / 2) (Tip 3)) [] |> should equal [1]

    printfn "task 2: ok"
//====================</task 2>===================

//====================<task 3>====================
type Operator = 
    | X of double
    | Sum of Operator * Operator
    | Sub of Operator * Operator
    | Mp of Operator * Operator
    | Div of Operator * Operator

let rec eval (op : Operator) = 
    match op with
    | X(x) -> x
    | Sum(op1, op2) -> (eval op1) + (eval op2)
    | Sub(op1, op2) -> (eval op1) - (eval op2)
    | Mp(op1, op2) -> (eval op1) * (eval op2)
    | Div(op1, op2) -> (eval op1) / (eval op2)

[<Test>]
let task3Tests =
    eval (Sum (X -3.0, X 18.0)) |> should equal 15.0
    eval (Sub (X 8.0, X -13.0)) |> should equal 21.0
    eval (Mp(Mp(X 2.0, X 2.0), Mp(X 2., X 2.))) |> should equal 16.0
    eval (Mp (Sum (X 3.0, X 7.0), Sub (X 1.13, X 9.13))) |> should equal -80.0
    eval (Div (Mp (X 3.0, X 7.0), Sub (X 1.13, X 9.13))) |> should equal -2.625
    printfn "task 3: ok"
//====================</task 3>===================

//====================<task 4>====================
let primeTest (num:uint64) = 
    match num with
    | x when x < 2UL -> false
    | 2UL -> true
    | x when (x &&& 1UL) = 0UL -> false
    | x ->
        let lim = (uint64 <| sqrt(double x)) + 1UL
        let rec test acc = 
            match acc with
            | x when x > lim -> true
            | x when (num % x = 0UL) -> false
            | _ -> test (acc + 2UL)
        test 3UL

let primeNumbers = 
    seq{ yield 2UL
         for acc in 3UL..2UL..uint64 ((1I <<< 64) - 1I) do if (primeTest acc) then yield acc}

[<Test>]
let task4Tests =
    primeTest 2UL |>  should be True
    primeTest 3UL |>  should be True
    primeTest 4UL |>  should be False
    primeTest 1000000007UL |>  should be True
    primeTest 1000000008UL |>  should be False
    primeTest 1000000009UL |>  should be True
    primeTest 1000000010UL |>  should be False
    primeTest 1000000011UL |>  should be False
    primeTest 1000000000000000007UL |>  should be False
    primeTest 1000000000000000013UL |>  should be False
    primeTest 1000000000000000011UL |>  should be False
    primeTest 100000000000000007UL |>  should be False
    primeTest 100000000000000013UL |>  should be True

    let ls = [2; 3; 5; 7; 11; 13; 17; 19; 23; 29; 31; 37; 41; 43; 47; 53; 59; 61; 67; 71; 73; 79; 83; 89; 97; 101; 103; 107; 109; 113; 127; 131; 137; 139; 149; 151; 157; 163; 167; 173; 179; 181; 191; 193; 197; 199; 211; 223; 227; 229; 233; 239; 241; 251; 257; 263; 269; 271;] // from OEIS
    Seq.toList (Seq.take ls.Length primeNumbers) |> should equal ls
    printfn "task 4: ok"
//====================</task 4>===================

(*_*)
[<EntryPoint>]
let main argv =
    0 // return an integer exit code
