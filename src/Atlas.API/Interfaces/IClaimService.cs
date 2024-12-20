namespace Atlas.API.Interfaces
{
    public interface IClaimService
    {
        string? GetClaim();
        bool HasDeveloperClaim();
    }
}
