using System;
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

        

        static double dichotomyMinX(double from, double to, double eps, double delta)
        {
            LogBoth(String.Format("DICHOTOMY START: EPS: {0}, DELTA: {1})", eps, delta));            
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

                LogBoth(String.Format("iteration {0}: x1={1}; x2={2}; from={3}; to={4}; middle={5}; val={6}",
                                        dichotomyIterations, x1, x2, from, to, (from + to) / 2, f((from + to) / 2))); 
            }

            return (from + to) / 2;
        }

        static double goldenSectionMinX(double from, double to, double eps) {
            LogBoth(String.Format("GOLDEN_SECTION START: EPS: {0})", eps)); 

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

                LogBoth(String.Format("iteration {0}: x1={1}; x2={2}; from={3}; to={4}; middle={5}; val={6}",
                                        goldenSectionIterations, x1, x2, from, to, (from + to) / 2, f((from + to) / 2))); 
            }

            return (to + from) / 2;
        }

        static void Main(string[] args)
        {

            txtMirror = new StreamWriter("output.txt");
            
            bool flag = true;
            while (flag) {
                double from, to, eps, delta;

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

                LogBoth("Input delta: ");
                if (!double.TryParse(Console.ReadLine(), out delta))
                {
                    Console.WriteLine("Wrong input");
                    continue;
                }
                else
                {
                    LogFile(delta.ToString());
                }


                LogBoth("\nRUN:\n");
                LogBoth("\nDICHOTOMY:\n");
                double minXD =  dichotomyMinX(from, to, eps, delta);

                LogBoth("\n\nGOLDEN SECTION:\n\n");
                double minXGS = goldenSectionMinX(from, to, eps);

                LogBoth(String.Format("\n\nDichotomyMinX: {0} ({1} iterations)", minXD, dichotomyIterations));
                LogBoth(String.Format("DichotomyMinValue: {0}", f(minXD)));

                
                LogBoth(String.Format("GoldenSectionMinX: {0} ({1} iterations)", minXGS, goldenSectionIterations));
                LogBoth(String.Format("GoldenSectionMinValue: {0}", f(minXGS)));

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
