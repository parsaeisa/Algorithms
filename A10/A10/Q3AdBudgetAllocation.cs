using System;
using System.Collections.Generic;
using TestCommon;

namespace A10
{
    public class Q3AdBudgetAllocation : Processor
    {
        public Q3AdBudgetAllocation(string testDataName) : base(testDataName) 
        {            
            // for (int i = 1; i <= 43; i++)
                //this.ExcludeTestCases(i);
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long, long[][], long[], string[]>)Solve);

        public override Action<string, string> Verifier { get; set; } =
            TestTools.SatVerifier;
        public List<string> CNF { get; private set; }

        private long[][] variables;   
        private List<long>[] nonZeros ;     

        public string[] Solve(long eqCount, long varCount, long[][] A, long[] b)
        {
            // write your code here
            this.CNF = new List<String>();
            CNF.Add("");
                        
            this.variables = new long[eqCount][];            

            this.nonZeros = new List<long>[eqCount];            
            for (int i = 0; i < eqCount; i++)
            {
                nonZeros[i] = new List<long>();

                if (varCount >= 3)   
                    variables[i] = A[i];
                else 
                {
                    variables[i] = new long[3];
                    for (int j = 0; j < varCount; j++)                    
                        variables[i][j] = A[i][j] ;                    
                }
                int zeroCount = 0  ;
                                
                for (int j = 0; j < varCount; j++)
                {                    
                    if (A[i][j] == 0)                   
                        zeroCount ++ ;     

                    if(A[i][j] != 0)
                        nonZeros[i].Add( j) ;

                }                                
                                                    

                if (zeroCount == varCount && b[i] < 0)
                {
                    CNF = new List<string>();
                    CNF.Add("2 1");
                    CNF.Add("1 0");
                    CNF.Add("-1 0");
                    return CNF.ToArray() ;
                }                        
                
                if(nonZeros[i].Count < 3)                
                    for (int j = nonZeros[i].Count ; j < 3; j++)                    
                        nonZeros[i].Add(-1);                                                 
            }
            
            for (int i = 0; i < eqCount; i++)
            {                        

                int a = 0 ;
                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        for (int z = 0; z < 2; z++)
                        {
                            long statement = 0 ;
                            statement += nonZeros[i][0] != -1 ? x* variables[i][nonZeros[i][0]] : 0 ;
                            statement += nonZeros[i][1] != -1 ? y* variables[i][nonZeros[i][1]] : 0 ;
                            statement += nonZeros[i][2] != -1 ? z* variables[i][nonZeros[i][2]] : 0 ;

                            if ( statement > b[i] )
                            {
                                a++ ;
                                string first =  nonZeros[i][0] != -1 ?( x == 0 ?  "-" + (nonZeros[i][0]+1) : (nonZeros[i][0]+1).ToString()) : "";
                                string second = nonZeros[i][1] != -1 ?( y == 0 ? "-" + (nonZeros[i][1] +1): (nonZeros[i][1]+1).ToString()) : "";
                                string third =  nonZeros[i][2] != -1 ?( z == 0 ?  "-" + (nonZeros[i][2]+1) : (nonZeros[i][2]+1).ToString()) : "";
                                CNF.Add(first + " " + second + " " + third + " 0" );
                            }
                        }
                    }
                }
                // if (a==8)
                // {
                //     CNF = new List<string>();
                //     CNF.Add("1 1");
                //     CNF.Add("-1 1 0");
                //     return CNF.ToArray() ;
                // }
                
            }

            CNF[0] = CNF.Count- 1 + " " + (varCount >=3 ? varCount : 3) ;            
            return CNF.ToArray() ;
        }
    }
}
