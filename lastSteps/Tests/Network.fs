open hw
open NUnit.Framework
open FsUnit

let nothingBesideZero (net : Network) =
    let mutable b = true
    for x in net.GetState() do
        x |> should equal b
        b <- false

[<Test>]
let ``weakVirus`` ()= 
    printfn "weakVirus : begin"
    let n = 10
    let net = Network((fun () -> 5ul))
    printfn "weakVirus : point 1"
    for i in 1..n do
        net.AddComputer(Computer(1ul))
    printfn "weakVirus : point 2"
    for i in 1..n-1 do
        net.AddEdge 0 i
    printfn "weakVirus : point 3"
    net.AddVirus 0
    nothingBesideZero net
    for i in 0..10000 do
        net.Step()
        nothingBesideZero net
    printfn "weakVirus : end"

[<Test>]
let ``strongVirusOneStep`` () = 
    printfn "strongVirusOneStep : begin"
    let n = 100000
    let net = Network((fun () -> 5ul))
    printfn "strongVirusOneStep : point 1"
    for i in 1..n do
        net.AddComputer(Computer(10ul))
    printfn "strongVirusOneStep : point 2"
    for i in 1..n-1 do
        net.AddEdge 0 i
    printfn "strongVirusOneStep : point 3"
    net.AddVirus 0
    nothingBesideZero net
    net.Step()
    for x in net.GetState() do
        x |> should be True
    printfn "strongVirusOneStep : end"

[<Test>]
let ``strongVirusTree`` () = 
    printfn "strongVirusTree : begin"
    let n = (1 <<< 16) - 1
    let net = Network((fun () -> 5ul))
    printfn "strongVirusTree : point 1"
    for i in 1..n do
        net.AddComputer(Computer(10ul))
    printfn "strongVirusTree : point 2"
    let mutable i = 0
    while ((i + 1) <<< 1) < n do // (i + 1) <<< 1 equal i * 2 + 2
        net.AddEdge i ((i <<< 1) + 1)
        net.AddEdge i ((i <<< 1) + 2)
        i <- i + 1
    printfn "strongVirusTree : point 3"
    net.AddVirus 0
    let mutable infestedTo : uint64 = 1UL
    nothingBesideZero net
    for i in 0..62 do
        net.Step()
        infestedTo <- (infestedTo <<< 1) + 1UL
        let x = net.GetState()
        for j in 0..n-1 do
            x.[j] |> should equal (uint64 j < infestedTo)
    printfn "strongVirusTree : end"
    
[<Test>]
let ``randomVirusTree`` ()= 
    printfn "randomVirusTree : begin"
    let n = (1 <<< 16) - 1
    let net = Network (fun () -> uint32 <| rnd.Next())
    printfn "randomVirusTree : point 1"
    for i in 1..n do
        net.AddComputer(Computer(uint32 <| rnd.Next()))
    printfn "randomVirusTree : point 2"
    let mutable i = 0
    while ((i + 1) <<< 1) < n do // (i + 1) <<< 1 equal i * 2 + 2
        net.AddEdge i ((i <<< 1) + 1)
        net.AddEdge i ((i <<< 1) + 2)
        i <- i + 1
    printfn "randomVirusTree : point 3"
    net.AddVirus 0
    let mutable infestedTo : uint64 = 1UL
    nothingBesideZero net
    for i in 0..62 do
        net.Step()
        if i < 63 then
            infestedTo <- (infestedTo <<< 1) + 1UL
        let now = net.GetState()
        now.[0] |> should be True
        for j in 1..n - 1 do
            if now.[j] then now.[(j - 1) >>> 1] |> should be True // parent infested
            if (i < 63) && (uint64 j >= infestedTo) then now.[j] |> should be False
    printfn "randomVirusTree : end"
    
//[<EntryPoint>]
//let main argv =
//    0 // return an integer exit code
