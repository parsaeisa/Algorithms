using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A11
{
    public class Q3SchoolBus
    {
        public static int Main()
        {
            string[] VandE = System.Console.ReadLine().Split(' ');
            int V = int.Parse(VandE[0]);
            int E = int.Parse(VandE[1]);
            
            long[,] matrix = new long[E,3] ;                        
            string[] aSplited ;

            for (int i = 0; i < E; i++)            
            {                
                aSplited = System.Console.ReadLine().Split(' ');                
                matrix[i,0] = long.Parse(aSplited[0]);                                    
                matrix[i,1] = long.Parse(aSplited[1]);      
                matrix[i,2] = long.Parse(aSplited[2]);      
            }                        

            Q3SchoolBus obj = new Q3SchoolBus();

            Tuple<long , long[]>  ans = obj.Solve(V, matrix ) ;
            
            System.Console.WriteLine(ans.Item1); 
            if(ans.Item1 != -1)           
                for (int i = 0; i < ans.Item2.Length; i++)                
                    System.Console.Write(ans.Item2[i] + " ");                            

            return 0 ;
        }
        private Node[] nodes;        
        private long minimumCycle;        
        private long nodeCount;
        Tuple<long, long[]> output;

        public bool Possibility { get; private set; }

        private int nodeIterator;        
        public virtual Tuple<long, long[]> Solve(long nodeCount, long[,] edges)
        {
            // write your code here

            this.nodes = new Node[nodeCount];
            
            this.Possibility = true ;            

            this.nodeCount = nodeCount;               

            for (int i=0; i<nodeCount; i++)            
                nodes[i] = new Node(i);   
            
            for(int i=0; i<edges.Length; i++)
            {
                // var edge = edges[i];
                nodes[edges[i,0] - 1].AddNeigbour(nodes[edges[i,1] - 1] , edges[i,2]);
                nodes[edges[i,1] - 1].AddNeigbour(nodes[edges[i,0] - 1] , edges[i,2]);
            }

            this.minimumCycle = long.MaxValue;

            for (this.nodeIterator = 0; nodeIterator < nodeCount; nodeIterator++)
            {
                if (nodes[nodeIterator].neighbours == null)
                    return new Tuple<long, long[]>(-1 , null);

                bool[] visitedInCycle = new bool[nodeCount] ;
                visitedInCycle[nodes[nodeIterator].index] = true;
                long visitedInCycleCount ;
                long[] outputArray = new long[nodeCount];
                outputArray[0] = nodes[nodeIterator].index+1;                                
                                
                for (int j =0; j < nodes[nodeIterator].neighbours.Count; j ++)
                {
                    long cycleWeight = nodes[nodeIterator].weights[j];
                    visitedInCycleCount = 1;
                    DFS(nodes[nodeIterator].neighbours[j], cycleWeight,  visitedInCycle, visitedInCycleCount,  outputArray);

                    if (this.Possibility == false)
                        return output;
                }                    

                break ;
            }
                       
            return output; 
        }

        private void DFS(Node node , long cycleWeight ,  bool [] visitedInCycleInput, long visitedInCycleCount, long[] outputArrayInput)
        {            

            bool[] visitedInCycle = (bool[]) visitedInCycleInput.Clone();
            long[] outputArray = (long[]) outputArrayInput.Clone();                        

            // if (outputArray[4] == 12 && outputArray[3] == 5 
            //     && outputArray[1] == 10 && outputArray[2] == 2 )
            // {
            //     int b = 12 ;
            // }

            bool isDeadEnd = false;
            
            visitedInCycle[node.index] = true;
            visitedInCycleCount++;
            outputArray[visitedInCycleCount - 1] = node.index+1;


            for (int neighbourIter =0; neighbourIter < node.neighbours.Count; neighbourIter ++)
            {                
                Node neighbour = node.neighbours[neighbourIter];
                long edgeWeight = node.weights[neighbourIter];       

                if (this.Possibility == false)
                    return ;                

                if (neighbour.neighbours.Count <= 1)
                {
                    this.output = new Tuple<long, long[]>(-1, null);
                    this.Possibility = false ;
                    return ;
                }         

                if (visitedInCycle[neighbour.index] == true)
                    continue;

                isDeadEnd = false;

                cycleWeight += edgeWeight;

                if (cycleWeight >= minimumCycle)
                    continue;

                if (visitedInCycleCount == nodeCount-1)                
                    visitedInCycle[nodeIterator] = false;                

                if (visitedInCycleCount == nodeCount )
                {
                    if (cycleWeight == 64)
                    {
                        int a =12 ;
                    }
                    this.minimumCycle = cycleWeight ;
                    this.output = new Tuple<long, long[]>(minimumCycle, outputArray);
                    continue;
                }

               
                DFS(neighbour, cycleWeight , visitedInCycle , visitedInCycleCount , outputArray);

                cycleWeight -= edgeWeight;
            }

            // if(isDeadEnd)
            // {
            //     this.output = new Tuple<long, long[]>(-1, null);
            //     this.Possibility = false;
            //     return;
            // }
        }

        class Node
        {
            public long index;
            public List<long> weights;
            public List<Node> neighbours;
            public bool visited;

            public Node( long index)
            {                
                visited = false;
                neighbours = null;
                this.index = index;
                weights = null;
            }

            internal void AddNeigbour(Node node , long weight)
            {
                if (neighbours == null)
                {
                    neighbours = new List<Node>();
                    weights = new List<long>();
                }

                this.neighbours.Add(node);
                this.weights.Add(weight);                   
            }
        }
    }
}

