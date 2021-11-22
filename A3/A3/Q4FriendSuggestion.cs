using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A3
{
    public class Q4FriendSuggestion:Processor
    {
        private List<Node> Nodes { get; set; }
        private List<Node> NodesR { get; set; }
        public long NodeCount { get; private set; }        

        long[] dist ;
        long[] distR ;                 

        List<long> H ;
        List<long> HR ;

        // PriorityQueue H ;
        // PriorityQueue HR;

        List<long> proc ;
        List<long> procR ;  

        public Q4FriendSuggestion(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long, long[][], long,long[][], long[]>)Solve);

        public long[] Solve(long NodeCount, long EdgeCount, 
                              long[][] edges, long QueriesCount, 
                              long[][] Queries)
        {            
            // Write your code here.

            Nodes = new List<Node>();
            NodesR = new List<Node>();
            for (int i = 0; i < NodeCount; i++)            
            {
                Nodes.Add(new Node(i+1));      
                NodesR.Add(new Node(i+1));      
            }                
            
            this.NodeCount = NodeCount ;         

            foreach (var e in edges)                                        
            {
                Nodes[(int)e[0]-1].AddNeighbour(Nodes[(int)e[1]-1].value , e[2] );
                NodesR[(int)e[1]-1].AddNeighbour(NodesR[(int)e[0]-1].value , e[2] );
            }

            dist = new long[NodeCount];
            distR = new long[NodeCount];

            long[] output = new long[QueriesCount];
            long ptr = 0 ;

            foreach (var q in Queries)        
            {
                if (q[0] == q[1])                
                    output[ptr] = 0 ;                
                else
                {
                    output[ptr] = BidirectionalDijkstra(q[0] , q[1] );            
                    if (output[ptr] > 100000)                
                        output[ptr] = -1 ;                
                }
                ptr ++ ;
            }

            return output;
        }

        public long BidirectionalDijkstra (long s , long t)
        {

            if(s == t )
                return 0 ;

            proc = new List<long>();
            procR = new List<long>();

            H = new List<long>();  HR = new List<long>();
            // H = new PriorityQueue(this.NodeCount);
            // HR = new PriorityQueue(NodeCount) ;

            for (int i = 0; i < NodeCount; i++)
            {
                dist[i] = 1000000 ;
                distR[i] = 1000000 ;

                H.Add(i);
                HR.Add(i);

                // H.Insert(i , dist);
                // HR.Insert(i , distR);

            }

            dist[s-1] = 0 ; distR[t-1] = 0 ;
                                                        

            while(true)
            {

                if (H.Count == 0 || HR.Count == 0)                
                    return 10000000 ;                

                long v = ExtractMin("dist");                
                // long v = H.ExtractMin(dist)+1;
                Proccess(v);
                if( procR.Contains(v) )
                    return ShortestPath();
                long vR = ExtractMin("distR");
                // long vR = HR.ExtractMin(distR)+1;
                ProccessR(vR);
                if( proc.Contains(vR) )
                    return ShortestPath();
            }            
        }

        class Node 
        {
            public long value ;
            public List<Tuple<long , long>> neighbours ;

            public Node(long value)
            {
                this.value = value;
                neighbours = null ;
            }

            public void AddNeighbour (long n, long weight)
            {
                if(neighbours == null)
                    neighbours = new List<Tuple<long, long>>();
                neighbours.Add(new Tuple<long, long>(n , weight));
            }
        }

        private void ProccessR(long v)
        {
            if (NodesR[(int)v-1].neighbours == null)
                return ;
                            
            foreach (var n in NodesR[(int)v-1].neighbours)
            {
                long p = distR[n.Item1 -1];
                if (distR[n.Item1-1] > distR[v-1] + n.Item2)                    
                {
                    distR[n.Item1-1] = distR[v-1] + n.Item2 ;                        
                    // HR.ChangePriority(n.Item1-1 , p , distR);                
                }
                procR.Add(v);            
            }
        }

        private void Proccess(long v)
        {      
            if(Nodes[(int)v-1].neighbours == null)
                return ;

            foreach (var n in Nodes[(int)v-1].neighbours)
            {
                long p = dist[n.Item1 -1];
                if (dist[n.Item1-1] > dist[v-1] + n.Item2)                    
                {
                    dist[n.Item1-1] = dist[v-1] + n.Item2 ;                        
                    // H.ChangePriority(n.Item1 -1 , p , dist);
                }
                proc.Add(v);
            }
        }

        private long ShortestPath()
        {
            long distance = 1000000 ;
            foreach (var p in proc)            
                if (dist[p-1] + distR[p-1] < distance)                                        
                    distance = distR[p-1] + dist[p-1] ;                                                    

            foreach (var pR in procR)            
                if (dist[pR-1] + distR[pR-1] < distance)                                        
                    distance = distR[pR-1] + dist[pR-1] ;                                                    

            return distance ;
        }
        
        
        public long ExtractMin(string kind)
        {
            if(kind == "dist")            
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
                return output +1 ;
            }
            else 
            {
                long min = distR[HR.First()];
                long minAddress = HR.First() ;        
                int minIndex = 0 ;

                for (int i = 1; i < HR.Count ; i++)
                {
                    if(distR[HR[i]] < min)
                    {
                        min = distR[HR[i]] ;
                        minIndex = i ;                    
                    }
                }

                long output = HR[minIndex];
                HR.RemoveAt(minIndex);
                return output +1 ;
            }
        } 
        
            
    }
}
