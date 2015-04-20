using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task2_gradient_descent
{

    class vec2
    {
        public double x, y;
        public vec2(double _x = 0, double _y = 0)
        {
            x = _x; y = _y;
        }
    }

    class Program
    {
        public static double f1(vec2 v)
        {
            return 100 * Math.Pow(v.y - v.x * v.x, 2) + 5 * Math.Pow(1 - v.x, 2);
        }

        public static double f2(vec2 v)
        {
            return Math.Pow(v.x * v.x + v.y - 11, 2) + Math.Pow(v.x + v.y * v.y - 7, 2);
        }


        public double drv(Func<vec2, double> f, vec2 p, vec2 dir) {
            double h = 0.000001;
            return (-f(new vec2(p.x + 2*h*dir.x, p.y + 2*h*dir.y)) 
                    + 8*f(new vec2(p.x + h*dir.x, p.y + h*dir.y))
                    - 8*f(new vec2(p.x - h*dir.x, p.y - h*dir.y))
                    + f(new vec2(p.x - 2*h*dir.x, p.y - 2*h*dir.y))) / (12 * h);
        }

        public vec2 grad(Func<vec2, double> f, vec2 p) {
            return new vec2(
                drv(f, p, new vec2(1, 0)),
                drv(f, p, new vec2(0, 1))
            );
        }

        public vec2 grad_n(Func<vec2, double> f, vec2 p) {
            var gr = grad(f, p);
            var n = Math.Sqrt(gr.x*gr.x + gr.y*gr.y);
            return new vec2(
                    gr.x/n,
                    gr.y/n
                );
        }

        public vec2 getRndPoint(Random randNum, vec2[] range)
        {
            return new vec2(
                    range[0].x + randNum.NextDouble() * (range[1].x - range[0].x),
                    range[0].y + randNum.NextDouble() * (range[1].y - range[0].y)
                );
        }

        public double dichotomyMinLambda(Func<vec2, double> f, vec2 p, double[] range, double eps, double delta)
        {            
            double x1, x2;

            double from = range[0], to = range[1];

            while (Math.Abs(f_lambda(f, p, to) - f_lambda(f, p, from)) > eps && Math.Abs(to - from) > 4 * delta)
            {

                x1 = (to + from) / 2 - delta;
                x2 = (to + from) / 2 + delta;
                if (f_lambda(f, p, x1) >= f_lambda(f, p, x2))
                {
                    from = x1;
                }else{
                    to = x2;
                }

                var lmbd = (from + to) / 2;
                LogBoth(String.Format("  lmbd={2} f={3}", from, to, lmbd, f_lambda(f, p, lmbd)));
            }

            return (from + to) / 2;
        }

        public double f_lambda(Func<vec2, double> f, vec2 p, double lambda) {
            var gr = grad_n(f, p);
            return f(new vec2(
                    p.x - lambda*gr.x,
                    p.y - lambda*gr.y
                ));
        }

        public double n_vec(vec2 v) {
            return Math.Sqrt(v.x*v.x + v.y*v.y);
        }

        public vec2 getMin(Func<vec2, double> f, vec2[] range, double eps) { 
            Random rand = new Random();
            var p = getRndPoint(rand, range);
            LogBoth(String.Format("Starting at point: f({0}, {1}) = {2}", p.x, p.y, f(p)));
            vec2 p_prev = p;
            double[] lambdaRange = new double[]{0, 10};

            
            var i = 0;
            
            do{
                LogBoth(String.Format("\n\nITERATION {0}:", i));
                p_prev = p;
                var gr = grad_n(f, p_prev);
                LogBoth(String.Format("Looking lambda at point ({0}, {1}) in range [{2}, {3}]",
                    p_prev.x, p_prev.y, lambdaRange[0], lambdaRange[1]));

                var lmbd = dichotomyMinLambda(f, p_prev, lambdaRange, eps, eps/100);                
                LogBoth(String.Format("Best lambda is {0}", lmbd));
                                
                p = new vec2(
                    p_prev.x - lmbd * gr.x,
                    p_prev.y - lmbd * gr.y
                );

                var f_p = f(p);
                LogBoth(String.Format("New point: f({0}, {1}) = {2}", p.x, p.y, f_p));

                
                

                i++;
            } while (Math.Abs(f(p) - f(p_prev)) > eps && Math.Sqrt(Math.Pow(p.x - p_prev.x, 2) + Math.Pow(p.y - p_prev.y, 2)) > eps);


            return p;

        }


        
        static void Main(string[] args)
        {
            txtMirror = new StreamWriter("output.txt");

            var inst = new Program();


            bool flag = true;
            while (flag) {
                double from_x, from_y, to_x, to_y, eps;

                LogBoth("Input from_x: ");
                if (!double.TryParse(Console.ReadLine(), out from_x))
                {
                    Console.WriteLine("Wrong input");
                    continue;
                }else{
                    LogFile(from_x.ToString());
                }

                LogBoth("Input from_y: ");
                if (!double.TryParse(Console.ReadLine(), out from_y))
                {
                    Console.WriteLine("Wrong input");
                    continue;
                }else{
                    LogFile(from_y.ToString());
                }

                LogBoth("Input to_x: ");
                if (!double.TryParse(Console.ReadLine(), out to_x))
                {
                    Console.WriteLine("Wrong input");
                    continue;
                }else{
                    LogFile(to_x.ToString());
                }

                LogBoth("Input to_y: ");
                if (!double.TryParse(Console.ReadLine(), out to_y))
                {
                    Console.WriteLine("Wrong input");
                    continue;
                }
                else
                {
                    LogFile(to_y.ToString());
                }

                LogBoth("Input eps: ");
                if (!double.TryParse(Console.ReadLine(), out eps))
                {
                    Console.WriteLine("Wrong input");
                    continue;
                }else{
                    LogFile(eps.ToString());
                }

                var range = new vec2[] { new vec2(from_x, from_y), new vec2(to_x, to_y) };

                LogBoth("START Himmelblau");
                var hmmlbl_min = inst.getMin(f2, range, eps);
                LogBoth(String.Format("Found Himmelblau min: f({0}, {1}) = {2}", hmmlbl_min.x, hmmlbl_min.y, f2(hmmlbl_min)));
                
                Console.WriteLine("Press any key to try with Rosenbrock. Esc - exit");                
                if (Console.ReadKey().KeyChar == 27) {
                    LogBoth(String.Format("Found Himmelblau min: f({0}, {1}) = {2}", hmmlbl_min.x, hmmlbl_min.y, f2(hmmlbl_min)));
                    break;
                }

                LogBoth("START Rosenbrock");
                var rsnbrck_min = inst.getMin(f1, range, eps);
                LogBoth(String.Format("Found Rosenbrock min: f({0}, {1}) = {2}", rsnbrck_min.x, rsnbrck_min.y, f1(rsnbrck_min)));

                LogBoth("\n\n\nRESULTS:");
                LogBoth(String.Format("Found Rosenbrock min: f({0}, {1}) = {2}", rsnbrck_min.x, rsnbrck_min.y, f1(rsnbrck_min)));
                LogBoth(String.Format("Found Himmelblau min: f({0}, {1}) = {2}", hmmlbl_min.x, hmmlbl_min.y, f2(hmmlbl_min)));

                txtMirror.Flush();

                Console.WriteLine("File output created 'output.txt'.\nPress to try again. Esc - exit");
                if (Console.ReadKey().KeyChar == 27) {
                    break;
                }
            }



            
            
            Console.ReadKey();
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

    }
}
