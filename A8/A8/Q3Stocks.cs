using System;
using System.Collections.Generic;
using TestCommon;

namespace A8
{
    public class Q3Stocks : Processor
    {
        public Q3Stocks(string testDataName) : base(testDataName) {
            // this.ExcludeTestCases(1,2,3,4,5,6,7);
         }

        public override string Process(string inStr) =>
        TestTools.Process(inStr, (Func<long, long, long[][], long>)Solve);
        List<Node> nodes ;
        public bool noPath;

        long[][] AdjacencyMatrix ;

        private long[] correspondingCrews;
        private long pointCount;
        private int pageCount;
        private long[][] matrix;
        private long stockCount;

        public virtual long Solve(long stockCount, long pointCount, long[][] matrix)
        {
            // write your code here
            this.pointCount =pointCount ;
            this.pageCount  = 0 ; 
            this.matrix = matrix ;
            this.stockCount = stockCount ;
            this.correspondingCrews = new long[stockCount];

            for (int i = 0; i < stockCount; i++)            
                correspondingCrews[i] =-1 ;

            nodes = new List<Node>();        

            long residualGraphNodeCount = (stockCount*2) + 2 ;    

            AdjacencyMatrix = new long[ residualGraphNodeCount ][];            

            for (int i = 0; i < residualGraphNodeCount; i++)            
            {
                nodes.Add(new Node(i));            
                AdjacencyMatrix[i] = new long[residualGraphNodeCount ];
            }   

            int source = 0 ,target = (int) residualGraphNodeCount -1 ;             

            for (int i = 0; i < stockCount; i++)
            {
                AdjacencyMatrix[source][1+i] = 1;
                nodes[source].addNeightbour(1+i);
                nodes[1+i].addNeightbour(source);

                AdjacencyMatrix[1+ stockCount +i][target] = 1;
                nodes[target].addNeightbour(1+ stockCount +i);
                nodes[1+ (int)stockCount +i].addNeightbour(target);

            }            

            for (int i = 0; i < stockCount; i++)                            
                for (int j = 0; j < stockCount; j++)                
                    if (stockComparer(j,i) == 1) 
                    {
                        AdjacencyMatrix[1+ i][1+ stockCount +j] = 1;
                        nodes[1+i].addNeightbour(1+ stockCount +j);
                        nodes[1+ (int)stockCount +j].addNeightbour(1+i);
                    }                                        



            long X = BipartiteMatching(nodes);
            return stockCount - X  ;
        }

        int stockComparer (int i , int j) // return 1 if stock(i) > stock()
        {            
            for (int k = 0; k < pointCount; k++)            
                if (matrix[i][k] <= matrix[j][k]) return 0 ;                                                      

            return 1 ;
        }

        private long BipartiteMatching(List<Node> nodes)
        {
            long X = ComputeMaxFlow();            
            return X ;
        }        

        private long ComputeMaxFlow()  => Edmond_Karp(nodes);        

        private long Edmond_Karp(List<Node> nodes)
        {
            long f = 0 ;
            noPath = false ;
            while(true)
            {                                
                FindSTPath(nodes.Count); // adjust parent property for nodes
                if(noPath)
                    return f ; 
                long X = minimumCapacityInP(); 
                gFlow(X);                     
                f += X ;            
            }            
        }

        private void gFlow(long g)
        {
            Node graphPtr = nodes[nodes.Count - 1];                        

            while(graphPtr != nodes[0])
            {                
                AdjacencyMatrix[(int)graphPtr.parent.value][(int)graphPtr.value] -= g ;                                                
                AdjacencyMatrix[(int)graphPtr.value][(int)graphPtr.parent.value] += g ;                                                


                if (graphPtr.parent.value <= stockCount && graphPtr.parent.value > 0)
                {
                   correspondingCrews[graphPtr.parent.value-1] = graphPtr.value - stockCount;
                   pageCount ++ ;
                }


                graphPtr = graphPtr.parent ;                
            }
            
        }

        private long minimumCapacityInP()
        {
            Node graphPtr = nodes[nodes.Count - 1];            
            long X = AdjacencyMatrix[(int)graphPtr.parent.value][(int)nodes.Count -1];

            while(graphPtr != nodes[0])
            {                
                if( AdjacencyMatrix[(int)graphPtr.parent.value][(int)graphPtr.value] < X )
                    X = AdjacencyMatrix[(int)graphPtr.parent.value][(int)graphPtr.value] ;                
                graphPtr = graphPtr.parent ;
            }

            return X ;
        }

        private void FindSTPath(long nodesCount)
        {
            Queue<Node> BFS_queue = new Queue<Node>();
            bool[] visited = new bool[nodes.Count];
            BFS_queue.Enqueue(nodes[0]);
            visited[0] = true ;
            Node currentNode = nodes[0] ;

            while(currentNode != nodes[nodes.Count -1])            
            {                
                if(BFS_queue.Count == 0)
                {
                    this.noPath = true ;
                    return ;
                }

                currentNode = BFS_queue.Dequeue();

                if (currentNode.neighboursIndexes != null)                
                {
                    for (int i = 0; i < currentNode.neighboursIndexes.Count ; i++)
                    {    
                        Node neighbour = nodes[(int)currentNode.neighboursIndexes[i]] ;
                        if (visited[neighbour.value] == false && AdjacencyMatrix[currentNode.value][neighbour.value] > 0 )
                        {                                            
                            neighbour.parent = currentNode ; 
                            visited[neighbour.value] = true ;
                            BFS_queue.Enqueue(neighbour);
                        }
                    }                    
                }

                
            }

        }
        
        class Node 
        {
            public long value ;
            public Node parent ;             

            public Node(long value)
            {
                this.value = value;                
            }             
            public List<long> neighboursIndexes ;            

            public void addNeightbour (long neighbourIndex)        
            {
                if (neighboursIndexes == null)                     
                    neighboursIndexes = new List<long>();                                                    
                
                neighboursIndexes.Add(neighbourIndex);          
            }
        }
    }
}
