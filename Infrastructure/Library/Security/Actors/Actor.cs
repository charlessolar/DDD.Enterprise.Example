using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo.Library.Security.Actors
{
    public class Actor : IActor
    {
        private List<IRule> _rules = new List<IRule>();

        public Actor(string description)
        {
            Description = description ?? string.Empty;
        }

#pragma warning disable 1591 // Xml Comments
        public void AddRule(IRule rule)
        {
            _rules.Add(rule);
        }

        public IEnumerable<IRule> Rules { get { return _rules; } }

        public AuthorizeActorResult IsAuthorized(object instance)
        {
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