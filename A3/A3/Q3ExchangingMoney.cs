using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;
namespace A3
{
    public class Q3ExchangingMoney : Processor
    {
        long[] dist ; 
        public Q3ExchangingMoney(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long, string[]>)Solve);

        public string[] Solve(long nodeCount, long[][] edges, long startNode)
        {
            //Write Your Code Here    
            dist = new long[nodeCount];
            for (int i = 0; i < nodeCount; i++)            
                dist[i] = 1000000 ;                        
                      
            dist[startNode-1] = 0 ;              
            
            for (int i = 0; i < nodeCount-1; i++)             
            {
                foreach (var e in edges)
                {
                    if(dist[e[1]-1] > dist[e[0]-1] + e[2])
                        dist[e[1]-1] = dist[e[0]-1] + e[2] ;
                }
            }                                                

            Queue<long> q = new Queue<long>();

            foreach (var e in edges)
                if(dist[e[1]-1] > dist[e[0]-1] + e[2] && dist[e[1]-1] < 10000)
                {
                    q.Enqueue(e[1]-1);
                    dist[e[1]-1] = dist[e[0]-1] + e[2] ;
                }                     

            string[] output = new string[nodeCount];      

            bool[] visited = new bool[nodeCount];                     

            while(q.Count != 0)
            {
                long n = q.Dequeue();
                visited[n] = true ;
                output[n] = "-" ;
                foreach (var e in edges)
                {
                    if(e[0] == n+1 && visited[e[1]-1] == false)
                        q.Enqueue(e[1]-1);
                }
            }            
            
            for (int i = 0; i < nodeCount; i++)            
            {
                if(output[i]!= "-")
                {
                    if(dist[i] <  100000)
                        output[i] = dist[i].ToString();
                    else
                        output[i] = "*" ;
                }
            }

            return output;
        }

        class Node 
        {
            long value ;
            public List<Tuple<long , long>> neighbours ; 
            public Node(long value)
            {
                this.value = value;
                // dist = 1000000 ;
                neighbours = new List<Tuple<long, long>>();
            }

            public void addNeighbour(long v , long weight)
            {
                neighbours.Add(new Tuple<long, long>(v , weight));
            }
        }
    }
}
