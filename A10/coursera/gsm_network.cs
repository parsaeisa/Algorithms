using System;
using System.Collections.Generic;

namespace A10
{
    public class Q1FrequencyAssignment
    {
        public static int Main()
        {
            string[] VandE = System.Console.ReadLine().Split(' ');
            int V = int.Parse(VandE[0]);
            int E = int.Parse(VandE[1]);
            
            long[,] matrix = new long[E,2] ;                        
            string[] aSplited ;

            for (int i = 0; i < E; i++)            
            {                
                aSplited = System.Console.ReadLine().Split(' ');                
                matrix[i,0] = long.Parse(aSplited[0]);                                    
                matrix[i,1] = long.Parse(aSplited[1]);      
            }                        

            Q1FrequencyAssignment obj = new Q1FrequencyAssignment();

            String[] ans = obj.Solve(V , E, matrix ) ;

            for (int i = 0; i < ans.Length; i++)
            {
                System.Console.WriteLine(ans[i]);                
            }

            return 0 ;
        } 
        private int nodeCount;
        private int edgeCount;

        public List<string> CNF { get; private set; }

        private long[,] variables;
        private long[] nodeColor;

        public String[] Solve(int V, int E, long[,] matrix)
        {
            // write your code here
            this.nodeCount =  V ;
            this.edgeCount = E ;

            this.CNF = new List<String>();
            CNF.Add("  ");

            this.variables = new long[nodeCount , 3];
            int variableIndex = 0 ;
            for (int i = 0; i < nodeCount; i++)            
                for (int j = 0; j < 3; j++)                
                {
                    variables[i,j] = variableIndex ;       
                    variableIndex += 1 ;      
                }

            for (int i = 0; i < nodeCount; i++)
            {
                String bigClause = "" ;
                for (int j = 0; j < 3; j++)                
                {
                    for (int k = j; k < 3; k++)                    
                        if (k!=j)                        
                            CNF.Add("-" + (variables[i,j]+1) + " -" + (variables[i,k]+1) + " 0");                        
                    bigClause += (variables[i,j]+1).ToString() + " ";                
                }                    
                bigClause += "0" ;        
                CNF.Add(bigClause);
            }

            for (int i = 0; i < edgeCount ; i++)            
                for (int j = 0; j < 3; j++)                
                    CNF.Add( "-" + (variables[matrix[i,0]-1,j]+1) + " -" + (variables[matrix[i,1]-1 , j]+1) + " 0");
             
            CNF[0] = CNF.Count- 1 + " " + (nodeCount * 3) ;
            return CNF.ToArray();
        }

        class Node 
        {
            public long value ;
            public bool color1,color2,color3 ;

        }

    }
}
