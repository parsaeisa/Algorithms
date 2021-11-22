using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace A3
{
    public class Q2DetectingAnomalies
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
            Q2DetectingAnomalies q = new Q2DetectingAnomalies();

            System.Console.WriteLine(q.Solve(NodeCount , edges , EdgeCount ));

            return 0 ;
        }

        long[] dist ;         

        long nodeCount ; 
        
        public long Solve(long nodeCount, long[,] edges , long edgeCount)
        {
            //Write Your Code Here 
            dist = new long[nodeCount];
            for (int i = 0; i < nodeCount; i++)            
                dist[i] = 10000 ;                        
                      
            dist[0] = 0 ;              
            
            for (int j = 0; j < nodeCount; j++)                                     
                for (int i = 0; i < edgeCount; i++)                
                {
                    if(dist[edges[i,1]-1] > dist[edges[i,0]-1] + edges[i,2] && j==nodeCount-1)
                        return 1 ;                    

                    if(dist[edges[i,1]-1] > dist[edges[i,0]-1] + edges[i,2])
                       dist[edges[i,1]-1] = dist[edges[i,0]-1] + edges[i,2] ;
                }                                        

            return 0;
        }

    }
}
