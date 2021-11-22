using System;
using System.Collections.Generic;

namespace A5
{
    public class Q2OptimalDiet 
    {    
        private int equationsCount;
        private bool isInfinity;
        private int variablesCount;
        private double[,] matrix;
        private List<List<int>> ascdsg;
        private double maximumValue;
        private double[][] tabularForm;
        private double[] solution;
        private string situation ;
        private bool[] used;
        private bool isThereIntersection;
        private bool isThereSolution;

        public int MATRIX_SIZE { get; private set; }
        public static int Main()
        {
            string[] sizeSplited = System.Console.ReadLine().Split(' ');
            int N =(int) Int64.Parse(sizeSplited[0].ToString()) ; // eq
            int M =(int) Int64.Parse(sizeSplited[1].ToString()) ; // vars 
            
            double[,] matrix = new double[N+1,M+1] ;                        
            string[] aSplited ;

            for (int i = 0; i < N; i++)            
            {                
                aSplited = System.Console.ReadLine().Split(' ');
                for (int j = 0; j < M; j++)            
                    matrix[i,j] = long.Parse(aSplited[j]);                                    
            }                    

            aSplited = System.Console.ReadLine().Split(' ');
            for (int i = 0; i < N; i++)            
                matrix[i,M] = double.Parse(aSplited[i]);            

            aSplited = System.Console.ReadLine().Split(' ');
            for (int i = 0; i < M; i++)
                matrix[N,i] = double.Parse(aSplited[i])            ;

            Q2OptimalDiet obj = new Q2OptimalDiet();

            string output = obj.Solve(N , M , matrix);

            System.Console.WriteLine(output);

            return 0 ;
        } 
        
        public string Solve(int N, int M, double[,] matrix)
        {
            this.equationsCount = N;
            this.variablesCount = M;
            this.MATRIX_SIZE = variablesCount ;
            this.solution = new double[variablesCount] ;
            this.used = new bool[MATRIX_SIZE];
            this.isThereSolution = false ;
            this.matrix = matrix ;
            this.ascdsg = new List<List<int>>();
            this.maximumValue = double.MinValue ;                
                                    
            // findMaximumValue (0, new List<int>()) ;

            int[] shit = new int[equationsCount+variablesCount+1];
            for (int i = 0; i < equationsCount + variablesCount+1; i++)            
                shit[i] = i ;    

            var subsets = CreateSubsets<int> (shit);

            for (int i = 0; i < subsets.Count; i++)            
                if (subsets[i].Length == variablesCount)    
                {                                
                    findIntersection(subsets[i]);   
                    // if(situation == "Infinity")         
                    //     return "Infinity" ;
                    // if(isThereIntersection == false)
                    //     return "No solution" ;
                }


            if(isThereSolution == false)
                return "No solution" ;     
            if(situation !="Bounded solution")
                return situation ;             

            situation += '\n' ;
            for (int i = 0; i < variablesCount; i++)            
                situation += solution[i] + " " ;     

            return situation ;       
        }    

        List<T[]> CreateSubsets<T>(T[] originalArray)
        {
            List<T[]> subsets = new List<T[]>();

            for (int i = 0; i < originalArray.Length; i++)
            {
                int subsetCount = subsets.Count;
                subsets.Add(new T[] { originalArray[i] });

                for (int j = 0; j < subsetCount; j++)
                {
                    T[] newSubset = new T[subsets[j].Length + 1];
                    subsets[j].CopyTo(newSubset, 0);
                    newSubset[newSubset.Length - 1] = originalArray[i];
                    subsets.Add(newSubset);
                }
            }

            return subsets;
        }

        private void findIntersection ( int[] inequalities )
        {   
            double[,] inequalsTable = new double[variablesCount , variablesCount+1] ;
            for (int i = 0; i < variablesCount; i++)            
            {
                if(inequalities[i] >= equationsCount + variablesCount)
                {
                    for (int j = 0; j < variablesCount; j++)                    
                        inequalsTable[i,j] = 1;             

                    inequalsTable[i,variablesCount] = Math.Pow(10,9);
                }
                else if ( inequalities[i]>=equationsCount)                
                    inequalsTable[i , inequalities[i] - equationsCount] = -1 ;                
                else
                    for (int j = 0; j <= variablesCount; j++)                
                        inequalsTable[i,j] = matrix[inequalities[i] , j] ;                
            }

            RowReduce (inequalsTable);
        }

        private void RowReduce( double[,] matrix)
        {            

            for (int i = 0; i < MATRIX_SIZE; i++)
            {
                // Leftmost non-zero
                // Swap row to top
                if (matrix[i,i]== 0)
                {
                    int k = i ;
                    while(k < MATRIX_SIZE && matrix[k,i]==0)   
                        k ++ ;

                    if(k<MATRIX_SIZE)
                        swapTwoRow(i , k , matrix);

                    else               
                    {
                        // double c = 1 ;
                        // for (int j = 0; j < MATRIX_SIZE; j++)                        
                        //     c *= matrix[j, variablesCount] ;                        

                        // if(MATRIX_SIZE %2 == 0 && c<0)


                        return ;                    
                    }                         
                }   

                // rescale to make pivot 1
                if(matrix[i,i] != 1 )
                // && matrix[i,i] != 0)
                    divide(i , matrix[i,i] , matrix); 

                // Subtract row from others to make
                // other entries in column 0
                for (int j = 0; j < MATRIX_SIZE; j++)
                {
                    if(j!=i)
                        subtractRowFromOthers(i,j, matrix);                    
                }                
            }            
            // situation = "No solution" ;            

            for (int i = 0; i < equationsCount; i++)
            {                
                //check for other inequalities 
                double a = 0 ;
                for (int j = 0; j < variablesCount; j++)                          
                    a += this.matrix[i,j] * matrix[j,variablesCount] ;                                                

                // double difference = Math.Abs(a*0.0000001) ;
                if (a>this.matrix[i,variablesCount] + .001 )      
                // if(a - this.matrix[i,variablesCount] >= .00001)                
                    return ;                                             
            }
            double value = 0 ;
            double sumOfAmounts = 0 ;
            for (int i = 0; i < MATRIX_SIZE; i++)            
            {
                if(matrix[i ,MATRIX_SIZE] < 0 - .001)
                    return ;   

                sumOfAmounts += matrix[i,MATRIX_SIZE] ;

                value += this.matrix[equationsCount,i] * matrix[i,MATRIX_SIZE];            
            }

            this.isThereSolution = true ;

            if (value > maximumValue)        
            {                
                this.maximumValue = value ;            
                if(sumOfAmounts > Math.Pow(10,9) - .001)
                {                
                    situation = "Infinity";
                    return ;
                }            
                situation = "Bounded solution" ;
                for (int i = 0; i < MATRIX_SIZE ; i++)                
                    this.solution[i] = matrix[i,MATRIX_SIZE];                
            }
            
            // return output ;
        }

        private void divide(int i, double v , double[,] matrix)
        {
            for (int j = 0; j <= MATRIX_SIZE; j++)            
                matrix[i,j] /= v ;            
        }

        
        private void swapTwoRow(int i, int k, double[,] matrix)
        {
            for (int j = 0; j <= MATRIX_SIZE; j++)
            {
                var tmp = matrix[i,j] ;
                matrix[i,j] = matrix[k,j] ;
                matrix[k,j] = tmp ;
            }
        }

        private void subtractRowFromOthers(int i, int j , double[,] matrix)
        {
            var g = matrix[j,i] ;
            for (int k = 0; k <= MATRIX_SIZE; k++)       
            {
                matrix[j,k] += -1 * g * matrix[i,k] ;
            }                     
        }

        private void multiply(int i, double v, long MATRIX_WIDTH)
        {
            for (int j = 0; j <= MATRIX_WIDTH; j++)            
                matrix[i,j] *= v ;            
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

    }
}
