using System;
using System.Collections.Generic;
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

        public vec2 getMin(Func<vec2, double> f){
            var range = new vec2[]{new vec2(0, 0), new vec2(10, 10)};
            Random rand = new Random();
            var rnd_pts = new vec2[] { getRndPoint(rand, range), getRndPoint(rand, range), getRndPoint(rand, range) };
            var sorted_pts = sortPoints(rnd_pts, f);

            var xc = new vec2(
                (sorted_pts[1].x + sorted_pts[2].x)/2,
                (sorted_pts[1].y + sorted_pts[2].y)/2
            );

            /*Console.WriteLine("SORTED PTS:");
            foreach (vec2 pt in sorted_pts) {
                Console.Write("f({0}, {1}) = {2} ", pt.x, pt.y, f(pt));
            }*/

            

            return new vec2();
        }

        static void Main(string[] args)
        {
            var inst = new Program();
            inst.getMin(Program.f1);
            Console.ReadKey();
        }
    }
}
