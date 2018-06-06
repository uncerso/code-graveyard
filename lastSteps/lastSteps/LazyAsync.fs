// Learn more about F# at http://fsharp.org
module hw

open System
open System.IO
open System.Threading

let read = bigint.Parse << Console.ReadLine
let readInt = Int32.Parse << Console.ReadLine
let readStr = Console.ReadLine
let rnd = System.Random()

type ILazy<'a> =
    abstract member Get: unit -> 'a

type SingleThreadedLazy<'a> (supplier : unit -> 'a) =
    let mutable res : option<'a> = None
    interface ILazy<'a> with
        member v.Get() =
            if res.IsNone then res <- Some <| supplier()
            res.Value

type MultiThreadedLazy<'a> (supplier : unit -> 'a) =
    let mutable res = None
    let lockObject = obj()
    interface ILazy<'a> with
        member v.Get() =
            if res.IsNone then
                Monitor.Enter lockObject
                try
                    if res.IsNone then
                        res <- Some <| supplier()
                finally
                    Monitor.Exit lockObject
            res.Value

type LockFreeMultiThreadedLazy<'a> (supplier : unit -> 'a) =
    let res : option<'a> ref = ref None
    interface ILazy<'a> with
        member v.Get() =
            if res.Value.IsNone then
                let resTmp = Some <| supplier()
                ignore <| Interlocked.CompareExchange(res, resTmp, None)
            res.Value.Value

type LazyFactory () =
    static member CreateSingleThreadedLazy supplier = SingleThreadedLazy supplier
    static member CreateMultiThreadedLazy supplier = MultiThreadedLazy supplier
    static member CreateLockFreeMultiThreadedLazy supplier = LockFreeMultiThreadedLazy supplier

(*_*)
[<EntryPoint>]
let main argv =
    0 // return an integer exit code