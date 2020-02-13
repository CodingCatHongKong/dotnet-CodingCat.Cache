namespace CodingCat.Cache.Interfaces
{
    public interface IKeyBuilder
    {
        string KeyPrefix { get; }

        string ToString();
    }
}