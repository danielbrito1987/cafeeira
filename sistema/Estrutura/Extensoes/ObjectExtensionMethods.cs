using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Estrutura.Extensoes
{
    public static class ObjectExtensionMethods
    {

        /// <summary>
        /// Verifica se o objeto é nulo.
        /// </summary>
        /// <param name="source">Objeto a ser verificado.</param>
        /// <returns>Retorna true se o valor for nulo, caso contrário retorna false.</returns>
        public static bool IsNull(this object source)
        {
            return source == null;
        }

        /// <summary>
        /// Verifica se o objeto é nulo ou possui valores iniciais como Zero ou "".
        /// </summary>
        /// <param name="source">Objeto a ser verificado.</param>
        /// <returns>Retorna true se o valor for nulo ou possui valores iniciais, caso contrário retorna false.</returns>
        public static bool IsEmpty(this object source, bool allowZero = false)
        {
            bool result = (source == null ||
                           (!allowZero && source.Equals(0)) ||
                           (!allowZero && source.ToString().Equals("0")) ||
                           (source is string && source.ToString().Trim().Equals(string.Empty)) ||
                           source.Equals(string.Empty) ||
                           (source is decimal && !allowZero && ((decimal)source).Equals(0)) ||
                           SqlDateTime.MinValue.Equals(source) ||
                           DateTime.MinValue.Equals(source)) ||
                           (source is ICollection && ((ICollection)source).Count == 0) ||
                           (source is IQueryable && !((IQueryable<object>)source).Any()) ||
                           TimeSpan.Zero.Equals(source);

            if (!result)
            {
                Type collectionType = source.GetType().GetInterfaces().SingleOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICollection<>));

                result = collectionType != null && ((int)collectionType.GetProperty("Count").GetValue(source, null)) == 0;
            }

            return result;
        }

        /// <summary>
        /// Substitui o valor nulo com o valor especificado.
        /// </summary>
        /// <param name="source">Objeto a ser verificado.</param>
        /// <param name="result">Objeto a ser retornado caso o parâmetro source seja nulo.</param>
        /// <returns>Retorna o próprio objeto se o mesmo não for nulo, caso o contrário retorna o valor do parâmetro result.</returns>
        public static T IsNull<T>(this T source, T result)
        {
            return source != null ? source : result;
        }



        /// <summary>
        /// Verifica se o objeto herda do tipo informado como parâmetro.
        /// </summary>
        /// <param name="source">Objeto que será verificado.</param>
        /// <param name="type">Tipo ancestral.</param>
        /// <returns>Retorno true quando o objeto herda do tipo informado como parâmetro, caso contrário retorna false.</returns>
        public static bool InheritsFrom<T>(this T source, Type type)
        {
            return source.IsNull() ? false : source.GetType().IsAssignableFrom(type) || source.GetType().IsSubclassOf(type);
        }

        /// <summary>
        /// Retorna um objeto do tipo especificado e cujo valor é equivalente ao objeto especificado
        /// </summary>
        /// <typeparam name="T">Tipo do objeto que será convertido</typeparam>
        /// <param name="source">Objeto que será convertido</param>
        /// <param name="type">Tipo para qual o objeto será convertido</param>
        /// <returns>Objeto convertido</returns>
        public static object ChangeType<T>(this T source, Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (source == null)
                    return null;

                NullableConverter converter = new NullableConverter(type);

                type = converter.UnderlyingType;
            }

            return Convert.ChangeType(source, type);
        }

        /// <summary>
        /// Valora a propriedade informada com o valor especificado
        /// </summary>
        /// <typeparam name="T">Tipo do objeto que terá a propriedade valorada</typeparam>
        /// <param name="source">Objeto que terá a propriedade valorada</param>
        /// <param name="propertyName">Nome da propriedade</param>
        /// <param name="value">Novo valor da propriedade</param>
        /// <param name="index">Caso a propriedade seja indexada informe o indice</param>
        public static void SetPropertyValue<T>(this T source, string propertyName, object value, object[] index = null)
        {
            PropertyInfo property = source.GetType().GetProperty(propertyName);

            if (property.IsNull())
                throw new Exception(String.Format("The object does not have the property \"{0}\"", propertyName));

            property.SetValue(source, value, index);
        }

        /// <summary>
        /// Recupera o valor da propriedade informada
        /// </summary>
        /// <typeparam name="T">Tipo do objeto que possui a propriedade retornada</typeparam>
        /// <param name="source">Objeto que possui a propriedade retornada</param>
        /// <param name="propertyName">Nome da propriedade</param>
        /// <param name="index">Caso a propriedade seja indexada informe o indice</param>
        public static object GetPropertyValue<T>(this T source, string propertyName, object[] index = null)
        {
            PropertyInfo property = source.GetType().GetProperty(propertyName);

            if (property.IsNull())
                throw new Exception(String.Format("The object does not have the property \"{0}\"", propertyName));

            return property.GetValue(source, index);
        }

        /// <summary>
        /// Retorna null ou o objeto convertido para string
        /// </summary>
        /// <typeparam name="T">Tipo do objeto</typeparam>
        /// <param name="source">Objeto que será convertido para string caso não seja nulo</param>
        public static string ToStringOrNull<T>(this T source)
        {
            return source.IsNull() ? null : source.ToString();
        }

        /// <summary>
        /// Realiza o typecast do objeto
        /// </summary>
        /// <typeparam name="T">Tipo destino</typeparam>
        /// <param name="source">Objeto que irá sofre o typecast</param>
        /// <returns>Objeto com typecast</returns>
        public static T As<T>(this object source)
        {
            return (T)source;
        }

        

        /// <summary>
        /// Método que retorna o próprio objeto, ou o valor default do tipo do objeto de retorno definido no seletor caso ocorra exceção do tipo NullReferenceException
        /// </summary>
        /// <returns>Valor default do tipo do objeto de retorno definido no seletor</returns>
        public static TResult TryGetValue<TSource, TResult>(this TSource source, Func<TSource, TResult> selector)
        {
            try
            {
                return selector(source);
            }
            catch (Exception e)
            {
                if (e is ArgumentNullException || e is NullReferenceException)
                    return default(TResult);

                throw;
            }
        }

        public static T CastTo<T>(this object value, T targetType)
        {
            return (T)value;
        }
    }
}
