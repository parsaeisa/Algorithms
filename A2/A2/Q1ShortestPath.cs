using System;
using System.Collections.Generic;
using TestCommon;

namespace A2
{
    public class Q1ShortestPath : Processor
    {
        
        public Q1ShortestPath(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long,long[][], long, long, long>)Solve);
        
        public long Solve(long NodeCount, long[][] edges, 
                          long StartNode,  long EndNode)
        {
            List<Node> nodes = new List<Node>();
            Queue <Node> queue = new Queue<Node>();
            for (int i = 0; i < NodeCount; i++)            
                nodes.Add(new Node(i+1 , NodeCount +1));      

            foreach (var e in edges)
            {
                nodes[(int)e[0]-1].AddNeighbour(e[1]);
                nodes[(int)e[1]-1].AddNeighbour(e[0]);
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
