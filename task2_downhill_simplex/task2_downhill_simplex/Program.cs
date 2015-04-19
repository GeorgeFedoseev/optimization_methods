using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task2_downhill_simplex // Метод деформированного многогранника
{

    class vec2 { 
        public double x, y;
        public vec2(double _x = 0, double _y = 0){
            x = _x; y = _y;
        }
    }

    class Program
    {

        const double alpha = 1, beta = 2.8, gamma = 0.4;

        public static double f1(vec2 v) {
            return 100*Math.Pow(v.y - v.x*v.x, 2) + 5*Math.Pow(1 - v.x, 2);
        }

        public static double f2(vec2 v)
        {
            return Math.Pow(v.x * v.x + v.y - 11, 2) + Math.Pow(v.x + v.y * v.y - 7, 2);
        }

        public vec2 getRndPoint(Random randNum, vec2[] range) {            
            return new vec2(
                    range[0].x + randNum.NextDouble() * (range[1].x - range[0].x),
                    range[0].y + randNum.NextDouble() * (range[1].y - range[0].y)
                );
        }

        public vec2[] sortPoints(vec2[] points, Func<vec2, double> f) {
            vec2 max = points[0], min = points[0], middle = points[0];
            double[] vals = new double[]{f(points[0]), f(points[1]), f(points[2])};

            double max_val = vals[0], min_val = vals[0];
            for(int k = 0; k < 3; k++){
                if(vals[k] >= max_val){
                    max = points[k];
                    max_val = vals[k];
                }

                if(vals[k] <= min_val){
                    min = points[k];
                    min_val = vals[k];
                }
            }

            for (int k = 0; k < 3; k++) {
                if (!points[k].Equals(max) && !points[k].Equals(min)) {
                    middle = points[k];
                }
            }

            return new vec2[] {max, middle, min};

        }

        public vec2 getMin(Func<vec2, double> f, vec2[] range, double eps){            
            Random rand = new Random();
            var rnd_pts = new vec2[] { getRndPoint(rand, range), getRndPoint(rand, range), getRndPoint(rand, range) };
            

            var points = rnd_pts;

            while(true){

                var sorted_pts = sortPoints(points, f);
                var x_h = sorted_pts[0]; //highest
                var x_g = sorted_pts[1]; // middle
                var x_l = sorted_pts[2]; // lowest               

                var f_h = f(x_h);
                var f_g = f(x_g);
                var f_l = f(x_l);

                Console.WriteLine("high: f({0}, {1}) = {2} ", x_h.x, x_h.y, f_h);
                Console.WriteLine("middle: f({0}, {1}) = {2} ", x_g.x, x_g.y, f_g);
                Console.WriteLine("low: f({0}, {1}) = {2} \n\n", x_l.x, x_l.y, f_l);

                var x_c = new vec2( // center
                    (x_g.x + x_l.x)/2,
                    (x_g.y + x_l.y)/2
                );
                

                var x_r = new vec2( // reflected
                    (1+alpha)*x_c.x - alpha*x_h.x,
                    (1+alpha)*x_c.y - alpha*x_h.y
                );

                var f_r = f(x_r);

                if(f_r < f_l){
                    // отраженная точка меньше минимальной
                    // направление удачное, пробуем растянуть симплекс
                    var x_e = new vec2( // extension
                        (1 - beta) * x_c.x + beta * x_r.x,
                        (1 - beta) * x_c.y + beta * x_r.y
                    );

                    var f_e = f(x_e);

                    if (f_e < f_r) {
                        // растягиваем
                        x_h = x_e;
                        Console.WriteLine("Растяжение");
                    }else {
                        // переместились слишком далеко
                        x_h = x_r;
                        Console.WriteLine("Too far");
                    }
                }else if(f_r > f_l && f_r < f_g){
                    // отраженная точка между минимальной и средней
                    // выбор точки неплохой (новая лучше двух прежних f_g и f_h)
                    x_h = x_r;
                }else if(f_r > f_g ){
                    if (f_r < f_h) {
                        // отраженная точка между средней и максимальной
                        // меняем местами x_r с x_h и f_r с f_h
                        var tmp_x = x_r;
                        x_r = x_h; x_h = tmp_x;
                        var tmp_f = f_r;
                        f_r = f_h; f_h = tmp_f;
                    }else{
                        // отраженная точка хуже некуда
                    }

                    // производим сжатие
                    var x_s = new vec2( // squeeze
                        gamma * x_h.x + (1 - gamma) * x_c.x,
                        gamma * x_h.y + (1 - gamma) * x_c.y
                    );

                    var f_s = f(x_s);
                    Console.WriteLine("Сжатие");

                    if(f_s < f_h){
                        x_h = x_s;
                    }else{
                        // первоначальные точки оказались самыми удачными
                        // делаем глобальное сжатие симплекса

                        Console.WriteLine("Глобальное сжатие");
                        x_g.x = x_l.x + (x_g.x - x_l.x) / 2;
                        x_g.y = x_l.y + (x_g.y - x_l.y) / 2;

                        x_h.x = x_l.x + (x_h.x - x_l.x) / 2;
                        x_h.y = x_l.y + (x_h.y - x_l.y) / 2;


                    }
                
                }

                // условие выхода
                var prx = Math.Sqrt(
                       (Math.Pow(f(x_l) - f(x_h), 2)
                       + Math.Pow(f(x_g) - f(x_h), 2)
                       + Math.Pow(f(x_l) - f(x_g), 2))/3
                    );

                if (prx < eps) {                    
                    return x_l;
                }

                points = new vec2[]{x_h, x_g, x_l};

            }           
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


                LogBoth("START Rosenbrock");
                var rsnbrck_min = inst.getMin(f1, range, eps);
                Console.WriteLine("Found Rosenbrock min: f({0}, {1}) = {2}", rsnbrck_min.x, rsnbrck_min.y, f1(rsnbrck_min));

                LogBoth("START Himmelblau");
                var hmmlbl_min = inst.getMin(f2, range, eps);
                Console.WriteLine("Found Himmelblau min: f({0}, {1}) = {2}", hmmlbl_min.x, hmmlbl_min.y, f2(hmmlbl_min));

                LogBoth("\n\n\nRESULTS:");
                Console.WriteLine("Found Rosenbrock min: f({0}, {1}) = {2}", rsnbrck_min.x, rsnbrck_min.y, f1(rsnbrck_min));
                Console.WriteLine("Found Himmelblau min: f({0}, {1}) = {2}", hmmlbl_min.x, hmmlbl_min.y, f2(hmmlbl_min));
               

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
