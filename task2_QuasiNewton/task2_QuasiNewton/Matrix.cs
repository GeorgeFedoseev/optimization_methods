using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task2_QuasiNewton
{
    class Matrix
    {

        protected double[,] data;

        public double x
        {
            get { return data[0, 0]; }
            set { data[0, 0] = value; }
        }

        public double y
        {
            get { return data[1, 0]; }
            set { data[1, 0] = value; }
        }

        public static Matrix vec2(double _x = 0, double _y = 0){
            Matrix m = new Matrix(2, 1);
            m.x = _x;
            m.y = _y;
            
            return m;
        }

        public Matrix(Matrix m) {
            data = m.data;
        }

        public Matrix(double[,] arr) {
            if (arr.GetLength(0) < 1 && arr.GetLength(1) < 0) {
                throw new Exception("Matrix cant be null");
            }

            data = arr;
        }
        
        public Matrix(int n, int m) {
            if(n < 1 || m < 1){
                throw new Exception("Matrix cant be null");
            }
            data = new double[n, m];            
        }

        public Matrix(int n)
        {   
            if(n < 1){
                throw new Exception("Matrix cant be null");
            }

            data = new double[n, n];
 
            for (int i = 0; i < n; i++) {
                data[i, i] = 1;
            }
        }


        

        static public Matrix operator -(Matrix m1)
        {            

            Matrix res = new Matrix(m1.getHeight(), m1.getWidth());

            for (int i = 0; i < m1.getHeight(); i++)
            {
                for (int j = 0; j < m1.getWidth(); j++)
                {
                    res.data[i, j] = -m1.data[i, j];
                }
            }

            return res;
        }

        static public Matrix operator -(Matrix m1, Matrix m2) {
            return m1 + (-m2);
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

        static public Matrix operator *(Matrix m1, double k) {
            Matrix res = new Matrix(m1.getHeight(), m1.getWidth());
            for (int i = 0; i < m1.getHeight(); i++) {
                for (int j = 0; j < m1.getWidth(); j++) {
                    res.data[i, j] = m1.data[i, j] * k;
                }
            }

            return res;
        }

        static public Matrix operator *(Matrix m1, Matrix m2) {
            if (m1.getWidth() != m2.getHeight()) {
                throw new Exception("Matrixes should: m1.w = m2.h for multpl");
            }

            Matrix res = new Matrix(m1.getHeight(), m2.getWidth());

            for (int i = 0; i < res.getHeight(); i++) {
                for (int j = 0; j < res.getWidth(); j++) {
                    double val = 0;
                    for(int r = 0; r < m1.getWidth(); r++){
                        val += m1.data[i, r] * m2.data[r, j];
                    }
                    res.data[i, j] = val;
                }
            }

            return res;
        }

        static public Matrix operator *(double k, Matrix m1) {
            return m1 * k;
        }

        public Matrix T() {
            Matrix res = new Matrix(getWidth(), getHeight());

            for (int i = 0; i < res.getHeight(); i++) {
                for (int j = 0; j < res.getWidth(); j++) {
                    res.data[i, j] = data[j, i];
                }
            }

            return res;

        }

        


        public double get(int i, int j){
            return data[i, j];
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
