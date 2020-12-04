using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Hash
{
    class Program
    {
        static void Main(string[] args)
        {
            if (File.Exists(@"diff.txt"))
            {
                File.WriteAllText(@"diff.txt", string.Empty);
            }

            if (File.Exists(@"hash.txt"))
            {
                File.WriteAllText(@"hash.txt", string.Empty);
            }

            var sumFunctionsList = new List<FunctionItem> {
                new FunctionItem() {
                    HashFunc = Md5Sum,
                    Description = "MD5 Sum"
                },
                new FunctionItem() {
                    HashFunc = Sha1Sum,
                    Description = "SHA1 Sum"
                },
                new FunctionItem() {
                    HashFunc = Sha256Sum,
                    Description = "SHA256 Sum"
                },
                new FunctionItem() {
                    HashFunc = Sha384Sum,
                    Description = "SHA384 Sum"
                },
                new FunctionItem() {
                    HashFunc = Sha512Sum,
                    Description = "SHA512 Sum"
                }
            };

            foreach(var function in sumFunctionsList)
            {
                Compare(function);
            }
        }

        private static void Compare(FunctionItem funcItem)
        {
            StreamReader srPersonal = new StreamReader("personal.txt");
            StreamReader srPersonal_ = new StreamReader("personal_.txt");
            StreamReader srHashPdf = new StreamReader("hash.pdf");
            StreamWriter swDiff = new StreamWriter("diff.txt", true);
            
            string hashPdf = srHashPdf.ReadToEnd();
            string textPersonal = hashPdf + srPersonal.ReadToEnd();
            string textPersonal_ = hashPdf + srPersonal_.ReadToEnd();

            var hash = funcItem.HashFunc("hash.txt", textPersonal);
            var hash_ = funcItem.HashFunc("hash_.txt", textPersonal_);

            BitArray hashBit = new BitArray(hash);
            BitArray hashBit_ = new BitArray(hash_);
            int counter = 0;
            int total = hashBit.Length;

            for (var i = 0; i < hashBit.Length; i++)
            {
                if (hashBit[i] == hashBit_[i])
                {
                    counter++;
                }
            }
            swDiff.WriteLine(funcItem.Description);
            swDiff.WriteLine($"{BitConverter.ToString(hash).Replace("-", "")} - cat hash.pdf personal.txt");
            swDiff.WriteLine($"{BitConverter.ToString(hash_).Replace("-", "")} - cat hash.pdf personal_.txt");
            swDiff.WriteLine($"{counter} / {total} bitów. {(counter * 100 / total)}%");
            swDiff.Close();
            srPersonal.Close();
            srPersonal_.Close();
            srHashPdf.Close();

            File.Delete(Path.Combine(@"./", "hash_.txt"));
        }

        public static byte[] Md5Sum(string path, string text)
        {
            byte[] hash;
            using (MD5 md5 = MD5.Create())
            {
                StreamWriter sw = new StreamWriter(path, true);
                hash = md5.ComputeHash(Encoding.UTF8.GetBytes(text));
                sw.WriteLine("MD5:");
                sw.WriteLine(BitConverter.ToString(hash).Replace("-", ""));
                sw.Close();
            }
            return hash;
        }

        public static byte[] Sha1Sum(string path, string text)
        {
            byte[] hash;
            using (SHA1 sha1 = SHA1.Create())
            {
                StreamWriter sw = new StreamWriter(path, true);
                hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(text));
                sw.WriteLine("SHA1:");
                sw.WriteLine(BitConverter.ToString(hash).Replace("-", ""));
                sw.Close();
            }
            return hash;
        }

        public static byte[] Sha256Sum(string path, string text)
        {
            byte[] hash;
            using (SHA256 sha256 = SHA256.Create())
            {
                StreamWriter sw = new StreamWriter(path, true);
                hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
                sw.WriteLine("SHA256:");
                sw.WriteLine(BitConverter.ToString(hash).Replace("-", ""));
                sw.Close();
            }
            return hash;
        }

        public static byte[] Sha384Sum(string path, string text)
        {
            byte[] hash;
            using (SHA384 sha384 = SHA384.Create())
            {
                StreamWriter sw = new StreamWriter(path, true);
                hash = sha384.ComputeHash(Encoding.UTF8.GetBytes(text));
                sw.WriteLine("SHA384:");
                sw.WriteLine(BitConverter.ToString(hash).Replace("-", ""));
                sw.Close();
            }
            return hash;
        }

        public static byte[] Sha512Sum(string path, string text)
        {
            byte[] hash;
            using (SHA512 sha512 = SHA512.Create())
            {
                StreamWriter sw = new StreamWriter(path, true);
                hash = sha512.ComputeHash(Encoding.UTF8.GetBytes(text));
                sw.WriteLine("SHA512:");
                sw.WriteLine(BitConverter.ToString(hash).Replace("-", ""));
                sw.Close();
            }
            return hash;
        }
    }

    class FunctionItem
    {
        public Func<string, string, byte[]> HashFunc;
        public string Description;
    }
}
