using System;
using System.Collections.Generic;
using TestCommon;

namespace A8
{
    public class Q1Evaquating : Processor
    {
        public Q1Evaquating(string testDataName) : base(testDataName) 
        {
            // this.ExcludeTestCases(34,35,36,37,38);
        } 

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long, long[][], long>)Solve);

        List<Node> nodes ;
        public bool noPath;

        long[][] AdjacencyMatrix ;

        public virtual long Solve(long nodeCount, long edgeCount, long[][] edges)
        {
            // write your code here
            nodes =  new List<Node>();         
            AdjacencyMatrix = new long[nodeCount][];
            for (int i = 0; i < nodeCount; i++)    
            {        
                nodes.Add(new Node(i))   ;     
                AdjacencyMatrix[i] = new long[nodeCount];
            }

            for (int i = 0; i < edgeCount; i++)    
            {        
                nodes[(int)edges[i][0]-1].addNeightbour((int)edges[i][1]-1);            
                nodes[(int)edges[i][1]-1].addNeightbour((int)edges[i][0]-1);            
                AdjacencyMatrix[edges[i][0]-1][edges[i][1]-1] += edges[i][2] ;                            
            }

            return Edmond_Karp (nodes);            
        }

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
                            neighbour.parent = currentNode ; // could make a problem
                            // nodes[(int) currentNode.neighboursIndexes[i]].edgeToParent = currentNode.weights[i] ;
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
