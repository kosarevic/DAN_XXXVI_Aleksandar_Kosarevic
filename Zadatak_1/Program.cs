using System;
using System.Collections.Generic;
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
        public static readonly object TheLock = new object();

        static void Main(string[] args)
        {
            Thread t1 = new Thread(GenerateMatrix);
            Thread t2 = new Thread(GenerateNumbers);
            t1.Start();
            t2.Start();
            t1.Join();
            t2.Join();

            foreach (var item in matrix)
            {
                Console.WriteLine(item);
            }
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

                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        foreach (int k in numbers)
                        {
                            matrix[i, j] = k;
                        }
                    }
                } 
            }
        }

        public static void GenerateNumbers()
        {
            Random r = new Random();

            lock (TheLock)
            {
                for (int i = 0; i < 10000; i++)
                {
                    numbers[i] = r.Next(10, 99);
                }

                Monitor.Pulse(TheLock);
            }
        }
    }
}
