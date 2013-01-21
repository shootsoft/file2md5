using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace file2md5
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            string output = string.Empty;

            if (args.Length > 1)
            {
                output = args[1];
            }
            else if (args.Length > 0)
            {
                output = args[0] + ".md5";
            }
            else
            {
                return;
            }

            try
            {
                if (File.Exists(args[0]))
                {
                    string md5 = GetMD5(args[0]);
                    if (File.Exists(output))
                    {
                        string expected = TxtReader.Read(output).Trim().ToUpper();
                        if (expected.Equals(md5))
                        {
                            Console.WriteLine("PASS!");
                        }
                        else 
                        {
                            Console.WriteLine("FAIL!");
                        }
                        Console.WriteLine("Expected: " + expected);
                        Console.WriteLine("Actual: " + md5);
                        Console.WriteLine("The acutal md5 value had been set to clipboard!");
                        Clipboard.SetDataObject(md5, true);
                        Console.Read();
                    }
                    else
                    {
                        File.WriteAllText(output, md5, Encoding.UTF8);
                    }                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.Read();
            }
        }

        static string GetMD5(string file)
        {
            MD5 md5 = MD5.Create();
            using (FileStream stream = new FileStream(file, FileMode.Open))
            {
                byte[] buff = md5.ComputeHash(stream);
                StringBuilder result = new StringBuilder(buff.Length * 2);
                for (int i = 0; i < buff.Length; i++)
                    result.Append(buff[i].ToString("X2"));
                return result.ToString();
            }

        }
    }
}
