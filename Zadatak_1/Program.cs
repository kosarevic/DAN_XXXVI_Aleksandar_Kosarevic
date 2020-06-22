using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Zadatak_1
{
    class Program
    {
        public static int[,] matrix;
        public static int[] numbers = new int[10000];
        public static readonly Random r = new Random();
        public static readonly object TheLock = new object();
        public static int[] unequalNumbers;

        static void Main(string[] args)
        {
            Thread t1 = new Thread(GenerateMatrix);
            Thread t2 = new Thread(GenerateNumbers);
            t1.Start();
            t2.Start();
            t1.Join();
            t2.Join();

            Thread t3 = new Thread(UnequalNumbers);
            t3.Start();
            t3.Join();

            Console.ReadLine();
        }

        public static void GenerateMatrix()
        {
            matrix = new int[100, 100];

            lock (TheLock)
            {
                while (numbers.Count() != 10000)
                {
                    Monitor.Wait(TheLock);
                }

                int count = 0;

                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        matrix[i, j] = numbers[count];
                        count++;
                    }
                }
            }
        }

        public static void GenerateNumbers()
        {
            lock (TheLock)
            {
                for (int i = 0; i < 10000; i++)
                {
                    numbers[i] = r.Next(10, 99);
                }

                Monitor.Pulse(TheLock);
            }
        }

        public static void UnequalNumbers()
        {
            List<int> temp = new List<int>();

            foreach (int i in matrix)
            {
                if (i % 2 != 0)
                {
                    temp.Add(i);
                }
            }

            unequalNumbers = new int[temp.Count];
            int count = 0;

            for (int i = 0; i < unequalNumbers.Length; i++)
            {
                unequalNumbers[i] = temp[count];
                count++;
            }

            using (StreamWriter sw = new StreamWriter("..//..//Files/UnequalNumbers.txt"))
            {
                foreach (int i in unequalNumbers)
                {
                    sw.WriteLine(i);
                }
            }
        }
    }
}
