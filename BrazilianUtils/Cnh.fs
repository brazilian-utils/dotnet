module BrazilianUtils.Cnh

/// Gera o primeiro dígito verificador e o usa para conferir o 10º dígito da CNH.
/// Também devolve o resto bruto (antes do arredondamento para 0 quando > 9), necessário
/// para decidir se o segundo dígito verificador precisa ser decrementado.
let private checkFirstVerificator (digits: int list) (firstVerificator: int) : bool * int =
    let sum =
        digits
        |> List.take 9
        |> List.mapi (fun i digit -> digit * (9 - i))
        |> List.sum

    let remainder = sum % 11
    let result = if remainder > 9 then 0 else remainder

    result = firstVerificator, remainder

/// Gera o segundo dígito verificador e o usa para conferir o 11º dígito da CNH.
/// `firstRemainder` é o resto bruto (antes do arredondamento para 0) do primeiro
/// verificador: quando ele é > 9, o segundo verificador precisa ser decrementado em 2 (DSC = 2).
let private checkSecondVerificator (digits: int list) (secondVerificator: int) (firstRemainder: int) : bool =
    let sum =
        digits
        |> List.take 9
        |> List.mapi (fun i digit -> digit * (i + 1))
        |> List.sum

    let mutable result = sum % 11

    if firstRemainder > 9 then
        result <- if (result - 2) < 0 then result + 9 else result - 2

    if result > 9 then
        result <- 0

    result = secondVerificator

/// Validates the registration number for the Brazilian CNH (Carteira Nacional de Habilitação) that was created in 2022.
/// Previous versions of the CNH are not supported in this version.
/// This function checks if the given CNH is valid based on the format and allowed characters,
/// verifying the verification digits.
///
/// Examples:
///     isValidCnh "12345678901" = false
///     isValidCnh "A2C45678901" = false
///     isValidCnh "98765432100" = true
///     isValidCnh "987654321-00" = true
let isValidCnh (cnh: string) : bool =
    // Clean the input and check for numbers only
    let cleanedCnh = 
        cnh 
        |> Seq.filter System.Char.IsDigit 
        |> Seq.toArray 
        |> System.String
    
    if System.String.IsNullOrEmpty(cleanedCnh) then
        false
    elif cleanedCnh.Length <> 11 then
        false
    // Reject sequences as "00000000000", "11111111111", etc.
    elif cleanedCnh |> Seq.forall (fun c -> c = cleanedCnh.[0]) then
        false
    else
        // Cast digits to list of integers
        let digits = 
            cleanedCnh 
            |> Seq.map (fun c -> int c - int '0') 
            |> Seq.toList
        
        let firstVerificator = digits.[9]
        let secondVerificator = digits.[10]

        // Checking the 10th digit
        let (isFirstValid, firstRemainder) = checkFirstVerificator digits firstVerificator
        if not isFirstValid then
            false
        else
            // Checking the 11th digit
            checkSecondVerificator digits secondVerificator firstRemainder

