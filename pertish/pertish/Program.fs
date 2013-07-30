module PERTish.App

open System.IO
open PERTish.Input
open PERTish.Analysis
open PERTish.Charts

let header = "
  _____  _______  ______ _______ _____ _______ _     _
 |_____] |______ |_____/    |      |   |______ |_____|
 |       |______ |    \_    |    __|__ ______| |     |

         https://github.com/tormaroe/PERTish

"


[<EntryPoint>]
let main argv = 

    printf "%s" header

    printfn ".. Parsing file %s" argv.[0]
    let workItems = File.ReadLines(argv.[0]) |> parseWorkItems

    printfn ".. Calculating normal distribution"
    let distribution = getTotal workItems |> getDistribution

    printfn ""
    printReport workItems distribution

    showChart "Estimate: Probability" distribution.Probability
    showChart "Estimate: Cumulative probability" distribution.Cumulative

    printfn ".. Opening chart windows"
    printfn "\nClose a chart window to exit PERTish"

    System.Windows.Forms.Application.Run()
    0 // return an integer exit code