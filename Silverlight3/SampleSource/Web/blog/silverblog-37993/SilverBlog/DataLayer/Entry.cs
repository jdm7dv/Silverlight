using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SilverBlog.DataLayer
{
    public class Entry
    {
        public static void CreateEntry(Model.Entry entry)
        {
            using (Model.ModelContainer context = Factory.GetContainer())
            {
                context.AddToEntries(entry);
                context.SaveChanges();
            }
        }

        public static IEnumerable<Model.Entry> GetLast10Entries()
        {
            using (Model.ModelContainer context = Factory.GetContainer())
            {
                return context.Entries.OrderBy(e => e.Created).Take(10).ToArray();
            }
        }
    }
}
