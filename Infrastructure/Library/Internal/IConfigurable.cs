using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Internal
{
    /// <summary>
    /// Represents an object that is configurable.
    /// </summary>
    /// <typeparam name="TConfiguration">Type of object being configured</typeparam>
    /// <typeparam name="TNext">Return type</typeparam>
    public interface IConfigurable<TConfiguration, out TNext>
    {
        /// <summary>
        /// Configures the current object.
        /// </summary>
        /// <param name="configurator">Action to configure the object.</param>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        TNext Configure(Action<TConfiguration> configurator);
    }
}