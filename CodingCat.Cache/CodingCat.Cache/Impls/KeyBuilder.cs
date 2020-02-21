using CodingCat.Cache.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodingCat.Cache.Impls
{
    public class KeyBuilder : IKeyBuilder
    {
        public const string SEPARATOR = "-";

        private List<string> segments { get; } = new List<string>();

        public string KeyPrefix { get; }
        public string UsingKey { get; private set; } = null;
        public string[] Segments => this.segments.ToArray();

        #region Constructor(s)

        public KeyBuilder(KeyBuilder oldBuilder)
        {
            this.KeyPrefix = oldBuilder.KeyPrefix;
        }

        public KeyBuilder(Type usingType, string keyPrefix)
        {
            this.KeyPrefix = string.Join(
                SEPARATOR,
                usingType?.FullName ??
                    throw new NullReferenceException(nameof(usingType)),
                keyPrefix ??
                    throw new NullReferenceException(nameof(keyPrefix))
            );
        }

        #endregion Constructor(s)

        public IKeyBuilder UseKey(string key) =>
            new KeyBuilder(this) { UsingKey = key };

        public IKeyBuilder AddSegments(params object[] segments)
        {
            this.segments
                .AddRange(segments
                    .Select(segment => segment.ToString())
                );
            return this;
        }

        public IKeyBuilder AddSegment(object segment)
        {
            this.segments.Add(segment.ToString());
            return this;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(this.UsingKey))
                throw new InvalidOperationException("must setup the using key");

            var fromSegments = string.Join(SEPARATOR, this.segments);
            var key = string.Join(
                SEPARATOR,
                this.KeyPrefix,
                this.UsingKey
            );

            return string.IsNullOrEmpty(fromSegments) ?
                key :
                string.Join(SEPARATOR, key, fromSegments);
        }
    }

    public class KeyBuilder<T> : KeyBuilder
    {
        #region Constructor(s)

        public KeyBuilder(string keyPrefix) : base(typeof(T), keyPrefix)
        {
        }

        #endregion Constructor(s)
    }
}