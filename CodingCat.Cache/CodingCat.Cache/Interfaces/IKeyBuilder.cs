namespace CodingCat.Cache.Interfaces
{
    public interface IKeyBuilder
    {
        string KeyPrefix { get; }
        string UsingKey { get; }

        IKeyBuilder UseKey(string key);

        IKeyBuilder AddSegments(params object[] segments);

        IKeyBuilder AddSegment(object segment);

        string ToString();
    }

    public interface IKeyBuilder<T> : IKeyBuilder { }
}