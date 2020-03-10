using CodingCat.Cache.Interfaces;
using System;

namespace CodingCat.Cache.Impls
{
    public class StorageConfiguration : IStorageConfiguration
    {
        public TimeSpan Expiry { get; set; }
    }
}