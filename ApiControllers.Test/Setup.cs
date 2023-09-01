using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiControllers.Test
{
    public static class Setup
    {
        public static HttpClient GetClient(ApiConfigurationService service)
        {
            var token = service.BusinessServiceAccessToken;

            var client = new HttpClient();
            client.SetBearerToken(token);
            return client;
        }
    }
}
