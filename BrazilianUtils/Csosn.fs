module BrazilianUtils.Csosn

/// Os 10 códigos da tabela oficial de CSOSN (Ajuste SINIEF 07/2005).
let private codigosValidos =
    set [ "101"; "102"; "103"; "201"; "202"; "203"; "300"; "400"; "500"; "900" ]

/// Valida se um código CSOSN (Código de Situação da Operação no Simples
/// Nacional) existe na tabela oficial. Um CSOSN não tem separador impresso,
/// então um valor mascarado é rejeitado.
///
/// Examples:
///     IsValid "101" = true
///     IsValid "999" = false
let IsValid (value: string) : bool =
    if isNull value then false else codigosValidos.Contains(value.Trim())
