using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace merkletree
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] files = Directory.GetFileSystemEntries("D:\\PHSH", "*.*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                Console.WriteLine(files[i]);
            }
            var hash = new MD5CryptoServiceProvider();
            LinkedList<byte[]> fileshash = new LinkedList<byte[]>();
            for (int i = 0; i < files.Length; i++)
			{
			    try 
                {
                    FileStream stream = new FileStream(files[i], FileMode.Open);
                    fileshash.AddLast(hash.ComputeHash(stream));
                    stream.Close();
                }
                catch (UnauthorizedAccessException e) { }
			}
            int lenfilehash = fileshash.Count;
            for (int i = 0; i < fileshash.Count; i++)
			{
			    Console.WriteLine(BitConverter.ToString(fileshash.ElementAt(i)));
			}

            int treehight = (int)(Math.Log(fileshash.Count, 2) + 1.99);
            
            LinkedList<byte[]>[] merkletree = new LinkedList<byte[]>[treehight];
            Console.WriteLine(fileshash.Count);
            merkletree[0] = fileshash;
            for (int i = 1; i < treehight; i++)
            {
                merkletree[i] = SumHash(fileshash);
                fileshash = merkletree[i];
            }
            Console.WriteLine();

            for (int i = 0; i < treehight; i++)
            {
                for (int j = 0; j < merkletree[i].Count; j++)
                {
                    Console.WriteLine(BitConverter.ToString(merkletree[i].ElementAt(j)));
                }
                Console.WriteLine();
                Console.WriteLine();
            }
            
            byte[] check = MerklePath("D://merkle//01. Wesley's Theory (Feat. George Clinton & Thundercat).mp3", 2, merkletree);
            Console.WriteLine(BitConverter.ToString(check));
            Console.WriteLine(treehight);
            Console.WriteLine(lenfilehash);
            
            
            Console.ReadKey();
        }

        public static LinkedList<byte[]> SumHash(LinkedList<byte[]> hashlist)
        {
            if (hashlist.Count % 2 != 0) hashlist.AddLast(hashlist.ElementAt(hashlist.Count - 1));
            LinkedList<byte[]> result = new LinkedList<byte[]>();
            var hash = new MD5CryptoServiceProvider();
            for (int i = 0; i < hashlist.Count; i += 2)
            {
                var sumhash = new byte[hashlist.ElementAt(0).Length * 2];
                hashlist.ElementAt(i).CopyTo(sumhash, 0);
                hashlist.ElementAt(i + 1).CopyTo(sumhash, hashlist.ElementAt(0).Length);
                result.AddLast(hash.ComputeHash(sumhash));
            }
            return result;
        }

        public static byte[] MerklePath(String filepath, int number, LinkedList<byte[]>[] tree)
        {
            var hash = new MD5CryptoServiceProvider();
            FileStream stream = new FileStream(filepath, FileMode.Open);
            byte[] hash1 = hash.ComputeHash(stream);
            stream.Close();
            int number1 = number;
            for (int i = 0; i < tree.Length - 1; i++)
            {
                var sumhash = new byte[hash1.Length * 2];
                int number2 = number1 % 2 != 0 ? number1 - 1 : number1 + 1;
                if (number1 > number2)
                {
                    tree[i].ElementAt(number2).CopyTo(sumhash, 0);
                    hash1.CopyTo(sumhash, hash1.Length);
                }
                else
                {
                    hash1.CopyTo(sumhash, 0);
                    tree[i].ElementAt(number2).CopyTo(sumhash, hash1.Length);                   
                }               
                hash1 = hash.ComputeHash(sumhash);
                number1 = number1 / 2;
            }
            return hash1;
        }
    }
}
