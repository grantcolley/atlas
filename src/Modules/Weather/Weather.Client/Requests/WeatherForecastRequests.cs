using Atlas.Core.Authentication;
using Atlas.Requests.Base;
using Atlas.Requests.Interfaces;
using Atlas.Requests.Model;
using Weather.Core.Model;

namespace Weather.Client.Requests
{
    public class WeatherForecastRequests : RequestBase, IWeatherForecastRequests
    {
        public WeatherForecastRequests(HttpClient httpClient)
            : base(httpClient)
        {
        }

        public WeatherForecastRequests(HttpClient httpClient, TokenProvider tokenProvider)
            : base(httpClient, tokenProvider)
        {
        }

        public async Task<IResponse<IEnumerable<WeatherForecast>>> GetWeatherForecastsAsync()
        {
            using var httpResponseMessage = await _httpClient.GetAsync("weatherforecast").ConfigureAwait(false);

            var responseList = await GetResponseAsync<IEnumerable<WeatherForecast>>(httpResponseMessage)
                .ConfigureAwait(false);

            Response<IEnumerable<WeatherForecast>> response = new()
            {
                IsSuccess = responseList.IsSuccess,
                Message = responseList.Message
            };

            if (responseList.IsSuccess)
            {
                response.Result = responseList.Result;
            }

            return response;
        }
    }
}
