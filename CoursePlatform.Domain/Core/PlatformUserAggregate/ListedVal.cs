using CoursePlatform.Domain.Core.PartnerAggregate;
using CoursePlatform.Domain.Core.ValueFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursePlatform.Domain.Core.PlatformUserAggregate
{
    /// <summary>
    /// 已经上架人的课程
    /// </summary>
    public class ListedVal:ValueBase<string, ItemVal>
    {

        public override bool Add(ItemVal item)
        {
            if (base.Items == null)
            {
                Items = new Dictionary<string, ItemVal>
                {
                    { item.Key, item }
                };
            }
            if (Items.ContainsKey(item.Key))
            {
                Items[item.Key].Count += 1;
            }
            else
            {
                Items.Add(item.Key, item);
            }
            return true;

        }

        public override bool Remove(string key)
        {
            if (this.Items is null or {Count: <= 0 })
            {
                return false;
            }
            if (this.Items.TryGetValue(key, out ItemVal val) is false)
            {
                return false;
            }
            val.Count -= 1;
            if (val.Count < 0)
            {
                return false;
            }

            this.Items.Remove(key);

            return true;
        }

    }

    public record ItemVal(string Key):ValItem<string>(Key)
    {

        public int Count { get; set; }
    }
}
