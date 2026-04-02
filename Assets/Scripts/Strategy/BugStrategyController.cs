using System;
using System.Collections.Generic;
using System.Linq;
using BugColony.Bugs;
using Zenject;

namespace BugColony.Strategies
{
    public class BugStrategyController
    {
        private Dictionary<BugType, IBugStrategy> _typeToStrategy;

        [Inject]
        private void Construct(List<IBugStrategy> strategies)
        {
            _typeToStrategy = strategies.ToDictionary(s => s.SupportedType);
        }

        public IBugStrategy GetStrategy(BugType type)
        {
            if (_typeToStrategy.TryGetValue(type, out var strategy))
                return strategy;
            throw new InvalidOperationException($"No strategy registered for BugType.{type}");
        }
    }
}
