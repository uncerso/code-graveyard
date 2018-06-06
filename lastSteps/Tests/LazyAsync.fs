open NUnit.Framework
open hw 
open FsUnit.TopLevelOperators

[<Test>]
let ``SingleThreadedLazyTest`` () = 
    let n = 100
    let sl = LazyFactory.CreateSingleThreadedLazy<int> rnd.Next
    let res = (sl :> ILazy<int>).Get()
    for i in 1..n do
        (sl :> ILazy<int>).Get() |> should equal res

[<Test>]
let ``MultiThreadedLazyTest`` () = 
    let n = 100
    let ml = LazyFactory.CreateMultiThreadedLazy<int> rnd.Next
    let ar = Array.init n (fun _ -> async { return (ml :> ILazy<int>).Get() } )
    let ans : array<int> = ar |> Async.Parallel |> Async.RunSynchronously
    let res = ans.[0]
    for i in 1..n - 1 do
        ans.[i] |> should equal res

[<Test>]
let ``LockFreeMultiThreadedLazyTest`` () = 
    let n = 100
    let lfml = LazyFactory.CreateLockFreeMultiThreadedLazy<int> rnd.Next
    let ar = Array.init n (fun _ -> async { return (lfml :> ILazy<int>).Get() } )
    let ans : array<int> = ar |> Async.Parallel |> Async.RunSynchronously
    let res = ans.[0]
    for i in 1..n - 1 do
        ans.[i] |> should equal res
