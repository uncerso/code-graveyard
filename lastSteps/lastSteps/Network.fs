// Learn more about F# at http://fsharp.org
module hw

open System
open System.IO

let read = bigint.Parse << Console.ReadLine
let readInt = Int32.Parse << Console.ReadLine
let readStr = Console.ReadLine
let rnd = System.Random()

type Computer (aChance : uint32 ) =
    let mutable containsVirus = false
    member v.setVirus() = containsVirus <- true;
    member v.probe(chance) = if not containsVirus then containsVirus <- chance < aChance;
    member v.isInfected() = containsVirus

type Network (gen : unit -> uint32) = 
    let mutable graph : array<list<int> > = [|[]|]
    let mutable computers : array<Computer> = [||]
    let mutable setOfInfected : list<int> = []
    member v.addEdge first second = 
        assert(0 <= first && first < graph.Length)
        assert(0 <= second && second < graph.Length)
        graph.[first] <- second::graph.[first]
        graph.[second] <- first::graph.[second]
    member v.addVirus pos =
        assert (0 <= pos && pos < computers.Length)
        computers.[pos].setVirus()
        setOfInfected <- pos::setOfInfected
    member v.addComputer (comp : Computer) =
        computers <- Array.append computers [|comp|]
        graph <- Array.append graph [|[]|]
    member v.getState() = 
        let mutable ans : array<bool> = Array.init computers.Length (fun _ -> false)
        for i in 0..computers.Length-1 do
            ans.[i] <- computers.[i].isInfected()
        ans
    member v.step() = 
        let mutable tmp = []
        for x in setOfInfected do
            for v in graph.[x] do
                if not <| computers.[v].isInfected() then
                    computers.[v].probe <| gen()
                    if computers.[v].isInfected() then
                        tmp <- v::tmp
        for x in tmp do
            setOfInfected <- x::setOfInfected

(*_*)
[<EntryPoint>]
let main argv =
    0 // return an integer exit code
