using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A11
{
    public class Q2FunParty 
    {

        public static int Main()
        {
            long V = long.Parse(System.Console.ReadLine());                    
            
            long [] funFactor = new long[V];
            long[][] matrix = new long[V-1][] ;                        
            string[] aSplited ;

            aSplited = System.Console.ReadLine().Split(' ');                
            for (int i = 0; i < V; i++)            
                funFactor[i] = long.Parse(aSplited[0]);              

            for (int i = 0; i < V-1; i++)            
            {         
                matrix[i] = new long[2];
                aSplited = System.Console.ReadLine().Split(' ');                
                matrix[i][0] = long.Parse(aSplited[0]);                                    
                matrix[i][1] = long.Parse(aSplited[1]);      
            }                        

            Q2FunParty obj = new Q2FunParty();

            long  ans = obj.Solve(V , funFactor, matrix ) ;

            System.Console.WriteLine(ans);

            return 0 ;
        }

        private Node[] nodes;

        public long[] D { get; private set; }

        public virtual long Solve(long n, long[] funFactors, long[][] hierarchy)
        {
            // write your code here
            this.nodes = new Node[n];
            this.D = new long[n];

            bool[] isRoot = new bool[n];
            for (int i = 0; i < n; i++)
            {
                isRoot[i] = true;
                nodes[i] = new Node(funFactors[i], i);
                D[i] = long.MaxValue;
            }

            for (int i = 0; i < hierarchy.Length; i++)
            {
                nodes[hierarchy[i][1]-1].addNeigbour(nodes[hierarchy[i][0]-1]) ;
                nodes[hierarchy[i][0]-1].addNeigbour(nodes[hierarchy[i][1]-1]) ;
            }

            Node root = nodes[0];

            Queue<Node> BFS_queue = new Queue<Node>();
            BFS_queue.Enqueue(root);            

            while(BFS_queue.Count > 0)
            {
                var currentNode = BFS_queue.Dequeue();
                currentNode.visited = true ;
                if(currentNode.neighbours != null)
                {
                    Node mustBeRemoved = null ;
                    for (int i = 0; i < currentNode.neighbours.Count  ; i++)
                    {
                        var currentNeighbour = currentNode.neighbours[i] ;                        

                        if (currentNeighbour.visited == true)                                                    
                            mustBeRemoved = currentNeighbour ;
                        else
                            BFS_queue.Enqueue(currentNeighbour);                                                

                    }
                    
                    if(mustBeRemoved != null)
                        currentNode.neighbours.Remove(mustBeRemoved);
                }
            }


            long output = 0;
            var a = FunParty(nodes[0]);
            // if(a>output)
            //     output = a;                     
            // return output;
            return a ;
        }
        

        private long FunParty(Node v)
        {
            if (D[v.index] != long.MaxValue)            
                return D[v.index] ;            

            if (v.neighbours == null)            
            {
                D[v.index] = v.weight;            
                return D[v.index] ;
            }                

            long m0 =0 , m1 = v.weight ;
            
            for (int i = 0; i < v.neighbours.Count; i++)
            {
                var u = nodes[v.neighbours[i].index] ;

                if (u.neighbours != null)
                {                                            
                    for (int j = 0; j < u.neighbours.Count; j++)
                    {
                        var w = nodes[u.neighbours[j].index] ;
                        m1 += FunParty(w);
                    }
                }
            }                

            for (int i = 0; i < v.neighbours.Count; i++)
            {
                var u = nodes[v.neighbours[i].index] ;
                m0 += FunParty(u);
            }            

            D[v.index] = Math.Max(m0 , m1);

            return D[v.index] ;
            
        }

        class Node 
        {
            public long index ; 
            public long weight ;
            public Node boss  ;
            public List<Node> neighbours ;                        
            public bool visited ;

            public Node(long weight, long index)
            {
                this.weight = weight;
                visited = false;
                neighbours = null;
                boss = null ;
                this.index = index;
            }

            internal void addNeigbour(Node node)
            {
                if(neighbours == null)
                    neighbours = new List< Node>();

                this.neighbours.Add(node);
            }
        }
    }
}
