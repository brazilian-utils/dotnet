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

let private paraTexto (value: obj) : string =
    match value with
    | :? string as s -> s
    | :? int as i -> string i
    | :? int64 as i -> string i
    | _ -> ""

/// Remove a formatação da CNH e mantém apenas dígitos, limitado a 11
/// dígitos.
let Parse (value: obj) : string =
    let apenasDigitos = paraTexto value |> Seq.filter System.Char.IsDigit |> Seq.toArray |> System.String
    if apenasDigitos.Length > 11 then apenasDigitos.Substring(0, 11) else apenasDigitos

/// Formata uma CNH no padrão `000000000-00`, aplicando a máscara até onde
/// os dígitos alcançarem.
let Format (value: obj) : string =
    let digitos = Parse value
    if digitos.Length > 9 then digitos.Substring(0, 9) + "-" + digitos.Substring(9) else digitos

/// Gera um número de CNH aleatório e válido (11 dígitos, sem formatação),
/// construído para satisfazer `isValidCnh` por construção.
let Generate () : string =
    let random = System.Random()
    let rec gerarBase () =
        let baseDigitos = List.init 9 (fun _ -> random.Next(0, 10))
        if baseDigitos |> List.distinct |> List.length = 1 then gerarBase () else baseDigitos

    let baseDigitos = gerarBase ()
    let soma1 = baseDigitos |> List.mapi (fun i digito -> digito * (9 - i)) |> List.sum
    let resto1 = soma1 % 11
    let primeiroDv = if resto1 > 9 then 0 else resto1

    let soma2 = baseDigitos |> List.mapi (fun i digito -> digito * (i + 1)) |> List.sum
    let mutable resto2 = soma2 % 11
    if resto1 > 9 then
        resto2 <- if (resto2 - 2) < 0 then resto2 + 9 else resto2 - 2
    if resto2 > 9 then
        resto2 <- 0

    (baseDigitos @ [ primeiroDv; resto2 ]) |> List.map string |> String.concat ""

