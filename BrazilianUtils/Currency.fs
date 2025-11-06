module BrazilianUtils.Currency

open System
open System.Globalization

/// Format a numeric value as Brazilian currency (R$).
///
/// Examples:
///     formatCurrency 1234.56M = Some "R$ 1.234,56"
///     formatCurrency 0M = Some "R$ 0,00"
///     formatCurrency -9876.54M = Some "R$ -9.876,54"
let formatCurrency (value: decimal) : string option =
    try
        let formattedValue = 
            sprintf "R$ %.2f" value
            |> fun s -> s.Replace(",", "_")
            |> fun s -> s.Replace(".", ",")
            |> fun s -> s.Replace("_", ".")
        Some formattedValue
    with
    | :? FormatException
    | :? OverflowException -> None

/// Convert a number to its textual representation in Brazilian Portuguese
let private numberToPortuguese (n: int64) : string =
    let units = [| ""; "um"; "dois"; "três"; "quatro"; "cinco"; "seis"; "sete"; "oito"; "nove" |]
    let teens = [| "dez"; "onze"; "doze"; "treze"; "quatorze"; "quinze"; "dezesseis"; "dezessete"; "dezoito"; "dezenove" |]
    let tens = [| ""; ""; "vinte"; "trinta"; "quarenta"; "cinquenta"; "sessenta"; "setenta"; "oitenta"; "noventa" |]
    let hundreds = [| ""; "cento"; "duzentos"; "trezentos"; "quatrocentos"; "quinhentos"; "seiscentos"; "setecentos"; "oitocentos"; "novecentos" |]
    
    let rec convertBelow1000 (n: int) : string =
        if n = 0 then ""
        elif n = 100 then "cem"
        elif n < 10 then units.[n]
        elif n < 20 then teens.[n - 10]
        elif n < 100 then
            let ten = n / 10
            let unit = n % 10
            if unit = 0 then tens.[ten]
            else sprintf "%s e %s" tens.[ten] units.[unit]
        else
            let hundred = n / 100
            let rest = n % 100
            if rest = 0 then hundreds.[hundred]
            else sprintf "%s e %s" hundreds.[hundred] (convertBelow1000 rest)
    
    let rec convert (n: int64) (scale: int) : string =
        if n = 0L then ""
        else
            let scaleNames = [| ""; "mil"; "milhão"; "bilhão"; "trilhão" |]
            let scalePlurals = [| ""; "mil"; "milhões"; "bilhões"; "trilhões" |]
            let divisor = pown 1000L scale
            let quotient = n / divisor
            let remainder = n % divisor
            
            if quotient = 0L then convert n (scale - 1)
            else
                let currentPart = int (quotient % 1000L)
                let higherParts = quotient / 1000L
                
                let partText = convertBelow1000 currentPart
                let scaleName = 
                    if scale = 0 then ""
                    elif scale = 1 then " mil"
                    elif currentPart = 1 then sprintf " %s" scaleNames.[scale]
                    else sprintf " %s" scalePlurals.[scale]
                
                let currentText = 
                    if String.IsNullOrEmpty partText then ""
                    else partText + scaleName
                
                let remainderText = convert remainder (scale - 1)
                
                let higherText = 
                    if higherParts > 0L then convert (higherParts * 1000L) scale
                    else ""
                
                let connector = 
                    if not (String.IsNullOrEmpty remainderText) && remainder < 100L && remainder > 0L then " e "
                    elif not (String.IsNullOrEmpty remainderText) then ", "
                    else ""
                
                if String.IsNullOrEmpty higherText then
                    currentText + connector + remainderText
                else
                    higherText + ", " + currentText + connector + remainderText
    
    if n = 0L then "zero"
    else
        // Determine maximum scale
        let mutable maxScale = 0
        let mutable temp = n
        while temp >= 1000L do
            temp <- temp / 1000L
            maxScale <- maxScale + 1
        
        convert n maxScale

/// Convert a monetary value in Brazilian Reais to textual representation.
///
/// Note:
///     - Values are rounded down to 2 decimal places
///     - Maximum supported value is 1 quadrillion reais
///     - Negative values are prefixed with "Menos"
///
/// Examples:
///     convertRealToText 1523.45M = Some "Mil, quinhentos e vinte e três reais e quarenta e cinco centavos"
///     convertRealToText 1.00M = Some "Um real"
///     convertRealToText 0.50M = Some "Cinquenta centavos"
///     convertRealToText 0.00M = Some "Zero reais"
let convertRealToText (amount: decimal) : string option =
    try
        // Round down to 2 decimal places
        let roundedAmount = Math.Floor(amount * 100M) / 100M
        
        if roundedAmount <> roundedAmount then None // NaN check
        elif abs roundedAmount > 1000000000000000.00M then None // 1 quadrillion
        else
            let negative = roundedAmount < 0M
            let absAmount = abs roundedAmount
            
            let reais = int64 (Math.Floor(absAmount))
            let centavos = int ((absAmount - decimal reais) * 100M)
            
            let parts = ResizeArray<string>()
            
            if reais > 0L then
                let reaisText = numberToPortuguese reais
                let currencyText = if reais = 1L then "real" else "reais"
                let connector = if reaisText.EndsWith("lhão") || reaisText.EndsWith("lhões") then "de " else ""
                parts.Add(sprintf "%s %s%s" reaisText connector currencyText)
            
            if centavos > 0 then
                let centavosText = numberToPortuguese (int64 centavos)
                let centavoWord = if centavos = 1 then "centavo" else "centavos"
                if reais > 0L then
                    parts.Add(sprintf "e %s %s" centavosText centavoWord)
                else
                    parts.Add(sprintf "%s %s" centavosText centavoWord)
            
            if reais = 0L && centavos = 0 then
                parts.Add("Zero reais")
            
            let result = String.Join(" ", parts)
            let finalResult = if negative then sprintf "Menos %s" result else result
            
            // Capitalize first letter
            let capitalized = 
                if String.IsNullOrEmpty finalResult then finalResult
                else Char.ToUpper(finalResult.[0]).ToString() + finalResult.Substring(1)
            
            Some capitalized
    with
    | :? InvalidOperationException
    | :? OverflowException
    | :? ArgumentException -> None
