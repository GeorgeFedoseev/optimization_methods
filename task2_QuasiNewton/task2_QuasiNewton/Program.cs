using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace task2_QuasiNewton // квазиньютоновский метод
{
        

    class Program
    {

        public static double f1(Matrix v)
        {
            return 100 * Math.Pow(v.y - v.x * v.x, 2) + 5 * Math.Pow(1 - v.x, 2);
        }

        public static double f2(Matrix v)
        {
            return Math.Pow(v.x * v.x + v.y - 11, 2) + Math.Pow(v.x + v.y * v.y - 7, 2);
        }


        public double drv(Func<Matrix, double> f, Matrix p, Matrix dir)
        {
            double h = 0.000001;
            return (-f(Matrix.vec2(p.x + 2 * h * dir.x, p.y + 2 * h * dir.y))
                    + 8 * f(Matrix.vec2(p.x + h * dir.x, p.y + h * dir.y))
                    - 8 * f(Matrix.vec2(p.x - h * dir.x, p.y - h * dir.y))
                    + f(Matrix.vec2(p.x - 2 * h * dir.x, p.y - 2 * h * dir.y))) / (12 * h);
        }

        public Matrix grad(Func<Matrix, double> f, Matrix p)
        {
            return Matrix.vec2(
                drv(f, p, Matrix.vec2(1, 0)),
                drv(f, p, Matrix.vec2(0, 1))
            );
        }

        public Matrix grad_n(Func<Matrix, double> f, Matrix p)
        {
            var gr = grad(f, p);
            var n = Math.Sqrt(gr.x * gr.x + gr.y * gr.y);
            return Matrix.vec2(
                    gr.x / n,
                    gr.y / n
                );
        }


        public double argmin_lambda(Func<Matrix, double> f, Matrix x, Matrix H, double[] range, double eps)
        {
            double x1, x2;

            

            double from = range[0], to = range[1];

            double delta = eps;

            while (Math.Abs(f_lambda(f, x, H, to) - f_lambda(f, x, H, from)) > eps && Math.Abs(to - from) > 4 * delta)
            {

                x1 = (to + from) / 2 - delta;
                x2 = (to + from) / 2 + delta;
                if (f_lambda(f, x, H, x1) >= f_lambda(f, x, H, x2))
                {
                    from = x1;
                }
                else
                {
                    to = x2;
                }

                var lmbd = (from + to) / 2;
                LogBoth(String.Format("    lmbd={2} f={3} from={0} to={1}", from, to, lmbd, f_lambda(f, x, H, lmbd)));
            }

            return (from + to) / 2;
        }

        public double f_lambda(Func<Matrix, double> f, Matrix x, Matrix H, double lambda)
        {
            var gr = grad_n(f, x);
            return f(x - lambda * H * gr);
        }

        public double n_vec(Matrix v)
        {
            return Math.Sqrt(v.x * v.x + v.y * v.y);
        }


        public Matrix getMin(Func<Matrix, double> f, Matrix startPoint, double eps)
        {
            Matrix H = new Matrix(2);
            Matrix x = startPoint;

            int k = 0;
            
            while (true) {
                var gr = grad(f, x);
                double lambda = argmin_lambda(f, x, H, new double[] { -n_vec(gr), n_vec(gr) }, eps);

                Matrix x_next = x - lambda * H * gr * (1/n_vec(gr));
                Matrix H_next;

                if ((k + 1) % 2 == 0) {
                    H_next = new Matrix(2);
                }else{
                    Matrix dx = x_next - x;
                    Matrix y = grad(f, x_next) - grad(f, x);
                    H_next = H + ((dx - H * y) * (dx - H * y).T())
                                        * (1/((dx - H*y).T()*y).get(0, 0));
                }


                LogBoth(String.Format("Check stop conditions: |grad| = {0}; |dx| = {1}; eps = {2}",
                        n_vec(grad(f, x_next)), n_vec(x_next - x), eps));
                if (n_vec(grad_n(f, x_next)) <= eps || n_vec(x_next - x) <= eps)
                {
                    return x_next;
                }

                LogBoth(String.Format("[k={3}]: f({0}, {1}) = {2}", x_next.x, x_next.y, f(x_next), k));

                x = x_next;
                H = H_next;
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

                Matrix startPoint = Matrix.vec2(start_x, start_y);

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
