module PERTish.App

open System.IO
open PERTish.Input
open PERTish.Analysis
open PERTish.Charts


[<EntryPoint>]
let main argv = 

    printfn ".. Parsing file %s" argv.[0]
    let workItems = File.ReadLines(argv.[0]) |> parseWorkItems

    printfn ".. Calculating normal distribution"
    let distribution = getTotal workItems |> getDistribution

    printfn ""
    printReport workItems distribution

    showChart "Estimate: Probability" distribution.Probability
    showChart "Estimate: Cumulative probability" distribution.Cumulative

    System.Windows.Forms.Application.Run()
    0 // return an integer exit code