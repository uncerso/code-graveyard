// Learn more about F# at http://fsharp.org
module hw

open System
open System.Collections
open System.Collections.Generic
open System.IO

let read = bigint.Parse << Console.ReadLine
let readInt = Int32.Parse << Console.ReadLine
let readStr = Console.ReadLine
let rnd = System.Random()

type Tree<'T> = 
| Tree of 'T * Tree<'T> * Tree<'T>
| Leaf
    
type BinaryTree<'T when 'T : comparison>() =
    let mutable root = Leaf
    let mutable size = 0

    let Iterator () = 
        let rec handle (node : Tree<'T>) = 
            seq {
                match node with
                | Tree(curValue, left, right) -> 
                    yield! handle left
                    yield curValue
                    yield! handle right
                | Leaf -> yield! Seq.empty
            }
        handle root

    interface IEnumerable<'T> with

        member this.GetEnumerator(): IEnumerator<'T> =
            Iterator().GetEnumerator()

        member this.GetEnumerator(): IEnumerator =
            Iterator().GetEnumerator() :> IEnumerator

    member v.Empty () = size = 0

    member v.Size () = size

    member v.Insert (value : 'T) =
        let rec handle (value : 'T) (node : Tree<'T>) = 
            match node with
            | Leaf -> Tree(value, Leaf, Leaf)
            | Tree(curValue, left, right) when value < curValue -> Tree(curValue, handle value left, right) 
            | Tree(curValue, left, right)  -> Tree(curValue, left, handle value right)
        root <- handle value root
        size <- size + 1

    member v.Erase (value : 'T) = 
        let rec cutLeft node =
            match node with
            | Tree(value, Leaf, right) -> (value, right)
            | Tree(value, left, right) -> 
                let (cutted, newChild) = cutLeft left
                (cutted, Tree(value, newChild, right))

        let rec handle value node =
            match node with
            | Leaf -> 
                size <- size + 1;
                Leaf
            | Tree(curValue, left, right) when value < curValue -> Tree(curValue, handle value left, right)
            | Tree(curValue, left, right) when curValue < value -> Tree(curValue, left, handle value right)
            | Tree(_, left, right) -> 
                if left = Leaf then right
                elif right = Leaf then left
                else 
                    let (cutted, newChild) = cutLeft right
                    Tree(cutted, left, newChild)

        root <- handle value root
        size <- size - 1

    member v.At (value : 'T) =
        let rec handle (value : 'T) (node : Tree<'T>) = 
            match node with
            | Tree(curValue, left, right) when value < curValue -> handle value left
            | Tree(curValue, left, right) when curValue < value -> handle value right
            | Leaf -> false
            | _ -> true
        handle value root

(*_*)
[<EntryPoint>]
let main argv =
    
    0 // return an integer exit code
