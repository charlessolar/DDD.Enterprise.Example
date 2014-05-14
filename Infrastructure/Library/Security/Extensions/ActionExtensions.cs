using Demo.Library.Security.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Security
{
    public static class ActionExtensions
    {
        public static ExecutingAction Executing(this IDescriptor descriptor)
        {
            var action = new ExecutingAction();
            descriptor.AddAction(action);
            return action;
        }
        public static ReadingAction Reading(this IDescriptor descriptor)
        {
            var action = new ReadingAction();
            descriptor.AddAction(action);
            return action;
        }
        public static ReceivingAction Receiving(this IDescriptor descriptor)
        {
            var action = new ReceivingAction();
            descriptor.AddAction(action);
            return action;
        }
        public static RequestingAction Requesting(this IDescriptor descriptor)
        {
            var action = new RequestingAction();
            descriptor.AddAction(action);
            return action;
        }
        public static WritingAction Writing(this IDescriptor descriptor)
        {
            var action = new WritingAction();
            descriptor.AddAction(action);
            return action;
        }
    }
}