using CodingCat.Cache.Interfaces;
using System;

namespace CodingCat.Cache.Impls
{
    public class KeyBuilderConfiguration : IKeyBuilderConfiguration
    {
        public Type UsingType { get; set; }
        public string KeyPrefix { get; set; }
    }
}