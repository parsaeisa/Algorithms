using System;
using System.Collections.Generic;
using System.Linq;
using TestCommon;

namespace A1
{
    public class Q3Acyclic : Processor
    {                
        Node[] vertices;

        long[] post ; 
        bool[] visited ; 
        long clock  ;
        private long output;

        public Q3Acyclic(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long>)Solve);

        public long Solve(long nodeCount, long[][] edges)
        {            

            vertices = new Node[nodeCount];       
            output = 0 ;     
            for (int i = 0; i < nodeCount; i++)            
                vertices[i] = new Node(i+1);  
            
            foreach (var e in edges)            
                vertices[e[0]-1].addNeigbour(e[1]);                        

            post = new long[nodeCount];
            visited = new bool[nodeCount];
            clock = 0 ;

            for (int i = 0; i < nodeCount; i++)            
                if (visited[i] == false)                
                    DFS(i+1);    

            foreach (var e in edges)            
                if (post[e[0]-1] < post[e[1]-1])                
                    return 1 ;                

            // return output ;
            return 0 ;            
        }

        private void DFS(long v)
        {
            visited[v-1] = true ;
            foreach (var n in vertices[v-1].neighbours)            
            {
                if (visited[n-1] == false) 
                {               
                    DFS(n);                                
                    // if (post[n-1] > post[v-1])
                    // {
                    //     this.output = 1 ;
                    //     return ;
                    // }
                }
            }
        
            postVisit(v);
        }

        private void postVisit(long v)
        {
            post[v-1] = clock ;
            clock ++ ;
        }
        class Node 
        {
            public long value ;
            public List<long> neighbours ;        

            public Node(long value)
            {
                this.value = value;                
                neighbours = new List<long>();                
            }

            internal void addNeigbour(long node)
            {
                this.neighbours.Add(node);
            }
        }

    }
}