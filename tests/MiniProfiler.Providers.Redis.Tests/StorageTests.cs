﻿using StackExchange.Profiling;
using StackExchange.Profiling.Storage;
using Xunit;

namespace Tests.Redis
{
    public class StorageTests : IClassFixture<RedisStorageFixture<StorageTests>>
    {
        private readonly RedisStorage _storage;

        public StorageTests(RedisStorageFixture<StorageTests> fixture)
        {
            _storage = fixture.Storage;
        }

        [Fact]
        public void Serialization()
        {
            var mp = GetMiniProfiler();

            var serialized = mp.ToRedisValue();
            Assert.NotNull(serialized);

            var deserialized = serialized.ToMiniProfiler();
            Assert.Equal(mp, deserialized);
        }

        [Fact]
        public void SaveAndGet()
        {
            var mp = GetMiniProfiler();
            _storage.Save(mp);

            var fetched = _storage.Load(mp.Id);
            Assert.Equal(mp, fetched);
        }

        private MiniProfiler GetMiniProfiler()
        {
            var mp = new MiniProfiler("Test");
            using (mp.Step("Foo"))
            {
                using (mp.CustomTiming("Hey", "There"))
                {
                    // heyyyyyyyyy
                }
            }
            return mp;
        }
    }
}
