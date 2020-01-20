using System;
using System.Collections.Generic;
using System.IO;
using System.Configuration;
//using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Estrutura.Exceptions;
using Estrutura.Extensoes;
using System.Diagnostics;
namespace Estrutura.Util
{
    public static class Tools
    {
        private static byte[] key = { };
        private static byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xab, 0xcd, 0xef };

        public static string CriptografarMD5(string pass)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(pass);
            byte[] hash = md5.ComputeHash(inputBytes);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public static string GerarSenhaAleatoria()
        {
            string senha = "";

            char[] caracters = new char[] {'Q','W','E','R','T','Y','U','I','O','P','A','S','D','F','G','H','J','K','L','Z','X','C','V','B','N','M',
                                           'q','w','e','r','t','y','u','i','o','p','a','s','d','f','g','h','j','k','l','z','x','c','v','b','n','m',
                                           '@','1','2','3','4','5','6','7','8','9','0'};

            Random rndPosition = new Random();

            int i = 0;
            int[] numbPositions = new int[2];

            while (i < 2)
            {
                numbPositions[i] = rndPosition.Next(0, 7);
                i++;
            }

            int upperPosition = rndPosition.Next(0, 7);

            Random r = new Random();

            for (i = 0; i < 8; i++)
            {
                char[] temp = caracters;

                if (numbPositions.Contains(i))
                    temp = caracters.Where(c => char.IsNumber(c)).ToArray();

                if (upperPosition == i)
                    temp = caracters.Where(c => char.IsUpper(c)).ToArray();

                senha += temp[r.Next(0, temp.Length)].ToString();
            }

            return senha.Trim();
        }

        public static string Right(string sValue, int iMaxLength)
        {
            if (string.IsNullOrEmpty(sValue))
            {
                sValue = string.Empty;
            }
            else if (sValue.Length > iMaxLength)
            {
                sValue = sValue.Substring(sValue.Length - iMaxLength, iMaxLength);
            }
            
            return sValue;
        }

        /// <summary>
        /// Fonte http://alexandrejmuniz.wordpress.com/2012/07/05/funcao-para-validar-senha-forte-com-expressao-regular-com-c/
        /// Função que verifica se a string informada “Tes123@#$” will be accepted.
        /// UMA LETRA MINUSCULA
        /// UMA LETRA MAIUSCULA
        /// UM NUMERO
        /// UM ESPECIAL
        /// NO MINIMO 8 CARACTERES
        /// </summary>
        /// <param name=”password”></param>
        /// <returns></returns>
        public static bool ValidarSenha(string password)
        {
            int tamanhoMinimo = 8;
            //int tamanhoMinusculo = 1;
            int tamanhoMaiusculo = 1;
            int tamanhoNumeros = 1;
            int tamanhoCaracteresEspeciais = 1;

            //// Definição de letras minusculas
            //Regex regTamanhoMinusculo = new Regex("[a-z]");

            // Definição de letras minusculas
            Regex regTamanhoMaiusculo = new Regex("[A-Z]");

            // Definição de letras minusculas
            Regex regTamanhoNumeros = new Regex("[0-9]");

            // Definição de letras minusculas
            Regex regCaracteresEspeciais = new Regex("[^a-zA-Z0-9]");

            string notificacao = string.Format("A senha precisa ter no mínimo {0} caracteres, contendo ao menos {1} letra maíuscula, {2} número e {3} símbolo.",
                tamanhoMinimo, tamanhoMaiusculo, tamanhoNumeros, tamanhoCaracteresEspeciais);

            // Verificando tamanho minimo
            if (password.Length < tamanhoMinimo) throw new CoreException(notificacao);

            //// Verificando caracteres minusculos
            //if (regTamanhoMinusculo.Matches(password).Count < tamanhoMinusculo) throw new CoreException(notificacao);

            // Verificando caracteres maiusculos
            if (regTamanhoMaiusculo.Matches(password).Count < tamanhoMaiusculo) throw new CoreException(notificacao);

            // Verificando numeros
            if (regTamanhoNumeros.Matches(password).Count < tamanhoNumeros) throw new CoreException(notificacao);

            // Verificando os diferentes
            if (regCaracteresEspeciais.Matches(password).Count < tamanhoCaracteresEspeciais) throw new CoreException(notificacao);

            return true;
        }

        public static string DescriptografarQueryString(string stringToDecrypt, string sEncryptionKey)
        {
            byte[] inputByteArray = new byte[stringToDecrypt.Length + 1];

            key = System.Text.Encoding.UTF8.GetBytes(sEncryptionKey);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            inputByteArray = Convert.FromBase64String(stringToDecrypt);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms,
                des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            System.Text.Encoding encoding = System.Text.Encoding.UTF8;
            return encoding.GetString(ms.ToArray());

        }

        public static string CriptografarQueryString(string stringToEncrypt, string SEncryptionKey)
        {
            try
            {
                key = System.Text.Encoding.UTF8.GetBytes(SEncryptionKey);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms,
                  des.CreateEncryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static string ValidarLogin(string login)
        {
            string email = null;

            //using (var domainContext = new PrincipalContext(ContextType.Domain, ConfigurationManager.AppSettings["dominioAD"]))
            //{
            //    var user = UserPrincipal.FindByIdentity(domainContext, IdentityType.SamAccountName, login);

            //    if (user == null)
            throw new CoreException("Login inexistente no Active Directory.");

            //    email = user.EmailAddress;
            //}

            return email;
        }

        public static void ValidarLogin(string login, string senha)
        {
            //using (var domainContext = new PrincipalContext(ContextType.Domain, ConfigurationManager.AppSettings["dominioAD"]))
            //{
            //    if (!domainContext.ValidateCredentials(login, senha))
            //        throw new CoreException("Login ou senha inválido no Active Directory.");
            //}
        }


        public static bool ValidarLoginActiveDirectory(string login, string senha)
        {
            bool isValid = false;
            //using (var domainContext = new PrincipalContext(ContextType.Domain, ConfigurationManager.AppSettings["dominioAD"]))
            //{
            //    if (domainContext.ValidateCredentials(login, senha))
            //        isValid = true;
            //}
            return isValid;
        }


        /*********************************************NOVAS FUNCIONALIDADES*********************************************/

        /// <summary>
        /// Cria um dicionário a partir de um enumerador
        /// </summary>
        /// <param name="enumerate">Enumerador que será convertido para dicionário</param>
        /// <returns>Dicionário criado a partir de um enumerador</returns>
        public static Dictionary<string, string> EnumToDictionary(Type enumerate)
        {
            Dictionary<string, string> temp = new Dictionary<string, string>();

            foreach (string name in Enum.GetNames(enumerate))
            {
                Enum enumValue = (Enum)Enum.Parse(enumerate, name);

                temp.Add(enumValue.ToString(), enumValue.AsString());
            }

            return temp;
        }
        
        /*********************************************NOVAS FUNCIONALIDADES*********************************************/
    }
}
