using CodingCat.Cache.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodingCat.Cache.Redis
{
    public class Storage : IStorage
    {
        private IDatabase database { get; }

        public TimeSpan Expiry { get; }

        #region Constructor(s)
        public Storage(IDatabase database, TimeSpan expiry)
        {
            this.database = database;

            this.Expiry = expiry;
        }
        #endregion

        public IStorage Add(IKeyBuilder key, string item)
        {
            if (!this.Exists(key))
            {
                var transaction = this.database.CreateTransaction();
                transaction.AddCondition(Condition.KeyNotExists(key.ToString()));

                transaction.StringSetAsync(
                    key.ToString(),
                    item,
                    this.Expiry
                );
                transaction.Execute();
            }
            return this;
        }

        public string Get(IKeyBuilder key)
        {
            if (this.Exists(key))
                return this.database.StringGet(key.ToString());

            return null;
        }

        public string Get(IKeyBuilder key, Func<string> callback)
        {
            return this.Exists(key) ?
                this.Get(key) :
                this.Add(key, callback()).Get(key);
        }

        public IStorage Delete(IKeyBuilder key)
        {
            this.database.KeyExpire(key.ToString(), TimeSpan.FromSeconds(-1));

            return this;
        }

        private bool Exists(IKeyBuilder key)
        {
            return this.database.KeyExists(key.ToString());
        }
    }
}
