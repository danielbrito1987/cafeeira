using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estrutura.Extensoes
{
    public static class DecimalExtensionMethods
    {
        /// <summary>
        /// Formata um CPF com a máscara 000.000.000-00
        /// </summary>
        /// <param name="strCpfCnpj">string Cpf</param>
        /// <returns></returns>
        public static string ToCPFFormat(this decimal cpf)
        {
            MaskedTextProvider mtpCpf = new MaskedTextProvider(@"000\.000\.000-00");
            mtpCpf.Set(cpf.ToString().PadLeft(11, '0'));
            return mtpCpf.ToString();
        }

        /// <summary>
        /// Formata um CNPJ com a máscara 00.000.000/0000-00
        /// </summary>
        /// <param name="cnpj">string Cnpj</param>
        /// <returns></returns>
        public static string ToCNPJFormat(this decimal cnpj)
        {
            MaskedTextProvider mtpCnpj = new MaskedTextProvider(@"00\.000\.000/0000-00");
            mtpCnpj.Set(cnpj.ToString().PadLeft(14, '0'));
            return mtpCnpj.ToString();
        }

        public enum DescriptionNumberType
        {
            Number,
            Currency,
            Percent
        }

        /// <summary>
        /// Método para escrever números por extenso
        /// </summary>
        /// <param name="number">Número a ser escrito</param>
        /// <param name="descriptionNumberType">Tipo de conversão (Number, Currency ou Percent). Default = Number </param>
        /// <param name="firstIteraction">Controle interno de recursão. Não usá-lo.</param>
        /// <returns>Número por extenso</returns>
        public static string InWords(this decimal number, DescriptionNumberType descriptionNumberType = DescriptionNumberType.Number, bool firstIteraction = true)
        {
            Int64 integerPart = Convert.ToInt64(Math.Truncate(number));

            if (firstIteraction)
            {
                if (number > 999999999999999999.99M)
                    throw new Exception("Value is out of range.");

                string result = String.Empty;

                if (number == 0)
                    result = "zero " + (descriptionNumberType == DescriptionNumberType.Currency ? "real" : "por cento");

                if (number < 0)
                {
                    result = "valor negativo de";
                    number = Math.Abs(number);
                }

                firstIteraction = false;

                Int64 hundredthPart = Convert.ToInt64(Math.Truncate((number - integerPart) * 100));

                if (integerPart != 0)
                    result += InWords(integerPart, firstIteraction: firstIteraction) + (descriptionNumberType == DescriptionNumberType.Currency ? integerPart <= 1 ? " real " : " reais " : descriptionNumberType == DescriptionNumberType.Percent ? hundredthPart > 0 ? "" : " por cento" : "");

                if (hundredthPart > 0)
                    if (integerPart != 0)
                        result += (descriptionNumberType == DescriptionNumberType.Percent ? " vírgula " : ", ") + InWords(hundredthPart, firstIteraction: firstIteraction) + (descriptionNumberType == DescriptionNumberType.Currency ? hundredthPart <= 1 ? " centavo" : " centavos" : descriptionNumberType == DescriptionNumberType.Percent ? " por cento" : "");
                    else
                        result += (descriptionNumberType == DescriptionNumberType.Percent ? "zero vírgula " : descriptionNumberType == DescriptionNumberType.Number ? "zero e " : "") + InWords(hundredthPart, firstIteraction: firstIteraction) + (descriptionNumberType == DescriptionNumberType.Currency ? hundredthPart <= 1 ? " centavo" : " centavos" : descriptionNumberType == DescriptionNumberType.Percent ? " por cento" : "");

                result = result.Trim();

                int lastIdxComma;

                if ((lastIdxComma = result.LastIndexOf(',')) != -1)
                    result = result.Remove(lastIdxComma, 1).Insert(lastIdxComma, " e");

                while (result.Contains("  "))
                    result = result.Replace("  ", " ");

                if (result.EndsWith(" e"))
                    result = result.Remove(result.Length - 2);

                return result.Replace(" e reais", " de reais").Replace(" e por cento", " por cento").Replace(", reais", " de reais").Replace(", e ", " e ").Replace(" e vírgula ", " vírgula ");
            }

            Int64 remainder = 0;

            if (integerPart > 0 && integerPart < 20)
                return new string[] { String.Empty, "um", "dois", "três", "quatro", "cinco", "seis", "sete", "oito", "nove", "dez", "onze", "doze", "treze", "quatorze", "quinze", "dezesseis", "dezessete", "dezoito", "dezenove" }[integerPart];
            else if (integerPart < 100)
                return new string[] { String.Empty, "dez", "vinte", "trinta", "quarenta", "cinquenta", "sessenta", "setenta", "oitenta", "noventa" }[Math.DivRem(integerPart, 10, out remainder)] + (remainder == 0 ? " " : " e " + InWords(remainder, firstIteraction: firstIteraction));
            else if (integerPart < 1000)
            {
                if (integerPart % 100 == 0 || integerPart >= 200)
                    return new string[] { String.Empty, "cem", "duzentos", "trezentos", "quatrocentos", "quinhentos", "seiscentos", "setecentos", "oitocentos", "novecentos" }[Math.DivRem(integerPart, 100, out remainder)] + (remainder == 0 ? " " : " e " + InWords(remainder, firstIteraction: firstIteraction));
                else
                    return " cento e " + InWords(integerPart % 100, firstIteraction: firstIteraction);
            }
            else if (integerPart < 1000000)
                return InWords(Math.DivRem(integerPart, 1000, out remainder), firstIteraction: firstIteraction) + " mil, " + InWords(remainder, firstIteraction: firstIteraction);
            else if (integerPart < 1000000000)
                return InWords(Math.DivRem(integerPart, 1000000, out remainder), firstIteraction: firstIteraction) + (integerPart < 2000000 ? " milhão, " : " milhões, ") + InWords(remainder, firstIteraction: firstIteraction);
            else if (integerPart < 1000000000000)
                return InWords(Math.DivRem(integerPart, 1000000000, out remainder), firstIteraction: firstIteraction) + (integerPart < 2000000000 ? " bilhão, " : " bilhões, ") + InWords(remainder, firstIteraction: firstIteraction);
            else if (integerPart < 1000000000000000)
                return InWords(Math.DivRem(integerPart, 1000000000000, out remainder), firstIteraction: firstIteraction) + (integerPart < 2000000000000 ? " trilhão, " : " trilhões, ") + InWords(remainder, firstIteraction: firstIteraction);
            else if (integerPart < 1000000000000000000)
                return InWords(Math.DivRem(integerPart, 1000000000000000, out remainder), firstIteraction: firstIteraction) + (integerPart < 2000000000000000 ? " quadrilhão, " : " quadrilhões, ") + InWords(remainder, firstIteraction: firstIteraction);

            return String.Empty;
        }

        /// <summary>
        /// Método para escrever um valor monetário por extenso
        /// </summary>
        /// <returns>Valor monetário por extenso</returns>
        public static string ToCurrencyInWords(this decimal currency)
        {
            return currency.InWords(DescriptionNumberType.Currency);
        }

        /// <summary>
        /// Método para escrever um valor monetário por extenso
        /// </summary>
        /// <returns>Valor monetário por extenso</returns>
        public static string ToCurrencyInWords(this decimal? currency)
        {
            return currency.HasValue ? currency.Value.ToCurrencyInWords() : null;
        }

        /// <summary>
        /// Método para retornar o valor em formato monetário
        /// </summary>
        /// <returns>Valor em formato monetário</returns>
        public static string ToCurrency(this decimal currency)
        {
            return String.Format("{0:c}", currency);
        }

        /// <summary>
        /// Método para retornar o valor em formato monetário
        /// </summary>
        /// <returns>Valor em formato monetário</returns>
        public static string ToCurrency(this decimal? currency)
        {
            if (currency.HasValue)
                return currency.Value.ToCurrency();

            return null;
        }

        /// <summary>
        /// Método para retornar o valor em formato especificado com duas casas decimais e separador de milhar
        /// </summary>
        /// <returns>Valor com duas casas decimais e separador de milhar</returns>
        public static string ToFormat(this decimal currency)
        {
            return currency.ToFormat(2, true);
        }

        /// <summary>
        /// Método para retornar o valor em formato especificado com duas casas decimais e separador de milhar
        /// </summary>
        /// <returns>Valor com duas casas decimais e separador de milhar</returns>
        public static string ToFormat(this decimal? currency)
        {
            if (currency.HasValue)
                return currency.Value.ToFormat();

            return null;
        }

        /// <summary>
        /// Método para retornar o valor em formato especificado com casas decimais e separador de milhar
        /// </summary>
        /// <returns>Valor com casas decimais e separador de milhar</returns>
        public static string ToFormat(this decimal value, int decimalDigits)
        {
            //Rafael
            return value.ToFormat(decimalDigits, true);
        }



        /// <summary>
        /// Método para retornar o valor em formato especificado com 2 casas decimais e separador de milhar
        /// </summary>
        /// <returns>Valor com 2 casas decimais e separador de milhar</returns>
        public static string ToFormat(this decimal value, bool thousandSeparator)
        {
            //Rafael
            return value.ToFormat(2, thousandSeparator);
        }

        /// <summary>
        /// Método para retornar o valor em formato especificado com casas decimais e/ou separador de milhar
        /// </summary>
        /// <returns>Valor com casas decimais e/ou separador de milhar</returns>
        public static string ToFormat(this decimal value, int decimalPlaces, bool showThousandSeparator)
        {
            if (decimalPlaces < 0)
                throw new Exception("The decimal places can not be less than 0.");

            string decimalFormat = String.Empty;

            if (decimalPlaces != 0)
                decimalFormat = "." + new String('0', decimalPlaces);

            string decimalSeparator = ",";

            System.Globalization.CultureInfo _cultureInfo = decimalSeparator == "." ? System.Globalization.CultureInfo.InvariantCulture : new System.Globalization.CultureInfo("pt-BR", true);

            string result = value.ToString((showThousandSeparator ? "0," : String.Empty) + "0" + decimalFormat, _cultureInfo).TrimStart('0');

            if (result.StartsWith(decimalSeparator))
                result = "0" + result;

            return result.Trim();
        }

        public static string ToStringTruncating(this decimal number)
        {
            int integer = Decimal.ToInt32((decimal)number);
            if (number == integer)
                return integer.ToString();

            return number.ToString().TrimEnd('0');
        }

        public static string ToStringTruncating(this decimal? number)
        {
            if (number == null)
                return String.Empty;

            return ((decimal)number).ToStringTruncating();
        }


        public static decimal Truncate(this decimal number, int decimals = 2)
        {
            if (decimals < 0)
                decimals = 0;

            decimals = (int)System.Math.Pow(10, decimals);

            return Math.Truncate(number * decimals) / decimals;
        }

        public static decimal? Truncate(this decimal? number, int decimals = 2)
        {
            if (number == null)
                return null;

            return number.Value.Truncate(decimals: decimals);
        }
    }
}
