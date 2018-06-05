open NUnit.Framework
open hw 

[<Test>]
let ``test`` () = 
    solve "http://hwproj.me/courses/9/terms/4"
    printfn "<----------------------------------------------->"
    solve "https://habr.com/post/268141/"
    printfn "<----------------------------------------------->"
