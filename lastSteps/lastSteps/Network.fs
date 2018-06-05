// Learn more about F# at http://fsharp.org
module hw

open System
open System.IO

let read = bigint.Parse << Console.ReadLine
let readInt = Int32.Parse << Console.ReadLine
let readStr = Console.ReadLine
let rnd = System.Random()

/// <summary>
/// Simulates the behavior of the computer under virus attacks
/// </summary>
type Computer (aChance : uint32 ) =
    let mutable containsVirus = false
    member v.SetVirus() = containsVirus <- true;
    member v.Probe(chance) = if not containsVirus then containsVirus <- chance < aChance;
    member v.IsInfected() = containsVirus

/// <summary>
/// Emulator of a network with viruses
/// </summary>
type Network (gen : unit -> uint32) = 
    let mutable graph : array<list<int> > = [|[]|]
    let mutable computers : array<Computer> = [||]
    let mutable setOfInfected : list<int> = []

/// <summary>
/// Add an edge between the 'first' and 'second' computer
/// </summary>
    member v.AddEdge first second = 
        assert(0 <= first && first < graph.Length)
        assert(0 <= second && second < graph.Length)
        graph.[first] <- second::graph.[first]
        graph.[second] <- first::graph.[second]

/// <summary>
/// Add a virus to the computer with number of 'pos'
/// </summary>
    member v.AddVirus pos =
        assert (0 <= pos && pos < computers.Length)
        computers.[pos].SetVirus()
        setOfInfected <- pos::setOfInfected

/// <summary>
/// Add a computer to the network
/// The smallest free number is assigned to it
/// </summary>
    member v.AddComputer (comp : Computer) =
        computers <- Array.append computers [|comp|]
        graph <- Array.append graph [|[]|]

/// <summary>
/// Emulator of a network with viruses
/// </summary>
    member v.GetState() = 
        let mutable ans : array<bool> = Array.init computers.Length (fun _ -> false)
        for i in 0..computers.Length - 1 do
            ans.[i] <- computers.[i].IsInfected()
        ans

/// <summary>
/// Next iteration of network emulation
/// </summary>
    member v.Step() = 
        let mutable tmp = []
        for x in setOfInfected do
            for v in graph.[x] do
                if not <| computers.[v].IsInfected() then
                    computers.[v].Probe <| gen()
                    if computers.[v].IsInfected() then
                        tmp <- v::tmp
        for x in tmp do
            setOfInfected <- x::setOfInfected

(*_*)
[<EntryPoint>]
let main argv =
    0 // return an integer exit code
