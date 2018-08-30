namespace BrazilianUtils
{
    public static class Cnpj
    {
        public static bool IsValid(string sourceCnpj, bool isRequired = true)
        {

            if (isRequired && string.IsNullOrEmpty(sourceCnpj))
                return false;

            if (!isRequired && string.IsNullOrEmpty(sourceCnpj))
                return true;

            var multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            var posicao = 0;
            var totalDigito1 = 0;
            var totalDigito2 = 0;
            var digitoVerificador1 = 0;
            var digitoVerificador2 = 0;

            bool digitosIdenticos = true;
            var ultimoDigito = -1;

            foreach (var c in sourceCnpj)
            {
                if (char.IsDigit(c))
                {
                    var digito = c - '0';
                    if (posicao != 0 && ultimoDigito != digito)
                    {
                        digitosIdenticos = false;
                    }

                    ultimoDigito = digito;
                    if (posicao < 12)
                    {
                        totalDigito1 += digito * multiplicador1[posicao];
                        totalDigito2 += digito * multiplicador2[posicao];
                    }
                    else if (posicao == 12)
                    {
                        digitoVerificador1 = digito;
                        totalDigito2 += digito * multiplicador2[posicao];
                    }
                    else if (posicao == 13)
                    {
                        digitoVerificador2 = digito;
                    }

                    posicao++;
                }
            }

            if (posicao > 14)
                return false;

            if (digitosIdenticos)
                return false;

            var digito1 = totalDigito1 % 11;
            digito1 = digito1 < 2
                ? 0
                : 11 - digito1;

            if (digitoVerificador1 != digito1)
                return false;

            var digito2 = totalDigito2 % 11;
            digito2 = digito2 < 2
                ? 0
                : 11 - digito2;

            if (digitoVerificador2 != digito2)
                return false;

            return true;
        }
    }
}