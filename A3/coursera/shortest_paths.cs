using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A3
{
    public class Q3ExchangingMoney 
    {
        public static int Main()
        {
            string[] s = System.Console.ReadLine().Split(' ');            
            long NodeCount = long.Parse(s[0]);
            long EdgeCount = long.Parse(s[1]);
            long[,] edges = new long[EdgeCount,3] ;
            for (long i = 0; i < EdgeCount; i++)
            {
                s = System.Console.ReadLine().Split(' ');
                edges[i,0] = long.Parse(s[0]);
                edges[i,1] = long.Parse(s[1]);
                edges[i,2] = long.Parse(s[2]);
            }         
            Q3ExchangingMoney obj = new Q3ExchangingMoney();
            
            long startNode =  long.Parse( System.Console.ReadLine()) ;

            foreach ( var l in obj.Solve(NodeCount , edges , EdgeCount , startNode))
                System.Console.WriteLine(l);

            return 0 ;
        }
        long[] dist ;         

        public string[] Solve(long nodeCount, long[,] edges, long edgeCount , long startNode)
        {
            //Write Your Code Here    
            dist = new long[nodeCount];
            for (int i = 0; i < nodeCount; i++)            
                dist[i] = 1000000 ;                        
                      
            dist[startNode-1] = 0 ;              
            
            for (int j = 0; j < nodeCount-1; j++)             
            {
                // foreach (var e in edges)
                for (int i = 0; i < edgeCount; i++)                
                {
                    if(dist[edges[i,1]-1] > dist[edges[i,0]-1] + edges[i,2])
                       dist[edges[i,1]-1] = dist[edges[i,0]-1] + edges[i,2] ;
                }
            }                                                

            Queue<long> q = new Queue<long>();

            // foreach (var e in edges)
            for (int i = 0; i < edgeCount; i++)                
                if(dist[edges[i,1]-1] > dist[edges[i,0]-1] + edges[i,2] && dist[edges[i,1]-1] < 10000)
                {
                    q.Enqueue(edges[i,1]-1);
                    dist[edges[i,1]-1] = dist[edges[i,0]-1] + edges[i,2] ;
                }                     

            string[] output = new string[nodeCount];      

            bool[] visited = new bool[nodeCount];                     

            while(q.Count != 0)
            {
                long n = q.Dequeue();
                visited[n] = true ;
                output[n] = "-" ;
                // foreach (var e in edges)
                for (int i = 0; i < edgeCount; i++)                
                {
                    if(edges[i,0] == n+1 && visited[edges[i,1]-1] == false)
                        q.Enqueue(edges[i,1]-1);
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
