using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BrazilianUtils
{
    public static class Cpf
    {
        public static bool IsValid(string sourceCPF, bool isRequired = true)
        {

            if (isRequired && string.IsNullOrEmpty(sourceCPF))
                return false;

            if (!isRequired && string.IsNullOrEmpty(sourceCPF))
                return true;

            var posicao = 0;
            var totalDigito1 = 0;
            var totalDigito2 = 0;
            var digitoVerificador1 = 0;
            var digitoVerificador2 = 0;

            bool digitosIdenticos = true;
            var ultimoDigito = -1;

            foreach (var c in sourceCPF)
            {
                if (char.IsDigit(c))
                {
                    var digito = c - '0';
                    if (posicao != 0 && ultimoDigito != digito)
                    {
                        digitosIdenticos = false;
                    }

                    ultimoDigito = digito;
                    if (posicao < 9)
                    {
                        totalDigito1 += digito * (10 - posicao);
                        totalDigito2 += digito * (11 - posicao);
                    }
                    else if (posicao == 9)
                    {
                        digitoVerificador1 = digito;
                    }
                    else if (posicao == 10)
                    {
                        digitoVerificador2 = digito;
                    }

                    posicao++;
                }
            }

            if (posicao > 11)
                return false;

            if (digitosIdenticos)
                return false;

            var digito1 = totalDigito1 % 11;
            digito1 = digito1 < 2
                ? 0
                : 11 - digito1;

            if (digitoVerificador1 != digito1)
                return false;

            totalDigito2 += digito1 * 2;
            var digito2 = totalDigito2 % 11;
            digito2 = digito2 < 2
                ? 0
                : 11 - digito2;

            if (digitoVerificador2 != digito2)
                return false;

            return true;
        }

        public static string Format(string sourceCpf)
        {
            var clearCpf = Helpers.OnlyNumbers(sourceCpf);
            var sb = new StringBuilder(clearCpf);
            sb.Insert(3, ".").Insert(7, ".").Insert(11, "-");

            return sb.ToString();
        }
    }
}
