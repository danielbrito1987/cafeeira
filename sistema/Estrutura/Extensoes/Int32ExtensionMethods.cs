using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Estrutura.Extensoes
{
    public static class Int32ExtensionMethods
    {
        /// <summary>
        /// Método para escrever números por extenso
        /// </summary>
        /// <returns>Número por extenso</returns>
        public static string InWords(this int number)
        {
            return Convert.ToDecimal(number).InWords();
        }

        /// <summary>
        /// Método para retornar o valor em formato monetário
        /// </summary>
        /// <returns>Valor em formato monetário</returns>
        public static string ToCurrency(this int currency)
        {
            return Convert.ToDecimal(currency).ToCurrency();
        }

        /// <summary>
        /// Método para retornar o valor em formato monetário
        /// </summary>
        /// <returns>Valor em formato monetário</returns>
        public static string ToCurrency(this int? currency)
        {
            //Rafael
            if (currency.HasValue)
                return currency.Value.ToCurrency();

            return null;
        }

        /// <summary>
        /// Método para retornar o valor em formato especificado com duas casas decimais e separador de milhar
        /// </summary>
        /// <returns>Valor com duas casas decimais e separador de milhar</returns>
        public static string ToFormat(this int currency)
        {
            return Convert.ToDecimal(currency).ToFormat();
        }

        /// <summary>
        /// Método para retornar o valor em formato especificado com duas casas decimais e separador de milhar
        /// </summary>
        /// <returns>Valor com duas casas decimais e separador de milhar</returns>
        public static string ToFormat(this int? currency)
        {
            if (currency.HasValue)
                return currency.Value.ToFormat();

            return null;
        }

        /// <summary>
        /// Método para retornar o valor em formato especificado com casas decimais e separador de milhar
        /// </summary>
        /// <returns>Valor com casas decimais e separador de milhar</returns>
        public static string ToFormat(this int value, int decimalDigits)
        {
            //Rafael
            return Convert.ToDecimal(value).ToFormat(decimalDigits);
        }

        /// <summary>
        /// Método para retornar o valor em formato especificado com 2 casas decimais e separador de milhar
        /// </summary>
        /// <returns>Valor com 2 casas decimais e separador de milhar</returns>
        public static string ToFormat(this int value, bool milharSeparator)
        {
            //Rafael
            return Convert.ToDecimal(value).ToFormat(milharSeparator);
        }

        /// <summary>
        /// Método para retornar o valor em formato especificado com casas decimais e/ou separador de milhar
        /// </summary>
        /// <returns>Valor com casas decimais e/ou separador de milhar</returns>
        public static string ToFormat(this int value, int decimalDigits, bool milharSeparator)
        {
            //Rafael
            return Convert.ToDecimal(value).ToFormat(decimalDigits, milharSeparator);
        }

        /// <summary>
        /// Método para verificar interseção entra faixas de inteiros
        /// </summary>
        /// <param name="start1">Número inicial da faixa informada pelo usuário</param>
        /// <param name="end1">Número final da faixa informada pelo usuário</param>
        /// <param name="start2">Número inicial da faixa no banco</param>
        /// <param name="end2">Número final da faixa no banco</param>
        /// <returns>Retorna true caso exista interseção entre as faixas</returns>
        public static Expression<Func<int, int, bool>> IntersectExp(int start1, int end1)
        {
            //Jose Carlos
            return (start2, end2) => (start1 == start2) || (start1 > start2 ? start1 <= end2 : start2 <= end1);
        }

        /// <summary>
        /// Método para verificar interseção entra faixas de inteiros do tipo Numero Ano
        /// </summary>
        /// <param name="start1">Número inicial da faixa informada pelo usuário</param>
        /// <param name="end1">Número final da faixa informada pelo usuário</param>
        /// <param name="anoStart1">Ano inicial da faixa informada pelo usuário</param>
        /// <param name="anoEnd1">Ano final da faixa informada pelo usuário</param>
        /// <returns></returns>
        public static Expression<Func<int, int, int, int, bool>> IntersectExp(int start1, int end1, int anoStart1, int anoEnd1)
        {
            //Leonardo Balarini
            return (start2, end2, anoStart2, anoEnd2) => (start1 == start2) && (anoStart2 == anoStart1)
                                                        || (anoStart2 == anoStart1 && (start1 > start2 ? start1 <= end2 : start2 <= end1))
                                                        || (anoEnd2 == anoEnd1 && anoStart2 != anoStart1 && end1 >= start2)
                                                        || (anoStart2 != anoStart1 && anoEnd2 != anoEnd1 && (anoStart1 > anoStart2 ? anoStart1 <= anoEnd2 : anoStart2 <= anoEnd1));
        }

    }
}
