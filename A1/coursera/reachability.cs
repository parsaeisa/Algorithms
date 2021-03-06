using System;
using System.Collections.Generic;
using System.Linq ; 

namespace A1
{
    public class Q1MazeExit 
    {
        public static int Main()
        {
            string[] s = System.Console.ReadLine().Split(' ');            
            long NodeCount = long.Parse(s[0]);
            long EdgeCount = long.Parse(s[1]);
            long[,] edges = new long[EdgeCount,2] ;
            for (long i = 0; i < EdgeCount; i++)
            {
                s = System.Console.ReadLine().Split(' ');
                edges[i,0] = long.Parse(s[0]);
                edges[i,1] = long.Parse(s[1]);
            }
            s = System.Console.ReadLine().Split(' ');            
            long StartNode = long.Parse(s[0]);
            long EndNode = long.Parse(s[1]);

            Q1MazeExit q = new Q1MazeExit();

            System.Console.WriteLine(q.Solve(NodeCount , edges , StartNode , EndNode , EdgeCount));

            return 0 ;
        }

        private List<Node> nodes ;

        long endNode ; 
        bool found ;        

        class Node
        {
            public long value ;
            public bool visited ; 

            public Node(long value)
            {
                this.value = value;
                visited = false ;
                neighbours = new List<long>();
            }

            public List<long> neighbours ;

            public void AddNeighbour (long n)
            {
                neighbours.Add(n);
            }

        }

        public long Solve(long nodeCount, long[,] edges, long StartNode, long EndNode , long EdgeCount)
        {            
            this.found = false ;
            this.endNode = EndNode ;

            // bool [] vertices = new bool[nodeCount];                            

            nodes = new List<Node>();
            
            for (int i = 0; i < nodeCount; i++)                                
                nodes.Add(new Node(i+1));                        
                
            
            // foreach (var e in edges)            
            for (int i = 0; i < EdgeCount; i++)            
            {
                nodes[(int)edges[i,0]-1].AddNeighbour(edges[i,1]);
                nodes[(int)edges[i,1]-1].AddNeighbour(edges[i,0]);
            }                    

            DFS( StartNode );  

            if( this.found == true)          
                return 1 ;
            else 
                return 0 ;            

        }

        private void DFS( long StartNode )
        {            
            if(this.found == true)
                    return ;

            // vertices[StartNode-1] = true ;            
            nodes[(int)StartNode-1].visited = true ;
            foreach (var n in nodes[(int)StartNode-1].neighbours)
            {
                if(n == this.endNode)                    
                {
                    this.found = true ;
                    return ;
                }

                // if(vertices[n-1] == false)
                if(nodes[(int)n-1].visited == false)
                    DFS( n);

                if(this.found == true)
                    return ;
            }            
                       
        }        

     }
}
