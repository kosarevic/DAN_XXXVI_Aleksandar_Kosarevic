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
        static void Main(string[] args)
        {
            Thread t = new Thread(GenerateMatrix);
            t.Start();
            
        }

        public static void GenerateMatrix()
        {
            int[,] matrix = new int[100, 100];
        }

        public static void GenerateNumbers()
        {
            Random r = new Random();

        }
    }
}
