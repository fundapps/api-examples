using FundAppsScripts.DTOs;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Net;

namespace FundAppsScripts.Scripts
{
    public partial class AdapptrScripts
    {
        public string UploadPositions()
        {
            var baseUrl = _adapptrConfig.BaseUrl;
            var username = _adapptrConfig.Username;
            var password = _adapptrConfig.Password;
            // your FundApps environment name
            var clientEnvironmentSubDomain = "";
            // csv file only
            var pathToFile = "";
            // the snapshot date of your positions in the format yyyy-MM-dd
            var snapshotDate = DateTime.Today.ToString("yyyy-MM-dd");

            //Example using RestSharp (https://github.com/restsharp/RestSharp)

            //Create a client which will connect to the HTTPS endpoint with the API credentials you have been provided
            var client = new RestClient(baseUrl)
            {
                Authenticator = new HttpBasicAuthenticator(username, password)
            };

            // make the HTTP POST request
            var request = new RestRequest($"/rest/api/v1/task/positions", Method.POST);

            // add body params to the request
            request.AddFile("positions", pathToFile, "text/csv");
            request.AddParameter("snapshotDate", snapshotDate, ParameterType.RequestBody);

            // add header with the rapptr environment
            request.AddHeader("X-Client-Environment", clientEnvironmentSubDomain);

            var response = client.Execute<TaskProfileResponse>(request);

            // if response comes back with a 200 status, then as task for the positions file was created successfully
            if ((response.StatusCode != HttpStatusCode.OK) && (response.StatusCode != HttpStatusCode.Accepted))
            {
                throw new Exception("Failed to send file. Received a HTTP " + (int)response.StatusCode + " " + response.StatusCode + " instead of HTTP 200 OK");
            }

            var taskId = response.Data.Id;

            return taskId;
            // success
        }
    }
}
