module BrazilianUtils.Helpers

open System

let internal isNotRepdigit (value : int list) =
    value
    |> Seq.forall (fun elem -> elem = value.[0])
    |> not

let internal charToInt (c : char) =
    int c - int '0'

let internal hasValue value =
    Seq.length value <> 0

let internal calculateModulus11 weights (value : int list) =
    weights
    |> List.mapi (fun i weight -> value.[i] * weight)
    |> List.sum
    |> (fun x -> x % 11)

let internal generateRandomNumbers count =
    let rnd = System.Random()
    List.init count (fun _ -> rnd.Next(0, 9))

let internal hasLength length value =
    List.length value = length

let OnlyNumbers value =
    value
    |> String.filter Char.IsDigit

let internal stringToIntList value =
    value
    |> OnlyNumbers
    |> Seq.map charToInt
    |> Seq.toList
