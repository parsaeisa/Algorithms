using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A11
{
    public class Q3SchoolBus : Processor
    {
        private Node[] nodes;        
        private long minimumCycle;        
        private long nodeCount;
        Tuple<long, long[]> output;

        public bool Possibility { get; private set; }

        private int nodeIterator;

        public Q3SchoolBus(string testDataName) : base(testDataName) {
            // for (int i = 1; i <= 20; i++)
            // {
            //     this.ExcludeTestCases(i);
            // }
            // this.ExcludeTestCases(21);
            // this.ExcludeTestCases(1, 2 ,3 ,4 );            
        }

        public override string Process(string inStr)=>
        TestTools.Process(inStr, (Func<long, long[][], Tuple<long, long[]>>)Solve);

        public override Action<string, string> Verifier { get; set; } =
            TestTools.TSPVerifier;

        public virtual Tuple<long, long[]> Solve(long nodeCount, long[][] edges)
        {
            // write your code here

            this.nodes = new Node[nodeCount];
            
            this.Possibility = true ;            

            this.nodeCount = nodeCount;               

            for (int i=0; i<nodeCount; i++)            
                nodes[i] = new Node(i);   
            
            for(int i=0; i<edges.Length; i++)
            {
                var edge = edges[i];
                nodes[edge[0] - 1].AddNeigbour(nodes[edge[1] - 1] , edge[2]);
                nodes[edge[1] - 1].AddNeigbour(nodes[edge[0] - 1] , edge[2]);
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

                
                if(minimumCycle == 64 && outputArray[0] == 1  
                    && outputArray[1] == 10
                        && outputArray[2] == 2
                            && outputArray[3] == 5)
                            {
                                int a = 3451345 ;
                            }   

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
                    if(minimumCycle == 64 && outputArray[0] == 1  
                        && outputArray[1] == 10
                            && outputArray[2] == 2
                                && outputArray[3] == 5)
                                {
                                    int a = 3451345 ;
                                }
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

