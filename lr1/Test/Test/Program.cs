using Newtonsoft.Json.Linq;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        //ST1000DM 003-1CH162: 1Тб, Latency 4.16ms, 7200rpm, write/read speed 156/156 Мб/с
        //Kingston [KVR1333D3N9/4G] Retail x2: 4GB, 1333Мгц, Latency 9, write/read speed 10600/10600Мб/с
        static void Main(string[] args)
        {
            var path = "D:\\HPIM6667.mov";
            MeasureSpeedReadFromDisk(5, 16384, path);
            MeasureTimeToSeekDrive(100, path);
            MeasureTimeOfRAM(100000, path);
            MeasureTimeOfCacheToAccessMemory(path);

            Console.ReadKey();
        }

        static void MeasureSpeedReadFromDisk(int experimentsCount, int blockSize_, string path)
        {
            var stopwatch = new Stopwatch();
            const int FILE_FLAG_NO_BUFFERING = 0x20000000;

            for (int i = 0; i < experimentsCount; i++)
            {
                using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, blockSize_, (FileOptions)FILE_FLAG_NO_BUFFERING))
                {
                    var bytes = new byte[fileStream.Length];
                    var blockSize = blockSize_;
                    var bytesReading = 0;
                    var bytesRemaining = fileStream.Length;

                    stopwatch.Start();
                    while (bytesRemaining > 0)
                    {
                        fileStream.Read(bytes, bytesReading, blockSize);
                        bytesReading += blockSize;
                        bytesRemaining -= blockSize;

                        if (bytesRemaining < blockSize)
                            blockSize = (int)bytesRemaining;
                    }
                    stopwatch.Stop();
                }
            }
            var averageSpeed = GetFileSizeInMb(path) / (stopwatch.Elapsed.TotalSeconds / experimentsCount);
            Console.WriteLine(String.Format("\n1) Speed of the serial read data from disk: {0:0.0} MB/s", averageSpeed));
        }

        static void MeasureTimeToSeekDrive(int experimentsCount, string path)
        {
            var random = new Random();
            var stopwatch = new Stopwatch();

            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                for (int i = 0; i < experimentsCount; i++)
                {
                    var newPosition = random.Next(int.MaxValue);
                    stopwatch.Start();
                    fileStream.Seek(newPosition, SeekOrigin.Begin);
                    fileStream.ReadByte();
                    stopwatch.Stop();
                }
            }
            var averageTime = stopwatch.Elapsed.TotalMilliseconds / experimentsCount;
            Console.WriteLine(String.Format("2) Time to seek of your drive: {0:0.0} milliseconds", averageTime));
        }

        static void MeasureTimeOfRAM(int experimentsCount, string path)
        {
            var random = new Random();
            var stopwatch = new Stopwatch();

            using (var memoryStream = new MemoryStream(File.ReadAllBytes(path)))
            {
                for (int i = 0; i < experimentsCount; i++)
                {
                    var newPosition = random.Next(int.MaxValue);

                    stopwatch.Start();
                    memoryStream.Seek(newPosition, SeekOrigin.Begin);
                    stopwatch.Stop();
                    memoryStream.ReadByte();
                }
            }
            var averageTime = stopwatch.Elapsed.TotalMilliseconds * 1000000 / experimentsCount;
            Console.WriteLine(String.Format("3) Time of random access memory: {0:0.0} nanoseconds", averageTime));
        }

        static void MeasureTimeOfCacheToAccessMemory(string path)
        {
            var stopwatch = new Stopwatch();

            var memoryStream = new MemoryStream(File.ReadAllBytes(path));

            var bytes = new byte[memoryStream.Length];
            stopwatch.Start();
            memoryStream.Read(bytes, 0, (int)memoryStream.Length);
            stopwatch.Stop();

            memoryStream.Close();

            var speed = GetFileSizeInMb(path) / (stopwatch.Elapsed.TotalSeconds);
            Console.WriteLine(String.Format("4) Time of the cache/sequential access to the memory : {0:0.0} MB/s", speed));
        }

        static long GetFileSizeInMb(string path)
        {
            return new FileInfo(path).Length / 1048576;
        }

        static void CreateSerialFile(string path, int linesCount)
        {
            // 50000000 = 1.33Gb
            using (var binaryWriter = new BinaryWriter(File.Open(path, FileMode.Create, FileAccess.Write)))
            {
                for (int i = 0; i < linesCount; i++)
                    binaryWriter.Write(String.Format("Create line number: {0}", i));
            }
        }
    }
}