using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading ;

namespace A11
{
    public class Q1CircuitDesign 
    {        

        public static int Main()
        {                        
            Thread newThread = new Thread( PrintOutPut,int.MaxValue);
            newThread.Start();

            return 0 ;
        }   
        
        static void PrintOutPut()
        {
            string[] VandE = System.Console.ReadLine().Split(' ');
            int V = int.Parse(VandE[0]);
            int E = int.Parse(VandE[1]);
            
            long[,] matrix = new long[E,2] ;                        
            string[] aSplited ;

            for (int i = 0; i < E; i++)            
            {                
                aSplited = System.Console.ReadLine().Split(' ');                
                matrix[i,0] = long.Parse(aSplited[0]);                                    
                matrix[i,1] = long.Parse(aSplited[1]);      
            }                   
            Q1CircuitDesign obj1 = new Q1CircuitDesign();     
            Tuple<bool , long[]>  ans = obj1.Solve(V , E, matrix ) ;

            if (ans.Item1 == false)            
                System.Console.WriteLine("UNSATISFIABLE");            
            else
            {
                System.Console.WriteLine("SATISFIABLE");            
                for (int i = 0; i < ans.Item2.Length; i++)                
                    System.Console.Write(ans.Item2[i] + " ");                
            }
        }   

        private long vertexCount;
        private bool satisfied;
        private bool[] visited;
        private long[,] adjacencyMatris;
        private long[] satisfyingAssignment;        
        private Node[] vertices;
        private List<long> sorted;
        private List<long> sccLiterals;        

        public virtual Tuple<bool, long[]> Solve(long vertexCount, long condCount, long[,] cnf)
        {

            this.vertexCount = vertexCount ;
            this.satisfied = true ;      

            this.visited = new bool[vertexCount *2]  ;
            // this.adjacencyMatris = new long[vertexCount * 2, vertexCount * 2];

            this.satisfyingAssignment = new long[vertexCount] ;            

            vertices = new Node[vertexCount*2];
            sorted = new List<long>();
            // build implication graph            
            for (int i = 0; i < vertexCount*2 ; i++)            
                vertices[i] = new Node(i);                 
            
            for (int i = 0; i < condCount; i++)
            {               

                long l1bar , l2 ;
                l1bar = cnf[i,0] > 0 ? cnf[i,0] -1 +  vertexCount : Math.Abs(cnf[i,0])-1  ;
                l2 = cnf[i,1] > 0 ? cnf[i,1] -1 : Math.Abs(cnf[i,1])-1 +  vertexCount ;            

                vertices[l1bar].addNeigbour(vertices[l2]);
                // adjacencyMatris[l1bar , l2] = 1 ;
                vertices[l2].addRNeigbour(vertices[l1bar]);                

                long l2bar , l1 ;
                l2bar = cnf[i,1] > 0 ? cnf[i,1] -1 +  vertexCount : Math.Abs(cnf[i,1])-1  ;
                l1 = cnf[i,0] > 0 ? cnf[i,0] -1 : Math.Abs(cnf[i,0])-1 +  vertexCount ;            

                vertices[l2bar].addNeigbour(vertices[l1]);
                // adjacencyMatris[l2bar , l1] = 1 ;
                vertices[l1].addRNeigbour(vertices[l2bar]);                
            }            
            for (int i = 0; i < vertexCount * 2; i++)
            {
                if (visited[i] == false)
                {
                    RDFS(i);                    
                }
            }                   
            
            visited = new bool[2*vertexCount]; 
                        
            for (int i = 0; i < sorted.Count; i++)            
            {                
                var v = sorted[sorted.Count - i - 1];
                if(visited[v] == false )
                {
                    sccLiterals = new List<long>();
                    DFS(v);                    
                    if (satisfied == false)                     
                        return new Tuple<bool, long[]>(satisfied, null) ;                                        
                }                
            }



            // write your code here
            return new Tuple<bool, long[]>(satisfied , this.satisfyingAssignment) ;
        }

        private void DFS(long v)
        {
            sccLiterals.Add(v);
            visited[v] = true ;
            if(vertices[v].neighbours != null)
                // foreach (var n in vertices[v].neighbours)            
                for (int i = 0; i < vertices[v].neighbours.Count; i++)
                // for (int i = 0; i < vertexCount*2; i++)
                // {
                //     if(adjacencyMatris[v,i]==1)
                    {
                        var n = vertices[v].neighbours[i];
                        // var n = vertices[i] ;
                        if (visited[n.value] == false)                
                            DFS(n.value);           
                    }
                // }

            if(v < vertexCount)
                if (sccLiterals.Contains(vertexCount + v))
                {
                    this.satisfied = false ;
                    return ;    
                }
            else if (vertexCount >= v)
                if ( sccLiterals.Contains(v-vertexCount ) )
                {
                    this.satisfied = false ;
                    return ;
                }   

            if(v >= vertexCount)
            {
                if(satisfyingAssignment[v - vertexCount] == 0)
                    satisfyingAssignment[v - vertexCount] = - (v-vertexCount+1) ;    
            }
            else
                if(satisfyingAssignment[v] == 0)
                    satisfyingAssignment[v] = v+1 ;    
        }

        private void RDFS(long v)
        {
            visited[v] = true ;
            if(vertices[v].Rneighbours != null)
                // foreach (var n in vertices[v].Rneighbours)            
                for (int i = 0; i < vertices[v].Rneighbours.Count; i++)
                // for (int i = 0; i < vertexCount*2; i++)
                // {
                //     if(adjacencyMatris[i,v]==1)                    
                    {
                        var n = vertices[v].Rneighbours[i];
                        // var n = vertices[i];
                        if (visited[n.value] == false)                
                            RDFS(n.value);                         
                    }
                // }
                        
            sorted.Add(v);
        }

        class Node 
        {
            public long value ;
            public List<Node> neighbours ;
            public List<Node> Rneighbours ;
            public bool visited ;

            public Node(long value)
            {
                this.value = value;
                visited = false ;
                neighbours = null ;
                Rneighbours = null ;
            }

            internal void addNeigbour(Node node)
            {
                if (neighbours == null)                
                    neighbours = new List<Node>();                
                this.neighbours.Add(node);
            }

            internal void addRNeigbour(Node node)
            {
                if (Rneighbours == null)                
                    Rneighbours = new List<Node>();                
                this.Rneighbours.Add(node);
            }
        }
    }
}
