using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace conjugate_gradients // метод сопряженных градиентов
{

    class vec2
    {
        public double x, y;
        public vec2(double _x = 0, double _y = 0)
        {
            x = _x; y = _y;
        }

        public static vec2 operator +(vec2 v1, vec2 v2) {
            return new vec2(
                    v1.x + v2.x,
                    v1.y + v2.y
                );
        }

        public static vec2 operator -(vec2 v1, vec2 v2) {
            return new vec2(
                    v1.x - v2.x,
                    v1.y - v2.y
                );
        }

        public static vec2 operator -(vec2 v1)
        {
            return new vec2(
                    -v1.x,
                    -v1.y
                );
        }

        public static vec2 operator *(vec2 v1, double k)
        {
            return new vec2(
                    v1.x*k,
                    v1.y*k
                );
        }

        public static vec2 operator *(double k, vec2 v1)
        {
            return new vec2(
                    v1.x * k,
                    v1.y * k
                );
        }

        public static explicit operator string(vec2 v)
        {
            return "("+v.x.ToString()+", "+v.y.ToString()+")";
        }

        public override string ToString(){
            return (string)this;
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


        public double drv(Func<vec2, double> f, vec2 p, vec2 dir)
        {
            double h = 0.000001;
            return (-f(new vec2(p.x + 2 * h * dir.x, p.y + 2 * h * dir.y))
                    + 8 * f(new vec2(p.x + h * dir.x, p.y + h * dir.y))
                    - 8 * f(new vec2(p.x - h * dir.x, p.y - h * dir.y))
                    + f(new vec2(p.x - 2 * h * dir.x, p.y - 2 * h * dir.y))) / (12 * h);
        }

        public vec2 grad(Func<vec2, double> f, vec2 p)
        {
            return new vec2(
                drv(f, p, new vec2(1, 0)),
                drv(f, p, new vec2(0, 1))
            );
        }

        public vec2 grad_n(Func<vec2, double> f, vec2 p)
        {
            var gr = grad(f, p);
            var n = Math.Sqrt(gr.x * gr.x + gr.y * gr.y);
            return new vec2(
                    gr.x / n,
                    gr.y / n
                );
        }
                

        public double argmin_lambda(Func<vec2, double> f, vec2 p, vec2 s, double[] range, double eps)
        {
            double x1, x2;

            double delta = eps / 10;

            double from = range[0], to = range[1];

            while (Math.Abs(f_lambda(f, p, s, to) - f_lambda(f, p, s, from)) > eps && Math.Abs(to - from) > 4 * delta)
            {

                x1 = (to + from) / 2 - delta;
                x2 = (to + from) / 2 + delta;
                if (f_lambda(f, p, s, x1) >= f_lambda(f, p, s, x2))
                {
                    from = x1;
                }
                else
                {
                    to = x2;
                }

                var lmbd = (from + to) / 2;
                LogBoth(String.Format("        lmbd={2} f={3}", from, to, lmbd, f_lambda(f, p, s, lmbd)));
            }

            return (from + to) / 2;
        }

        public double f_lambda(Func<vec2, double> f, vec2 p, vec2 s,  double lambda)
        {
            var gr = grad_n(f, p);
            return f(p + lambda*s);
        }

        public double n_vec(vec2 v)
        {
            return Math.Sqrt(v.x * v.x + v.y * v.y);
        }


        public vec2 getMin(Func<vec2, double> f, vec2 startPoint, double eps) {
            vec2 xk = startPoint;

            var k = 0;
            while (true) {
                LogBoth(String.Format("k = {0}", k));
                vec2 skj = -grad_n(f, xk);

                vec2 xkj = xk;
                var j = 0;
                while (true) {
                    LogBoth(String.Format("    j = {0}", j));
                    double lambda = argmin_lambda(f, xkj, skj, new double[] { -1, 1 }, eps);
                    vec2 xkj_next = xkj + lambda*skj;
                    double w = Math.Pow(n_vec(grad_n(f, xkj_next)), 2) /
                            Math.Pow(n_vec(grad_n(f, xkj)), 2);
                    vec2 skj_next = -grad_n(f, xkj_next) + w * skj;

                    LogBoth(String.Format("check conditions: ||Skj_next|| = {0}; |xkj_next - xkj| = {1}; eps = {2}",
                                                n_vec(skj_next), n_vec(xkj_next - xkj),
                                                eps));
                    if (n_vec(skj_next) < eps || n_vec(xkj_next - xkj) < eps) {
                        LogBoth(String.Format("FOUND MIN at {0}", xkj_next.ToString()));
                        return xkj_next;
                    }else{
                        if (j + 1 < 2) {
                            j++;
                            continue;
                        }else{
                            LogBoth(String.Format("next k, at point {0}", xkj_next.ToString()));
                            xk = xkj_next;
                            break;
                        }
                    }
                }

                k++;
            }
        }

        static void Main(string[] args)
        {
            txtMirror = new StreamWriter("output.txt");
            var inst = new Program();

            bool flag = true;
            while (flag)
            {
                double start_x, start_y, eps;

                LogBoth("Input start_x: ");
                if (!double.TryParse(Console.ReadLine(), out start_x))
                {
                    Console.WriteLine("Wrong input");
                    continue;
                }
                else
                {
                    LogFile(start_x.ToString());
                }

                LogBoth("Input start_y: ");
                if (!double.TryParse(Console.ReadLine(), out start_y))
                {
                    Console.WriteLine("Wrong input");
                    continue;
                }
                else
                {
                    LogFile(start_y.ToString());
                }


                LogBoth("Input eps: ");
                if (!double.TryParse(Console.ReadLine(), out eps))
                {
                    Console.WriteLine("Wrong input");
                    continue;
                }
                else
                {
                    LogFile(eps.ToString());
                }

                vec2 startPoint = new vec2(start_x, start_y);

                LogBoth("START Himmelblau");
                var hmmlbl_min = inst.getMin(f2, startPoint, eps);
                LogBoth(String.Format("Found Himmelblau min: f({0}, {1}) = {2}", hmmlbl_min.x, hmmlbl_min.y, f2(hmmlbl_min)));

                Console.WriteLine("Press any key to try with Rosenbrock. Esc - exit");
                if (Console.ReadKey().KeyChar == 27)
                {                    
                    break;
                }

                LogBoth("START Rosenbrock");
                var rsnbrck_min = inst.getMin(f1, startPoint, eps);
                LogBoth(String.Format("Found Rosenbrock min: f({0}, {1}) = {2}", rsnbrck_min.x, rsnbrck_min.y, f1(rsnbrck_min)));

                LogBoth("\n\n\nRESULTS:");
                LogBoth(String.Format("Found Rosenbrock min: f({0}, {1}) = {2}", rsnbrck_min.x, rsnbrck_min.y, f1(rsnbrck_min)));
                LogBoth(String.Format("Found Himmelblau min: f({0}, {1}) = {2}", hmmlbl_min.x, hmmlbl_min.y, f2(hmmlbl_min)));

                txtMirror.Flush();

                Console.WriteLine("File output created 'output.txt'.\nPress to try again. Esc - exit");
                if (Console.ReadKey().KeyChar == 27)
                {
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
    }
}
