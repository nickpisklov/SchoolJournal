using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolJournal.ViewModels
{
    public class JournalPaging<T> : List<T>
    {
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }

        public JournalPaging(List<T> items, int count, int pageIndex, int pagesSize) 
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pagesSize);
            this.AddRange(items);
        }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
        public static JournalPaging<T> Create(List<T> source, int pageIndex, int pageSize) 
        {
            var count = source.Count();
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new JournalPaging<T>(items, count, pageIndex, pageSize);
        }
    }
}
