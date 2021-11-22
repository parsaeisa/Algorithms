using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;
// using GeoCoordinatePortable;

namespace A4
{
    public class Q3ComputeDistance : Processor
    {
        public Q3ComputeDistance(string testDataName) : base(testDataName) { }

        public static readonly char[] IgnoreChars = new char[] { '\n', '\r', ' ' };
        public static readonly char[] NewLineChars = new char[] { '\n', '\r' };
        private static double[][] ReadTree(IEnumerable<string> lines)
        {
            return lines.Select(line => 
                line.Split(IgnoreChars, StringSplitOptions.RemoveEmptyEntries)
                                     .Select(n => double.Parse(n)).ToArray()
                            ).ToArray();
        }
        public override string Process(string inStr)
        {
            return Process(inStr, (Func<long, long, double[][], double[][], long,
                                    long[][], double[]>)Solve);
        }
        public static string Process(string inStr, Func<long, long, double[][]
                                  ,double[][], long, long[][], double[]> processor)
        {
           var lines = inStr.Split(NewLineChars, StringSplitOptions.RemoveEmptyEntries);
           long[] count = lines.First().Split(IgnoreChars,
                                              StringSplitOptions.RemoveEmptyEntries)
                                        .Select(n => long.Parse(n))
                                        .ToArray();
            double[][] points = ReadTree(lines.Skip(1).Take((int)count[0]));
            double[][] edges = ReadTree(lines.Skip(1 + (int)count[0]).Take((int)count[1]));
            long queryCount = long.Parse(lines.Skip(1 + (int)count[0] + (int)count[1]) 
                                         .Take(1).FirstOrDefault());
            long[][] queries = ReadTree(lines.Skip(2 + (int)count[0] + (int)count[1]))
                                        .Select(x => x.Select(z => (long)z).ToArray())
                                        .ToArray();

            return string.Join("\n", processor(count[0], count[1], points, edges,
                                queryCount, queries));
        }
                

        double[] dist ;
        double [] weights ;
        private long nodeCount;
        private IEnumerable<object> edges;

        public List<double> H { get; private set; }        
        
        List<Node> nodes ;
        private double[][] points;

        private bool [] visited ;        

        public double[] Solve(long nodeCount,
                            long edgeCount,
                            double[][] points,
                            double[][] edges,
                            long queriesCount,
                            long[][] queries)
        {
            H = new List<double>();
            dist = new double[nodeCount];

            long i = 0;
            this.nodeCount = nodeCount;
            this.edges = edges;
            this.points = points;
            double[] output = new double[queriesCount];
            
            BuildGraph(nodeCount, points, edges);

            foreach (var q in queries)
            {
                if (q[0] == q[1])                
                    output[i] = 0 ;                
                else
                {                
                    visited = new bool[nodeCount];
                    H = new List<double>();
                    for (int j = 0; j < nodeCount; j++)
                    {
                        H.Add(j);
                        dist[j] = long.MaxValue ;
                    } 
                    output[i] = AStar(q[0], q[1]);
                }
                i++;
            }

            return output;
        }

        private void BuildGraph(long nodeCount, double[][] points, double[][] edges)
        {
            nodes = new List<Node>();

            for (int i = 0; i < nodeCount; i++)
            {
                H.Add(i);
                nodes.Add(new Node(i, points[i][0], points[i][0]));
            }

            foreach (var e in edges)
                nodes[(int)e[0] - 1].addNeighbour(e[1] - 1, e[2]);
        }

        public double ExtractMin()
        {
            double min = dist[(int)H.First()];
            double minAddress = H.First() ;        
            int minIndex = 0 ;

            for (int i = 1; i < H.Count ; i++)
            {
                if(dist[(int)H[i]] < min)
                {
                    min = dist[(int)H[i]] ;
                    minIndex = i ;                    
                }
            }

            double output = H[(int)minIndex];
            H.RemoveAt(minIndex);
            return output ;
        } 

        private double AStar(long s, long t)
        {                       

            dist[(int)s-1] = 0 ;
            // nodes[(int)s-1].dist = 0 ;
            // sorted.Add(s-1);            

            while(H.Count != 0)
            {         
                double nIndex = ExtractMin();                       
                visited[(int)nIndex] = true ;                
                Node n = nodes[(int)nIndex] ;

                if(n.value == t-1)
                    return dist[(int)t-1]  + nodes[(int)s-1].potential(points[t-1][0] , points[t-1][1]) ;    

                foreach (var neighbour in n.neighbours)
                {                    

                    if ( ! visited[(int)neighbour.Item1])
                    {                                            
                        
                        Node neigh = nodes[(int)neighbour.Item1] ;                        
                        if ( dist[(int)neighbour.Item1] > dist [(int)n.value]
                            + neighbour.Item2 - n.potential(points[t-1][0] , points[t-1][1]) 
                                          + neigh.potential(points[t-1][0] , points[t-1][1]) )
                        {                            
                            dist[(int)neighbour.Item1] = dist [(int)n.value]
                            + neighbour.Item2 - n.potential(points[t-1][0] , points[t-1][1]) 
                                          + neigh.potential(points[t-1][0] , points[t-1][1]);                                                                            
                                                        
                        }

                        // dijkstra and works
                        // if ( dist[(int)neighbour.Item1] > dist [(int)n.value] + neighbour.Item2 )                        
                        //     dist[(int)neighbour.Item1] = dist [(int)n.value] + neighbour.Item2 ;                                                                                                                                                            
                                                
                    }
                }
            }                        

            if (dist[(int)t-1] == long.MaxValue)
                return -1 ;   

            return dist[(int)t-1]  + nodes[(int)s-1].potential(points[t-1][0] , points[t-1][1]) ;                                     
             
        }
                

        class Node
        {
            public long value ;
            public double dist ;            
            public bool visited ;            
            public List<Tuple<double,double>> neighbours ;
            public Tuple<double , double> coordinate ;
            public double distanceFromSource;                                   

            public Node(long value , double x , double y)
            {
                dist = 1000000 ;
                visited = false ;
                this.value = value ;
                this.neighbours = new List<Tuple<double, double>>();
                this.coordinate = new Tuple<double, double>(x,y);                
                distanceFromSource = 0 ;
            }

            public double potential( double targetx , double targety)
            {
                return Math.Sqrt( Math.Pow((coordinate.Item1 - targetx) , 2)
                        + Math.Pow((coordinate.Item2 - targety) , 2) );
            }

            public void addNeighbour (double n , double weight ) => neighbours.Add(new Tuple<double, double>(n,weight));
            
        }

    }
}