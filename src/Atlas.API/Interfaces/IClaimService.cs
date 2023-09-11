using Atlas.Core.Models;
using System.Security.Claims;

namespace Atlas.API.Interfaces
{
    public interface IClaimService
    {
        string? GetClaim();
    }
}
