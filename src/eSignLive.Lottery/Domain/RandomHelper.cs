using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eSignLive.Lottery.Domain
{
    public static class RandomHelper
    {
        private static int _seedCounter = new Random().Next();
        [ThreadStatic]
        private static Random _rng;
        public static Random Instance
        {
            get
            {
                if (_rng == null)
                {
                    int seed = Interlocked.Increment(ref _seedCounter);
                    _rng = new Random(seed);
                }
                return _rng;
            }
        }
    }
}