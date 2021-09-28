using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace J.Net.BV
{
    public partial class DocumentsStatuses
    {
        public int Id { get; }
        public int DocumentId { get; set; }
        public int StatusId { get; set; }
        public DateTime DateTime { get; set; }

        public enum Status
        {
            CREATED = 1,
            DELETED = 2
        }
    }
}
