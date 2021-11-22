using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestCommon;

namespace A1
{
    public class Q5StronglyConnected: Processor
    {
        Node[] vertices ;
        List<long> sorted ;

        public Q5StronglyConnected(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long[][], long>)Solve);

        public long Solve(long nodeCount, long[][] edges)
        {
            vertices = new Node[nodeCount];
            sorted = new List<long>();

            for (int i = 0; i < nodeCount; i++)            
                vertices[i] = new Node(i+1);            

            foreach (var e in edges)
            {
                vertices[e[0]-1].addNeigbour(e[1]);
                vertices[e[1]-1].addRNeigbour(e[0]);                
            }

            for (int i = 0; i < nodeCount; i++)
            {
                if (vertices[i].visited == false)
                {
                    RDFS(i+1);
                }
            }       

            long output = 0 ;

            for (int i = 0; i < nodeCount; i++)            
                vertices[i].visited = false ;            

            foreach (var v in sorted)
            {
                if(vertices[v-1].visited == false )
                {
                    DFS(v);
                    output ++ ;
                }                
            }

            return output ;

        }

        private void DFS(long v)
        {
            vertices[v-1].visited = true ;
            foreach (var n in vertices[v-1].neighbours)            
                if (vertices[n-1].visited == false)                
                    DFS(n);                            
        }

        private void RDFS(long v)
        {
            vertices[v-1].visited = true ;
            foreach (var n in vertices[v-1].Rneighbours)            
                if (vertices[n-1].visited == false)                
                    RDFS(n);                
            sorted.Insert(0 , v);
        }

        class Node 
        {
            public long value ;
            public List<long> neighbours ;
            public List<long> Rneighbours ;
            public bool visited ;

            public Node(long value)
            {
                this.value = value;
                visited = false ;
                neighbours = new List<long>();
                Rneighbours = new List<long>();
            }

            internal void addNeigbour(long node)
            {
                this.neighbours.Add(node);
            }

            internal void addRNeigbour(long node)
            {
                this.Rneighbours.Add(node);
            }
        }
    }
}
