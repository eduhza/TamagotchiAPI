using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamagotchiAPI.Service {
    public class GetJsonService {

        public static async Task<T> GetJsonAsync<T>(string baseUrl, string resource) {
            var options = new RestClientOptions(baseUrl) {
                MaxTimeout = -1,
            };

            var client = new RestClient(options);
            var request = new RestRequest(resource, Method.Get);

            var body = @"";
            request.AddParameter("text/plain", body, ParameterType.RequestBody);

            var response = client.Execute<T>(request);

            return response.Data;
        }

    }
}
