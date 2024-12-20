using Atlas.Core.Constants;
using Atlas.Core.Models;
using Atlas.Requests.Base;
using Atlas.Requests.Interfaces;

namespace Atlas.Requests.API
{
    public class DeveloperRequests(HttpClient httpClient) : RequestBase(httpClient), IDeveloperRequests
    {
        public async Task<IResponse<DatabaseStatus?>> GetDatabaseStatusAsync()
        {
            using HttpResponseMessage httpResponseMessage
                = await _httpClient.GetAsync(AtlasAPIEndpoints.GET_DATABASE_STATUS).ConfigureAwait(false);

            IResponse<DatabaseStatus?> response = await GetResponseAsync<DatabaseStatus?>(httpResponseMessage)
                .ConfigureAwait(false);

            return response;
        }

        public async Task<IResponse<DatabaseStatus?>> CreateDatabaseAsync()
        {
            using HttpResponseMessage httpResponseMessage
                = await _httpClient.GetAsync(AtlasAPIEndpoints.CREATE_DATABASE).ConfigureAwait(false);

            IResponse<DatabaseStatus?> response = await GetResponseAsync<DatabaseStatus?>(httpResponseMessage)
                .ConfigureAwait(false);

            return response;
        }

        public async Task<IResponse<DatabaseStatus?>> SeedDatabaseAsync()
        {
            using HttpResponseMessage httpResponseMessage
                = await _httpClient.GetAsync(AtlasAPIEndpoints.SEED_DATABASE).ConfigureAwait(false);

            IResponse<DatabaseStatus?> response = await GetResponseAsync<DatabaseStatus?>(httpResponseMessage)
                .ConfigureAwait(false);

            return response;
        }
    }
}
