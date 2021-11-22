using System;
using System.Collections.Generic;

namespace A2
{
    public class Q1ShortestPath 
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

            System.Console.WriteLine(Solve(NodeCount , edges , StartNode , EndNode , EdgeCount));

            return 0 ;
        }
        
        
        public static long  Solve(long NodeCount, long[,] edges, 
                          long StartNode,  long EndNode , long EdgeCount)
        {
            List<Node> nodes = new List<Node>();
            Queue <Node> queue = new Queue<Node>();
            for (int i = 0; i < NodeCount; i++)            
                nodes.Add(new Node(i+1 , NodeCount +1));      

            // foreach (var e in edges)
            for (int i = 0; i < EdgeCount; i++)            
            {
                nodes[(int)edges[i,0]-1].AddNeighbour(edges[i,1]);
                nodes[(int)edges[i,1]-1].AddNeighbour(edges[i,0]);
            }      

            nodes[(int)StartNode-1].visited = true ;
            nodes[(int)StartNode-1].distance = 0 ; 
            queue.Enqueue(nodes[(int)StartNode-1]);            

            while(queue.Count != 0 )
            {
                Node node= queue.Dequeue();

                foreach (var n in node.neighbours)
                {
                    if(nodes[(int)n-1].visited == false)
                    {                                                           
                        nodes[(int)n-1].visited = true ;
                        nodes[(int)n-1].distance = node.distance +1 ;
                        if(nodes[(int)n-1].value == EndNode)
                            return nodes[(int)n-1].distance ;

                        queue.Enqueue(nodes[(int)n-1]);
                    }
                }
            }

            return -1 ;
        }

        class Node
        {
            public long value ;
            public List<long> neighbours ;
            public bool visited ; 
            public long distance ; 
            public Node(long value , long distance)
            {
                this.value = value;
                neighbours = new List<long>();
                visited = false ;
                distance = distance ;
            }            
            
            public void AddNeighbour(long index) => neighbours.Add(index);            
        }
    }
}
