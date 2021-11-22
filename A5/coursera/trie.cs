using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A5
{
    public class Q1ConstructTrie 
    {
        public static int Main()
        {
            long n = long.Parse( System.Console.ReadLine());                                                            
            string[] patterns = new string[n];
            for (int i = 0; i < n; i++)
            {
                patterns[i] = System.Console.ReadLine();
            }

            Q1ConstructTrie obj = new Q1ConstructTrie();

            foreach (var item in obj.Solve(n , patterns))
            {
                System.Console.WriteLine(item);   
            }            

            return 0 ;
        }        

        public long clock ;

        public List<Node> Nodes ;        

        public string[] Solve(long n, string[] patterns)
        {
            Nodes = new List<Node>();
            clock = 0 ;            
            List<string> output = new List<string>() ;
            Nodes.Add(new Node(clock ++));

            foreach (var pattern in patterns)
            {
                Node currentNode = Nodes[0] ;
                for (int i = 0; i < pattern.Length; i++)
                {
                    char currentSymbol = pattern[i];
                    if(currentNode.neighbours_labels.Count == 0)
                    {
                        Nodes.Add( new Node(clock++));                        
                        currentNode.AddNeighbour( clock-1 , currentSymbol);
                        output.Add(currentNode.value + "->" + (clock-1).ToString() + ":" + currentSymbol);
                        currentNode = Nodes[(int)clock-1] ;
                    }

                    for (int j = 0; j < currentNode.neighbours_labels.Count; j++)                                                                
                    {
                        if (currentNode.neighbours_labels[j] == currentSymbol)
                        {
                            currentNode = Nodes[(int)currentNode.neighbours_ids[j]];
                            break ;
                        }
                        else if(j==currentNode.neighbours_labels.Count - 1)                        
                        {                            
                            Nodes.Add( new Node(clock++));                            
                            currentNode.AddNeighbour( clock-1 , currentSymbol);
                            output.Add(currentNode.value + "->" + (clock-1).ToString() + ":" + currentSymbol);
                            currentNode = Nodes[(int)clock-1] ;
                        }
                    }
                    // else                    
                }
            }

            
            return output.ToArray() ;            
        }

        public class Node {
            public long value ;

            public Node(long value)
            {
                this.value = value;
                neighbours_ids = new List<long>();
                neighbours_labels = new List<char>();
            }

            public List<long> neighbours_ids ;
            public List<char> neighbours_labels ;

            internal void AddNeighbour(long value, char currentSymbol)
            {
                neighbours_ids.Add(value);
                neighbours_labels.Add(currentSymbol);
            }
        }
    }
}
