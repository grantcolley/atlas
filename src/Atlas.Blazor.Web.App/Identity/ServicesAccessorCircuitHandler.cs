using Microsoft.AspNetCore.Components.Server.Circuits;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Atlas.Blazor.Web.App.Identity
{
    public class ServicesAccessorCircuitHandler : CircuitHandler
    {
        readonly IServiceProvider _iServiceProvider;
        readonly CircuitServicesAccessor _circuitServicesAccessor;

        public ServicesAccessorCircuitHandler(IServiceProvider services,
            CircuitServicesAccessor servicesAccessor)
        {
            _iServiceProvider = services;
            _circuitServicesAccessor = servicesAccessor;
        }

        public override Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken)
        {
            _circuitServicesAccessor.Services = _iServiceProvider;

            return base.OnCircuitOpenedAsync(circuit, cancellationToken);
        }

        public override Task OnConnectionUpAsync(Circuit circuit, CancellationToken cancellationToken)
        {
            _circuitServicesAccessor.Services = _iServiceProvider;

            return base.OnConnectionUpAsync(circuit, cancellationToken);
        }

        public override Func<CircuitInboundActivityContext, Task> CreateInboundActivityHandler(
            Func<CircuitInboundActivityContext, Task> next)
        {
            return async context =>
            {
                _circuitServicesAccessor.Services = _iServiceProvider;

                await next(context);

                _circuitServicesAccessor.Services = null;
            };
        }

        public override Task OnConnectionDownAsync(Circuit circuit, CancellationToken cancellationToken)
        {
            _circuitServicesAccessor.Services = default;

            return base.OnConnectionDownAsync(circuit, cancellationToken);
        }

        public override Task OnCircuitClosedAsync(Circuit circuit, CancellationToken cancellationToken)
        {
            _circuitServicesAccessor.Services = default;

            return base.OnCircuitClosedAsync(circuit, cancellationToken);
        }
    }
}
