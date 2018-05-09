open FsUnit
open hw 
open FsCheck

let test () = 
    solve "http://hwproj.me/courses/9/terms/4"
    printfn "<----------------------------------------------->"
    solve "https://habr.com/post/268141/"
    printfn "<----------------------------------------------->"

[<EntryPoint>]
let main argv =
    test()
    0 // return an integer exit code
    