// Learn more about F# at http://fsharp.org
module hw

open System
open System.IO

let read = bigint.Parse << Console.ReadLine
let readInt = Int32.Parse << Console.ReadLine
let readStr = Console.ReadLine
let rnd = System.Random()

//=======================<task1>=======================
let checkBrackets (str:string) =
    let rec check ls =
        match ls with
        | [] -> []
        | [x] -> [x]
        | t::others ->
            let res = check others
            match res with
            | [] -> [t]
            | x::tail ->
                match t with
                | '[' -> if (x = ']') then tail else t::res
                | '{' -> if (x = '}') then tail else t::res
                | '(' -> if (x = ')') then tail else t::res
                | _ -> t::res
    (check <| Seq.toList str).Length = 0
//=======================</task1>======================

//=======================<task2>=======================
let func : int -> list<int> -> list<int> = 
    //func x l = List.map (fun y -> y * x) l
    //func x = List.map (fun y -> y * x)
    //func x = List.map (fun y -> (*) x y)
    //func x = List.map (fun y -> ((*) x) y)
    //func x = List.map ((*) x)
    //func x = List.map <| (*) x
    List.map << (*)
//=======================</task2>======================

//=======================<task3>=======================
let getInput hint =
    printf "%s" hint
    readStr()
let add ls = 
    (getInput "Enter a name: ", getInput "Enter a phone number: ")::ls

let findPhone ls name = 
    let rec find name ans ls =
        match ls with
        | [] -> ans
        | x::tail -> 
            if ((fst x) = name) then  find name ((snd x)::ans) tail
            else find name ans tail
    find name [] ls

let findName ls number =
    let rec find number ans ls =
        match ls with
        | [] -> ans
        | x::tail -> 
            if ((snd x) = number) then find number ((fst x)::ans) tail
            else find number ans tail
    find number [] ls

let rec print ls = 
    match ls with
    | [] -> ()
    | x::tail -> printfn "%s : %s" <| fst x <| snd x; print tail

let printToFile ls path = 
    use fout = new System.IO.StreamWriter(File.OpenWrite(path))
    let rec print (fout:StreamWriter) ls=
        match ls with
        | [] -> ()
        | s::tail -> fout.WriteLine((fst s) + " " + (snd s)); print fout tail
    print fout ls

let readFromFile path = 
    if (File.Exists(path) = false) 
    then printfn "File %s don't exist" path; None
    else
        use fin = new System.IO.StreamReader(File.OpenRead(path))
        let rec read (fin:StreamReader) =
            if (fin.EndOfStream) 
            then []
            else
                let s : string[] = fin.ReadLine().Split(' ', '\n')
                if (s.Length < 2) then []
                else (s.[0], s.[1])::read fin
        Some <| read fin

let showHelp() = 
    printfn "0 - exit"
    printfn "1 - add a record"
    printfn "2 - find phone numbers by name"
    printfn "3 - find names by phone number"
    printfn "4 - print all records"
    printfn "5 - print all records to file"
    printfn "6 - read records from file"

let run() = 
    let rec handle ls=
        printf ">> "
        let t = readStr()
        match t with
        | "0" -> ()
        | "1" -> handle <| add ls
        | "2" -> List.iter (fun x -> printfn "%s" x) (findPhone ls (getInput "Enter a name: "))
                 handle ls
        | "3" -> List.iter (fun x -> printfn "%s" x) (findName ls (getInput "Enter a phone number: "))
                 handle ls
        | "4" -> print ls
                 handle ls
        | "5" -> printToFile ls (getInput "Enter file name: ")
                 handle ls
        | "6" -> match readFromFile(getInput "Enter file name: ") with
                    | Some(x) -> handle x
                    | _ -> handle ls
        | _ -> showHelp()
               handle ls
    showHelp()
    handle []
//=======================</task3>======================
(*_*)
[<EntryPoint>]
let main argv =
    run()
    0 // return an integer exit code
