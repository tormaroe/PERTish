module PERTish.InputTests

open PERTish.Input

open NUnit.Framework
open FsUnit

[<Test>]
let ``parse single line of input`` ()=
    parseLine "A Test     item 1          2,2              3.3   " 
    |> should equal { Label = "A"
                      Text = "Test item"
                      Optimistic = 1.0m
                      Nominal = 2.2m
                      Pessimistic = 3.3m }