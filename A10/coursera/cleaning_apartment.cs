using System;
using System.Collections.Generic;

namespace A10
{
    public class Q2CleaningApartment 
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

            Q2CleaningApartment obj = new Q2CleaningApartment();

            String[] ans = obj.Solve(V , E, matrix ) ;

            for (int i = 0; i < ans.Length; i++)
            {
                System.Console.WriteLine(ans[i]);                
            }

            return 0 ;
        } 
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
