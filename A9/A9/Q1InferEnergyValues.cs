using System;
using TestCommon;

namespace A9
{
    public class Q1InferEnergyValues : Processor
    {
        public long MATRIX_SIZE { get; private set; }

        private double[,] matrix;

        public Q1InferEnergyValues(string testDataName) : base(testDataName)
        {
            
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, double[,], double[]>)Solve);

        public double[] Solve(long MATRIX_SIZE, double[,] matrix)
        {
            this.MATRIX_SIZE = MATRIX_SIZE ;
            this.matrix = matrix ;
            // Comment the line below and write your code here
            return RowReduce( MATRIX_SIZE , matrix);
        }

        private double[] RowReduce(long MATRIX_SIZE, double[,] matrix)
        {
            for (int i = 0; i < MATRIX_SIZE; i++)
            {
                // Leftmost non-zero
                // Swap row to top
                if (matrix[i,i]== 0)
                {
                    int k = i ;
                    while(matrix[k,i]==0)   
                        k ++ ;

                    swapTwoRow(i , k);
                }   

                // rescale to make pivot 1
                divide(i , matrix[i,i]); 

                // Subtract row from others to make
                // other entries in column 0
                for (int j = 0; j < MATRIX_SIZE; j++)
                {
                    if(j!=i)
                        subtractRowFromOthers(i,j);                    
                }                
            }

            double[] output = new double[MATRIX_SIZE];

            for (int i = 0; i < MATRIX_SIZE; i++)            
                output[i] = round(matrix[i,MATRIX_SIZE]);            
            
            return output ;
        }

        private double round(double v)
        {
            double f = v , output = 1 ;
            if (v<0)            
                v *= -1 ;            
            int decimalPart = (int)v;
            double aasharPart = v- decimalPart ;

            if (aasharPart > 0.25 && aasharPart < 0.75)            
                output =  ( decimalPart + 0.5) ;

            if (aasharPart < 0.25)            
                output = decimalPart ;            

            if(aasharPart > 0.75)
                output = decimalPart + 1;        

            if(f<0)
                output *= -1 ;

            if(output == -0)
                output = 0;
            return output ;
            
        }

        private void subtractRowFromOthers(int i, int j)
        {
            var g = matrix[j,i] ;
            for (int k = 0; k <= MATRIX_SIZE; k++)       
            {
                matrix[j,k] += -1 * g * matrix[i,k] ;
            }                     
        }
        

        private void divide(int i, double v)
        {
            for (int j = 0; j <= MATRIX_SIZE; j++)            
                matrix[i,j] /= v ;            
        }

        private void swapTwoRow(int i, int k)
        {
            for (int j = 0; j <= MATRIX_SIZE; j++)
            {
                var tmp = matrix[i,j] ;
                matrix[i,j] = matrix[k,j] ;
                matrix[k,j] = tmp ;
            }
        }
    }
}
