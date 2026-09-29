module BrazilianUtils.CreditCard

open System

/// Aplica o algoritmo de Luhn sobre a lista de dígitos (da esquerda para a
/// direita) e devolve se o total é múltiplo de 10.
let private algoritmoLuhn (digitos: int list) : bool =
    digitos
    |> List.rev
    |> List.mapi (fun i digito ->
        if i % 2 = 1 then
            let dobrado = digito * 2
            if dobrado > 9 then dobrado - 9 else dobrado
        else
            digito)
    |> List.sum
    |> fun soma -> soma % 10 = 0

/// Valida um número de cartão de pagamento (crédito ou débito) pelo
/// algoritmo de Luhn: 12 a 19 dígitos e dígito verificador correto.
///
/// Não faz detecção de bandeira, faixa de emissor, validade ou CVV. Um
/// número cujos dígitos são todos iguais é rejeitado mesmo passando no Luhn.
///
/// Aceita string ou número, com espaço, '.', '-' e '/' entre os dígitos;
/// letras tornam o valor inválido.
let IsValid (value: obj) : bool =
    let texto =
        match value with
        | null -> ""
        | :? string as s -> s
        | v -> Convert.ToString(v, Globalization.CultureInfo.InvariantCulture)

    if String.IsNullOrEmpty texto then
        false
    else
        let temCaractereInvalido =
            texto |> Seq.exists (fun c -> not (Char.IsDigit c) && not (" .-/".Contains(c)))

        if temCaractereInvalido then
            false
        else
            let apenasDigitos = texto |> Seq.filter Char.IsDigit |> Seq.toArray |> String
            if apenasDigitos.Length < 12 || apenasDigitos.Length > 19 then
                false
            elif apenasDigitos |> Seq.distinct |> Seq.length = 1 then
                false
            else
                apenasDigitos
                |> Seq.map (fun c -> int c - int '0')
                |> Seq.toList
                |> algoritmoLuhn
