using System;
using System.Collections.Generic;
using System.Data.Common;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace J.Net.BV
{
    public partial class Documents
    {
        public int Id { get; set; }
        public double? Amount { get; set; }
        public string Description { get; set; }

		public static Documents fromDataReader(DbDataReader reader)
		{
			Documents Document = new Documents()
			{
				Id = (int)reader["id"],
				Amount = reader.IsDBNull(reader.GetOrdinal("Amount")) ? 0 : (double)reader["Amount"],
				Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? String.Empty : (string)reader["Description"]
			};

			return Document;
		}
	}
}
