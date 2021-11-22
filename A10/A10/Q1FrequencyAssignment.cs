using System;
using System.Collections.Generic;
using TestCommon;

namespace A10
{
    public class Q1FrequencyAssignment : Processor
    {
        private int nodeCount;
        private int edgeCount;

        public List<string> CNF { get; private set; }

        private long[,] variables;
        private long[] nodeColor;

        public Q1FrequencyAssignment(string testDataName) : base(testDataName) {
            // this.ExcludeTestCases(1);
         }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<int, int, long[,], string[]>)Solve);


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

        public override Action<string, string> Verifier { get; set; } =
            TestTools.SatVerifier;

        class Node 
        {
            public long value ;
            public bool color1,color2,color3 ;

        }

    }
}
