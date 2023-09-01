using System;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;

namespace ApiControllers.Test
{
    public class ApiConfigurationService
    {
        private string _businessServiceAccessToken;
        private string _businessServiceRefreshToken;
        private long _currentTokenExpiresIn;
        public string BusinessServiceAccessToken
        {
            get
            {
                //  if(_businessServiceAccessToken.)
                return _businessServiceAccessToken;
            }
            set { _businessServiceAccessToken = value; }
        }
        public string CurrentTokenExpiresIn
        {
            get
            {
                //if (_businessServiceAccessToken.)
                return _businessServiceAccessToken;
            }
            set { _businessServiceAccessToken = value; }
        }

        private  TokenResponse RefreshToken(string refreshToken)
        {
            var disco = Task.Run(() => DiscoveryClient.GetAsync(ServiceEndPoint));
            disco.Wait();
            var tokenClient = new TokenClient(disco.Result.TokenEndpoint, _clientId, _clientSecret);
            var tokenResponse = Task.Run(() => tokenClient.RequestRefreshTokenAsync(refreshToken));
            tokenResponse.Wait();
            if (tokenResponse.Result.IsError)
            {
                Console.WriteLine(tokenResponse.Result.Error); //TODO: LogError

            }

            _businessServiceAccessToken = tokenResponse.Result.AccessToken;
            _businessServiceRefreshToken = tokenResponse.Result.RefreshToken;
            //return response;

            throw new NotImplementedException();
        }

        public string BusinessServiceRefreshToken
        {
            get { return _businessServiceRefreshToken; }
            set { _businessServiceRefreshToken = value; }
        }

        public void RefreshToken()
        {

        }
        public string ServiceEndPoint { get; set; }
        private string _clientName;
        private string _clientId;
        private string _clientSecret;
        private string _userName;
        private string _userPassword;
      
        private static IConfigurationRoot _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        public ApiConfigurationService()
        {
            ServiceEndPoint = _configuration.GetValue<string>("APISettings:BizServerEndPoint");
            _clientName = _configuration.GetValue<string>("APISettings:ClientId");
            _clientId = _configuration.GetValue<string>("APISettings:ClientId");
            _userName = _configuration.GetValue<string>("APISettings:StoreSalesRep");
            _userPassword = _configuration.GetValue<string>("APISettings:SalesRepPassword");
            _clientSecret = _configuration.GetValue<string>("APISettings:ClientSecret");

            var disco = Task.Run(() => DiscoveryClient.GetAsync(ServiceEndPoint));
            disco.Wait();
            // request token
            try
            {
                var tokenClient = new TokenClient(disco.Result.TokenEndpoint, _clientId, _clientSecret);
                var tokenResponse = Task.Run(() => tokenClient.RequestResourceOwnerPasswordAsync(_userName, _userPassword, "apis offline_access profile"));
                tokenResponse.Wait();
                if (tokenResponse.Result.IsError)
                {
                    Console.WriteLine(tokenResponse.Result.Error); //TODO: LogError
                }

                _businessServiceAccessToken = tokenResponse.Result.AccessToken;
                _businessServiceRefreshToken = tokenResponse.Result.RefreshToken;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex); // TODO LogError
             
            }
        }
    }
}
        

    

