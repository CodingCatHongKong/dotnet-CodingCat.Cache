namespace CodingCat.Cache.Interfaces
{
    public interface IStorage
    {
        IStorage Add(IKeyBuilder key, string item);
        string Get(IKeyBuilder key);
        IStorage Delete(IKeyBuilder key);
    }
}
