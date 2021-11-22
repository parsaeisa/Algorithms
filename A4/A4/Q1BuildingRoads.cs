using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A4
{
    public class Q1BuildingRoads : Processor
    {
        public Q1BuildingRoads(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], double>)Solve);

        long[]  parent ;
        double[] cost ;
        List<long> priorityQueue ;

        long[][] points ;

        public double Solve(long pointCount, long[][] points)
        {
            cost = new double[pointCount];
            this.points = points ;
            parent = new long[pointCount];
            priorityQueue = new List<long>();

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

            double output = 0 ;
            for (int i = 0; i < pointCount; i++)            
                output += cost[i] ;            

            return Math.Round(output , 6) ;
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
            return Math.Sqrt( Math.Pow((points[n][0] - points[i][0]) , 2)
                    + Math.Pow((points[n][1] - points[i][1]) , 2) ) ;            
        }
    }
}
