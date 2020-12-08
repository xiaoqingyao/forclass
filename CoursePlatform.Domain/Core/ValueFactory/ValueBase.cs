using System.Collections.Generic;

namespace CoursePlatform.Domain.Core.ValueFactory
{
    public class ValueBase<TKey,Tval> where Tval : ValItem<TKey>
    {


        public IDictionary<TKey, Tval> Items { get; set; }

        public virtual bool Add(Tval item)
        {
            if (this.Items is null)
            {
                this.Items = new Dictionary<TKey, Tval>();
            }
            return this.Items.TryAdd(item.Key, item);
        }

        public virtual bool Remove(TKey key)
        {
            if (this.Items is null)
            {
                return false;
            }

            return this.Items.Remove(key);
        }

        public int Count
        {
            get
            {
                return this.Items == null ? 0 : this.Items.Count;
            }
        }

        public void Clear()
        {
            this.Items?.Clear();
        }

        public bool Has(TKey key)
        {
            if (Items is null or {Count: <= 0 })
            {
                return false;
            }

            return this.Items.ContainsKey(key);
        }
    }

    public record ValItem<T>(T Key)
    {

    }
}