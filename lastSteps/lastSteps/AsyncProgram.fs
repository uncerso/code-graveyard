// Learn more about F# at http://fsharp.org
module hw

open System
open System.IO
open System.Net
open System.Text.RegularExpressions
open System.Timers

let read = bigint.Parse << Console.ReadLine
let readInt = Int32.Parse << Console.ReadLine
let readStr = Console.ReadLine
let rnd = System.Random()

let wait time = 
    let timer = new Timer(time)
    let timerEvent = Async.AwaitEvent(timer.Elapsed) |> Async.Ignore
    timer.Start()
    Async.RunSynchronously timerEvent

let fetchAsync (url : string) html finished =
    async{
        let request = WebRequest.Create(url)
        use! response = request.AsyncGetResponse()
        use stream = response.GetResponseStream()
        use reader = new StreamReader(stream)
        do html := reader.ReadToEnd()
        do finished := true
    }

let solve (url : string) = 
    let rec iterate (urls : Match) =
        let amountOfNonHrefChars = 9
        if (urls.Success) then
            let finished = ref false
            let html = ref ""
            let url = urls.Value.Substring(amountOfNonHrefChars, urls.Value.Length - amountOfNonHrefChars - 2)
            Async.Start <| fetchAsync url html finished 
            iterate <| urls.NextMatch()
            while not !finished do 
                printfn "Waiting for %s" <| urls.Value.Substring(amountOfNonHrefChars, urls.Value.Length - amountOfNonHrefChars - 2)
                wait 500.0
            printfn "Read %d characters for %s" html.Value.Length url
        ()
    let finished = ref false
    let html = ref ""
    fetchAsync url html finished |> Async.RunSynchronously
    let urls = Regex.Match(!html, "<a href=\"http://[^\"]*\">")
    iterate urls

(*_*)
[<EntryPoint>]
let main argv =
    0 // return an integer exit code
