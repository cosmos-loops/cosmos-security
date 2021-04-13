﻿// ReSharper disable once CheckNamespace
namespace Cosmos.Security.Verification
{
    /// <summary>
    /// SHA Hash Function Factory
    /// </summary>
    public static class ShaFactory
    {
        public static ShaFunction Create(ShaTypes type = ShaTypes.Sha1) => new(type);
    }
}