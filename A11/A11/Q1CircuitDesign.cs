using System;
using System.Collections.Generic;
using System.Linq;
using TestCommon;
using System.Threading ;
// using Microsoft.SolverFoundation.Solvers;

namespace A11
{
    public class Q1CircuitDesign : Processor
    {        
        private long vertexCount;
        private bool satisfied;
        private bool[] visited;
        private long[,] adjacencyMatris;
        private long[] satisfyingAssignment;        
        private Node[] vertices;
        private List<long> sorted;
        private List<long> sccLiterals;
        private long index ;

        Stack<Node> S ;

        public Q1CircuitDesign(string testDataName) : base(testDataName) {
            this.ExcludeTestCases(1,2,3,4,5);
        }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, 
                (Func<long, long, long[][], Tuple<bool, long[]>>)Solve);

        public override Action<string, string> Verifier =>
            TestTools.SatAssignmentVerifier;

        public virtual Tuple<bool, long[]> Solve(long vertexCount, long condCount, long[][] cnf)
        {
            this.index = 0 ;            
            this.S = new Stack<Node>();

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
                l1bar = cnf[i][0] > 0 ? cnf[i][0] -1 +  vertexCount : Math.Abs(cnf[i][0])-1  ;
                l2 = cnf[i][1] > 0 ? cnf[i][1] -1 : Math.Abs(cnf[i][1])-1 +  vertexCount ;            

                vertices[l1bar].addNeigbour(vertices[l2]);
                // adjacencyMatris[l1bar , l2] = 1 ;
                // vertices[l2].addRNeigbour(vertices[l1bar]);                

                long l2bar , l1 ;
                l2bar = cnf[i][1] > 0 ? cnf[i][1] -1 +  vertexCount : Math.Abs(cnf[i][1])-1  ;
                l1 = cnf[i][0] > 0 ? cnf[i][0] -1 : Math.Abs(cnf[i][0])-1 +  vertexCount ;            

                vertices[l2bar].addNeigbour(vertices[l1]);
                // adjacencyMatris[l2bar , l1] = 1 ;
                // vertices[l1].addRNeigbour(vertices[l2bar]);                
            }            
            
            for (int i = 0; i < vertices.Length ; i++)
            {
                var v = vertices[i] ;
                if(v.index == -1)
                    strongComponent(v);
            }


            // write your code here
            return new Tuple<bool, long[]>(satisfied , this.satisfyingAssignment) ;
        }

        private void strongComponent(Node v)
        {
            v.index = this.index ;
            v.lowlink = this.index ;
            index++ ;
            S.Push(v);
            v.onStack = true ;

            if(v.neighbours != null)
                for (int i = 0; i < v.neighbours.Count; i++)
                {
                    var w = v.neighbours[i] ;
                    if (w.index == -1)
                    {
                        strongComponent(w);
                        v.lowlink = Math.Min(v.lowlink , w.lowlink);

                    }
                    else if (w.onStack)
                        v.lowlink = Math.Min(v.lowlink , w.index) ;
                }

            if(v.lowlink == v.index)
            {
                List<long> sccLiterals = new List<long>() ;
                var w = S.Pop();
                while(w != v)
                {
                    w.onStack = false ;
                    sccLiterals.Add(w.value);
                    // add w to current strongly connected component
                    if(w.value < vertexCount)
                        if (sccLiterals.Contains(vertexCount + w.value))
                        {
                            this.satisfied = false ;
                            return ;    
                        }
                    else if (vertexCount >= w.value)
                        if ( sccLiterals.Contains(w.value-vertexCount ) )
                        {
                            this.satisfied = false ;
                            return ;
                        }   

                    if(w.value >= vertexCount)
                    {
                        if(satisfyingAssignment[w.value - vertexCount] == 0)
                            satisfyingAssignment[w.value - vertexCount] = - (w.value-vertexCount+1) ;    
                    }
                    else
                        if(satisfyingAssignment[w.value] == 0)
                            satisfyingAssignment[w.value] = w.value+1 ; 
                    w = S.Pop();
                }
            }
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
            public long index , lowlink  ;
            public bool onStack;
            

            public Node(long value)
            {
                this.index = -1 ;
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
