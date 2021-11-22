using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace A1
{
    public class Q2AddExitToMaze 
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
            Q2AddExitToMaze q = new Q2AddExitToMaze();

            System.Console.WriteLine(q.Solve(NodeCount , edges , EdgeCount));

            return 0 ;
        }

        int c = 0 ;
        long[] cnum ;
        bool[] visited ; 
        // bool[] vertices ;
        List<Node> vertices ; 

        public long Solve(long nodeCount, long[,] edges , long EdgeCount)
        {           
            c =0 ; 
            visited = new bool[nodeCount];            
            vertices = new List<Node>();            

            for (int i = 0; i < nodeCount; i++)            
                vertices.Add(new Node(i+1));            

            // foreach (var e in edges)
            for(int i=0 ; i<EdgeCount ; i++)
            {
                vertices[(int)edges[i,0]-1].addNeigbour(vertices[(int)edges[i,1]-1]);
                vertices[(int)edges[i,1]-1].addNeigbour(vertices[(int)edges[i,0]-1]);
            }

            for (int i=0 ; i<nodeCount ; i++)
            {
                if(visited[i] == false )
                {
                    DFS(vertices[i]);
                    c +=1 ;
                }
            }

            return c ;
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

    }
}
