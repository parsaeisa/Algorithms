using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A5
{
    public class Q4SuffixTree : Processor
    {
        public Q4SuffixTree(string testDataName) : base(testDataName)
        {
            this.VerifyResultWithoutOrder = true;
            this.ExcludeTestCases(9);
        }

        public long clock ;
        

        public class Node {
            public Node( )
            {
                // this.value = value;                
                neighbours = new Dictionary<string, Node>();                
            }
            public long value ;                        
            public Dictionary<string , Node > neighbours ;
            internal void AddNeighbour(Node neighbour, string edgeLabel ) => neighbours.Add( edgeLabel , neighbour ) ;            
        }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<String, String[]>)Solve);

        List<string> output ;

        public string[] Solve(string text)
        {            
            Node root = new Node();
            clock = 0;            

            output = new List<string>() ;

            for (int j = 0; j < text.Length; j++)
            {                
                Node currentNode = root;                    
                string pattern = text.Substring(j);
                bool addNewOne = true , shouldBreak = false ;
                int i=0 ;
                
                while( i < pattern.Length)
                {                                                        
                    char currentSymbol = pattern[i];                                                            
                    long f =0 ;
                    foreach (var neighbour in currentNode.neighbours)
                    {                        
                        if(neighbour.Key[0] == currentSymbol)
                        {          
                            addNewOne = false ;                 
                            while(i < neighbour.Key.Length && neighbour.Key[(int)f] == pattern[i]) 
                            {
                                f++ ;                                
                                i++ ;                                                                    
                            }

                            if(i >= neighbour.Key.Length)
                            {
                                pattern = pattern.Substring(i);
                                currentNode = currentNode.neighbours[neighbour.Key];                                
                                break ;
                            }
                            
                            Node l = currentNode.neighbours[neighbour.Key];
                            currentNode.neighbours.Remove(neighbour.Key) ;
                                                        
                            currentNode.AddNeighbour(new Node() , pattern.Substring(0 , i) );
                            
                            Node newGenerated = currentNode.neighbours[pattern.Substring(0 , i)];
                            newGenerated.AddNeighbour(l , neighbour.Key.Substring(i) );                                                        
                            newGenerated.AddNeighbour(new Node()  , pattern.Substring(i) );

                            // currentNode = Nodes[(int)clock - 1]; // ???????

                            shouldBreak = true ;
                            break ;
                           
                        }                                 
                    }                           

                    if(shouldBreak == true)                                                     
                        break ;                        

                    if (addNewOne)
                    {
                        // Nodes.Add(new Node (clock++));
                        // currentNode.neighbours.Add(pattern , clock - 1);
                        currentNode.AddNeighbour(new Node() , pattern );
                        break ;
                    }
                }
                
            }  


            // Node root = Nodes[0];
            dfs(root );

            return output.ToArray() ;
        }

        private void dfs(Node root)
        {
            foreach (var neighbour in root.neighbours)
            {
                output.Add(neighbour.Key);
                dfs(root.neighbours[neighbour.Key]);
            }
        }
    }
}
