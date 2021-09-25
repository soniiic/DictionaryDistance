using System.Collections.Generic;

namespace DictionaryDistance
{
    internal class DistanceResult
    {
        public int Depth { get; }
        public IEnumerable<string> Path { get; }

        public DistanceResult(int depth, IEnumerable<string> path)
        {
            Depth = depth;
            Path = path;
        }
    }
}