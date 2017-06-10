using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;

namespace H2Shield.Includes
{
    public class WebHandler
    {
        private string Api = "https://www.cartographer.online/H2Cartographer/api/new_api.php";

        public CheckBanResult CheckBan(string username, string rememberToken)
        {
            var pairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("launcher", "1"),
                new KeyValuePair<string, string>("serial", Security.GetSerial())
            };
            pairs.Add(new KeyValuePair<string, string>("token", rememberToken));

            var content = new FormUrlEncodedContent(pairs);
            using (var client = new HttpClient())
            {
                var response = client.PostAsync(Api, content).Result;
                var contentString = response.Content.ReadAsStringAsync().Result;
                if (contentString == "banned")
                {
                    foreach (Process P in Process.GetProcessesByName("halo2")) P.Kill();
                    return CheckBanResult.Banned;
                }
            }

            return CheckBanResult.NotBanned;
        }

        public LoginResult Login(string username, string password, string rememberToken = "")
        {
            var pairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("launcher", "1"),
                new KeyValuePair<string, string>("serial", Security.GetSerial())
            };

            if (rememberToken != "") pairs.Add(new KeyValuePair<string, string>("token", rememberToken));
            else
            {
                pairs.Add(new KeyValuePair<string, string>("user", username));
                pairs.Add(new KeyValuePair<string, string>("pass", password));

            }

            var content = new FormUrlEncodedContent(pairs);
            using (var client = new HttpClient())
            {
                var response = client.PostAsync(Api, content).Result;
                var contentString = response.Content.ReadAsStringAsync().Result;

                LoginResult Result = new LoginResult();

                Result.LoginResultEnum = LoginResultEnum.GenericFailure;
                Result.LoginToken = "-1";

                if (rememberToken != "" && rememberToken.Length == 32)
                {
                    if (contentString == "0") Result.LoginResultEnum = LoginResultEnum.InvalidLoginToken;
                    else if (contentString == "banned") Result.LoginResultEnum = LoginResultEnum.Banned;
                    else if (contentString == "1")
                    {
                        Result.LoginResultEnum = LoginResultEnum.Successfull;
                        Result.LoginToken = rememberToken;
                    }
                }
                else
                {
                    if (contentString == "0") Result.LoginResultEnum = LoginResultEnum.InvalidUsernameOrPassword;
                    else if (contentString == "banned") Result.LoginResultEnum = LoginResultEnum.Banned;
                    else if (contentString.Length == 32)
                    {
                        Result.LoginResultEnum = LoginResultEnum.Successfull;
                        Result.LoginToken = contentString;
                    }
                }
                return Result;
            }
        }
        /*
        public bool Register(string user, string pass, string email)
        {
            var pairs = new List<KeyValuePair<string, string>>
            {
            new KeyValuePair<string, string>("launcher", "1"),
            new KeyValuePair<string, string>("user", user),
            new KeyValuePair<string, string>("pass", pass),
            new KeyValuePair<string, string>("email", email)
            };
            var content = new FormUrlEncodedContent(pairs);
            using (var client = new HttpClient())
            {
                var response = client.PostAsync(Api, content).Result;
                var contentString = response.Content.ReadAsStringAsync().Result;
                return (contentString == "1") ? true : false;
            }
        }
        */
    }
}
