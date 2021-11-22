using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace A8
{
    public class Q2Airlines
    {

        public static int Main()
        {
            string[] s = System.Console.ReadLine().Split(' ');
            long flightCount = long.Parse(s[0]);
            long crewCount = long.Parse(s[1]);
            
            long[,] edges = new long[flightCount,crewCount] ;                        
            string[] aSplited ;

            for (int i = 0; i < flightCount; i++)            
            {                
                aSplited = System.Console.ReadLine().Split(' ');
                for (int j = 0; j < crewCount; j++)                
                    edges[i,j] = long.Parse(aSplited[j]);                                       
            }            

            Q2Airlines obj = new Q2Airlines();

            long[] ans = obj.Solve(flightCount , crewCount ,edges) ;

            for (int i = 0; i < flightCount; i++)
            {
                System.Console.Write(ans[i]);
                System.Console.Write(" ");
            }            

            return 0 ;
        }
    
        long[][] AdjacencyMatrix ;
        List<Node> nodes ;
        private SortedSet<Node> sortedCrews;
        bool noPath ;
        private long crewCount;
        private long flightCount;
        private long[] correspondingCrews;
        long howManyTimesWeGotToSink ;

        public virtual long[] Solve(long flightCount, long crewCount, long[,] info)
        {            
            howManyTimesWeGotToSink=0 ;
            this.crewCount = crewCount ;
            this.flightCount = flightCount ;
            long newMatrixLength = flightCount + crewCount + 2 ;
            AdjacencyMatrix = new long[newMatrixLength][];
            nodes = new List<Node>();            

            for (int i = 0; i < newMatrixLength; i++)    
            {        
                nodes.Add(new Node(i))   ;     
                AdjacencyMatrix[i] = new long[newMatrixLength];
            }

            // connect flights to source 
            // connect flights to crews
            for (int i = 0; i < flightCount; i++)            
            {
                AdjacencyMatrix[0][i+1] = 1 ;  
                nodes[0].addNeightbour(i+1);
                nodes[i+1].addNeightbour(0);
                for (int j = 0; j < crewCount; j++)                
                {
                    AdjacencyMatrix[i+1][j + 1 + flightCount] = info[i,j] ;                
                    if(info[i,j] == 1 )
                    {
                        nodes[i+1].addNeightbour(j + 1 + flightCount );
                        nodes[j + 1 + (int) flightCount].addNeightbour(i+1);
                    }
                }
                
            }

            // connect crews to sink
            for (int i = 0; i < crewCount; i++) 
            {   
                Node crew = nodes[i+(int)flightCount + 1] ;                        
                AdjacencyMatrix[i+flightCount + 1][newMatrixLength-1] = 1 ;            
                crew.addNeightbour(newMatrixLength-1);
                crew.passedThrough = new Dictionary<long, long>();
                nodes[(int)newMatrixLength-1].addNeightbour(i+(int)flightCount + 1);
            }
            
            correspondingCrews = new long[flightCount];
            for (int i = 0; i < flightCount; i++)            
                correspondingCrews[i] =-1 ;                        

            return BipartiteMatching(nodes);

        }

        private long[] BipartiteMatching(List<Node> nodes)
        {
            ComputeMaxFlow();            
            return correspondingCrews ;
        }
        private void ComputeMaxFlow()  
        {
            Edmond_Karp(nodes);        
        }

        private void Edmond_Karp(List<Node> nodes)
        {
            long f = 0 ;
            noPath = false ;
            while(true)
            {                                
                FindSTPath(nodes.Count); // adjust parent property for nodes
                if(noPath)
                    return ; 
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


                if (graphPtr.parent.value <= flightCount && graphPtr.parent.value > 0)
                    correspondingCrews[graphPtr.parent.value-1] = graphPtr.value - flightCount;


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
            public Dictionary<long , long> passedThrough;

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
