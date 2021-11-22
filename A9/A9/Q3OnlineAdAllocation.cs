using System;
using System.Collections.Generic;
using TestCommon;

namespace A9
{
    public class Q3OnlineAdAllocation : Processor
    {
        private int equationsCount;
        private int variablesCount;
        private double[,] matrix;
        private double[][] tabularForm;
        private List<int> stackVariable;
        private List<int> artificialVariable;
        private List<int> surplusVariable;

        private long[] outputVariablesIndex ;

        public long M { get; private set; }                    

        public Q3OnlineAdAllocation(string testDataName) : base(testDataName)
        {
            // this.ExcludeTestCases(2,27,32,39);
            // for (int i = 1; i <= 38; i++)            
            //     this.ExcludeTestCases(i);
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<int, int, double[,], String>)Solve);

        public string Solve(int c, int v, double[,] matrix1)
        {
            // Comment the line below and write your code here
            this.equationsCount =c;
            this.variablesCount = v;
            this.matrix = matrix1 ;
            this.tabularForm = new double[equationsCount+1][];
            
            this.artificialVariable = new List<int>();
            this.stackVariable = new List<int>();
            this.surplusVariable = new List<int>();                


            // Comment the line below and write your code here
            return Simplex();
        }

        
                private void InitializePahse1(double[,] matrix)
        {
            int columnZ = 1 , columnRHS=1, columnBV=1 ;
            int tabularFormWidth = columnZ +  variablesCount + stackVariable.Count + artificialVariable.Count + surplusVariable.Count + columnRHS + columnBV  ;
            this.tabularForm = new double[equationsCount+1][];
            
            // puttinh matrix input
            for (int i = 0; i < equationsCount ; i++)
            {                
                var coefficientsStartingPoint = columnZ ;
                tabularForm[i+1] = new double[tabularFormWidth];
                for (int j = 0; j < variablesCount; j++)
                    tabularForm[i+1][j+coefficientsStartingPoint] = matrix[i, j];

                // setting right hand sides                
                tabularForm[i+1][
                    variablesCount + stackVariable.Count + artificialVariable.Count + surplusVariable.Count + columnZ 
                ] = matrix[i, variablesCount];                
            }                        

            for (int i = 0; i < stackVariable.Count; i++)               
            {
                tabularForm[stackVariable[i]+1 ][ i + variablesCount + columnZ] = 1 ;                        
                tabularForm[stackVariable[i]+1 ][tabularFormWidth-1] = i + variablesCount + columnZ ;
            }

            for (int i = 0; i < surplusVariable.Count; i++)               
            {
                int surplusStartIndex  = variablesCount + stackVariable.Count + columnZ  ; 
                tabularForm[surplusVariable[i]+1 ][ surplusStartIndex + i ] = -1 ;                        
            }

            tabularForm[0] = new double[tabularFormWidth] ;
            tabularForm[0][0] = 1 ;            

            for (int i = 0; i < artificialVariable.Count; i++)
            {                
                int artificialStartIndex = columnZ + variablesCount + stackVariable.Count + surplusVariable.Count;
                
                tabularForm[artificialVariable[i]+1][i+ artificialStartIndex ] = 1 ;
                tabularForm[artificialVariable[i]+1][tabularFormWidth-1] = i+ artificialStartIndex ;
                tabularForm[0][i + artificialStartIndex] = 1 ;
            }
            
        }                

        private string Simplex()
        {
            // make right hand side of inequalities positive
            // and make inequalities equalities by stack and artificial variables
            for (int i = 0; i < equationsCount; i++)            
                if (matrix[i,variablesCount] < 0)                
                {
                    multiply(i,-1,variablesCount);                
                    artificialVariable.Add(i);
                    surplusVariable.Add(i); // surplus is the 'e' in video 
                } else 
                    stackVariable.Add(i);
            
            InitializePahse1(matrix);

            // remove M from artificial variables 
            for (int i = 0; i < artificialVariable.Count; i++)    
            {                
                for (int k = 0; k < tabularForm[0].Length-1; k++)       
                {
                    tabularForm[0][k] -= tabularForm[artificialVariable[i]+1][k] ;
                }
            }                        

            // get feasibility
            // int[] basicVariables = new int[tabularForm.Length]; ; //= computeBasicVariables () ;
            
            // START PHASE 1

            int tabularFormWidth = tabularForm[0].Length ;
            double mostNegative = tabularForm[equationsCount][0];
            int mostNegativeIndex = 0 ;
            while(true)
            {
                // find most negative 
                mostNegative = tabularForm[equationsCount][0];
                mostNegativeIndex = 0 ;
                bool isThereNegative = false ;
                for (int i = 1; i < tabularFormWidth - 2 ; i++) // -1 or -1 -1 ???
                {
                    if(tabularForm[0][i] < 0)
                        isThereNegative = true ;
                    if(tabularForm[0][i] < mostNegative )
                    {
                        mostNegative = tabularForm[0][i];
                        mostNegativeIndex = i ;
                    }
                }
                if (isThereNegative == false)
                    break ;
                // find minimum ratio                
                double minimumRatio = 100000;
                bool isTherePoisitive = false ;
                int minimumRatioInex = 0 ;
                for (int i = 1; i < equationsCount+1; i++)
                {
                    if (tabularForm[i][mostNegativeIndex] !=0)
                    {                                            
                        var ratio = tabularForm[i][tabularFormWidth-2] / tabularForm[i][mostNegativeIndex] ;
                        if(ratio >= 0)
                        {
                            isTherePoisitive = true ;
                            if ( ratio < minimumRatio )
                            {
                                minimumRatio = ratio;
                                minimumRatioInex = i ;
                            }
                        }
                    }
                }
                                
                if(isTherePoisitive == false)
                    return "Infinity" ;

                tabularForm[minimumRatioInex][tabularFormWidth-1]= mostNegativeIndex ;

                for (int j = 0; j < equationsCount+1; j++)
                {    
                    if (j==minimumRatioInex)
                        continue ;
                
                    var pivotValue = tabularForm[minimumRatioInex][mostNegativeIndex] ;
                    var b = tabularForm[j][mostNegativeIndex] ;
                    for (int i = 1; i < tabularFormWidth-1 ; i++)
                    {
                        tabularForm[minimumRatioInex][i] = (tabularForm[minimumRatioInex][i] / pivotValue) ;
                        tabularForm[j][i] += -1 * b * tabularForm[minimumRatioInex][i] ;                    
                    }
                }
                
                
            }

            // if(gotoPhase2 == false)
            if (tabularForm[0][tabularFormWidth-2] != 0)            
                return "No Solution" ;

            for (int i = 1; i < variablesCount+1; i++)            
                tabularForm[0][i] = -1 * matrix[equationsCount,i-1] ;            
            bool gotoPhase2 = true ;
            for (int i = 1; i < equationsCount+1; i++)
            {
                var equationBasicVariable = tabularForm[i][tabularFormWidth-1] ;
                if(equationBasicVariable >=1 && equationBasicVariable < variablesCount+1)
                {
                    var pivotValue = tabularForm[i][(int)equationBasicVariable] ;
                    var b = tabularForm[0][(int)equationBasicVariable] ;
                    for (int j = 1; j < tabularFormWidth-1 ; j++)
                    {
                        tabularForm[i][j] = (tabularForm[i][j] / pivotValue) ;
                        tabularForm[0][j] += -1 * b * tabularForm[i][j] ;                    
                    }
                }

                if(equationBasicVariable >= 1+variablesCount + stackVariable.Count + surplusVariable.Count
                    && equationBasicVariable < 1+variablesCount + stackVariable.Count + surplusVariable.Count + artificialVariable.Count)
                    gotoPhase2 = false ;
            }

            // PHASE 2


            mostNegative = tabularForm[equationsCount][0];
            mostNegativeIndex = 0 ;
            while(true)
            {
                // find most negative 
                mostNegative = tabularForm[equationsCount][0];
                mostNegativeIndex = 0 ;
                bool isThereNegative = false ;
                for (int i = 1; i < tabularFormWidth - 2 ; i++) // -1 or -1 -1 ???
                {
                    if(i >= 1+ variablesCount + stackVariable.Count + surplusVariable.Count
                        && i < 1+ variablesCount + stackVariable.Count + surplusVariable.Count + artificialVariable.Count )
                        continue ;

                    if(tabularForm[0][i] < 0)
                        isThereNegative = true ;
                    if(tabularForm[0][i] < mostNegative )
                    {
                        mostNegative = tabularForm[0][i];
                        mostNegativeIndex = i ;
                    }
                }
                if (isThereNegative == false)
                    break ;
                // find minimum ratio                
                double minimumRatio = double.MaxValue;
                bool isTherePoisitive = false ;
                int minimumRatioInex = 0 ;
                for (int i = 1; i < equationsCount+1; i++)
                {
                    if (tabularForm[i][mostNegativeIndex] !=0)
                    {                                            
                        var ratio = tabularForm[i][tabularFormWidth-2] / tabularForm[i][mostNegativeIndex] ;
                        if(ratio >= 0)
                        {
                            isTherePoisitive = true ;
                            if ( ratio < minimumRatio )
                            {
                                minimumRatio = ratio;
                                minimumRatioInex = i ;
                            }
                        }
                    }
                }            
                                
                if(isTherePoisitive == false)
                    return "Infinity" ;

                tabularForm[minimumRatioInex][tabularFormWidth-1]= mostNegativeIndex ;

                for (int j = 0; j < equationsCount+1; j++)
                {    
                    if (j==minimumRatioInex)
                        continue ;
                
                    var pivotValue = tabularForm[minimumRatioInex][mostNegativeIndex] ;
                    var b = tabularForm[j][mostNegativeIndex] ;
                    for (int i = 1; i < tabularFormWidth-1 ; i++)
                    {
                        var a = tabularForm[minimumRatioInex][i] / pivotValue ;
                        tabularForm[minimumRatioInex][i] = tabularForm[minimumRatioInex][i] / pivotValue ;
                        tabularForm[j][i] += -1 * b * tabularForm[minimumRatioInex][i] ;                    
                    }
                }                
                
            }

            double[] answer = new double[variablesCount] ;

            for (int i = 1; i < equationsCount+1; i++)
            {
                var equationBasicVariable = tabularForm[i][tabularFormWidth-1] ;
                if(equationBasicVariable >=1 && equationBasicVariable < variablesCount+1)                
                    answer[(int)tabularForm[i][tabularFormWidth-1]-1] = round(tabularForm[i][tabularFormWidth-2]);                
            }

            string output = "Bounded Solution" ;            
            output += '\n' ;
            for (int i = 0; i < variablesCount; i++)            
                output += answer[i] + " " ;            
            

            return output ; 

                                                     
        }

        private void subtractRowFromOthers(int i, int j, int MATRIX_SIZE, int v3)
        {
            var g = tabularForm[j][v3] / tabularForm[i][v3]  ;
            for (int k = 0; k <= MATRIX_SIZE; k++)       
            {
                tabularForm[j][k] += -1 * g * tabularForm[i][k] ;
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
