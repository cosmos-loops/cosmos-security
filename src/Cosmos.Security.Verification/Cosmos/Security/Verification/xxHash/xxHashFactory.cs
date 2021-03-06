﻿// ReSharper disable once CheckNamespace

namespace Cosmos.Security.Verification
{
    /// <summary>
    /// xxHash Hash Function Factory
    /// </summary>
    public static class xxHashFactory
    {
        public static IxxXHash Create(xxHashTypes type = xxHashTypes.xxHashBit32)
        {
            return Create(type, new xxHashConfig());
        }

        public static IxxXHash Create(xxHashTypes type, xxHashConfig config)
        {
            config.CheckNull(nameof(config));
            config = config.Clone();
            config.HashSizeInBits = (int) type;
            return new xxHashFunction(config);
        }
    }
}