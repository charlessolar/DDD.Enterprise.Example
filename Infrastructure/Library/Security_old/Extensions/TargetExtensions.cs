using Demo.Library.Security.Actions;
using Demo.Library.Security.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Security
{
    public static class TargetExtensions
    {
        public static QueryTarget Queries(this ReceivingAction action)
        {
            var target = new QueryTarget();
            action.AddTarget(target);
            return target;
        }

        public static FunctionTarget Functions(this ExecutingAction action)
        {
            var target = new FunctionTarget();
            action.AddTarget(target);
            return target;
        }

        public static PropertyTarget Properties(this ReadingAction action)
        {
            var target = new PropertyTarget();
            action.AddTarget(target);
            return target;
        }
        public static PropertyTarget Properties(this WritingAction action)
        {
            var target = new PropertyTarget();
            action.AddTarget(target);
            return target;
        }
    }
}