using System;
using System.Collections.Generic;

#nullable disable

namespace SchoolJournal
{
    public partial class Subject
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool? IsBeginning { get; set; }
        public bool? IsMiddle { get; set; }
        public bool? IsSenior { get; set; }
    }
}
