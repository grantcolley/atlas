namespace Atlas.Core.Models
{
    public class DatabaseStatus
    {
        public bool CanConnect { get; set; }
        public bool CanCreate { get; set; }
        public bool CanSeedData { get; set; }
    }
}
