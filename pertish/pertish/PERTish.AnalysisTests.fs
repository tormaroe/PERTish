module PERTish.AnalysisTests

open PERTish.Analysis

open NUnit.Framework
open FsUnit

[<Test>]
let ``calculate expected estimate`` ()=
    expectedEstimate 1.0m 3.0m 6.0m |> should equal 3.2m
    
[<Test>]
let ``calculate standard deviation`` ()=
    standardDeviation 4.0m 12.0m |> should equal 1.3m
