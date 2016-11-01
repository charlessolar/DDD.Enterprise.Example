using Demo.Library.Extensions;
using System;
using System.Collections.Generic;

namespace Demo.Library.Algorithms.Frugal
{
    /// <summary>
    /// Maintain a running estimate of a quantile over a stream with very small memory requirements
    /// using the algorithm frugal_2u found in:
    /// http://arxiv.org/pdf/1407.1121v1.pdf
    /// "Frugal Streaming for Estimating Quantiles: One (or two) memory suffices" by Ma, Muthukrishnan and Sandler (2014).
    /// 
    /// One can, for instance, track the median value of a stream of data, or the 68th percentile, or the third decile.
    /// This estimate follows recent values of data; it is not an estimate over all time.
    /// Thus if the quantile you are measuring changes, this will adapt and track the new value.
    /// 
    /// Caveat: The published algorithm uses integers. While this implementation uses doubles, the quantile values cannot
    /// be resolved any finer than one, the minimum step size. To resolve to finer values would require small
    /// changes to this algorithm and much testing to decide how to balance convergence speed with accuracy.
    /// 
    /// Usage:
    /// 
    ///   // Let's track the median, which has quantile = 0.5.
    ///   var seed = 100; // Educated guess for the median.
    ///   var estimator = new FrugalQuantile(seed, 0.5, FrugalQuantile.LinearStepAdjuster);
    ///   IEnumerable data = ... your data ...;
    ///   foreach (var item in data) {
    ///       var newEstimate = estimator.Add(item);
    ///       // Do something with estimate...
    ///   }
    /// 
    /// Author: Paul A. Chernoch
    /// </summary>
    public class FrugalQuantile
    {
        #region Standard functions you can use for StepAdjuster.

        /// <summary>
        /// Best step adjuster found so far because it converges fast without overshooting.
        /// Every time the step grows by an amount that increases by one:
        ///    1, 2, 4, 7, 11, 16, 22, 29...
        /// </summary>
        public static Func<double, double> LinearStepAdjuster = oldStep => oldStep + 1;

        /// <summary>
        /// Step adjuster used in the published paper, which is good, but not as good as LinearStepAdjuster.
        /// Every time the step increases by one:
        ///    1, 2, 3, 4, 5, 6...
        /// </summary>
        public static Func<double, double> ConstantStepAdjuster = oldStep => 1;

        #endregion

        #region Input parameters

        /// <summary>
        /// Quantile whose estimate will be maintained.
        /// If 0.5, the median will be estimated.
        /// If 0.75, the third quartile will be estimated.
        /// Id 0.2, the second decile will be estimated.
        /// etc...
        /// </summary>
        public double Quantile { get; set; }

        /// <summary>
        /// Function to dynamically adjust the step size based on the previous step size.
        /// 
        /// NOTE: Best function found so far: 
        ///    StepAdjuster = step => step + 1;
        /// </summary>
        public Func<double, double> StepAdjuster { get; set; }

        #endregion

        #region Output parameters

        /// <summary>
        /// The running estimate of the value found at the given quantile.
        /// 
        /// This is the value returned by the most recent call to Add.
        /// </summary>
        public double Estimate { get; set; }

        #endregion

        #region Internal state
        private readonly object _lock = new object();
        /// <summary> Max number of elements to hold in the direct representation </summary>
        private const int DirectCounterMaxElements = 100;
        /// <summary> Set for direct counting of elements </summary>
        private ICollection<double> _directCount;
        /// <summary>
        /// Amount to add to or subtract from the current estimate, depending on whether our estimate is too low or too high.
        /// 
        /// As the algorithm proceeds, this is adjusted up and down to improve convergence.
        /// </summary>
        private double Step { get; set; }

        /// <summary>
        /// Tracks whether the previous adjustment was to increase the Estimate or decrease it.
        /// 
        /// If +1, the Estimate increased.
        /// If -1, the Estimate decreased.
        /// This should always have the value +1 or -1.
        /// </summary>
        private sbyte Sign { get; set; }

        /// <summary>
        /// Random number generator.
        /// 
        /// Note: One could refactor to use the C# Random class instead. I prefer FastRandom.
        /// </summary>
        private FastRandom Rand { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a FrugalQuantile to track a running estimate of a quantile value.
        /// </summary>
        /// <param name="seed">Initial estimate for the quantile.
        /// A good initial estimate permits more rapid convergence.</param>    
        /// <param name="quantile">Quantile to estimate, in the exclusive range [0,1].
        /// The default is 0.5, the median.
        /// </param>
        /// <param name="stepAdjuster">Function that can update the step size to improve the rate of convergence.
        /// Its parameter is the previous step size.
        /// The default lambda for this parameter is good, but there are better functions, like this one:
        ///     stepAdjuster = step => step + 1
        /// Researching the function best for your data is recommended.
        /// </param>
        public FrugalQuantile(double quantile)
        {
            if (quantile <= 0 || quantile >= 1)
                throw new ArgumentOutOfRangeException(nameof(quantile), "Must be between zero and one, exclusive.");
            Quantile = quantile;
            this._directCount = new List<double>();
            Step = 1;
            Sign = 1;
            StepAdjuster = ConstantStepAdjuster;
            Rand = new FastRandom();
        }
        internal FrugalQuantile(FrugalState state)
        {
            this.Quantile = state.Quantile;
            this.Estimate = state.Estimate;
            this._directCount = state.DirectCount == null ? null : new List<double>(state.DirectCount);

            StepAdjuster = ConstantStepAdjuster;
            Rand = new FastRandom();
        }

        #endregion

        /// <summary>
        /// Update the quantile Estimate to reflect the latest value arriving from the stream and return that estimate.
        /// </summary>
        /// <param name="item">Data Item arriving from the stream.
        /// Note: This algorithm was designed for use on non-negative integers. Its accuracy or suitability
        /// for negative values is not guaranteed.
        /// </param>
        /// <returns>The new Estimate.</returns>
        public double Add(double item)
        {
            if (this._directCount != null)
            {
                lock (_lock)
                {
                    this._directCount.Add(item);
                    this.Estimate = this._directCount.Percentile(this.Quantile);

                    if (this._directCount.Count > DirectCounterMaxElements)
                        this._directCount = null;
                }
            }
            else
            {

                // This is implemented to resemble as close as possible the pseudo code for function frugal_2u
                // on this page:
                // http://research.neustar.biz/2013/09/16/sketch-of-the-day-frugal-streaming/
                var m = Estimate;
                var q = Quantile;
                var f = StepAdjuster;
                var random = Rand.NextDouble();
                if (item > m && random > 1 - q)
                {
                    // Increment the step size if and only if the estimate keeps moving in
                    // the same direction. Step size is incremented by the result of applying
                    // the specified step function to the previous step size.
                    Step += (Sign > 0 ? 1 : -1) * f(Step);
                    // Increment the estimate by step size if step is positive. Otherwise,
                    // increment the step size by one.
                    m += Step > 0 ? Step : 1;
                    // Mark that the estimate increased this step
                    Sign = 1;
                    // If the estimate overshot the item in the stream, pull the estimate back
                    // and re-adjust the step size.
                    if (m > item)
                    {
                        Step += (item - m);
                        m = item;
                    }
                }
                else if (item < m && random > q)
                {
                    // If the item is less than the stream, follow all of the same steps as
                    // above, with signs reversed.
                    Step += (Sign < 0 ? 1 : -1) * f(Step);
                    m -= Step > 0 ? Step : 1;
                    Sign = -1;
                    if (m < item)
                    {
                        Step += (m - item);
                        m = item;
                    }
                }
                // Damp down the step size to avoid oscillation.
                if ((m - item) * Sign < 0 && Step > 1)
                    Step = 1;

                Estimate = m;
            }
            return Estimate;
        }

        internal FrugalState GetState()
        {
            lock (_lock)
            {
                return new FrugalState
                {
                    DirectCount = this._directCount != null ? new List<double>(this._directCount) : null,
                    Estimate = this.Estimate,
                    Quantile = this.Quantile,
                };
            }
        }
    }
}
