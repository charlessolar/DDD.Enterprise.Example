using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Demo.Library.Security.Actors
{
    public class Actor : IActor
    {
        private List<IRule> _rules = new List<IRule>();

        public Actor()
        {
            Description = String.Empty;
        }
        public Actor(string description)
        {
            Contract.Assert(!String.IsNullOrEmpty(description));

            Description = description;
        }

#pragma warning disable 1591 // Xml Comments
        public virtual void AddRule(IRule rule)
        {
            Contract.Requires(rule != null);

            _rules.Add(rule);
        }

        public virtual IEnumerable<IRule> Rules { get { return _rules; } }

        public virtual AuthorizeActorResult IsAuthorized(object instance)
        {
            Contract.Requires(instance != null);

            var result = new AuthorizeActorResult(this);
            foreach (var rule in _rules)
            {
                try
                {
                    if (!rule.IsAuthorized(instance))
                        result.AddBrokenRule(rule);
                }
                catch (Exception ex)
                {
                    result.AddErrorRule(rule, ex);
                }
            }
            return result;
        }

        public string Description { get; private set; }
#pragma warning restore 1591 // Xml Comments
    }
}