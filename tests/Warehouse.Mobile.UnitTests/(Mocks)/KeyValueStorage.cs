using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using PerpetualEngine.Storage;
using Warehouse.Core.Plugins;

namespace Warehouse.Mobile.UnitTests.Mocks
{
    public class KeyValueStorage : IKeyValueStorage
    {
        private SimpleStorage _simpleStorage;

        public KeyValueStorage(string storageKey)
        {
            _simpleStorage = SimpleStorage.EditGroup(storageKey);
        }

        public IList<string> Keys => DesktopSimpleStorage.StoredKeys;

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
