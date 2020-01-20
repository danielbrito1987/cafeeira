using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Estrutura.Extensoes
{
    public static class DateTimeExtensionMethods
    {
        /// <summary>
        /// Retorna a data em string com o formato do settings.culture informado no web.config.
        /// </summary>
        /// <param name="dateTime">Data que será convertida para string.</param>
        /// <returns>Retorna a data em string.</returns>
        public static string ToCultureString(this DateTime? dateTime, bool? hour = null, bool? monthYear = null)
        {
            if (dateTime == null)
                return null;

            return ((DateTime)dateTime).ToCultureString(hour, monthYear);
        }

        /// <summary>
        /// Retorna a data em string com o formato do settings.culture informado no web.config.
        /// </summary>
        /// <param name="dateTime">Data que será convertida para string.</param>
        /// <param name="hour">Indica se a hora está inclusa.</param>
        /// <param name="monthYear">Formato para data MM/YYYY</param>
        /// <returns>Retorna a data em string.</returns>
        public static string ToCultureString(this DateTime dateTime, bool? hour = null, bool? monthYear = null)
        {
            string dateFormat = "dd{0}MM{0}yyyy";
            string dateSeparator = "/";
            string mdDateFormat = "d{0}m";
            string myDateFormat = "m{0}Y";
            string monthYearDateFormat = String.Format(myDateFormat, dateSeparator);
            string dateFormatAux = String.Format(dateFormat, dateSeparator);


            if (hour == true)
                return ((DateTime)dateTime).ToString(dateFormat);
            else if (monthYear == true)
                return ((DateTime)dateTime).ToString(monthYearDateFormat);
            else
                return ((DateTime)dateTime).ToString(dateFormatAux);
        }

        /// <summary>
        /// Retorna a data escrita por extenso.
        /// </summary>
        /// <param name="dateTime">Data que será escrita por extenso.</param>
        /// <param name="includeWeekDay">Indica se o dia da semana será incluído.</param>
        /// <returns>Retorna a data escrita por extenso.</returns>
        public static string InWords(this DateTime dateTime, bool includeWeekDay = false, bool includeOrdinal = false)
        {
            string weekDay = String.Empty;

            if (includeWeekDay)
            {
                switch (dateTime.DayOfWeek)
                {
                    case DayOfWeek.Sunday:
                        weekDay = "Sunday";
                        break;
                    case DayOfWeek.Monday:
                        weekDay = "Monday";
                        break;
                    case DayOfWeek.Tuesday:
                        weekDay = "Tuesday";
                        break;
                    case DayOfWeek.Wednesday:
                        weekDay = "Wednesday";
                        break;
                    case DayOfWeek.Thursday:
                        weekDay = "Thursday";
                        break;
                    case DayOfWeek.Friday:
                        weekDay = "Friday";
                        break;
                    case DayOfWeek.Saturday:
                        weekDay = "Saturday";
                        break;
                }

                weekDay += ", ";
            }

            string ordinal = String.Empty;

            if (includeOrdinal)
            {
                if (dateTime.Day % 10 == 1)
                    ordinal = "st";
                else if (dateTime.Day % 10 == 2)
                    ordinal = "nd";
                else if (dateTime.Day % 10 == 3)
                    ordinal = "rd";
                else
                    ordinal = "th";
            }

            if (System.Threading.Thread.CurrentThread.CurrentCulture.Name == "pt-BR")
                return String.Format("{0}{1} de {2} de {3}", weekDay, dateTime.Day, dateTime.ToString("MMMM", System.Threading.Thread.CurrentThread.CurrentCulture), dateTime.ToString("yyyy"));
            else
                return String.Format("{0}{1} {2}{3}, {4}", weekDay, dateTime.ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture), dateTime.Day, ordinal, dateTime.ToString("yyyy"));
        }

        /// <summary>
        /// Adiciona somente dias úteis na data atual
        /// </summary>
        /// <param name="date">Data Atual</param>
        /// <param name="days">Número de dias a adicionar</param>
        /// <returns>Data Atualizada somente com dias úteis</returns>
        public static DateTime? AddBusinessDays(this DateTime date, int days)
        {
            if (days == 0) return date;

            if (date.DayOfWeek == DayOfWeek.Saturday)
            {
                date = date.AddDays(2);
                days -= 1;
            }
            else if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                date = date.AddDays(1);
                days -= 1;
            }

            date = date.AddDays(days / 5 * 7);
            int extraDays = days % 5;

            if ((int)date.DayOfWeek + extraDays > 5)
            {
                extraDays += 2;
            }

            DateTime? date2 = date.AddDays(extraDays);
            return date2;
        }

        /// <summary>
        /// Calcula a quantidade de dias úteis entre um intervalo de duas datas
        /// </summary>
        /// <param name="start">Data início</param>
        /// <param name="end">Data fim</param>
        /// <returns>Número de dias úteis</returns>
        public static int GetBusinessDays(this DateTime start, DateTime end)
        {
            if (start.DayOfWeek == DayOfWeek.Saturday)
            {
                start = start.AddDays(2);
            }
            else if (start.DayOfWeek == DayOfWeek.Sunday)
            {
                start = start.AddDays(1);
            }

            if (end.DayOfWeek == DayOfWeek.Saturday)
            {
                end = end.AddDays(-1);
            }
            else if (end.DayOfWeek == DayOfWeek.Sunday)
            {
                end = end.AddDays(-2);
            }

            int diff = (int)end.Subtract(start).TotalDays;

            int result = diff / 7 * 5 + diff % 7;

            if (end.DayOfWeek < start.DayOfWeek)
            {
                return result - 2;
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// Verifica se o dia da data atual é um dia útil
        /// </summary>
        /// <param name="date">Data Atual</param>
        /// <returns>True / False</returns>
        public static bool IsBusinessDay(this DateTime date)
        {
            // Verificando se o dia é um dia da semana
            if (date.DayOfWeek == DayOfWeek.Saturday)
                return false;

            if (date.DayOfWeek == DayOfWeek.Sunday)
                return false;

            return true;
        }

        /// <summary>
        /// Verifica se o dia da data atual é um dia útil
        /// </summary>
        /// <param name="date">Data Atual</param>
        /// <param name="dias">Número de dias úteis a serem adicionados</param>
        /// <returns>True / False</returns>
        public static bool IsBusinessDay(this DateTime date, int dias)
        {
            // Adicionando somente dias úteis a data atual
            DateTime? date2 = date.AddBusinessDays(dias);

            // Verificando se o dia é um dia da semana
            if (date.DayOfWeek == DayOfWeek.Saturday)
                return false;

            if (date.DayOfWeek == DayOfWeek.Sunday)
                return false;

            return true;
        }


        /// <summary>
        /// Calculates number of business days, taking into account:
        ///  - weekends (Saturdays and Sundays)
        ///  - bank holidays in the middle of the week
        /// </summary>
        /// <param name="firstDay">First day in the time interval</param>
        /// <param name="lastDay">Last day in the time interval</param>
        /// <param name="bankHolidays">List of bank holidays excluding weekends</param>
        /// <returns>Number of business days during the 'span'</returns>
        public static int BusinessDaysUntil(this DateTime firstDay, DateTime lastDay, params DateTime[] bankHolidays)
        {
            firstDay = firstDay.Date;
            lastDay = lastDay.Date;
            if (firstDay > lastDay)
                throw new ArgumentException("Incorrect last day " + lastDay);

            TimeSpan span = lastDay - firstDay;
            int businessDays = span.Days + 1;
            int fullWeekCount = businessDays / 7;
            // find out if there are weekends during the time exceedng the full weeks
            if (businessDays > fullWeekCount * 7)
            {
                // we are here to find out if there is a 1-day or 2-days weekend
                // in the time interval remaining after subtracting the complete weeks
                int firstDayOfWeek = (int)firstDay.DayOfWeek;
                int lastDayOfWeek = (int)lastDay.DayOfWeek;
                if (lastDayOfWeek < firstDayOfWeek)
                    lastDayOfWeek += 7;
                if (firstDayOfWeek <= 6)
                {
                    if (lastDayOfWeek >= 7)// Both Saturday and Sunday are in the remaining time interval
                        businessDays -= 2;
                    else if (lastDayOfWeek >= 6)// Only Saturday is in the remaining time interval
                        businessDays -= 1;
                }
                else if (firstDayOfWeek <= 7 && lastDayOfWeek >= 7)// Only Sunday is in the remaining time interval
                    businessDays -= 1;
            }

            // subtract the weekends during the full weeks in the interval
            businessDays -= fullWeekCount + fullWeekCount;

            // subtract the number of bank holidays during the time interval
            foreach (DateTime bankHoliday in bankHolidays)
            {
                DateTime bh = bankHoliday.Date;
                if (firstDay <= bh && bh <= lastDay)
                    --businessDays;
            }

            return businessDays;
        }

        /// <summary>
        /// Método para verificar interseção entra períodos de datas
        /// </summary>
        /// <param name="start1">Data inicial do período informado pelo usuário</param>
        /// <param name="end1">Data final do período informado pelo usuário</param>
        /// <param name="start2">Data inicial do período no banco</param>
        /// <param name="end2">Data final do período no banco</param>
        /// <returns>Retorna true caso exista interseção entre as faixas</returns>
        public static Expression<Func<int, int, bool>> IntersectExp(int start1, int end1)
        {
            //Jose Carlos
            return (start2, end2) => (start1 == start2) || (start1 > start2 ? start1 <= end2 : start2 <= end1);
        }

        /// <summary>
        /// Verifica se é a data de sistema utilizada em telas de relatórios e processos.
        /// </summary>
        public static bool IsUsingSystemDate(this DateTime? dateTime)
        {
            if (dateTime.IsNull())
            {
                return false;
            }
            else
            {
                return ((DateTime)dateTime).IsUsingSystemDate();
            }
        }

        /// <summary>
        /// Verifica se é a data de sistema utilizada em telas de relatórios e processos.
        /// </summary>
        public static bool IsUsingSystemDate(this DateTime dateTime)
        {
            if (dateTime == System.Data.SqlTypes.SqlDateTime.MinValue.Value)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void ThisWeek(out DateTime firstDay, out DateTime lastDay, DateTime? referenceDate = null)
        {
            DateTime date = referenceDate == null ? DateTime.Now.Date : referenceDate.Value.Date;

            firstDay = date.AddDays(DayOfWeek.Sunday - date.DayOfWeek);
            lastDay = firstDay.AddDays(7).AddMilliseconds(-1);
        }

        public static void PreviousWeek(out DateTime firstDay, out DateTime lastDay, DateTime? referenceDate = null)
        {
            DateTimeExtensionMethods.ThisWeek(out firstDay, out lastDay, (referenceDate == null ? DateTime.Now.Date : referenceDate.Value.Date).AddDays(-7));
        }

        public static void ThisWeekAndAhead(out DateTime firstDay, out DateTime lastDay, DateTime? referenceDate = null)
        {
            DateTimeExtensionMethods.ThisWeek(out firstDay, out lastDay, referenceDate);
            lastDay = System.Data.SqlTypes.SqlDateTime.MaxValue.Value;
        }

        public static void NextWeek(out DateTime firstDay, out DateTime lastDay, DateTime? referenceDate = null)
        {
            DateTimeExtensionMethods.ThisWeek(out firstDay, out lastDay, (referenceDate == null ? DateTime.Now.Date : referenceDate.Value.Date).AddDays(7));
        }
    }
}
