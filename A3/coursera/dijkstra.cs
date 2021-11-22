using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A3
{
    public class Q1MinCost 
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
            Q1MinCost obj = new Q1MinCost();

            s = System.Console.ReadLine().Split(' ');
            long startNode = long.Parse(s[0]);
            long endNode   = long.Parse(s[1]);

            System.Console.WriteLine(obj.Solve(NodeCount , edges , EdgeCount , startNode , endNode));

            return 0 ;
        }

        long [] dist ;
        long [] prev ;  
        bool [] visited ;
        List<long> H ;        


        public long Solve(long nodeCount, long[,] edges, long edgeCount , long startNode, long endNode)
        {

            visited = new bool[nodeCount];
            //Write Your Code Here       
            dist = new long[nodeCount];            

            for (int i = 0; i < nodeCount; i++)            
                dist[i] = long.MaxValue ;            

            dist[startNode-1] = 0 ;

            H = new List<long>();            

            for (long i = 0; i < nodeCount ; i++)                
                H.Add(i);

            while(H.Count != 0 )
            {
                long n = ExtractMin(); 
                visited[(int)n] = true ;

                // foreach (var e in edges)
                for (int i = 0; i < edgeCount; i++)                
                {
                    if (edges[i,0] -1 == n)
                    {
                        if (! visited[edges[i,1]-1])                                                                                
                            if (dist[edges[i,1]-1] > dist[edges[i,0]-1] + edges[i,2])                        
                                dist[edges[i,1]-1] = dist[edges[i,0]-1] + edges[i,2] ;                                                    
                    }
                }

            }

            if(dist[ endNode-1] == long.MaxValue)
                return -1 ;
            return dist[endNode-1];
        }

        public long ExtractMin()
        {
            long min = dist[H.First()];
            long minAddress = H.First() ;        
            int minIndex = 0 ;

            for (int i = 1; i < H.Count ; i++)
            {
                if(dist[H[i]] < min)
                {
                    min = dist[H[i]] ;
                    minIndex = i ;                    
                }
            }

            long output = H[minIndex];
            H.RemoveAt(minIndex);
            return output ;
        }
        
    }
}
