using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestCommon;

namespace A1
{
    public class Q2AddExitToMaze : Processor
    {

        int c = 0 ;
        long[] cnum ;
        bool[] visited ; 
        // bool[] vertices ;
        List<Node> vertices ; 
        public Q2AddExitToMaze(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long>)Solve);

        public long Solve(long nodeCount, long[][] edges)
        {           
            c =0 ; 
            visited = new bool[nodeCount];            

            vertices = new List<Node>();            

            for (int i = 0; i < nodeCount; i++)            
                vertices.Add(new Node(i+1));            

            foreach (var e in edges)
            {
                vertices[(int)e[0]-1].addNeigbour(vertices[(int)e[1]-1]);
                vertices[(int)e[1]-1].addNeigbour(vertices[(int)e[0]-1]);
            }

            for (int i=0 ; i<nodeCount ; i++)
            {
                if(visited[i] == false )
                {
                    DFS(vertices[i]);
                    c +=1 ;
                }
            }

            if(c>1)
                return c ;
            else
                return 1 ;
        }

        class Node 
        {
            public long value ;
            public List<Node> neighbours ;
            public bool visited ;

            public Node(long value)
            {
                this.value = value;
                visited = false ;
                neighbours = new List<Node>();
            }

            internal void addNeigbour(Node node)
            {
                this.neighbours.Add(node);
            }
        }
        
        private void DFS(Node node)
        {
            visited[node.value -1] = true ;
            foreach (var n in node.neighbours  )
            {
                if(visited[n.value -1] == false)
                    DFS(n);
            }   
        }

        // public void DFS (long[][] edges , long StartNode )
        // {
        //     this.vertices[StartNode-1] = true ;                       
        //     foreach (var e in edges)
        //     {       
        //         if(e[0] == StartNode)       

        //             if(vertices[e[1]-1] == false)                    
        //                 DFS(edges , e[1] );                     
                
        //         if(e[1] == StartNode)                
        //             if(vertices[e[0]-1] == false)                    
        //                 DFS(edges , e[0] );                                                        
        //     }               
        // }

    }
}
