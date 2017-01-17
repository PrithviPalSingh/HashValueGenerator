using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Configuration;

namespace HasValueGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            string value = string.Empty;
            bool logHash = Convert.ToBoolean(ConfigurationManager.AppSettings["LogHash"]);
            Console.Write("Please provide LOG file path: ");
            string logFilePath = string.Empty;

            try
            {
                logFilePath = Console.ReadLine().Replace("\"", "");

                do
                {
                    Console.Write("Please provide file path: ");
                    string filePath = Console.ReadLine().Replace("\"", "");

                    string hashValue = string.Empty;
                    using (var md5 = MD5.Create())
                    {
                        hashValue = Encoding.Default.GetString(md5.ComputeHash(File.ReadAllBytes(filePath)));
                    }

                    Console.WriteLine(hashValue.GetHashCode());

                    if (logHash)
                    {
                        List<string> formattedString = new List<string> { string.Format("{0}: Has value generated for file {1} is {2}", DateTime.Now.ToString(), filePath, hashValue.GetHashCode()) };
                        WriteToLogFile(logFilePath, formattedString);
                    }

                    Console.Write("Do you want to generate another hash (Y/N): ");
                    value = Console.ReadLine();
                } while (value.Equals("y", StringComparison.CurrentCultureIgnoreCase));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (!string.IsNullOrWhiteSpace(logFilePath))
                {
                    List<string> formattedString = new List<string> { string.Format("{0}: Exception: {1}", DateTime.Now.ToString(), ex.Message) };
                    WriteToLogFile(logFilePath, formattedString);
                }
            }
            finally
            {
                Console.WriteLine("Please enter a key to exit");
                Console.Read();
            }
        }

        private static void WriteToLogFile(string logFilePath, List<string> formattedString)
        {
            try
            {
                logFilePath = Path.Combine(logFilePath, "log.txt");
                File.AppendAllLines(logFilePath, formattedString);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
