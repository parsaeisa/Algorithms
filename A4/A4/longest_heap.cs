
using System;

namespace A4
{
    public class binary_max_heap 
    {
        public double[] H ;
        private long size;
        private long maxSize ;

        public binary_max_heap(double[] h , long d)
        {
            H = h;
            size = 0 ;
            maxSize = d ;
        }

        public long parent(long i) 
        {
            long a = i/2 ;
            return a ; 
        }
        public long LeftChild(long i) => 2*i ;
        public long RightChild(long i) => 2*i +1 ;

        private void SiftUp(long i)
        {
            while(i>0 && H[parent(i)] < H[i])
            {
                swap(parent(i) , i);
                i = parent(i);                
            }
        }

        private void SiftDown(long i)
        {
            long maxIndex = i ;
            long l = LeftChild(i);
            if (l<= size &&  H[l] > H[maxIndex])
                maxIndex = l ;
            
            long r = RightChild(i);
            if (r<= size &&  H[r] > H[maxIndex])
                maxIndex = r ;

            if (i != maxIndex)
            {
                swap(i , maxIndex);
                SiftDown(maxIndex);
            }
        }

        public void insert(double p)
        {
            // if (size == maxSize-1)
            //     remove();
            
            H[size] = p ;                        
            SiftUp(size);
            size ++ ;
        }

        public void remove ()
        {
            H[size] = 100000 ;
            SiftUp(size);
            ExtracMax();
        }

        private void ExtracMax()
        {
            double result = H[0];
            H[0] = H[size];
            size -- ;
            SiftDown(0);
        }

        private void swap(long i , long j)
        {
            double temp = H[i];
            H[i] = H[j] ;
            H[j] = temp ;
        }
    }
}
