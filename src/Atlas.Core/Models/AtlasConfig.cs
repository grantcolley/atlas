namespace Atlas.Core.Models
{
    public class AtlasConfig
    {
        public bool DatabaseMigrate { get; set; }
        public bool DatabaseCreate { get; set; }
        public bool DatabaseSeedData { get; set; }
        public bool DatabaseSeedLogs { get; set; }
    }
}
