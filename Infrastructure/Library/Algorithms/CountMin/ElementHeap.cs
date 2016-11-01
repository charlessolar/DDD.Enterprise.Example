using System.Collections.Generic;
using System.Linq;

namespace Demo.Library.Algorithms.CountMin
{
    internal class ElementHeap
    {
        internal List<Element> Heap { get; set; }

        /// <summary>
        /// Create a new ElementHeap that can store the top-k elements.
        /// </summary>
        /// <param name="k">The number of top elements to track</param>
        internal ElementHeap(int k)
        {
            this.Heap = new List<Element>(k);
        }

        /// <summary>
        /// Get the count of the number of items on the heap.
        /// </summary>
        /// <returns>The number of items on the heap</returns>
        internal int Len()
        {
            return this.Heap.Count;
        }

        /// <summary>
        /// Return whether or not the item at i-position on the heap is less than the
        /// item at j-position.
        /// </summary>
        /// <param name="i">Item 1</param>
        /// <param name="j">Item 2</param>
        /// <returns>
        /// Whether or not the item at i-position on the heap is less than the item at
        /// j-position.
        /// </returns>
        internal bool Less(int i, int j)
        {
            return this.Heap[i].Freq < this.Heap[j].Freq;
        }

        /// <summary>
        /// Swap the items at i-position and j-position on the heap.
        /// </summary>
        /// <param name="i">Item 1</param>
        /// <param name="j">Item 2</param>
        internal void Swap(int i, int j)
        {
            var temp = this.Heap[i];
            Heap[i] = Heap[j];
            Heap[j] = temp;
        }

        /// <summary>
        /// Push an Element onto the heap.
        /// </summary>
        /// <param name="e">The Element to push onto the heap</param>
        internal void Push(Element e)
        {
            this.Heap.Add(e);
            this.Up(this.Len() - 1);
        }

        /// <summary>
        /// Remove the Element at the top of the heap.
        /// </summary>
        /// <returns>The Element that was removed</returns>
        internal Element Pop()
        {
            var elementToRemove = this.Heap[0];
            this.Heap.Remove(elementToRemove);
            return elementToRemove;
        }

        internal void Up(int j)
        {
            while (true)
            {
                var i = (j - 1) / 2; // parent
                if (i == j || !this.Less(j, i))
                {
                    break;
                }
                this.Swap(i, j);
                j = i;
            }
        }

        internal void Down(int i, int n)
        {
            while (true)
            {
                var j1 = 2 * i + 1;
                if (j1 >= n || j1 < 0)
                {
                    // j1 < - after int overflow
                    break;
                }
                var j = j1; // left child
                var j2 = j1 + 1;
                if (j2 < n && !this.Less(j1, j2))
                {
                    j = j2; // 2*i + 2 // right child
                }
                if (!this.Less(j, i))
                {
                    break;
                }
                this.Swap(i, j);
                i = j;
            }
        }

        /// <summary>
        /// Returns the top-k elements from lowest to highest frequency.
        /// </summary>
        /// <returns>The top-k elements from lowest to highest frequency</returns>
        internal Element[] Elements()
        {
            if (this.Len() == 0)
            {
                return new Element[0];
            }

            return this.Heap
                .OrderBy(x => x.Freq)
                .ToArray();
        }

        /// <summary>
        /// Adds the data to the top-k heap. If the data is already an element, the
        /// frequency is updated. If the heap already has k elements, the element with
        /// the minimum frequency is removed.
        /// </summary>
        /// <param name="data">The data to insert</param>
        /// <param name="freq">The frequency to associate with the data</param>
        internal void Insert(byte[] data, ulong freq, uint k)
        {
            for (int i = 0; i < this.Len(); i++)
            {
                var element = this.Heap[i];
                if (data.SequenceEqual(element.Data))
                {
                    // Element already in top-k.
                    element.Freq = freq;
                    return;
                }
            }

            if (this.Len() == k)
            {
                // Remove minimum-frequency element.
                this.Pop();
            }

            // Add element to top-k.
            this.Push(new Element
            {
                Data = data,
                Freq = freq,
            });
        }

        /// <summary>
        /// Indicates if the given frequency falls within the top-k heap.
        /// </summary>
        /// <param name="freq">The frequency to check</param>
        /// <returns>Whether or not the frequency falls within the top-k heap</returns>
        internal bool IsTop(ulong freq, uint k)
        {
            if (this.Len() < k)
            {
                return true;
            }

            return freq >= this.Heap[0].Freq;
        }
    }
}
