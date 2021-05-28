using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Data.Common;
using System.Data;
using System.ComponentModel;

namespace SIDC_Development_Helper
{
    /// <summary>
    /// Name: SIDC DLL Common Data Helper
    /// <para/>
    /// Developer: Angelito D. De Sagun
    /// <para/>
    /// Date: 2021-05-23
    /// <para/>
    /// Revision Date:--
    /// </summary>
    public class CommonHelper
    {
        /// <summary>
        /// Call this method to get XMLSettings from root path (eg. bin/release or bin/debug)
        /// </summary>
        /// <param name="attribute">attribute name</param>
        /// <param name="xmlFileName">filename of xml file (*const variable in your form)</param>
        /// <returns>string</returns>
        public static string GetXMLSetting(string attribute, string xmlFileName)
        {
            var doc = XDocument.Load(string.Concat(Application.StartupPath, $@"\{xmlFileName}"));

            var res = doc.Elements("configuration").Descendants(attribute).First().Value;
            return res;
        }

        /// <summary>
        /// Write error log to root path (eg. bin/debug,bin/release,root/).
        /// <para/>
        /// Note: Method name will not work on "async method"
        /// </summary>
        /// <param name="error"></param>
        public static void WriteLog(Exception error)
        {
            try
            {
                StackFrame frame = new StackFrame(1, true);
                var method = frame.GetMethod();
                var fileName = frame.GetFileName();
                var lineNumber = frame.GetFileLineNumber();
                var callingMethod = $"Catch Method: {method.Name} => Catch Line: {lineNumber} => File Name: {fileName}";

                string logFilePath = Application.StartupPath + @"\AppLog.txt";

                string errorMessage = $"{DateTime.Now} => Message : {error.Message.Replace("\n", " ")} => {callingMethod}";
                if (File.Exists(logFilePath))
                {
                    File.AppendAllText(logFilePath, Environment.NewLine + errorMessage);
                }
                else
                {
                    File.WriteAllText(logFilePath, errorMessage);
                }
            }
            catch
            {
                File.WriteAllText(Application.StartupPath + @"\FileOpenError.txt", $@"Cannot write error log. {Application.StartupPath}\Applog.txt is open");
            }
        }

        /// <summary>
        /// Get DB string result
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static string GetDBString(DbDataReader dr, string fieldName)
        {
            return dr.IsDBNull(dr.GetOrdinal(fieldName)) ? string.Empty : dr.GetString(dr.GetOrdinal(fieldName));
        }

        /// <summary>
        /// Get DB decimal result (Default value: 0.00)
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static decimal GetDBDecimal(DbDataReader dr, string fieldName)
        {
            return dr.IsDBNull(dr.GetOrdinal(fieldName)) ? 0.00M : dr.GetDecimal(dr.GetOrdinal(fieldName));
        }

        /// <summary>
        /// Get DB Integer result (Default value: 0)
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static int GetDBInteger(DbDataReader dr, string fieldName)
        {
            return dr.IsDBNull(dr.GetOrdinal(fieldName)) ? 0 : dr.GetInt32(dr.GetOrdinal(fieldName));
        }

        /// <summary>
        /// Get DB Datetime result (Default value: -1000 years)
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static DateTime GetDBDatetime(DbDataReader dr, string fieldName)
        {
            return dr.IsDBNull(dr.GetOrdinal(fieldName)) ? DateTime.Now.AddYears(-1000) : dr.GetDateTime(dr.GetOrdinal(fieldName));
        }

        /// <summary>
        /// Get DB Boolean result (Default value: false)
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static bool GetDBBoolean(DbDataReader dr, string fieldName)
        {
            return dr.IsDBNull(dr.GetOrdinal(fieldName)) ? false : dr.GetBoolean(dr.GetOrdinal(fieldName));
        }

        /// <summary>
        /// Get DB Long int result (Default value: -1)
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static long GetDBLong(DbDataReader dr, string fieldName)
        {
            return dr.IsDBNull(dr.GetOrdinal(fieldName)) ? -1 : dr.GetInt64(dr.GetOrdinal(fieldName));
        }

        /// <summary>
        /// Call this method function to get local IP Address
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIP()
        {
            try
            {
                var pc = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());

                var host = pc.AddressList.FirstOrDefault(f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                return host.ToString();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Convert List to DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
                TypeDescriptor.GetProperties(typeof(T));

            DataTable table = new DataTable();

            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }
    }
}
