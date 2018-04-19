// Learn more about F# at http://fsharp.org
module hw

open System
open System.IO

let read = bigint.Parse << Console.ReadLine
let readInt = Int32.Parse << Console.ReadLine
let readStr = Console.ReadLine
let rnd = System.Random()

//=======================<task1>=======================
let checkBranches ls =
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
    (check ls).Length = 0
//=======================</task1>======================

//=======================<task2>=======================
let func : int -> list<int> -> list<int> = 
    List.map << (*)
//=======================</task2>======================

//=======================<task3>=======================
let printTextAndReadStringAnswer text =
    printf "%s" text
    readStr()
let add ls = 
    (printTextAndReadStringAnswer "Enter a name: ", printTextAndReadStringAnswer "Enter a phone number: ")::ls

let findPhone ls = 
    let name = printTextAndReadStringAnswer "Enter a name: "
    let rec find name ls =
        match ls with
        | [] -> ()
        | x::tail -> 
            if ((fst x) = name) then printfn "%s" <| snd x
            find name tail
    find name ls

let findName ls = 
    let number = printTextAndReadStringAnswer "Enter a phone number: "
    let rec find number ls =
        match ls with
        | [] -> ()
        | x::tail -> 
            if ((snd x) = number) then printfn "%s" <| fst x 
            find number tail
    find number ls

let rec print ls = 
    match ls with
    | [] -> ()
    | x::tail -> printfn "%s : %s" <| fst x <| snd x; print tail

let printToFile ls = 
    let path = printTextAndReadStringAnswer "Enter file name: "
    use fout = new System.IO.StreamWriter(File.OpenWrite(path))
    let rec print (fout:StreamWriter) ls=
        match ls with
        | [] -> ()
        | s::tail -> fout.WriteLine((fst s) + " " + (snd s)); print fout tail
    print fout ls

let readFromFile() = 
    let path = printTextAndReadStringAnswer "Enter file name: "
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

let showHelp ()= 
    printfn "0 - exit"
    printfn "1 - add a record"
    printfn "2 - find phone numbers by name"
    printfn "3 - find names by phone number"
    printfn "4 - print all records"
    printfn "5 - print all records to file"
    printfn "6 - read records from file"
//=======================</task3>======================
(*_*)
[<EntryPoint>]
let main argv =
    let ls = ref []
    showHelp()
    while true do
        printf ">> "
        let t = readStr()
        match t with
        | "0" -> exit(0)
        | "1" -> ls := add !ls
        | "2" -> findPhone !ls
        | "3" -> findName !ls
        | "4" -> print !ls
        | "5" -> printToFile !ls
        | "6" -> match readFromFile() with
                 | Some(x) -> ls := x
                 | _ -> ()
        | _ -> showHelp()
    0 // return an integer exit code
