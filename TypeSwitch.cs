using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Classes
{
   public class TypeSwitch
   {
      private readonly Action _defaultAction;
      private readonly bool _useStrictComparison;
      private readonly Dictionary<Type, Action<object>> _matches = new Dictionary<Type, Action<object>>();


      public TypeSwitch(Action defaultAction,bool useStrictComparison = false)
      {
         _defaultAction = defaultAction;
         _useStrictComparison = useStrictComparison;
      }

      public TypeSwitch Case<T>(Action<T> action)
      {
         var match = _useStrictComparison
            ? _matches.SingleOrDefault(y => y.Key.IsAssignableFrom(typeof(T)))
            : _matches.SingleOrDefault(y => y.Key == typeof(T));

         if (match.Key != null)
            throw new Exception("Cannot add similar type twice.");
         _matches.Add(typeof(T), x => action((T) x));
         return this;
      }

      public void Switch(object x)
      {
         var match = !_useStrictComparison
            ? _matches.SingleOrDefault(y => y.Key.IsInstanceOfType(x))
            : _matches.SingleOrDefault(y => y.Key == x.GetType());

         if (match.Key != null) _matches[x.GetType()](x);
         else _defaultAction?.Invoke();
      }
   }
}