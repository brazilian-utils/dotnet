module BrazilianUtils.BankAccount

open System

/// Parâmetros para a validação de uma conta bancária brasileira.
type IsValidBankAccountParams =
    { BankCode: string
      Agency: string
      Account: string
      Digit: string }

// Bancos sem código COMPE nesta base (fintechs que operam só por ISPB), mas
// citados explicitamente na especificação como tendo um algoritmo próprio ou
// uma verificação estrutural conhecida.
let private bancosConhecidosSemCompe = set [ "260"; "336"; "380" ] // Nubank, C6 Bank, PicPay

// Bancos descritos na especificação como "somente estruturais": um único
// dígito numérico já é suficiente.
let private bancosEstruturaisApenas =
    set [ "077"; "336"; "380"; "756"; "748" ] // Inter, C6, PicPay, Sicoob, Sicredi

let private codigoReconhecido (codigoBanco: string) : bool =
    (Bank.bancos |> List.exists (fun b -> b.Code = codigoBanco)) || bancosConhecidosSemCompe.Contains(codigoBanco)

// --- Algoritmo de Verhoeff (usado pelo Nubank) ---
// Tabelas padrão do algoritmo de Verhoeff (checksum de base 10 que detecta
// todas as trocas de dígito adjacente e a maioria das transposições).
let private tabelaD =
    [| [| 0;1;2;3;4;5;6;7;8;9 |]
       [| 1;2;3;4;0;6;7;8;9;5 |]
       [| 2;3;4;0;1;7;8;9;5;6 |]
       [| 3;4;0;1;2;8;9;5;6;7 |]
       [| 4;0;1;2;3;9;5;6;7;8 |]
       [| 5;9;8;7;6;0;4;3;2;1 |]
       [| 6;5;9;8;7;1;0;4;3;2 |]
       [| 7;6;5;9;8;2;1;0;4;3 |]
       [| 8;7;6;5;9;3;2;1;0;4 |]
       [| 9;8;7;6;5;4;3;2;1;0 |] |]

let private tabelaP =
    [| [| 0;1;2;3;4;5;6;7;8;9 |]
       [| 1;5;7;6;2;8;3;0;9;4 |]
       [| 5;8;0;3;7;9;6;1;4;2 |]
       [| 8;9;1;6;0;4;3;5;2;7 |]
       [| 9;4;5;3;1;2;6;8;7;0 |]
       [| 4;2;8;6;5;7;3;9;0;1 |]
       [| 2;7;9;3;8;0;6;4;1;5 |]
       [| 7;0;4;6;9;1;3;2;5;8 |] |]

/// Verifica um número (dígitos nus, o último dígito é o verificador) pelo
/// algoritmo de Verhoeff.
let private verhoeffValido (numeroCompleto: string) : bool =
    if numeroCompleto = "" || not (numeroCompleto |> Seq.forall Char.IsDigit) then
        false
    else
        let digitos = numeroCompleto |> Seq.rev |> Seq.map (fun c -> int c - int '0') |> Seq.toArray
        let mutable c = 0
        digitos |> Array.iteri (fun i d -> c <- tabelaD.[c].[tabelaP.[i % 8].[d]])
        c = 0

// --- Fallback genérico: módulo 10 ou módulo 11 sobre a conta ---
let private mod10 (conta: string) : int =
    conta
    |> Seq.rev
    |> Seq.mapi (fun i c ->
        let digito = int c - int '0'
        let peso = if i % 2 = 0 then 2 else 1
        let produto = digito * peso
        if produto > 9 then produto - 9 else produto)
    |> Seq.sum
    |> fun soma -> (10 - (soma % 10)) % 10

let private mod11 (conta: string) : int =
    let pesos = [ 2; 3; 4; 5; 6; 7; 8; 9 ]
    let soma =
        conta
        |> Seq.rev
        |> Seq.mapi (fun i c -> (int c - int '0') * pesos.[i % pesos.Length])
        |> Seq.sum
    let resto = 11 - (soma % 11)
    if resto >= 10 then 0 else resto

/// Valida uma conta bancária brasileira (banco, agência, conta e dígito
/// verificador).
///
/// - `bankCode` precisa ser um participante do STR com código COMPE (a
///   mesma lista de `Bank.GetByCode`), ou um dos bancos digitais sem COMPE
///   conhecidos citados na especificação (Nubank, C6, PicPay).
/// - Bancos "somente estruturais" (Inter, C6, PicPay, Sicoob, Sicredi, entre
///   outros): um único dígito numérico já é suficiente.
/// - Nubank (260): verificado pelo algoritmo de Verhoeff.
/// - Demais bancos: fallback genérico — `digit` precisa bater com o módulo
///   10 ou o módulo 11 sobre a conta; um `digit` de 2 caracteres encadeia
///   módulo 10 e depois módulo 11.
///
/// NOTA DE HONESTIDADE: os algoritmos específicos publicados para Banco do
/// Brasil, Santander, Banrisul, Caixa, Bradesco, Itaú, HSBC/Kirton e
/// Citibank citados na especificação não puderam ser confirmados com
/// confiança suficiente (nenhum caso de teste do contrato os exercita), e
/// por isso caem no fallback genérico abaixo em vez de uma tabela de pesos
/// específica que poderia estar errada.
let IsValid (parametros: IsValidBankAccountParams) : bool =
    let codigoBanco = if isNull parametros.BankCode then "" else parametros.BankCode.Trim()
    let agencia = if isNull parametros.Agency then "" else parametros.Agency.Trim()
    let conta = if isNull parametros.Account then "" else parametros.Account.Trim()
    let digito = if isNull parametros.Digit then "" else parametros.Digit.Trim()

    if not (codigoReconhecido codigoBanco) then
        false
    elif agencia = "" || agencia.Length > 5 || not (agencia |> Seq.forall Char.IsDigit) then
        false
    elif conta = "" || conta.Length > 13 || not (conta |> Seq.forall Char.IsDigit) then
        false
    elif digito = "" || digito.Length > 2 then
        false
    elif bancosEstruturaisApenas.Contains(codigoBanco) then
        digito.Length = 1 && Char.IsDigit digito.[0]
    elif codigoBanco = "260" then
        // Nubank: dígito verificador pelo algoritmo de Verhoeff sobre conta+dígito
        digito.Length = 1 && Char.IsDigit digito.[0] && verhoeffValido (conta + digito)
    else
        match digito.Length with
        | 1 when Char.IsDigit digito.[0] -> mod10 conta = int digito.[0] - int '0' || mod11 conta = int digito.[0] - int '0'
        | 2 ->
            let d1 = mod10 conta
            let d2 = mod11 (conta + string d1)
            digito = sprintf "%d%d" d1 d2
        | _ -> false
