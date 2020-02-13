namespace CodingCat.Cache.Impls.Interfaces
{
    public interface IKeyBuilder
    {
        string KeyPrefix { get; }

        string ToString();
    }
}