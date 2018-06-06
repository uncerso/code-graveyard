open NUnit.Framework
open FsUnit
open hw 

let rec permutations list taken = 
  seq { if Set.count taken = List.length list then yield [] else
        for l in list do
          if not (Set.contains l taken) then 
            for perm in permutations list (Set.add l taken)  do
              yield l::perm }

let testIterator (x:List<int>) =
    let bst = BinaryTree<int>()
    for t in x do bst.Insert t;
    let mutable t = 0
    for it in bst do
        it |> should equal t
        t <- t + 1;
    t |> should equal x.Length

let testFind (x:List<int>) = 
    let bst = BinaryTree<int>()
    for t in x do bst.Insert t;
    for i in -1..x.Length do
        (bst.At i ) |> should equal (not (i < 0 || i >= x.Length))

let testErase (x:List<int>) = 
    for k in -1..x.Length do
        let bst = BinaryTree<int>()
        for t in x do bst.Insert t
        bst.Erase k
        let mutable t = 0;
        for p in bst do
            if t = k then t <- t + 1
            t |> should equal p
            t <- t + 1

let randomTest (n) =
    let bst = BinaryTree<int>()
    let mutable st = Set.empty
    for i in 1..n do
        let el = rnd.Next()
        if rnd.Next() % 2 = 0 then
            if Set.contains el st then
                bst.At el |> should be True
            else
                bst.At el |> should be False
                bst.Insert el
                bst.At el |> should be True
        else
            let sz = bst.Size()
            if Set.contains el st then
                bst.At el |> should be True
                bst.Erase el
                bst.At el |> should be False
                bst.Size() |> should equal (sz - 1)
            else
                bst.At el |> should be False
                bst.Erase el
                bst.At el |> should be False
                bst.Size() |> should equal sz
            
[<Test>]
let ``test`` () = 
    let n = 8
    for x in permutations (List.init n id) Set.empty do
        testIterator x

    for x in permutations (List.init n id) Set.empty do
        testFind x

    for x in permutations (List.init (n - 1) id) Set.empty do
        testErase x
    for i in 1..100 do
        randomTest(100)
