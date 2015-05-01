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

        public static vec2 operator +(vec2 v1)
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
                }
                else
                {
                    to = x2;
                }

                var lmbd = (from + to) / 2;
                LogBoth(String.Format("  lmbd={2} f={3}", from, to, lmbd, f_lambda(f, p, lmbd)));
            }

            return (from + to) / 2;
        }

        public double f_lambda(Func<vec2, double> f, vec2 p, double lambda)
        {
            var gr = grad_n(f, p);
            return f(new vec2(
                    p.x - lambda * gr.x,
                    p.y - lambda * gr.y
                ));
        }

        public double n_vec(vec2 v)
        {
            return Math.Sqrt(v.x * v.x + v.y * v.y);
        }


        public vec2 getMin(Func<vec2, double> f, vec2 startPoint, double eps) {
            vec2 p = startPoint;


            return p;
        }

        static void Main(string[] args)
        {
            txtMirror = new StreamWriter("output.txt");
            var inst = new Program();

            


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
