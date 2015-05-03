using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task2_QuasiNewton
{
    class Matrix
    {

        double[,] data;

        public Matrix(double[,] arr) {
            if (arr.GetLength(0) > 0 && arr.GetLength(1) > 0) {
                data = arr;
            }else{
                throw new Exception("Matrix cant be null");
            }
        }
        
        public Matrix(int n, int m) {
            if(n == 0 || m == 0){
                throw new Exception("Matrix cant be null");
            }
            data = new double[n, m];            
        }

        static public Matrix operator +(Matrix m1, Matrix m2) {
            if (m1.getWidth() != m2.getWidth() || m1.getHeight() != m2.getHeight()) {
                throw new Exception("Matrix must have same dim for adding");
            }

            Matrix res = new Matrix(m1.getHeight(), m1.getWidth());

            for (int i = 0; i < m1.getHeight(); i++) { 
                for (int j = 0; j < m1.getWidth(); j++) {
                    res.data[i, j] = m1.data[i, j] + m2.data[i, j];
                }
            }

            return res;
        }

        public int getHeight(){
            return data.GetLength(0);
        }

        public int getWidth(){
            return data.GetLength(1);
        }


        public override string ToString(){
            var output = "";
            for (int i = 0; i < getHeight(); i++) {
                var row = "";
                for (int j = 0; j < getWidth(); j++) {
                    row += " " + data[i, j].ToString() + " ";
                }
                row = "[" + row + "]";
                output += row + "\n";
            }

            return output;
        }
    }
}
