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

let fetchAsync (url : string) finished =
    async{
        let request = WebRequest.Create(url)
        use! response = request.AsyncGetResponse()
        use stream = response.GetResponseStream()
        use reader = new StreamReader(stream)
        let html = reader.ReadToEnd()
        do printfn "Read %d characters for %s" html.Length url
        do finished := true
    }

let solve (url : string) = 
    let rec iterate (urls : Match) =
        let amountOfNonHrefChars = 9
        if (urls.Success) then
            let finished = ref false
            Async.Start <| fetchAsync (urls.Value.Substring(amountOfNonHrefChars, urls.Value.Length - amountOfNonHrefChars - 2)) finished 
            iterate <| urls.NextMatch()
            while not !finished do 
                printfn "Waiting for %s" <| urls.Value.Substring(amountOfNonHrefChars, urls.Value.Length - amountOfNonHrefChars - 2)
                wait 500.0
        ()
    let request = WebRequest.Create(url)
    use response = request.GetResponse()
    use stream = response.GetResponseStream()
    use reader = new StreamReader(stream)
    let html = reader.ReadToEnd();
    let urls = Regex.Match(html, "<a href=\"http://[^\"]*\">")
    iterate urls

(*_*)
[<EntryPoint>]
let main argv =
    0 // return an integer exit code
