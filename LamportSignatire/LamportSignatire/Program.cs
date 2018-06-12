using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace LamportSignatire
{
    
    class Program
    {
        static int L = 128;      
        static void Main(string[] args)
        {

            byte[,][] closedkey = GetClosedKey(L);
            byte[,][] openedkey = GetOpenedKey(closedkey);
            byte[][] signature = GetSignature("D://rock.zip", closedkey);
            //openedkey[5, 0][5] = 100;
            //openedkey[5, 1][5] = 100;
            bool isEqual = CheckedSignature("D://rock1.zip", openedkey, signature);                       
            Console.WriteLine(isEqual);
            Console.WriteLine(BitConverter.ToString(openedkey[5, 0]));
            Console.ReadKey();

        }

        public static byte[][] GetSignature(string filepath, byte[,][] closedkey)
        {
            FileStream reader = new FileStream("D://rock.zip", FileMode.Open);
            byte[][] signature = new byte[L][];
            byte[] filehash = GetChecksumBuffered(reader);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < filehash.Length; i++)
            {
                sb.Append(Convert.ToString(filehash[i], 2).PadLeft(8, '0'));
            }
            string messageHash = sb.ToString();
            for (int i = 0; i < L; i++)
            {
                signature[i] = closedkey[i, Convert.ToInt32(Convert.ToString(messageHash.ElementAt(i)))];
            }
            return signature;
            
        }

        public static bool CheckedSignature(string filepath, byte[,][] openedkey, byte[][] signature)
        {
            FileStream reader = new FileStream(filepath, FileMode.Open);
            var hash = new MD5CryptoServiceProvider();
            byte[][] opensignature = new byte[L][];
            byte[] message = GetChecksumBuffered(reader);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < message.Length; i++)
            {
                sb.Append(Convert.ToString(message[i], 2).PadLeft(8, '0'));
            }
            string messageHash = sb.ToString();
            for (int i = 0; i < L; i++)
            {
                opensignature[i] = openedkey[i, Convert.ToInt32(Convert.ToString(messageHash.ElementAt(i)))];
            }
            byte[][] hashsignature = new byte[L][];
            for (int i = 0; i < L; i++)
            {
                hashsignature[i] = hash.ComputeHash(signature[i]); 
            }
            bool check = true;
            for (int i = 0; i < signature.Length; i++)
            {
                if (!opensignature[i].SequenceEqual(hashsignature[i])) check = false;
                //Console.WriteLine(BitConverter.ToString(opensignature[i]));
                //Console.WriteLine(BitConverter.ToString(hashsignature[i]));
            }

            return check;
        }

        public static byte[] GetChecksumBuffered(Stream stream)
        {
                var sha = new MD5CryptoServiceProvider();    
                byte[] checksum = sha.ComputeHash(stream);
                stream.Close();
                return checksum;
        }

        public static byte[,][] GetClosedKey(int size)
        {
            byte[,][] result = new byte[size, 2][];
            Random ran = new Random();
            for (int i = 0; i < size; i++)
            {
                byte[] key1 = new byte[size / 8];
                byte[] key2 = new byte[size / 8];
                for (int j = 0; j < size / 8; j++)
			    {
			        key1[j] = Convert.ToByte(ran.Next(0,256));
                    key2[j] = Convert.ToByte(ran.Next(0,256));
			    }
                result[i, 0] = key1;
                result[i, 1] = key2;
            }
            return result;
        }

        public static byte[,][] GetOpenedKey(byte[,][] closed)
        {
            byte[,][] result = new byte[L, 2][];
            var hash = new MD5CryptoServiceProvider();
            for (int i = 0; i < L; i++)
            {
                result[i, 0] = hash.ComputeHash(closed[i, 0]);
                result[i, 1] = hash.ComputeHash(closed[i, 1]);
            }
            return result;
        }
    }


}
