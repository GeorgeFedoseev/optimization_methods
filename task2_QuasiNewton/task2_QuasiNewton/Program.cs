using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace task2_QuasiNewton
{
    class Program
    {
        static void Main(string[] args)
        {

            Matrix m1 = new Matrix(new double[,] { { 2, 0 }, { 0, 1 } });
            Matrix m2 = new Matrix(new double[,]{{1, 0}, {0, 1}});


            Console.WriteLine((m1));
            Console.WriteLine((m2));
            Console.WriteLine((m1*m2));

            Console.WriteLine(Matrix.ones(5));

            Console.ReadKey();
        }
    }
}
