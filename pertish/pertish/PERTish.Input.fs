module PERTish.Input

open System

(*
    Source file example:

    A  Some text          2.2    4     8.5
    B  Some other task    3      4     7

    Delimiter can be tab or (multiple) space or both
*)


type WorkItem = {
    Label: string
    Text: string
    Optimistic: decimal
    Nominal: decimal
    Pessimistic: decimal }

let safeToDouble str =
    match Decimal.TryParse(str) with
    | true,  d -> d
    | false, _ -> 
        if str.Contains(",") 
        then str.Replace(',','.') 
        else str.Replace('.',',')
        |> Decimal.Parse

let parseLine (l:String) =
    let splitted = l.Split([|' ';'\t'|])
                   |> Array.filter (fun s -> s.Length > 0)
    let len = splitted.Length

    { Label         = splitted.[0]
      Text          = String.Join (" ", splitted.[1..(len - 4)])
      Optimistic    = splitted.[len - 3] |> safeToDouble
      Nominal       = splitted.[len - 2] |> safeToDouble
      Pessimistic   = splitted.[len - 1] |> safeToDouble }

let parseWorkItems ls = ls |> Seq.map parseLine