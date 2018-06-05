open FsUnit
open hw 
open FsCheck

let nothingBesideZero (net : Network) =
    let mutable b = true
    for x in net.getState() do
        x |> should equal b
        b <- false

let weak_virus ()= 
    printfn "weak_virus : begin"
    let n = 10
    let net = Network((fun () -> 5ul))
    printfn "weak_virus : point 1"
    for i in 1..n do
        net.addComputer(Computer(1ul))
    printfn "weak_virus : point 2"
    for i in 1..n-1 do
        net.addEdge 0 i
    printfn "weak_virus : point 3"
    net.addVirus 0
    nothingBesideZero net
    for i in 0..10000 do
        net.step()
        nothingBesideZero net
    printfn "weak_virus : end"

let strong_virus_one_step () = 
    printfn "strong_virus_one_step : begin"
    let n = 100000
    let net = Network((fun () -> 5ul))
    printfn "strong_virus_one_step : point 1"
    for i in 1..n do
        net.addComputer(Computer(10ul))
    printfn "strong_virus_one_step : point 2"
    for i in 1..n-1 do
        net.addEdge 0 i
    printfn "strong_virus_one_step : point 3"
    net.addVirus 0
    nothingBesideZero net
    net.step()
    for x in net.getState() do
        x |> should be True
    printfn "strong_virus_one_step : end"

let strong_virus_tree () = 
    printfn "strong_virus_tree : begin"
    let n = (1 <<< 16) - 1
    let net = Network((fun () -> 5ul))
    printfn "strong_virus_tree : point 1"
    for i in 1..n do
        net.addComputer(Computer(10ul))
    printfn "strong_virus_tree : point 2"
    let mutable i = 0
    while ((i + 1) <<< 1) < n do // (i + 1) <<< 1 equal i * 2 + 2
        net.addEdge i ((i <<< 1) + 1)
        net.addEdge i ((i <<< 1) + 2)
        i <- i + 1
    printfn "strong_virus_tree : point 3"
    net.addVirus 0
    let mutable infestedTo : uint64 = 1UL
    nothingBesideZero net
    for i in 0..62 do
        net.step()
        infestedTo <- (infestedTo <<< 1) + 1UL
        let x = net.getState()
        for j in 0..n-1 do
            x.[j] |> should equal (uint64 j < infestedTo)
    printfn "strong_virus_tree : end"
    
let random_virus_tree ()= 
    printfn "random_virus_tree : begin"
    let n = (1 <<< 16) - 1
    let net = Network (fun () -> uint32 <| rnd.Next())
    printfn "random_virus_tree : point 1"
    for i in 1..n do
        net.addComputer(Computer(uint32 <| rnd.Next()))
    printfn "random_virus_tree : point 2"
    let mutable i = 0
    while ((i + 1) <<< 1) < n do // (i + 1) <<< 1 equal i * 2 + 2
        net.addEdge i ((i <<< 1) + 1)
        net.addEdge i ((i <<< 1) + 2)
        i <- i + 1
    printfn "random_virus_tree : point 3"
    net.addVirus 0
    let mutable infestedTo : uint64 = 1UL
    nothingBesideZero net
    for i in 0..62 do
        net.step()
        if i < 63 then
            infestedTo <- (infestedTo <<< 1) + 1UL
        let now = net.getState()
        now.[0] |> should be True
        for j in 1..n-1 do
            if now.[j] then now.[(j - 1) >>> 1] |> should be True // parent infested
            if (i < 63) && (uint64 j >= infestedTo) then now.[j] |> should be False
    printfn "random_virus_tree : end"
    
[<EntryPoint>]
let main argv =
    weak_virus()
    strong_virus_one_step()
    strong_virus_tree()
    random_virus_tree ()
    0 // return an integer exit code
