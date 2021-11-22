using System;
using System.Collections.Generic;
using System.Linq;
namespace A1
{
    public class Q3Acyclic
    {        

        public static int Main()
        {
            string[] s = System.Console.ReadLine().Split(' ');            
            long NodeCount = long.Parse(s[0]);
            long EdgeCount = long.Parse(s[1]);
            // long[,] edges = new long[EdgeCount,2] ;
            List<Tuple<long,long>> edges = new List<Tuple<long,long>>();
            for (long i = 0; i < EdgeCount; i++)
            {
                s = System.Console.ReadLine().Split(' ');
                // edges[i,0] = long.Parse(s[0]);
                // edges[i,1] = long.Parse(s[1]);
                edges.Add(new Tuple<long, long>(long.Parse(s[0]) , long.Parse(s[1])));
            }            

            Q3Acyclic q = new Q3Acyclic();

            System.Console.WriteLine(q.Solve(NodeCount , edges));

            return 0 ;
        }

        bool[] is_sink ;
        List<long> removed ;        
        public long Solve(long nodeCount, List<Tuple<long , long>> edges)
        {            
            removed = new List<long>();
            is_sink = new bool[nodeCount];
            for (int i = 0; i < nodeCount; i++)                         
                is_sink[i] = true ;  
            return isAcyclic(nodeCount , edges);
        }

        private long isAcyclic(long nodeCount, List<Tuple<long, long>> edges)
        {
            if(nodeCount <= 1)
                return 0 ;                                   

            for (int i = 0; i < is_sink.Length; i++)                         
                is_sink[i] = true ;  

            // foreach (var e in edges)                        
            for (int i = 0; i < edges.Count ; i++)            
            {
                is_sink[edges[i].Item1-1] = false ;                                                                 
            }

            for (long i = 0; i < is_sink.Length; i++)                                    
                if(is_sink[i] == true && !removed.Contains(i))                
                {
                    removed.Add(i);                    
                    return isAcyclic(nodeCount-1, edges.Where(edge => edge.Item2 != i+1).ToList() ) ;                            
                }
    
            return 1 ;            
        }
    }
}