module BrazilianUtils.Vin

open System

/// Transliteração de um caractere do VIN para seu valor numérico (regra
/// norte-americana). `I`, `O` e `Q` não são permitidos.
let private valorCaractere (c: char) : int option =
    match Char.ToUpper c with
    | d when d >= '0' && d <= '9' -> Some (int d - int '0')
    | 'A' -> Some 1
    | 'B' -> Some 2
    | 'C' -> Some 3
    | 'D' -> Some 4
    | 'E' -> Some 5
    | 'F' -> Some 6
    | 'G' -> Some 7
    | 'H' -> Some 8
    | 'J' -> Some 1
    | 'K' -> Some 2
    | 'L' -> Some 3
    | 'M' -> Some 4
    | 'N' -> Some 5
    | 'P' -> Some 7
    | 'R' -> Some 9
    | 'S' -> Some 2
    | 'T' -> Some 3
    | 'U' -> Some 4
    | 'V' -> Some 5
    | 'W' -> Some 6
    | 'X' -> Some 7
    | 'Y' -> Some 8
    | 'Z' -> Some 9
    | _ -> None

let private pesos = [| 8; 7; 6; 5; 4; 3; 2; 10; 0; 9; 8; 7; 6; 5; 4; 3; 2 |]

/// Valida estruturalmente um VIN (Vehicle Identification Number / chassi):
/// 17 caracteres, nenhuma das letras excluídas `I`, `O`, `Q`, e o dígito
/// verificador na posição 9 (regra norte-americana), sem diferenciar maiúsculas
/// de minúsculas.
///
/// Um VIN cujos caracteres são todos iguais é rejeitado mesmo quando o dígito
/// verificador confere. As regras brasileiras não exigem esse dígito
/// verificador, então muitos chassis fabricados no Brasil falham aqui.
let IsValid (value: string) : bool =
    if isNull value || value.Length <> 17 then
        false
    else
        let maiusculo = value.ToUpper()
        if maiusculo |> Seq.exists (fun c -> c = 'I' || c = 'O' || c = 'Q') then
            false
        elif maiusculo |> Seq.distinct |> Seq.length = 1 then
            false
        else
            let valores = maiusculo |> Seq.map valorCaractere |> Seq.toArray
            if valores |> Array.exists Option.isNone then
                false
            else
                let soma =
                    Array.zip valores pesos
                    |> Array.sumBy (fun (v, p) -> (Option.get v) * p)
                let resto = soma % 11
                let digitoEsperado = if resto = 10 then "X" else string resto
                string maiusculo.[8] = digitoEsperado
