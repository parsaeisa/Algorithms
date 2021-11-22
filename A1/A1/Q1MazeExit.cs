using System;
using System.Collections.Generic;
using TestCommon;
using System.Linq ; 

namespace A1
{
    public class Q1MazeExit : Processor
    {
        private List<Node> nodes ;

        long endNode ; 
        bool found ;

        public Q1MazeExit(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long, long, long>)Solve);

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

        public long Solve(long nodeCount, long[][] edges, long StartNode, long EndNode)
        {            
            this.found = false ;
            this.endNode = EndNode ;

            // bool [] vertices = new bool[nodeCount];                            

            nodes = new List<Node>();
            
            for (int i = 0; i < nodeCount; i++)                                
                nodes.Add(new Node(i+1));                        
                
            
            foreach (var e in edges)            
            {
                nodes[(int)e[0]-1].AddNeighbour(e[1]);
                nodes[(int)e[1]-1].AddNeighbour(e[0]);
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
