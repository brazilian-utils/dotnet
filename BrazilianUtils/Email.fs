module BrazilianUtils.Email

open System
open System.Text.RegularExpressions

// Subconjunto prático do "valid e-mail address" do WHATWG HTML:
// parte local com letras, dígitos e "_'+-.", sem ponto nas pontas nem
// dois pontos seguidos; domínio com pelo menos um ponto, rótulos de até
// 63 caracteres e o rótulo final com 2 a 63 letras.
let private padraoParteLocal = @"[A-Za-z0-9_'+-]+(\.[A-Za-z0-9_'+-]+)*"
let private padraoRotuloDominio = @"[A-Za-z0-9](?:[A-Za-z0-9-]{0,61}[A-Za-z0-9])?"
let private padraoEmail =
    Regex(
        $"^{padraoParteLocal}@({padraoRotuloDominio}\\.)+[A-Za-z]{{2,63}}$",
        RegexOptions.Compiled)

/// Valida se um endereço de e-mail é válido (subconjunto prático do padrão
/// WHATWG HTML "valid e-mail address").
///
/// Examples:
///     IsValid "usuario@exemplo.com" = true
///     IsValid "" = false
let IsValid (value: string) : bool =
    if String.IsNullOrWhiteSpace value then
        false
    else
        let valor = value.Trim()
        valor.Length = value.Length && padraoEmail.IsMatch(valor)
