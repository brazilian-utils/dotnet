module BrazilianUtils.PixKey

open System
open System.Text.RegularExpressions
open BrazilianUtils

type PixKeyInfo =
    { Type: string
      Value: string }

let private padraoUuid =
    Regex(@"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$")

/// Identifica uma chave Pix e a normaliza para a forma canônica esperada
/// pelo DICT dentro de um BR Code; devolve None quando o valor não é uma
/// chave Pix válida.
///
/// - Um valor de 11 dígitos válido como CPF e como celular é um CPF, a não
///   ser que esteja escrito como telefone (prefixo `+55` ou DDD entre
///   parênteses).
/// - Telefones fixos não são chaves Pix; um e-mail com mais de 77
///   caracteres é rejeitado.
let GetInfo (value: string) : PixKeyInfo option =
    if String.IsNullOrWhiteSpace value then
        None
    else
        let valorAparado = value.Trim()
        let apenasDigitos = valorAparado |> Seq.filter Char.IsDigit |> Seq.toArray |> String
        let pareceTelefoneEscrito = valorAparado.Contains("+") || valorAparado.Contains("(")

        if padraoUuid.IsMatch(valorAparado) then
            Some { Type = "evp"; Value = valorAparado.ToLowerInvariant() }
        elif valorAparado.Contains("@") then
            if valorAparado.Length <= 77 && Email.IsValid valorAparado then
                Some { Type = "email"; Value = valorAparado.ToLowerInvariant() }
            else
                None
        elif apenasDigitos.Length = 14 && Cnpj.IsValid valorAparado then
            Some { Type = "cnpj"; Value = apenasDigitos }
        elif apenasDigitos.Length = 11 && not pareceTelefoneEscrito && Cpf.IsValid valorAparado then
            Some { Type = "cpf"; Value = apenasDigitos }
        elif Phone.IsValidMobile valorAparado then
            let semCodigoPais = Phone.Parse valorAparado
            let numero = if semCodigoPais.Length > 11 then semCodigoPais.Substring(semCodigoPais.Length - 11) else semCodigoPais
            Some { Type = "phone"; Value = "+55" + numero }
        else
            None

/// Valida uma chave Pix (CPF, CNPJ, e-mail, celular brasileiro ou chave
/// aleatória EVP), conforme os formatos do DICT.
///
/// `allowedTypes` restringe os tipos aceitos (`cpf`, `cnpj`, `email`,
/// `phone`, `evp`); uma lista vazia rejeita tudo.
let IsValid (value: string) (allowedTypes: string list option) : bool =
    match GetInfo value with
    | None -> false
    | Some info ->
        match allowedTypes with
        | None -> true
        | Some permitidos -> List.contains info.Type permitidos
