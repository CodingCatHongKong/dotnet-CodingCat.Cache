using System;

namespace CodingCat.Cache.Interfaces
{
    public interface IKeyBuilderConfiguration
    {
        Type UsingType { get; }
        string KeyPrefix { get; }
    }
}