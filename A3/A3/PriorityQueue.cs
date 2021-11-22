using System;
using System.Collections.Generic;

namespace A3
{
    class PriorityQueue 
        {
            // List<Tuple<long , long>> H ;
            long[] H ;
            long size ;

            public PriorityQueue(long nodeCount) 
            {
                H = new long[nodeCount];
                size = -1 ;
            }

            private long Parent(long i) => i/2 ;
            private long LeftChild(long i) => 2 * i ;
            private long RightChild(long i) => 2*i + 1 ;

            private void SiftUp(long i , long[] dist)
            {
                while(i>0 && dist[H[(int)Parent(i)]] > dist[H[(int)i]] )
                {
                    swap(Parent(i) , i);
                    i = Parent(i);
                }
            }          

            private void SiftDown(long i , long[] dist)
            {
                long minIndex = i ;
                
                long l = LeftChild(i);
                if(l <= size && dist[H[(int)l]] < dist[H[(int)minIndex]])
                    minIndex = l ; 

                long r = RightChild(i);
                if(r <= size && dist[H[(int)r]]< dist[H[(int)minIndex]])
                    minIndex = r ; 

                if(i != minIndex)
                {
                    swap(i , minIndex);
                    SiftDown(minIndex , dist );
                }
            }        

            private void swap(long a , long b )
            {
                long tmp = H[(int)a] ;
                H[(int)a]= H[(int)b] ;
                H[(int)b] = tmp ;            
            }  

            public void Insert(long a , long[] dist)
            {
                size ++ ;
                H[size] = a ;
                SiftUp(size , dist);
            }

            public long ExtractMin(long[] dist)
            {
                long output  = H[0];
                H[0] = H[size];
                size -- ;                
                SiftDown(0 , dist);
                return output ;
            }
    
            public void ChangePriority( long i , long p , long[] dist)
            {
                long oldp = H[ (int) i];                
                // H[(int) i ] = p ;
                
                if( p > dist[oldp])
                    SiftUp(i , dist);
                else
                    SiftDown(i , dist);
            }

        }
}