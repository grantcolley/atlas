using Atlas.Blazor.Base;
using Atlas.Blazor.Models;
using Atlas.Requests.Interfaces;
using Microsoft.AspNetCore.Components;
using Weather.Client.Requests;
using Weather.Core.Model;

namespace Weather.Client.Components
{
    public abstract class WeatherDisplayBase : AtlasComponentBase
    {
        protected IEnumerable<WeatherForecast>? Forecasts;

        [Inject]
        public IWeatherForecastRequests? WeatherForecastRequests { get; set; }

        [Parameter]
        public PageArgs? PageArgs { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if(WeatherForecastRequests == null) throw new NullReferenceException(nameof(WeatherForecastRequests));

            IResponse<IEnumerable<WeatherForecast>> result = await WeatherForecastRequests.GetWeatherForecastsAsync();

            Forecasts = GetResponse(result);
        }
    }
}
