using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Lg.EducationPlatform.Common
{
    public class Security
    {
        #region 1.0 MD5加密 StrToMD5()
        /// <summary>
        ///  MD5加密是不可逆的.Message Digest5信息摘要加密算法
        ///  SHA-1也是散列算法，比MD5更安全。
        ///  MD5是32位的
        /// </summary>
        /// <param name="strPwd">密码明文</param>
        /// <returns>密文</returns>
        public static string StrToMD5(string strPwd)
        {
            //先给要加密的字符串加上前后缀
            //GetBytes()得到一个系统默认编码格式的字节数组
            string sourcePwd = DateTime.Now.Year + strPwd + DateTime.Now.Month;
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(sourcePwd);

            //实例化一个MD5对象
            MD5 md5 = MD5.Create();

            //进行MD5加密，加密后是一个字节数组，此处要注意字符串转换为字节数组byte[]时要指定编码
            //ComputeHash（）计算指定字节数组的哈希值，请注意，能生成哈希值的不止有MD5加密算法，SHA-1等等也可以
            byte[] encryptBytes = md5.ComputeHash(buffer);

            encryptBytes.Reverse();//反转生成的MD5码

            StringBuilder sb = new StringBuilder();

            //只取MD5码的一部分，这样恶意访问者无法知道取的是哪几位,得到的字符串使用十六进制类型格式化
            for (int i = 3; i < encryptBytes.Length; i++)
            {
                sb.Append(encryptBytes[i].ToString("X2"));
            }

            return sb.ToString();
        }
        #endregion

        #region 解密Base64
        /// <summary>
        /// 解密Base64
        /// </summary>
        /// <param name="asContent">需要解密的字符串</param>
        /// <returns></returns>
        public static string Base64TextUTF8Decode(string asContent)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(asContent);
                UTF8Encoding encoding = new UTF8Encoding();
                return encoding.GetString(bytes);
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region 加密Base64
        /// <summary>
        /// 加密Base64
        /// </summary>
        /// <param name="asText">需要加密的字符串</param>
        /// <returns></returns>
        public static string Base64UTF8Encode(string asText)
        {
            try
            {
                string s = asText;
                UTF8Encoding encoding = new UTF8Encoding();
                return Convert.ToBase64String(encoding.GetBytes(s));
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region  解密UTF8
        /// <summary>
        /// 解密UTF8
        /// </summary>
        /// <param name="asEnCodeText">需要解密的字符串</param>
        /// <returns></returns>
        public static string DecodeUTF8Text(string asEnCodeText)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(asEnCodeText);
                UTF8Encoding encoding = new UTF8Encoding();
                return encoding.GetString(bytes);
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region 加密UTF8
        /// <summary>
        /// 加密UTF8
        /// </summary>
        /// <param name="asDecodeText">需要加密的字符串</param>
        /// <returns></returns>
        public string EnCodeUTF8Text(string asDecodeText)
        {
            try
            {
                UTF8Encoding encoding = new UTF8Encoding();
                return Convert.ToBase64String(encoding.GetBytes(asDecodeText));
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region Encrypt密钥
        /// <summary>
        /// 定义Encrypt加密的密钥
        /// </summary>
        public string EncryKeyword = "&x%x#g@j?m,z:c*h*1^8)7(3#7$9%0*6%7!7@9";
        #endregion

        #region EncryptHelper加密

        /// <summary>
        /// 加密函数
        /// </summary>
        /// <param name="strText">要加密的字符串</param>
        /// <returns></returns>
        public String Encrypt(String strText)
        {
            string strEncrKey = EncryKeyword;
            Byte[] byKey = { };
            Byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            try
            {
                byKey = System.Text.Encoding.UTF8.GetBytes(strEncrKey.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                Byte[] inputByteArray = Encoding.UTF8.GetBytes(strText);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        #endregion

        #region EncryptHelper解密


        /// <summary>
        /// 解密函数
        /// </summary>
        /// <param name="strText">要解密的字符串</param>
        /// <returns></returns>
        public String Decrypt(String strText)
        {
            string sDecrKey = EncryKeyword;
            Byte[] byKey = { };
            Byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            Byte[] inputByteArray = new byte[strText.Length];
            try
            {
                byKey = System.Text.Encoding.UTF8.GetBytes(sDecrKey.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(strText);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                return encoding.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        #endregion

        #region DeCodeID 解密
        /// <summary>
        /// DeCodeID 解密
        /// </summary>
        /// <param name="Encode">需要解密的字符串函数</param>
        /// <returns></returns>
        public static string DecodeID(string Encode)
        {
            if (Encode == "")
            {
                return "";
            }
            int length = Encode.Length;
            if (length < 5)
            {
                return "";
            }
            int num2 = Convert.ToInt32(Encode.Substring(2, 1));
            int num3 = Convert.ToInt32(Encode.Substring(4, 1));
            int startIndex = ((num2 * 2) + (num3 * 2)) + 7;
            if (length < (startIndex + 20))
            {
                return "";
            }
            num2 = (Convert.ToInt32(Encode.Substring(startIndex, 1)) * 10) + Convert.ToInt32(Encode.Substring(startIndex + 1, 1));
            startIndex = (startIndex + 2) - 1;
            string str = "";
            for (int i = 0; i < num2; i++)
            {
                startIndex = (startIndex + 3) + 1;
                str = str + Encode.Substring(startIndex, 1);
            }
            return str;
        }
        #endregion

        #region EncodeID 加密
        /// <summary>
        /// EncodeID 加密
        /// </summary>
        /// <param name="ID">需要加密的字符串</param>
        /// <returns></returns>
        public static string EncodeID(string ID)
        {
            int num6;
            int num = (DateTime.Now.Hour * DateTime.Now.Minute) * DateTime.Now.Second;
            if (num < 100)
            {
                num = (num + 0x70) * 0x25;
            }
            string str = "";
            for (num6 = 1; num6 < 100; num6++)
            {
                int num2 = num * (num6 % 50);
                str = str + num2.ToString().Trim();
                if (str.Length > 120)
                {
                    break;
                }
            }
            int num3 = Convert.ToInt32(str.Substring(2, 1));
            int num4 = Convert.ToInt32(str.Substring(4, 1));
            int length = ((num3 * 2) + (num4 * 2)) + 7;
            num3 = ID.Length;
            string str2 = str.Substring(0, length);
            if (num3 < 10)
            {
                str2 = str2 + "0";
            }
            str2 = str2 + num3.ToString().Trim();
            for (num6 = 0; num6 < num3; num6++)
            {
                str2 = str2 + str.Substring(1 + (num6 * 3), 3) + ID.Substring(num6, 1);
            }
            num3 = str2.Length;
            try
            {
                str2 = str2 + str.Substring(20, 110 - num3);
            }
            catch
            {
            }
            return str2;
        }
        #endregion

        #region DecodeURL 解密
        /// <summary>
        /// DecodeURL 解密
        /// </summary>
        /// <param name="NameValueStr">需要解密的字符串函数</param>
        /// <returns></returns>
        public static string DecodeURL(string NameValueStr)
        {
            uint num;
            if ((NameValueStr == null) || (NameValueStr == ""))
            {
                return "";
            }
            string str = NameValueStr.Trim().Replace("X", "1U1").Replace("Y", "2U2").Replace("Z", "3U3").Replace("V", "4U5").Replace("W", "7U6").Replace("O", "8U7").Replace("P", "0U8").Replace("Q", "0U9").Replace("R", "1U0").Replace("S", "2U4").Replace("x", "U1").Replace("y", "U2").Replace("z", "U3").Replace("u", "U4").Replace("v", "U5").Replace("w", "U6").Replace("o", "U7").Replace("p", "U8").Replace("q", "U9").Replace("r", "U0");
            string str2 = "";
            string str3 = "";
            for (int i = 1; i < str.Length; i++)
            {
                if (str[i] == 'U')
                {
                    if (str2 != "")
                    {
                        num = Convert.ToUInt32(str2);
                        str3 = str3 + Convert.ToChar(num).ToString().Trim();
                        str2 = "";
                    }
                }
                else
                {
                    char ch2 = str[i];
                    str2 = str2 + ch2.ToString().Trim();
                }
            }
            if (str2 != "")
            {
                num = Convert.ToUInt32(str2);
                str3 = str3 + Convert.ToChar(num).ToString().Trim();
            }
            return str3;
        }
        #endregion

        #region EncodeURL 加密
        /// <summary>
        /// EncodeURL 加密
        /// </summary>
        /// <param name="NameValueStr">需要加密的字符串函数</param>
        /// <returns></returns>
        public static string EncodeURL(string NameValueStr)
        {
            if ((NameValueStr == null) || (NameValueStr == ""))
            {
                return "";
            }
            string str = "";
            NameValueStr = NameValueStr.Trim();
            for (int i = 0; i < NameValueStr.Length; i++)
            {
                str = str + "U" + Convert.ToUInt16(NameValueStr[i]).ToString().Trim();
            }
            return str.Replace("1U1", "X").Replace("2U2", "Y").Replace("3U3", "Z").Replace("4U5", "V").Replace("7U6", "W").Replace("8U7", "O").Replace("0U8", "P").Replace("0U9", "Q").Replace("1U0", "R").Replace("2U4", "S").Replace("U1", "x").Replace("U2", "y").Replace("U3", "z").Replace("U4", "u").Replace("U5", "v").Replace("U6", "w").Replace("U7", "o").Replace("U8", "p").Replace("U9", "q").Replace("U0", "r");
        }
        #endregion
    }
}
