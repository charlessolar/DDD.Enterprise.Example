namespace Demo.Library.Algorithms.CountMin
{
    /// <summary>
    /// TopK uses a Count-Min Sketch to calculate the top-K frequent elements in a
    /// stream.
    /// </summary>
    public class TopK
    {
        private CountMinSketch Cms { get; set; }
        private uint K { get; set; }
        internal uint N { get; set; }
        private ElementHeap elements { get; set; }

        /// <summary>
        /// Creates a new TopK backed by a Count-Min sketch whose relative accuracy is
        /// within a factor of epsilon with probability delta. It tracks the k-most
        /// frequent elements.
        /// </summary>
        /// <param name="epsilon">Relative-accuracy factor</param>
        /// <param name="delta">Relative-accuracy probability</param>
        /// <param name="k">Number of top elements to track</param>
        /// <returns></returns>
        public TopK(double epsilon, double delta, uint k)
        {
            this.Cms = new CountMinSketch(epsilon, delta);
            this.K = k;
            this.elements = new ElementHeap((int)k);
        }

        /// <summary>
        /// Will add the data to the Count-Min Sketch and update the top-k heap if
        /// applicable. Returns the TopK to allow for chaining.
        /// </summary>
        /// <param name="data">The data to add</param>
        /// <returns>The TopK</returns>
        public TopK Add(byte[] data)
        {
            this.Cms.Add(data);
            this.N++;

            var freq = this.Cms.Count(data);
            if (this.elements.IsTop(freq, this.K))
            {
                elements.Insert(data, freq, this.K);
            }

            return this;
        }

        /// <summary>
        /// Returns the top-k elements from lowest to highest frequency.
        /// </summary>
        /// <returns>The top-k elements from lowest to highest frequency</returns>
        public Element[] Elements()
        {
            return elements.Elements();
        }

        /// <summary>
        /// Restores the TopK to its original state. It returns itself to allow for
        /// chaining.
        /// </summary>
        /// <returns>The TopK</returns>
        public TopK Reset()
        {
            this.Cms.Reset();
            this.elements = new ElementHeap((int)K);
            this.N = 0;
            return this;
        }
    }
}
