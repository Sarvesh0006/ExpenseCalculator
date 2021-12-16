using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Dapper;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using Microsoft.Extensions.Options;

namespace ExpenseCalculator.DataLayer
{
    public class dataLayer
    {
        public static object ExcuteNonQuery(string procedureName, object parameters)
        {
            dynamic Result = null;
            using (IDbConnection db = ORMConnection.GetConnection())
            {
                try
                {
                    db.Open();
                    Result = db.Execute(procedureName, parameters, commandType: CommandType.StoredProcedure);
                    if (Convert.ToInt16(Result) > 0)
                    {
                        Result = ResponseResult.SuccessResponse("Success");
                    }
                    else
                    {
                        Result = ResponseResult.FailedResponse("Failed");
                    }
                }
                catch (Exception ex)
                {
                    Result = ResponseResult.ExceptionResponse("Internal Server Error", ex.Message.ToString());
                }
                finally
                {
                    db.Close();
                }
            }
            return Result;
        }
        public static async Task<object> ExcuteNonQueryAsync(string procedureName, object parameters)
        {
            dynamic Result = null;
            using (IDbConnection db = ORMConnection.GetConnection())
            {
                try
                {
                    db.Open();
                    Result = await db.ExecuteAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
                    if (Convert.ToInt16(Result) > 0)
                    {
                        Result = ResponseResult.SuccessResponse("Success");
                    }
                    else
                    {
                        Result = ResponseResult.FailedResponse("Failed");
                    }
                }
                catch (Exception ex)
                {
                    Result = ResponseResult.ExceptionResponse("Internal Server Error", ex.Message.ToString());
                }
                finally
                {
                    db.Close();
                }
            }
            return Result;
        }


        public static object Query(string procedureName, object parameters)
        {
            dynamic Result = null;
            using (IDbConnection db = ORMConnection.GetConnection())
            {
                try
                {
                    db.Open();
                    Result = db.Query<object>(procedureName, parameters, commandType: CommandType.StoredProcedure);
                    Result = ResponseResult.SuccessResponse("Success", Result);
                }
                catch (Exception ex)
                {
                    Result = ResponseResult.ExceptionResponse("Internal Server Error", ex.Message.ToString());
                }
                finally
                {
                    db.Close();
                }
            }
            return Result;
        }
        public static async Task<object> QueryAsync(string procedureName, object parameters)
        {
            dynamic Result = null;
            using (IDbConnection db = ORMConnection.GetConnection())
            {
                try
                {
                    db.Open();
                    Result = await db.QueryAsync<object>(procedureName, parameters, commandType: CommandType.StoredProcedure);
                    Result = ResponseResult.SuccessResponse("Success", Result);
                }
                catch (Exception ex)
                {
                    Result = ResponseResult.ExceptionResponse("Internal Server Error", ex.Message.ToString());
                }
                finally
                {
                    db.Close();
                }
            }
            return Result;
        }


        public static object QueryFirstOrDefault(string procedureName, object parameters)
        {
            dynamic Result = null;
            using (IDbConnection db = ORMConnection.GetConnection())
            {
                try
                {
                    db.Open();
                    Result = db.QueryFirstOrDefault<object>(procedureName, parameters, commandType: CommandType.StoredProcedure);
                    Result = ResponseResult.SuccessResponse("Success", Result);
                }
                catch (Exception ex)
                {
                    Result = ResponseResult.ExceptionResponse("Internal Server Error", ex.Message.ToString());
                }
                finally
                {
                    db.Close();
                }
            }
            return Result;
        }
        public static async Task<object> QueryFirstOrDefaultAsync(string procedureName, object parameters)
        {
            dynamic Result = null;
            using (IDbConnection db = ORMConnection.GetConnection())
            {
                try
                {
                    db.Open();
                    Result = await db.QueryFirstOrDefaultAsync<object>(procedureName, parameters, commandType: CommandType.StoredProcedure);
                    Result = ResponseResult.SuccessResponse("Success", Result);
                }
                catch (Exception ex)
                {
                    Result = ResponseResult.ExceptionResponse("Internal Server Error", ex.Message.ToString());
                }
                finally
                {
                    db.Close();
                }
            }
            return Result;
        }

        public static async Task<object> QueryFirstOrDefaultAsyncWithDBResponse(string procedureName, object parameters)
        {
            dynamic Result = null;
            using (IDbConnection db = ORMConnection.GetConnection())
            {
                try
                {
                    db.Open();
                    Result = await db.QueryFirstOrDefaultAsync<object>(procedureName, parameters, commandType: CommandType.StoredProcedure, commandTimeout: 600);
                }
                catch (Exception ex)
                {
                    Result = ResponseResult.ExceptionResponse("Internal Server Error", ex.Message.ToString());
                }
                finally
                {
                    db.Close();
                }
            }
            return Result;
        }
    }

    public static class ResponseResult
    {
        public static object SuccessResponse(object ResponseMessage)
        {
            var result = new { responseCode = 200, responseMessage = ResponseMessage };
            return result;
        }

        public static object SuccessResponse(object ResponseMessage, object ResponseResult)
        {
            var result = new { responseCode = 200, responseMessage = ResponseMessage, responseResult = ResponseResult };
            return result;
        }

        public static object FailedResponse(object ResponseMessage)
        {
            var result = new { responseCode = 0, responseMessage = ResponseMessage };
            return result;
        }

        public static object FailedResponse(object ResponseMessage, object ResponseResult)
        {
            var result = new { responseCode = 0, responseMessage = ResponseMessage, responseResult = ResponseResult };
            return result;
        }

        public static object ExceptionResponse(object ResponseMessage)
        {
            var result = new { responseCode = 500, responseMessage = ResponseMessage };
            return result;
        }

        public static object ExceptionResponse(object ResponseMessage, object ResponseResult)
        {
            var result = new { responseCode = 500, responseMessage = ResponseMessage, responseResult = ResponseResult };
            return result;
        }
    }


    public static class Security
    {
        public static string GetMd5Hash(string input)
        {
            MD5 md5Hash = MD5.Create();
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        static string EncryptionKey = "MAKV2SPBNI99212";
        public static string Encrypt(string clearText)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        public static string Decrypt(string cipherText)
        {
            //string EncryptionKey = "MAKV2SPBNI99212";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }


        public static string EncodeBase64String(string input)
        {
            var PlainText = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(PlainText);
        }

        public static string DecodeBase64String(string input)
        {
            var base64EncodedBytes = Convert.FromBase64String(input);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
