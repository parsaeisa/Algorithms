using System;
using System.Collections.Generic;
using TestCommon;

namespace A10
{
    public class Q2CleaningApartment : Processor
    {
        public Q2CleaningApartment(string testDataName) : base(testDataName)
        {
            // for (int i = 1 ; i<= 12; i++)
            // {
            //     this.ExcludeTestCases(i);
            // }
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<int, int, long[,], string[]>)Solve);

        public override Action<string, string> Verifier { get; set; } =
            TestTools.SatVerifier;
        public List<string> CNF { get; private set; }
        private List<Node> Nodes { get; set; }

        private long[,] variables;

        public String[] Solve(int V, int E, long[,] matrix)
        {
            // write your code here
            this.CNF = new List<String>();
            CNF.Add(" ");

            this.Nodes = new List<Node>();

            this.variables = new long[V,V];

            int variableIndex = 1 ;
            for (int i = 0; i < V; i++)            
            {
                Nodes.Add(new Node(i+1));
                for (int j = 0; j < V; j++)                
                {
                    variables[i,j] = variableIndex ;       
                    variableIndex += 1 ;      
                }
            }

            for (int i = 0; i < E; i++)
            {
                Nodes[(int)matrix[i,0]-1].addNeighbour(matrix[i,1]-1);
                Nodes[(int)matrix[i,1]-1].addNeighbour(matrix[i,0]-1);
            }

            for (int i = 0; i < V; i++)
            {
                String bigClause = "" ;
                for (int j = 0; j < V; j++)
                {       

                    for (int k = 0; k < j; k++)
                    {
                        // if(k < V-1 )
                        if(i != j)
                            if(Nodes[i].Neighbours == null || !Nodes[i].Neighbours.Contains(j))
                                CNF.Add("-" + variables[i,k] + " -" + variables[j,k+1] + " 0");
                    }             

                    for (int k = j; k < V; k++)
                    {
                        if(k < V-1 && i!=j )
                            if(Nodes[i].Neighbours == null || !Nodes[i].Neighbours.Contains(j))
                                CNF.Add("-" + variables[i,k] + " -" + variables[j,k+1] + " 0");

                        else if(i!= j && k == V-1)
                            if(Nodes[i].Neighbours == null || !Nodes[i].Neighbours.Contains(j))
                                CNF.Add("-" + variables[i,k] + " -" + variables[j,0] + " 0");

                        if(k != j)
                            CNF.Add("-" + variables[i,j] + " -" + variables[i,k] + " 0");
                    }
                    
                        
                    bigClause += variables[i,j].ToString() + " ";                
                }
                bigClause += "0" ;        
                CNF.Add(bigClause);
                
            }            

            for (int i = 0; i < V; i++)
            {
                String bigClause = "" ;
                for (int j = 0; j < V; j++)
                {                                        
                    for (int k = i; k < V; k++)                                            
                        if(k != i)
                            CNF.Add("-" + variables[j,i] + " -" + variables[j,k] + " 0");                    
                                            
                    bigClause += variables[j,i].ToString() + " ";                
                }
                bigClause += "0" ;        
                CNF.Add(bigClause);
                
            }     

            CNF[0] = CNF.Count- 1 + " " + (V*V) ;
            return CNF.ToArray();
        }

        class Node 
        {
            long value ;

            public Node(long value)
            {
                this.value = value;
                Neighbours = null ;
            }

            public List<long> Neighbours ; 

            public void addNeighbour (long neighbourIndex)
            {
                if (Neighbours == null)                
                    Neighbours = new List<long>();                

                Neighbours.Add(neighbourIndex);
            }

            
        }

    }
}
