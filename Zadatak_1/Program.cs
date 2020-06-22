using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Zadatak_1
{
    /// <summary>
    /// Application simulates example of Monitor.Pusle and Monitor.Wait.
    /// </summary>
    class Program
    {
        //Static variables necessary for application functionality.
        public static int[,] matrix;
        public static int[] numbers = new int[10000];
        public static readonly Random r = new Random();
        public static readonly object TheLock = new object();
        public static int[] unequalNumbers;
        public static bool done = false;

        static void Main(string[] args)
        {
            Thread t1 = new Thread(GenerateMatrix);
            Thread t2 = new Thread(GenerateNumbers);
            t1.Start();
            t2.Start();
            t1.Join();
            t2.Join();

            Thread t3 = new Thread(UnequalNumbers);
            Thread t4 = new Thread(DisplayUnequalNumbers);
            t3.Start();
            t4.Start();
            t3.Join();
            t4.Join();

            Console.ReadLine();
        }
        /// <summary>
        /// Method responsible for initiating the matrix and populating it with random numbers.
        /// </summary>
        public static void GenerateMatrix()
        {
            //Matrix is initialized.
            matrix = new int[100, 100];

            lock (TheLock)
            {
                //Untill Thread t2 finishes generating values, thread pauses here.
                if (numbers.Count() != 10000)
                {
                    Monitor.Wait(TheLock);
                }

                int count = 0;
                //Random numbers are being assigned to matrix in code below.
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
        /// <summary>
        /// Method responsible for generating 10000 random numbers ment for populating the matrix.
        /// </summary>
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
        /// <summary>
        /// Method finds all unequal numbers present in 10000 number list and stores them in a file.
        /// </summary>
        public static void UnequalNumbers()
        {
            List<int> temp = new List<int>();
            //Search for unequal numbers.
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

            lock (TheLock)
            {
                //Unequal numbers are being written in txt file.
                using (StreamWriter sw = new StreamWriter("..//..//Files/UnequalNumbers.txt"))
                {
                    foreach (int i in unequalNumbers)
                    {
                        sw.WriteLine(i);
                    }
                }

                done = true;
                Monitor.Pulse(TheLock);
            }
        }
        /// <summary>
        /// Method displays all unequal numbers written on a text file.
        /// </summary>
        public static void DisplayUnequalNumbers()
        {
            lock (TheLock)
            {
                if (!done)
                {
                    Monitor.Wait(TheLock);
                }
                //Unequal numbers are being displayed on console.
                string line = "";
                using (StreamReader sr = new StreamReader("..//..//Files/UnequalNumbers.txt"))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                    }
                }
            }
        }
    }
}
