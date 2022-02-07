using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using PerpetualEngine.Storage;
using Warehouse.Core.Plugins;

namespace Warehouse.Droid.Services
{
    public class SimpleKeyValueStorage : IKeyValueStorage
    {
        private SimpleStorage _simpleStorage;

        public SimpleKeyValueStorage(string storageKey)
        {
            _simpleStorage = SimpleStorage.EditGroup(storageKey);
        }

        public IList<string> Keys => throw new NotSupportedException("Keys collection is not supported for SimpleStorage on Android platform");

        public bool Contains(string key)
        {
            return _simpleStorage.HasKey(key);
        }

        public T Get<T>(string key)
        {
            if (typeof(T) == typeof(JObject))
            {
                var json = _simpleStorage.Get(key);
                if (string.IsNullOrEmpty(json))
                {
                    return (T)(object)new JObject();
                }
                return (T)(object)JObject.Parse(json);
            }
            return _simpleStorage.Get<T>(key);
        }

        public void Remove(string key)
        {
            _simpleStorage.Delete(key);
        }

        public void Set<T>(string key, T @object)
        {
            if (typeof(T) == typeof(JObject))
            {
                _simpleStorage.Put(key, @object.ToString());
            }
            else
            {
                _simpleStorage.Put(key, @object);
            }
        }
    }
}
