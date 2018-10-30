module BrazilianUtils.Phone

open BrazilianUtils

let private areaCodes =
    [ [ 1; 1 ]
      [ 1; 2 ]
      [ 1; 3 ]
      [ 1; 4 ]
      [ 1; 5 ]
      [ 1; 6 ]
      [ 1; 7 ]
      [ 1; 8 ]
      [ 1; 9 ]
      [ 2; 1 ]
      [ 2; 2 ]
      [ 2; 4 ]
      [ 2; 7 ]
      [ 2; 8 ]
      [ 3; 1 ]
      [ 3; 2 ]
      [ 3; 3 ]
      [ 3; 4 ]
      [ 3; 5 ]
      [ 3; 7 ]
      [ 3; 8 ]
      [ 4; 1 ]
      [ 4; 2 ]
      [ 4; 3 ]
      [ 4; 4 ]
      [ 4; 5 ]
      [ 4; 6 ]
      [ 4; 7 ]
      [ 4; 8 ]
      [ 4; 9 ]
      [ 5; 1 ]
      [ 5; 3 ]
      [ 5; 4 ]
      [ 5; 5 ]
      [ 6; 1 ]
      [ 6; 2 ]
      [ 6; 3 ]
      [ 6; 4 ]
      [ 6; 5 ]
      [ 6; 6 ]
      [ 6; 7 ]
      [ 6; 8 ]
      [ 6; 9 ]
      [ 7; 1 ]
      [ 7; 3 ]
      [ 7; 4 ]
      [ 7; 5 ]
      [ 7; 7 ]
      [ 7; 9 ]
      [ 8; 1 ]
      [ 8; 2 ]
      [ 8; 3 ]
      [ 8; 4 ]
      [ 8; 5 ]
      [ 8; 6 ]
      [ 8; 7 ]
      [ 8; 8 ]
      [ 8; 9 ]
      [ 9; 1 ]
      [ 9; 2 ]
      [ 9; 3 ]
      [ 9; 4 ]
      [ 9; 5 ]
      [ 9; 6 ]
      [ 9; 7 ]
      [ 9; 8 ]
      [ 9; 9 ] ]



let private mobilePhoneLength = 11

let private landlinePhoneLength = 10

let private hasLenght (phone : int list) =
    match phone.Length with
    | x when x = mobilePhoneLength -> true
    | x when x = landlinePhoneLength -> true
    | _ -> false

let private isValidDigit (phone : int list) =
    let isValidLandlineDigit digit =
        [ 2; 3; 4; 5 ] |> List.exists (fun x -> x = digit)
        
    let isValidMobileDigit digit =
        [ 6; 7; 8; 9 ] |> List.exists (fun x -> x = digit)

    match phone.Length with
    | x when x = mobilePhoneLength -> phone.[2] |> isValidMobileDigit
    | x when x = landlinePhoneLength -> phone.[2] |> isValidLandlineDigit
    | _ -> false

let private isValidCode (phone: int list) =
    areaCodes
    |> List.exists (fun x -> x.[0] = phone.[0] && x.[1] = phone.[1])

let IsValid phone =
    let clearValue = Helpers.stringToIntList phone
    [ hasLenght; isValidCode; isValidDigit ] |> Seq.forall (fun validator -> validator clearValue)
