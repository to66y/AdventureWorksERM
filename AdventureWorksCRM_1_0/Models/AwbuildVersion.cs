using System;
using System.Collections.Generic;

#nullable disable

namespace AdventureWorksCRM_1_0.Models.AppDbContext
{
    public partial class AwbuildVersion
    {
        public byte SystemInformationId { get; set; }
        public string DatabaseVersion { get; set; }
        public DateTime VersionDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
