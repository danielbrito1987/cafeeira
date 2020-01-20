using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Estrutura.Extensoes
{
    public static class StringExtensionMethods
    {
        /// <summary>
        /// Formata um CPF com a máscara 000.000.000-00
        /// </summary>
        /// <param name="cpf">string Cpf</param>
        /// <returns></returns>
        public static string ToCPFFormat(this string cpf)
        {
            if (!cpf.IsEmpty())
            {
                cpf = cpf.Trim();

                MaskedTextProvider mtpCpf = new MaskedTextProvider(@"000\.000\.000-00");
                mtpCpf.Set(cpf.PadLeft(11, '0'));
                return mtpCpf.ToString();
            }
            return null;
        }

        /// <summary>
        /// Remove a formatação de CPF
        /// </summary>
        /// <param name="cpf">string Cpf</param>
        /// <returns></returns>
        public static string ClearCPFFormat(this string cpf)
        {
            if (!cpf.IsEmpty())
            {
                cpf = cpf.Trim();

                return cpf.ToString().Replace(".", "").Replace("-", "");
            }
            return null;
        }

        public static string ToPhoneNumberFormat(this string phoneNumber)
        {
            if (!phoneNumber.IsEmpty())
            {
                phoneNumber = phoneNumber.Trim();

                MaskedTextProvider mtpPhone1 = new MaskedTextProvider(@"(00) 00000-0000");
                MaskedTextProvider mtpPhone2 = new MaskedTextProvider(@"(00) 0000-0000");
                //27996090383
                //2796090383
                if (phoneNumber.Length == 11)
                {
                    mtpPhone1.Set(phoneNumber);
                    return mtpPhone1.ToString();
                } else if (phoneNumber.Length == 10)
                {
                    mtpPhone2.Set(phoneNumber);
                    return mtpPhone2.ToString();
                }
                else
                {
                    return phoneNumber;
                }
                
            }

            return null;
        }

        /// <summary>
        /// Formata um CNPJ com a máscara 00.000.000/0000-00
        /// </summary>
        /// <param name="cnpj">string Cnpj</param>
        /// <returns></returns>
        public static string ToCNPJFormat(this string cnpj)
        {
            if (!cnpj.IsEmpty())
            {
                cnpj = cnpj.Trim();

                MaskedTextProvider mtpCnpj = new MaskedTextProvider(@"00\.000\.000/0000-00");
                mtpCnpj.Set(cnpj.PadLeft(14, '0'));
                return mtpCnpj.ToString();
            }

            return null;
        }

        /// <summary>
        /// Remove a formatação de CNPJ
        /// </summary>
        /// <param name="cnpj">string Cnpj</param>
        /// <returns></returns>
        public static string ClearCNPJFormat(this string cnpj)
        {
            if (!cnpj.IsEmpty())
            {
                cnpj = cnpj.Trim();

                return cnpj.ToString().Replace(".", "").Replace("-", "").Replace("/", "").Trim();
            }

            return null;
        }

        /// <summary>
        /// Formata um CEP com a máscara 00.000-00
        /// </summary>
        /// <param name="cnpj">string CEP</param>
        /// <returns></returns>
        public static string ToCEPFormat(this string cep)
        {
            if (!cep.IsEmpty())
            {
                MaskedTextProvider mtpCEP = new MaskedTextProvider(@"00\.000-000");
                mtpCEP.Set(cep.PadLeft(8, '0'));
                return mtpCEP.ToString();
            }

            return null;
        }

        /// <summary>
        /// Remove a formatação de CEP
        /// </summary>
        /// <param name="cep">string CEP</param>
        /// <returns></returns>
        public static string ClearCEPFormat(this string cep)
        {
            if (!cep.IsEmpty())
            {
                cep = cep.Trim();

                return cep.ToString().Replace("-", "").Replace(".", "").Trim();
            }

            return null;
        }

        /// <summary>
        /// Remove a formatação do telefone
        /// </summary>
        /// <param name="tel">string tel</param>
        /// <returns></returns>
        public static string ClearTelFormat(this string tel)
        {
            if (!tel.IsEmpty())
            {
                tel = tel.Trim();

                return tel.ToString().Replace("-", "").Replace("(", "").Replace(")", "").Trim();
            }

            return null;
        }

        /// <summary>
        /// Formata uma inscricao estadual com a máscara 000.000.000.000
        /// </summary>
        /// <param name="insc">string inscricaoEstadual</param>
        /// <returns></returns>
        public static string ToInscricaoEstadualFormat(this string insc)
        {
            if (!insc.IsEmpty())
            {
                insc = insc.Trim();

                MaskedTextProvider mtpInsc = new MaskedTextProvider(@"000\.000\.000\.000");
                mtpInsc.Set(insc.PadLeft(12, '0'));
                return mtpInsc.ToString();
            }

            return null;
        }

        /// <summary>
        /// Remove a formatação de inscricao estadual
        /// </summary>
        /// <param name="insc">string inscricaoEstadual</param>
        /// <returns></returns>
        public static string ClearInscricaoEstadualFormat(this string insc)
        {
            if (!insc.IsEmpty())
            {
                return insc.ToString().Replace(".", "").Trim();
            }

            return null;
        }

        public static string OnlyNumber(this string texto)
        {
            return System.Text.RegularExpressions.Regex.Replace(texto, @"[^\d]", String.Empty).Trim();
        }

        /// <summary>
        /// Valida um CNPJ passado como parâmetro
        /// </summary>
        /// <param name="cnpj">CNPJ a ser validado</param>
        /// <returns>True/False</returns>
        public static bool IsValidCNPJ(this string cnpj)
        {
            cnpj = cnpj.Trim().Replace(".", String.Empty).Replace("/", String.Empty).Replace("-", String.Empty);

            if (cnpj.Length != 14)
                return false;

            string tempCnpj = cnpj.Substring(0, 12);

            string digito = String.Empty;

            for (int i = 0; i < 2; i++)
            {
                bool dec = false;

                int m = 0;

                int soma = 0;

                for (int j = 0; j < 12 + i; j++)
                {
                    if (dec)
                        m--;
                    else
                        m = (5 + i - j);

                    soma += int.Parse(tempCnpj[j].ToString()) * m;

                    if (m == 2 && (dec = true))
                        m = 10;
                }

                digito += soma % 11 < 2 ? 0 : 11 - (soma % 11);

                tempCnpj += digito;
            }

            return cnpj.EndsWith(digito);
        }

        /// <summary>
        /// Valida um CPF passado como parâmetro
        /// </summary>
        /// <param name="cpf">CPF a ser validado</param>
        /// <returns>Tue/False</returns>
        public static bool IsValidCPF(this string cpf)
        {
            cpf = cpf.Trim().Replace(".", String.Empty).Replace("-", String.Empty);

            if (cpf.Length != 11 || cpf == "00000000000" || cpf == "11111111111" || cpf == "22222222222" || cpf == "33333333333" || cpf == "44444444444" || cpf == "55555555555" || cpf == "66666666666" || cpf == "77777777777" || cpf == "88888888888" || cpf == "99999999999")
                return false;

            if (cpf.Length != 11)
                return false;

            string tempCpf = cpf.Substring(0, 9);

            string digito = String.Empty;

            for (int i = 0; i < 2; i++)
            {
                int soma = 0;

                for (int j = 0; j < 9 + i; j++)
                    soma += int.Parse(tempCpf[j].ToString()) * (10 + i - j);

                digito += soma % 11 < 2 ? 0 : 11 - (soma % 11);

                tempCpf += digito;
            }

            return cpf.EndsWith(digito);
        }

        /// <summary>
        /// Verifica a validade do e-mail
        /// </summary>
        /// <param name="email">string email</param>
        /// <returns></returns>
        public static bool IsValidEmail(this string email)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(email.Trim(), @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$");
        }

        /// <summary>
        /// Formata a String de acordo com os parâmetros informados
        /// </summary>
        /// <param name="format">String com formato</param>
        /// <param name="args">Parâmetros utilizados na formatação</param>
        /// <returns>String formatada</returns>
        public static string FormatString(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        /// <summary>
        /// Replica a string
        /// </summary>
        /// <param name="count">Quantidade de repetições</param>
        /// <returns>String replicada</returns>
        public static string DupeString(this string input, int? count)
        {
            string result = String.Empty;
            while (count-- > 0)
                result += input;

            return result;
        }

        public static Expression<Func<string, bool>> IsNullOrEmptyExp()
        {
            return inputString => inputString == null || inputString == String.Empty;
        }

        public static string ToPascalCase(this string str)
        {
            if (String.IsNullOrEmpty(str))
                return str;

            str = str.ToLower();

            TextInfo textInfo = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo;

            return textInfo.ToTitleCase(str);
        }

        public static string RemoveSpecialChars(this string str)
        {
            //var normalizedString = str.Normalize(System.Text.NormalizationForm.FormD);
            //var stringBuilder = new System.Text.StringBuilder();

            //foreach (var c in normalizedString)
            //    if (System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c) != System.Globalization.UnicodeCategory.NonSpacingMark)
            //        stringBuilder.Append(c);

            //str = stringBuilder.ToString().Normalize(System.Text.NormalizationForm.FormC);

            return System.Text.Encoding.UTF8.GetString(System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(str));
        }

        public static string ExtractBodyFromHTML(this string html)
        {
            var match = new System.Text.RegularExpressions.Regex(@"(?<=<body.*?>)(.*)(?=<\/body>)", System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.IgnoreCase).Match(html);
            if (match.Success)
                return match.Groups[0].Value.Trim();

            return html.Trim();
        }

        public static string ReplaceHTMLBody(this string html, string body)
        {
            return System.Text.RegularExpressions.Regex.Replace(html, @"(<body.*?>)(.*)(<\/body>)", "$1" + body + "$3", System.Text.RegularExpressions.RegexOptions.Singleline | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        }
    }
}
