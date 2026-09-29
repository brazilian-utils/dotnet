module BrazilianUtils.Tests.PixPayloadTests

open BrazilianUtils
open Xunit

// Payload de controle, só com chave (sem URL), CRC correto: prova que a
// montagem dos payloads usados abaixo é válida (ou seja, que o teste
// seguinte falha pela regra certa, não por CRC/estrutura incorreta).
[<Fact>]
let payloadWithOnlyKeyShouldBeValid () =
    let payload = "00020101021126380014br.gov.bcb.pix0116user@example.com5204000053039865802BR5910LOJA TESTE6009SAO PAULO6304C50B"
    PixPayload.IsValid payload
    |> Assert.True

[<Fact>]
let payloadWithBothKeyAndUrlShouldBeInvalid () =
    // Regressão: `extrairCampos` só rejeitava quando chave e URL estavam
    // ambas ausentes, mas aceitava quando ambas estavam presentes ao mesmo
    // tempo (chave estática e URL dinâmica são mutuamente exclusivas).
    let payload = "00020101021226640014br.gov.bcb.pix0116user@example.com2522pix.example.com/abc1235204000053039865802BR5910LOJA TESTE6009SAO PAULO63045BBC"
    PixPayload.IsValid payload
    |> Assert.False
