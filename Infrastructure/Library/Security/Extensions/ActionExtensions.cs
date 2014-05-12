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
        public static ExecutingAction Executing(this IBuilder builder)
        {
            return new ExecutingAction();
        }
        public static ReadingAction Reading(this IBuilder builder)
        {
            return new ReadingAction();
        }
        public static ReceivingAction Receiving(this IBuilder builder)
        {
            return new ReceivingAction();
        }
        public static RequestingAction Requesting(this IBuilder builder)
        {
            return new RequestingAction();
        }
        public static WritingAction Writing(this IBuilder builder)
        {
            return new WritingAction();
        }
    }
}