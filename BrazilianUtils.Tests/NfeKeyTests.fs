module BrazilianUtils.Tests.NfeKeyTests

open BrazilianUtils
open Xunit

// Todas as chaves abaixo têm 44 dígitos, dígito verificador correto e demais
// campos estruturalmente válidos; só o código numérico (`cNF`, os 8 dígitos
// antes do DV) e/ou o modelo variam, para exercitar isoladamente a regra
// B03-10 do MOC.

[<Theory>]
// Modelo 55 (NF-e): cNF todo repetido.
[<InlineData("35240912345678000199550010000000011111111117")>]
// Modelo 55 (NF-e): cNF sequencial crescente.
[<InlineData("35240912345678000199550010000000091012345673")>]
// Modelo 55 (NF-e): cNF sequencial decrescente.
[<InlineData("35240912345678000199550010000000091876543217")>]
// Modelo 55 (NF-e): cNF igual ao nNF (mesmo valor numérico).
[<InlineData("35240912345678000199550010000001231000001235")>]
let shouldBeInvalidByCodigoNumericoRule (chave: string) =
    NfeKey.IsValid chave
    |> Assert.False

[<Fact>]
let shouldStillBeValidForModelOutsideNfeAndNfce () =
    // Regressão: a regra B03-10 só vale para os modelos 55 (NF-e) e 65
    // (NFC-e). Uma chave de CT-e (modelo 57) com cNF todo repetido continua
    // válida.
    NfeKey.IsValid "35240912345678000199570010000000011111111114"
    |> Assert.True
