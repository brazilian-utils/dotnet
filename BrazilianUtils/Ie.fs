module BrazilianUtils.Ie

open System

/// Quantidade(s) de dígitos aceitas para a Inscrição Estadual de cada UF,
/// após remover os caracteres de máscara. Alguns estados aceitam mais de uma
/// quantidade (ex.: BA tem duas faixas de numeração histórica; PE, RO e TO
/// reservam um tamanho maior para produtor rural).
let private tamanhosPorUf =
    Map.ofList
        [ "AC", [ 13 ]
          "AL", [ 9 ]
          "AM", [ 9 ]
          "AP", [ 9 ]
          "BA", [ 8; 9 ]
          "CE", [ 9 ]
          "DF", [ 13 ]
          "ES", [ 9 ]
          "GO", [ 9 ]
          "MA", [ 9 ]
          "MG", [ 13 ]
          "MS", [ 9 ]
          "MT", [ 11 ]
          "PA", [ 9 ]
          "PB", [ 9 ]
          "PE", [ 9; 14 ]
          "PI", [ 9 ]
          "PR", [ 10 ]
          "RJ", [ 8 ]
          "RN", [ 9; 10 ]
          "RO", [ 9; 14 ]
          "RR", [ 9 ]
          "RS", [ 10 ]
          "SC", [ 9 ]
          "SE", [ 9 ]
          "SP", [ 12 ]
          "TO", [ 9; 11 ] ]

let private separadoresAceitos = set [ ' '; '.'; '-'; '/' ]

/// Remove os caracteres de máscara tolerados (espaço, ponto, hífen e barra),
/// mantendo o restante como está (inclusive letras, para o caso do produtor
/// rural de SP).
let private limparMascara (value: string) : string =
    value |> String.filter (fun c -> not (separadoresAceitos.Contains c))

/// Valida a Inscrição Estadual de forma puramente estrutural: apenas a
/// quantidade de dígitos esperada por UF, sem dígito verificador.
///
/// A Receita/SINTEGRA não publica um algoritmo de dígito verificador único:
/// são 27 tabelas estaduais distintas (cada uma com seu próprio módulo e
/// pesos), o contrato deste projeto não fornece nenhum caso de teste com
/// dígitos verificadores reais, e não há como verificá-los de forma
/// confiável nesta biblioteca. Por isso esta função verifica apenas a
/// estrutura (quantidade de dígitos por estado), da mesma forma que a versão
/// em Go — as versões em Rust e Ruby estão sendo alinhadas ao mesmo critério.
///
/// - `state` é a UF (sigla de 2 letras); desconhecida, devolve `false`.
/// - São toleradas como máscara: espaço, ponto, hífen e barra.
/// - Caso especial: em SP, um valor começando com `P`/`p` é a inscrição de
///   produtor rural, válida apenas com 13 caracteres depois de limpar a
///   máscara (verificado antes de qualquer checagem de "só dígitos", já que
///   `P` não é um dígito).
/// - Fora desse caso, qualquer caractere que não seja dígito invalida o
///   valor.
let IsValid (value: string) (state: string) : bool =
    if isNull state then
        false
    else
        let uf = state.Trim().ToUpperInvariant()
        match tamanhosPorUf |> Map.tryFind uf with
        | None -> false
        | Some tamanhosAceitos ->
            let valorLimpo = if isNull value then "" else limparMascara value
            if valorLimpo = "" then
                false
            elif uf = "SP" && (valorLimpo.[0] = 'P' || valorLimpo.[0] = 'p') then
                valorLimpo.Length = 13
            elif valorLimpo |> Seq.forall Char.IsDigit then
                tamanhosAceitos |> List.contains valorLimpo.Length
            else
                false
