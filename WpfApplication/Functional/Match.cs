using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WpfApplication.Functional
{
    public class Match<T> : Dictionary<IEnumerable, Func<T>>
    {
        private readonly object[] _arguments;

        public Match(params object[] arguments)
        {
            _arguments = arguments;
        }

        public T Evaluate()
        {
            foreach (var pattern in this)
            {
                var matches = pattern.Key.OfType<object>().Zip(_arguments, Compare).All(p => p);
                if (matches)
                {
                    return pattern.Value();
                }
            }
            throw new InvalidOperationException("Non-exhaustive pattern matching.");
        }

        private static bool Compare(object patternValue, object value)
        {
            return value == Any.Value || Equals(patternValue, value);
        }

        public static implicit operator T(Match<T> match)
        {
            return match.Evaluate();
        }
    }
}
