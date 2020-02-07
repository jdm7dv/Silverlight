using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SilverBlog.BusinessLayer
{
    public static class Entry
    {

        public static DTO.Entry Testmethod()
        {
            //ES.ModelContainer context = new ES.ModelContainer();
            //ES.Entry entry = context.EntrySet.SingleOrDefault(e => e.EntryId == 1);

            DataLayer.Model.Entry entry = new DataLayer.Model.Entry { EntryId = 44, Title = "This is a service response" };

            return entry.AsDto();
        }

        public static void CreateEntry(DTO.Entry entry)
        {
            DataLayer.Model.Entry modEntry = entry.AsModel();
            modEntry.Created = DateTime.UtcNow;
            DataLayer.Entry.CreateEntry(modEntry);
        }

        public static IEnumerable<SilverBlog.DTO.Entry> GetLast10Entries()
        {
            return DataLayer.Entry.GetLast10Entries().AsDTO();
        }
    }
}
