# CodingCat.Cache

Provides handy API to manage caches across different sources.


#### Using the `IKeyBuilder`

When doing the caches, sometimes the `key` to the cache is quite troubling us when a programmer uses `contact-email` to represent the company's contact email, but the other programmer uses the same key for the CS team email.

We then figured out the better solution maybe enforce all `keys` require a prefix such as `var key = "PageAboutUs-contact-email";` or `var key = "PageContactUs-contact-email";`

Moreover, when a project needs a distributed cache server (e.g. `Redis`), but its budget cannot affort both the prod & staging, we would also like to prepend the environment to the `keys`.

With the `IKeyBuilder`, we could do this almost without the keyboard.

```csharp
var environment = Environment.Current.ToString(); // -- PRODUCTION or STAGING or DEBUG
var usingKey = new KeyBuilder<Product>(environment) // -- or new KeyBuilder(this.GetType(), environment);
  .UseKey("mobile")
  .AddSegments("Apple", "iPhone XR"); // -- CodingCat.Demo.Models.Product-DEBUG-Apple-iPhone XR
```

or 

```csharp
var usingKey = new KeyBuilder<Product>("DEV")
  .UseKey("GetProductById")
  .AddSegment(productId);
```

or

```csharp
var usingKey = new KeyBuilder<Product>(environment)
  .UseKey(nameof(GetAllFeaturedProducts)); // -- only the UseKey is required
```


#### Using the `IStorage`

To allow easier dependency injection, we would like to wrap the caching classes into an interface thus we could replace the dependencies without any worries. Currently we support `MemoryCache` and `StackExchange.Redis`

```csharp
var storage = new Storage(TimeSpan.FromSeconds(60)); // -- Memory.Storage
storage.Add(usingKey, JsonConvert.SerializeObject(product));

var product = JsonConvert.DeserializeObject<Product>(
  storage.Get(usingKey)
);

storage.Delete(usingKey);

/// ---- ////
var usingKey = this.keyBuilder
  .UseKey(nameof(GetProductByProductId))
  .AddSegment(productId);
var product = JsonConvert.DeserializedObject<Product>(
  storage.Get(
    usingKey,
    () => JsonConvert.SerializeObject(
      this.Database.Products.GetById(productId)
    ) // -- callback if not found
  )
);
```


#### What if micro-services?

When we are coding for some micro-services, one case is the cache is already exists on the distributed cache server (e.g. `Redis`), however, we would also like to cache the data within the service itself, in order to avoid high traffic to the cache server when we have a million instances of the services running.

In this case, there is a `IStorageManager` which allows developers to consume more than 1 cache sources and control its behavior.

```csharp
using MemoryStorage = CodingCat.Cache.Memory.Storage;
using RedisStorage = CodingCat.Cache.Redis.Storage;

....

public IStorageManager StorageManager { get; }

public Constructor() {
  var expiry = TimeSpan.FromSeconds(60);
  var memoryStorage = new MemoryStorage(expiry);
  var redisStorage = new RedisStorage(
    this.redisConnection.GetDatabase(),
    expiry
  );
  
  // -- Default: if the cache is found in fallback storages but not the default storage, only return the value but not saving into the default storage
  // -- SaveFromFallback: if the cache is found in the fallback storages but not the default storage, the manager will save the value from fallback to the default storage
  this.StorageManager = new StorageManager(FallbackPolicy.SaveFromFallback)
    .SetDefault(memoryStorage)
    .AddFallback(redisStorage);
}

...
var product = this.StorageManager.Get(usingKey); // -- Serialized JSON
var featuredProducts = this.StorageManager
  .Get(otherUsingKey, FallbackPolicy.Default); // -- Support override the behavior!
  
var comments = this.StorageManager
  .Get(
    commentsUsingKey,
    () => this.SocialNetworkProvider.GetCommentsOf(id) // -- Need to deserialize from JSON before retun
  );
```

#### Bonus (CodingCat.Cache.Redis)

In order to allow connecting the Redis server with timeout & retry, we extended the `string` and `ConfigurationOptions` with said configurations to retry up to a given number if connect failed or timed out.

```csharp
var redisConnection = "127.0.0.1".CreateRedisConnection(
  TimeSpan.FromSeconds(60), // -- timeout
  TimeSpan.FromSeconds(3), // -- retry interval
  3 // -- retry up to
);
```


#### Target Frameworks

- .Net 4.6.1+
- .Net Standard 2.0+