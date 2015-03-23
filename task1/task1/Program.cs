﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task1
{
    class Program
    {
        const double a = 211.0/4.0, b = 9;
        const double phi = (1.0 + 2.236) / 2.0;
        

        static long dichotomyIterations, goldenSectionIterations;

        

        static double dichotomyMinX(double from, double to, double eps)
        {
            double delta = eps / 10;
            double x1, x2;
            dichotomyIterations = 0;
            while (Math.Abs(to - from) > eps && Math.Abs(to - from) >= 2 * delta)
            {
                x1 = (to + from) / 2 - delta;
                x2 = (to + from) / 2 + delta;
                if (f(x1) > f(x2)) {
                    from = x1;
                }else{
                    to = x2;
                }

                dichotomyIterations++;
            }

            return (from + to) / 2;
        }

        static double goldenSectionMinX(double from, double to, double eps) {
            double x1, x2;

            goldenSectionIterations = 0;
            while (Math.Abs(to - from) > eps) { 
                x1 = to - (to-from)/phi;
                x2 = from + (to - from) / phi;
                if(f(x1) >= f(x2)){
                    from = x1;
                }else{
                    to = x2;
                }

                goldenSectionIterations++;
            }

            return (to + from) / 2;
        }

        static void Main(string[] args)
        {

            txtMirror = new StreamWriter("output.txt");
            
            bool flag = true;
            while (flag) {
                double from, to, eps;

                LogBoth("Input from: ");
                if (!double.TryParse(Console.ReadLine(), out from))
                {
                    Console.WriteLine("Wrong input");
                    continue;
                }else{
                    LogFile(from.ToString());
                }
                LogBoth("Input to: ");
                if (!double.TryParse(Console.ReadLine(), out to))
                {
                    Console.WriteLine("Wrong input");
                    continue;
                }else{
                    LogFile(to.ToString());
                }
                LogBoth("Input eps: ");
                if (!double.TryParse(Console.ReadLine(), out eps))
                {
                    Console.WriteLine("Wrong input");
                    continue;
                }else{
                    LogFile(eps.ToString());
                }



                LogBoth(String.Format("DichotomyMinX: {0} ({1} iterations)", dichotomyMinX(from, to, eps), dichotomyIterations));
                LogBoth(String.Format("DichotomyMinValue: {0}", f(dichotomyMinX(from, to, eps))));

                LogBoth(String.Format("GoldenSectionMinX: {0} ({1} iterations)", goldenSectionMinX(from, to, eps), goldenSectionIterations));
                LogBoth(String.Format("GoldenSectionMinValue: {0}", f(goldenSectionMinX(from, to, eps))));

                txtMirror.Flush();

                Console.WriteLine("File output created 'output.txt'.\nPress to try again. Esc - exit");
                if (Console.ReadKey().KeyChar == 27) {
                    break;
                }
            }
            
            
        }

        static TextWriter txtMirror;

        static void LogBoth(string strText)
        {
            Console.WriteLine(strText);
            txtMirror.WriteLine(strText);
        }

        static void LogFile(string strText)
        {
            txtMirror.WriteLine(strText);
        }

        static double f(double x)
        {
            return a / Math.Exp(x) + b * x;
        }
    }
}