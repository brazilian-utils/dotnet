namespace BrazilianUtils.Tests

open System
open Xunit
open BrazilianUtils.Currency

module CurrencyFormatTests =

    [<Fact>]
    let ``Format when value is a decimal value`` () =
        let actual = Format (box 123236.70M)
        Assert.Equal<string>("123.236,70", actual)

    [<Fact>]
    let ``Format when value is a float-like value converted to decimal`` () =
        let actual = Format (box (decimal 123236.70))
        Assert.Equal<string>("123.236,70", actual)

    [<Fact>]
    let ``Format when value is negative`` () =
        let actual = Format (box -123236.70M)
        Assert.Equal<string>("-123.236,70", actual)

    [<Fact>]
    let ``Format when value is zero`` () =
        let actual = Format (box 0.00M)
        Assert.Equal<string>("0,00", actual)

    [<Fact>]
    let ``Format value decimal replace rounding`` () =
        let actual = Format (box -123236.7676M)
        Assert.Equal<string>("-123.236,77", actual)

    [<Fact>]
    let ``Format accepts a dot-decimal string`` () =
        let actual = Format (box "1234.56")
        Assert.Equal<string>("1.234,56", actual)

    [<Fact>]
    let ``Format accepts an already Brazilian-formatted string`` () =
        let actual = Format (box "1.234,56")
        Assert.Equal<string>("1.234,56", actual)

    [<Fact>]
    let ``Format accepts a string with the R$ symbol`` () =
        let actual = Format (box "R$ 1.234,56")
        Assert.Equal<string>("1.234,56", actual)

    [<Fact>]
    let ``Format returns empty string for invalid input`` () =
        let actual = Format (box "not a number")
        Assert.Equal<string>("", actual)

module ParseCurrencyTests =

    let private arredondar (valor: float) = System.Math.Round(valor, 2)

    [<Fact>]
    let ``Parse with symbol`` () =
        Assert.Equal<float>(1234.56, arredondar (Parse "R$ 1.234,56"))

    [<Fact>]
    let ``Parse comma decimal`` () =
        Assert.Equal<float>(1234.56, arredondar (Parse "1234,56"))

    [<Fact>]
    let ``Parse digits only is read as cents`` () =
        Assert.Equal<float>(12.34, arredondar (Parse "1234"))

    [<Fact>]
    let ``Parse empty string is zero`` () =
        Assert.Equal<float>(0.0, arredondar (Parse ""))

module ConvertToWordsTests =

    let private assertText (expected: string) (actual: string) =
        Assert.Equal<string>(expected, actual)

    [<Fact>]
    let ``ConvertToWords basic cases`` () =
        assertText "zero reais" (ConvertToWords 0.00M)
        assertText "um centavo" (ConvertToWords 0.01M)
        assertText "cinquenta centavos" (ConvertToWords 0.50M)
        assertText "um real" (ConvertToWords 1.00M)
        assertText "menos cinquenta reais e vinte e cinco centavos" (ConvertToWords -50.25M)
        assertText "mil quinhentos e vinte e três reais e quarenta e cinco centavos" (ConvertToWords 1523.45M)
        assertText "um milhão de reais" (ConvertToWords 1000000.00M)
        assertText "dois milhões de reais" (ConvertToWords 2000000.00M)
        assertText "um bilhão de reais" (ConvertToWords 1000000000.00M)
        assertText "dois bilhões de reais" (ConvertToWords 2000000000.00M)
        assertText "um trilhão de reais" (ConvertToWords 1000000000000.00M)
        assertText "dois trilhões de reais" (ConvertToWords 2000000000000.00M)
        assertText "um milhão de reais e quarenta e cinco centavos" (ConvertToWords 1000000.45M)
        assertText "dois bilhões de reais e noventa e nove centavos" (ConvertToWords 2000000000.99M)
        assertText
            "um bilhão duzentos e trinta e quatro milhões quinhentos e sessenta e sete mil oitocentos e noventa reais e cinquenta centavos"
            (ConvertToWords 1234567890.50M)

    [<Fact>]
    let ``ConvertToWords almost zero values`` () =
        assertText "zero reais" (ConvertToWords 0.001M)
        assertText "zero reais" (ConvertToWords 0.009M)

    [<Fact>]
    let ``ConvertToWords negative millions`` () =
        assertText "menos um milhão de reais" (ConvertToWords -1000000.00M)
        assertText "menos dois milhões de reais e cinquenta centavos" (ConvertToWords -2000000.50M)

    [<Fact>]
    let ``ConvertToWords billions with cents`` () =
        assertText "um bilhão de reais e um centavo" (ConvertToWords 1000000000.01M)
        assertText "um bilhão de reais e noventa e nove centavos" (ConvertToWords 1000000000.99M)

    [<Fact>]
    let ``ConvertToWords very large composed number`` () =
        assertText
            "novecentos e noventa e nove bilhões novecentos e noventa e nove milhões novecentos e noventa e nove mil novecentos e noventa e nove reais e noventa e nove centavos"
            (ConvertToWords 999999999999.99M)

    [<Fact>]
    let ``ConvertToWords trillions with cents`` () =
        assertText "um trilhão de reais e um centavo" (ConvertToWords 1000000000000.01M)
        assertText "um trilhão de reais e noventa e nove centavos" (ConvertToWords 1000000000000.99M)
        assertText
            "nove trilhões novecentos e noventa e nove bilhões novecentos e noventa e nove milhões novecentos e noventa e nove mil novecentos e noventa e nove reais e noventa e nove centavos"
            (ConvertToWords 9999999999999.99M)

    [<Fact>]
    let ``ConvertToWords rejects amounts above 999 trillion reais`` () =
        // Acima do limite de 999 trilhões de reais o resultado é uma string vazia
        assertText "" (ConvertToWords 1000000000000000.00M)

    [<Fact>]
    let ``ConvertToWords edge cases return empty string`` () =
        assertText "" (ConvertToWords -1000000000000001.00M)
        assertText "" (ConvertToWords 1000000000000001.00M)

        let tooLarge = 79228162514264337593543950335.00M // ~Decimal.MaxValue-ish literal
        assertText "" (ConvertToWords tooLarge)
