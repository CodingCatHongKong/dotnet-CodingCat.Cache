using CodingCat.Cache.Interfaces;

namespace CodingCat.Cache.Impls
{
    public class KeyBuilderConfiguration : IKeyBuilderConfiguration
    {
        public string KeyPrefix { get; set; }
    }
}