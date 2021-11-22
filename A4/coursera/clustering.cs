using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static A4.Q2Clustering;

namespace A4
{
    public class Q2Clustering 
    {      
        public static int Main()
        {
            long pointCount = long.Parse(System.Console.ReadLine());                                    
            long[,] points = new long[pointCount,2] ;
            string[] s ;
            for (long i = 0; i < pointCount; i++)
            {
                s = System.Console.ReadLine().Split(' ');
                points[i,0] = long.Parse(s[0]);
                points[i,1] = long.Parse(s[1]);                
            }         
            Q2Clustering obj = new Q2Clustering();            

            long clusterCount = long.Parse(System.Console.ReadLine());                                    

            System.Console.WriteLine(obj.Solve(pointCount , points, clusterCount));

            return 0 ;
        } 
    
        long[]  parent ;
        double[] cost ;
        List<long> priorityQueue ;
        long[,] points ;

        List<double> longests ;
        private long clustercount;

        public double Solve(long pointCount, long[,] points, long clusterCount)
        {
            cost = new double[pointCount];
            this.points = points ;
            parent = new long[pointCount];
            priorityQueue = new List<long>();
            longests = new List<double>();
            this.clustercount = clusterCount ;
            longests.Add(0);

            priorityQueue.Add(0);
            for (int i = 1; i < pointCount; i++)            
            {
                cost[i] = 10000 ;                                    
                priorityQueue.Add(i);
            }

            while(priorityQueue.Count > 0)
            {
                long n = ExtractMin() ;

                for (int i = 0; i < pointCount; i++)                
                {
                    double weight = w(n,i);
                    if(priorityQueue.Contains(i) && cost[i] > weight )
                    {
                        cost[i] = weight ;
                        parent[i] = n ;
                    }
                }
            }                        

            for (int i = 0; i < pointCount; i++)            
                insert(cost[i]);            

            return Math.Round(longests[(int)clusterCount-2] , 6);            
        }

        private void insert (double p)
        {
            long i =0 ;
            while(p < longests[(int)i])            
            {
                i ++ ;            
                if (i == clustercount)                
                    return ;                
            }                

            longests.Insert((int)i , p);
        }

        private long ExtractMin()
        {
            double min = cost[priorityQueue.First()];
            long minAddress = priorityQueue.First() ;        
            int minIndex = 0 ;

            for (int i = 1; i < priorityQueue.Count ; i++)
            {
                if(cost[priorityQueue[i]] < min)
                {
                    min = cost[priorityQueue[i]] ;
                    minIndex = i ;                    
                }
            }

            long output = priorityQueue[minIndex];
            priorityQueue.RemoveAt(minIndex);
            return output ;
        }

        private double w(long n, int i)
        {
            return Math.Sqrt( Math.Pow((points[n,0] - points[i,0]) , 2)
                    + Math.Pow((points[n,1] - points[i,1]) , 2) ) ;            
        }
    }
}
