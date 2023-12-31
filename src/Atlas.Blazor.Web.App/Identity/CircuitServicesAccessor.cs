using System;
using System.Threading;

namespace Atlas.Blazor.Web.App.Identity
{
    public class CircuitServicesAccessor
    {
        static readonly AsyncLocal<IServiceProvider?> blazorServices = new();

        public IServiceProvider? Services
        {
            get => blazorServices.Value;
            set => blazorServices.Value = value;
        }
    }
}
