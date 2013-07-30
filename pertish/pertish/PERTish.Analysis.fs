module PERTish.Analysis

open System
open PERTish.Input
open NormalDistribution

type EstimationTotal = {
    Optimistic: decimal
    Pessimistic: decimal
    Nominal: decimal
    Expected: decimal
    Variance: decimal }

let roundToOneDecimal d = Decimal.Round (d, 1)

(*
    expected time (TE): 
    the best estimate of the time required to accomplish a task, 
    accounting for the fact that things don't always proceed as normal 
    (the implication being that the expected time is the average time 
    the task would require if the task were repeated on a number of 
    occasions over an extended period of time).

    TE = (O + 4M + P) ÷ 6
*)
let expectedEstimate optimistic nominal pessimistic =
    (optimistic + (4m * nominal) + pessimistic) / 6m 
    |> roundToOneDecimal
    
let standardDeviation low high =
    (high - low) / 6m
    |> roundToOneDecimal

let variance stDev = stDev * stDev

let getTotal (items: WorkItem seq) =
    let expected (wi:WorkItem) = expectedEstimate wi.Optimistic wi.Nominal wi.Pessimistic
    let variance (wi:WorkItem) = standardDeviation wi.Optimistic wi.Pessimistic |> variance
    {
        Optimistic  = items |> Seq.sumBy (fun wi -> wi.Optimistic)
        Pessimistic = items |> Seq.sumBy (fun wi -> wi.Pessimistic)
        Nominal     = items |> Seq.sumBy (fun wi -> wi.Nominal)
        Expected    = items |> Seq.sumBy expected
        Variance    = items |> Seq.sumBy variance
    }

let trim n s =
    if String.length s > n
    then s.[0..(n-3)] |> sprintf "%s.."
    else s

type Distribution = {
    Probability: (float * float) seq
    Cumulative: (float * float) seq }

let getDistribution (estimate:EstimationTotal) = 
    let dist = NormalDist( mean = float estimate.Expected, 
                           var = Math.Sqrt (float estimate.Variance) )
    let prob = seq { 
        for x in (float estimate.Optimistic) .. 0.1 .. (float estimate.Pessimistic) -> 
            x, dist.PDF(x) * 100.0 }
    let cumu = seq {
        for x in (float estimate.Optimistic) .. 0.1 .. (float estimate.Pessimistic) ->
            x, dist.CDF(x) * 100.0 }
    { Probability = prob ; Cumulative = cumu }

let first (x,_) = x

let getPercentCertainty p dist =
    dist.Cumulative
    |> Seq.find (fun (_,y) -> (Convert.ToInt32 y) >= p)

let printReport (items: WorkItem seq) (dist: Distribution) =

    let printPercentage p =
        let est, cert = getPercentCertainty p dist
        printfn "| %.1f%% chance of DONE after..                  | %8.1f |" cert est

    let total = getTotal items
    printfn "------------------------------------------------------------"
    printfn "| %-2s | %-40s | %8s |" "" "WORK ITEM" "EXPECTED"
    printfn "------------------------------------------------------------"
    for item in items do
        printfn "| %-2s | %-40s | %8.1f |"
                item.Label
                (trim 40 item.Text)
                (expectedEstimate item.Optimistic item.Nominal item.Pessimistic)
    printfn "------------------------------------------------------------"
    printfn "| Optimistic estimate                           | %8.1f |" total.Optimistic
    printfn "| Nominal estimate                              | %8.1f |" total.Nominal
    printfn "| Pessimistic estimate                          | %8.1f |" total.Pessimistic
    printfn "------------------------------------------------------------"
    printfn "| +++ EXPECTED ESTIMATE +++                     | %8.1f |" total.Expected
    printfn "------------------------------------------------------------"
    printPercentage 25
    printPercentage 50
    printPercentage 75
    printPercentage 100
    printfn "------------------------------------------------------------"
