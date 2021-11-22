using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A3
{
    public class Q1MinCost : Processor
    {

        long [] dist ;
        long [] prev ;        
        List<long> H ;

        public Q1MinCost(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long, long, long>)Solve);


        public long Solve(long nodeCount, long[][] edges, long startNode, long endNode)
        {
            //Write Your Code Here       
            dist = new long[nodeCount];            

            for (int i = 0; i < nodeCount; i++)            
                dist[i] = 10000 ;            

            dist[startNode-1] = 0 ;

            H = new List<long>();            

            for (long i = 0; i < nodeCount ; i++)                
                H.Add(i);

            while(H.Count != 0 )
            {
                long n = ExtractMin();                

                foreach (var e in edges)
                {
                    if (e[0] -1 == n)
                    {
                        if (dist[e[1]-1] > dist[e[0]-1] + e[2])
                        {
                            dist[e[1]-1] = dist[e[0]-1] + e[2] ;                            
                        }
                    }
                }

            }

            if(dist[ endNode-1] == 10000)
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
