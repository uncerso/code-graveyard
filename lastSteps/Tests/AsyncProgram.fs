open NUnit.Framework
open hw 
open System.Text.RegularExpressions
open FsUnit.TopLevelOperators

[<Test>]
let ``test`` () = 
    solve "http://hwproj.me/courses/9/terms/4"
    printfn "<----------------------------------------------->"
    solve "https://habr.com/post/268141/"
    printfn "<----------------------------------------------->"
    solve "https://www.google.com/"
    printfn "<----------------------------------------------->"
    solve "https://habr.com/feed/"
    printfn "<----------------------------------------------->"

    let amountOfNonHrefChars = 9
    let finished = ref false
    let html = ref ""

    fetchAsync "https://www.google.com/" html finished |> Async.RunSynchronously
    let urls = Regex.Match(!html, "<a href=\"http://[^\"]*\">")
    let url = urls.Value.Substring(amountOfNonHrefChars, urls.Value.Length - amountOfNonHrefChars - 2)
    url |> should equal "http://www.google.ru/intl/ru/services/"

    fetchAsync "http://hwproj.me/courses/9/terms/4" html finished |> Async.RunSynchronously
    let mutable urls = Regex.Match(!html, "<a href=\"http://[^\"]*\">")
    let mutable flag = false
    while urls.Success do
        let url = urls.Value.Substring(amountOfNonHrefChars, urls.Value.Length - amountOfNonHrefChars - 2)
        if not flag then flag <- url = "http://fsharp.org/"
        urls <- urls.NextMatch()
    flag |> should be True
