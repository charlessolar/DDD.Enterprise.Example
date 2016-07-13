using NServiceBus.Features;
using NServiceBus.MessageInterfaces.MessageMapper.Reflection;
using NServiceBus.ObjectBuilder;

namespace NServiceBus.Newtonsoft.Json
{
    /// <summary>
    /// Uses JSON as the message serialization.
    /// </summary>
    public class NewtonsoftSerialization : Feature
    {
        internal NewtonsoftSerialization()
        {
            EnableByDefault();
            Prerequisite(this.ShouldSerializationFeatureBeEnabled, "NewtonsoftSerialization not enable since serialization definition not detected.");
        }

        /// <summary>
        /// See <see cref="Feature.Setup"/>
        /// </summary>
        protected override void Setup(FeatureConfigurationContext context)
        {
            Guard.AgainstNull(context, "context");
            context.Container.ConfigureComponent<MessageMapper>(DependencyLifecycle.SingleInstance);
            var c = context.Container.ConfigureComponent<JsonMessageSerializer>(DependencyLifecycle.SingleInstance);

            context.Settings.ApplyTo<JsonMessageSerializer>((IComponentConfig)c);
        }
    }
}