namespace BrazilianUtils.Tests

open System
open Xunit
open BrazilianUtils.Currency

module CurrencyFormatTests =

    [<Fact>]
    let ``formatCurrency when value is a decimal value`` () =
        let actual = formatCurrency 123236.70M
        Assert.Equal<string option>(Some "R$ 123.236,70", actual)

    [<Fact>]
    let ``formatCurrency when value is a float-like value converted to decimal`` () =
        let actual = formatCurrency (decimal 123236.70)
        Assert.Equal<string option>(Some "R$ 123.236,70", actual)

    [<Fact>]
    let ``formatCurrency when value is negative`` () =
        let actual = formatCurrency -123236.70M
        Assert.Equal<string option>(Some "R$ -123.236,70", actual)

    [<Fact>]
    let ``formatCurrency when value is zero`` () =
        let actual = formatCurrency 0.00M
        Assert.Equal<string option>(Some "R$ 0,00", actual)

    [<Fact>]
    let ``formatCurrency value decimal replace rounding`` () =
        let actual = formatCurrency -123236.7676M
        Assert.Equal<string option>(Some "R$ -123.236,77", actual)

module ConvertRealToTextTests =

    let private assertText expected actual =
        Assert.Equal<string option>(Some expected, actual)

    [<Fact>]
    let ``convertRealToText basic cases`` () =
        assertText "Zero reais" (convertRealToText 0.00M)
        assertText "Um centavo" (convertRealToText 0.01M)
        assertText "Cinquenta centavos" (convertRealToText 0.50M)
        assertText "Um real" (convertRealToText 1.00M)
        assertText "Menos cinquenta reais e vinte e cinco centavos" (convertRealToText -50.25M)
        assertText "Mil, quinhentos e vinte e três reais e quarenta e cinco centavos" (convertRealToText 1523.45M)
        assertText "Um milhão de reais" (convertRealToText 1000000.00M)
        assertText "Dois milhões de reais" (convertRealToText 2000000.00M)
        assertText "Um bilhão de reais" (convertRealToText 1000000000.00M)
        assertText "Dois bilhões de reais" (convertRealToText 2000000000.00M)
        assertText "Um trilhão de reais" (convertRealToText 1000000000000.00M)
        assertText "Dois trilhões de reais" (convertRealToText 2000000000000.00M)
        assertText "Um milhão de reais e quarenta e cinco centavos" (convertRealToText 1000000.45M)
        assertText "Dois bilhões de reais e noventa e nove centavos" (convertRealToText 2000000000.99M)
        assertText "Um bilhão, duzentos e trinta e quatro milhões, quinhentos e sessenta e sete mil, oitocentos e noventa reais e cinquenta centavos" (convertRealToText 1234567890.50M)

    [<Fact>]
    let ``convertRealToText almost zero values`` () =
        assertText "Zero reais" (convertRealToText 0.001M)
        assertText "Zero reais" (convertRealToText 0.009M)

    [<Fact>]
    let ``convertRealToText negative millions`` () =
        assertText "Menos um milhão de reais" (convertRealToText -1000000.00M)
        assertText "Menos dois milhões de reais e cinquenta centavos" (convertRealToText -2000000.50M)

    [<Fact>]
    let ``convertRealToText billions with cents`` () =
        assertText "Um bilhão de reais e um centavo" (convertRealToText 1000000000.01M)
        assertText "Um bilhão de reais e noventa e nove centavos" (convertRealToText 1000000000.99M)

    [<Fact>]
    let ``convertRealToText very large composed number`` () =
        assertText
            "Novecentos e noventa e nove bilhões, novecentos e noventa e nove milhões, novecentos e noventa e nove mil, novecentos e noventa e nove reais e noventa e nove centavos"
            (convertRealToText 999999999999.99M)

    [<Fact>]
    let ``convertRealToText trillions with cents`` () =
        assertText "Um trilhão de reais e um centavo" (convertRealToText 1000000000000.01M)
        assertText "Um trilhão de reais e noventa e nove centavos" (convertRealToText 1000000000000.99M)
        assertText
            "Nove trilhões, novecentos e noventa e nove bilhões, novecentos e noventa e nove milhões, novecentos e noventa e nove mil, novecentos e noventa e nove reais e noventa e nove centavos"
            (convertRealToText 9999999999999.99M)

    [<Fact>]
    let ``convertRealToText one quadrillion`` () =
        assertText "Um quatrilhão de reais" (convertRealToText 1000000000000000.00M)

    [<Fact>]
    let ``convertRealToText edge cases return None`` () =
        // Out of supported range -> None
        Assert.Equal< string option >(None, convertRealToText -1000000000000001.00M)
        Assert.Equal< string option >(None, convertRealToText 1000000000000001.00M)

        // Decimal.NaN / Infinity are not representable in decimal; we can exercise extreme overflow-like values instead
        // Use an obviously too-large value that should be rejected by the implementation
        let tooLarge = 79228162514264337593543950335.00M // ~Decimal.MaxValue-ish literal
        Assert.Equal<string option>(None, convertRealToText tooLarge)
