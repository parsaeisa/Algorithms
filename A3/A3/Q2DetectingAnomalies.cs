using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;
namespace A3
{
    public class Q2DetectingAnomalies:Processor
    {
        
        long[] dist ;         

        long nodeCount ; 
        public Q2DetectingAnomalies(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long>)Solve);        

        public long Solve(long nodeCount, long[][] edges)
        {
            //Write Your Code Here 
            dist = new long[nodeCount];
            for (int i = 0; i < nodeCount; i++)            
                dist[i] = 10000 ;                        
                      
            dist[0] = 0 ;              
            
            for (int i = 0; i < nodeCount; i++)                         
                foreach (var e in edges)
                {
                    if(dist[e[1]-1] > dist[e[0]-1] + e[2] && i==nodeCount-1)
                        return 1 ;                    

                    if(dist[e[1]-1] > dist[e[0]-1] + e[2])
                        dist[e[1]-1] = dist[e[0]-1] + e[2] ;
                }                                        

            return 0;
        }

    }
}
