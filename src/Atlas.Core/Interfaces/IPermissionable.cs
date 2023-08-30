using System.Collections.Generic;

namespace Atlas.Core.Interfaces
{
    public interface IPermissionable
    {
        bool IsPermitted(IEnumerable<string?> permissions);
    }
}
