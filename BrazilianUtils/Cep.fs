module BrazilianUtils.Cep

open BrazilianUtils
open BrazilianUtils

let private hasCepLength =
    let cepLength = 8;
    Helpers.hasLength cepLength

let IsValid cep =
    let clearValue = Helpers.stringToIntList cep
    hasCepLength clearValue
