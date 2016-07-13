using System;

namespace NServiceBus.Newtonsoft.Json
{
    static class Guard
    {

        // ReSharper disable UnusedParameter.Global
        public static void AgainstNull(object value, string argumentName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }
    }
}