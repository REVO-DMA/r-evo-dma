using CompileTimeObfuscator;
using DnsClient;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Tarkov_DMA_Backend.Misc
{
    public sealed partial class Auth
    {
        [ObfuscatedString("api.evodma.com")]
        private static partial string DomainName();
        [ObfuscatedString("evodma.com")]
        private static partial string ExpectedServerIdentity();
        [ObfuscatedString("session-token")]
        private static partial string SessionTokenHeaderName();
        [ObfuscatedString("productID")]
        private static partial string productID();
#if COMMERCIAL || RELEASE
        [ObfuscatedString("ee728768-bd14-489b-9a7c-69b48324e003")]
        private static partial string productID_UUID();
#else
        [ObfuscatedString("b558f30e-c4cf-4b47-bb30-71e70ba72983")]
        private static partial string productID_UUID();
#endif

        [ObfuscatedString("hwid")]
        private static partial string hwid();
        [ObfuscatedString("https://")]
        private static partial string protocol();
        [ObfuscatedString("MIIFSgIBAzCCBRAGCSqGSIb3DQEHAaCCBQEEggT9MIIE+TCCA+8GCSqGSIb3DQEHBqCCA+AwggPcAgEAMIID1QYJKoZIhvcNAQcBMBwGCiqGSIb3DQEMAQYwDgQIMgL0vbvLdA0CAggAgIIDqJlz2UrtPiDmJDqij774WeDQFQtbc+iQgNMFqR4ESbTuu+b5Z59hKF+JUGek8AzPWsE1POIE4Vk3iNO/RU2hitig6sOkrVTe0KXFtt5PTldXUEESUVGJs9Q1d3UbhOicKMmkPRAeyUoS4qFGI8m/kCmjLpKdFl1jSIwWXZy3cWUYYrh2viNrVHUyJslgLWNj5lFxh87j/YDCq+l1zUzcab+TxtqRD0XwXghcHem1m0KaiUSf2567tkG85DaL6w0xayUcc/kwp3AE9V2rnwUi+nXV4lCLSVj06kbA1ievjv2Jy6L+kDAx1oyRLPo7M9YKzkRR75xllyvJ8+m7JYUDrIO8khZIdXZYxWrSfmtDSW/NzaCaOOUnKBQUPjyLBep15Ii1Ls3Z7Ax48Dx40OAWbc9zTSM+q+TXsQeR3hZXhmaLF1zlxE0o9UcrBskrS/R1ujQRE+RXwwoDXsuJvm8eHeo9vA9IfMiIiGOqbgV6GiybQjZn1+iQYCyQDU/sNgkl/Vh5SC8z3gQTkqyFInLQYX8bfbYay5hlHNfFJCRP472O427+qYhix+RmuLxv6x56k2KArOlEuFaduxBOe0XAkZAXH6VwPzPqquambTfHUOf5pp5tKLZooJ0Q+XEdvLmPns49TL7pJV7UNvPHG8DKHrhYdQdrFtajcv8Q6RQHN/1h23OJuUgvpGv1vBY9loG3PkBXoT+m418ncZ2AvSmFc0K09mwbm9SENx9nyoHP0lv1oF+bShWfJ/qXUPPVwtf5UUrMbhUndO+q+fbc03nMLLrSvbS6tux6UQija2EDyn/6c0j6HSNEH/gsR8Yx6F5rdtHeZx3BLiJM3k8MQtqciOEACYrsq3nxunpr7DpYBwtQRFqVhW1zopP34YmZdv04ZEgEGTbM0tK7Yd534OABux5N1OCV+a70qSCwzTrc6oz2ctvBJPZC6/SJGG/M4ypaCTqGfGoWM7CBeR5butOOR9GSUVmBdMIJ6QtHY+zKKztJ8qaBDlsdXtVizeK/3CtZ1lDtFVV9F1TtFsscYZ8IvsbkhfTirF5/K//WhOIVPWrZUodP7cb2Hn/9oyvXBQn1uq4NexYSzcN7dH9k/qYD3TAn/Q+0jtSobG3xal//RAEI9V+R/fvqCVPegjoqV1Rmrk/TQAZsz0RkBSGVlcwACRwE3v+1jebZ+i3ZuoEcyNjhwTa88p694DnkfQe6QpsN9tvfb2t0WgjjY/fAc7YFJf5WDLgJKm4JQTCCAQIGCSqGSIb3DQEHAaCB9ASB8TCB7jCB6wYLKoZIhvcNAQwKAQKggbQwgbEwHAYKKoZIhvcNAQwBAzAOBAioiZUbXv/HuAICCAAEgZA4splPP1I/XOByZzRzJrhqlUX/+QCrl+oe/JuL0WXeE45IGR5fow9ZpgaI26TJg6gzxE3DeOmdGMTk9+YO9zwoFWyYk5EHZZnzUgiXaOpOR1Ek6EO0h0fA5v7xA9uhs4iLjf317kBHsDOjy6BCYjLQu/LuQCnbf4tSahA0zPcIOiZZ+b0dZmnyzJMWBaGevksxJTAjBgkqhkiG9w0BCRUxFgQUeWuqoQIc0ZqKQRNMl2gu5q4NmdIwMTAhMAkGBSsOAwIaBQAEFJnBYBx7FRLcDBXhy7e+l6Hk/cUrBAjH43+5EdS6zAICCAA=")]
        private static partial string Base64ClientCert();

        private readonly IReadOnlyList<string> _ipAddresses;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Auth()
        {
            try
            {
                var lookup = new LookupClient(new LookupClientOptions
                {
                    RequestDnsSecRecords = true,
                    UseCache = false,
                    ThrowDnsErrors = true
                });
                var result = lookup.Query(DomainName(), QueryType.A);

                var records = result.Answers.ARecords();
                List<string> ipAddresses = new();
                foreach (var record in records)
                {
                    var address = record.Address;

                    if (address == null)
                        continue;

                    ipAddresses.Add(address.ToString());
                }

                _ipAddresses = ipAddresses;
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[DNS] failed to resolve backend: {ex}");
                MessageBox.ShowError($"Failed to resolve backend: {ex.Message}", "EVO DMA");
                SentrySdk.CaptureException(ex);
                Environment.Exit(1);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private HttpClientHandler GetHttpHandler()
        {
            byte[] clientCertBytes = Convert.FromBase64String(Base64ClientCert());
            var clientCert = new X509Certificate2(clientCertBytes);

            return new()
            {
                ClientCertificates = { clientCert },
                ServerCertificateCustomValidationCallback = (HttpRequestMessage request, X509Certificate2 certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) =>
                {
                    // Check if there are any SSL policy errors
                    if (sslPolicyErrors != SslPolicyErrors.None)
                    {
                        // SSL policy errors occurred, handle them (you can log, throw an exception, etc.)
                        Logger.WriteLine($"SSL Policy Errors: {sslPolicyErrors}");
                        return false;
                    }

                    // Check if the certificate is valid (not expired, etc.)
                    if (certificate == null || DateTime.Now > certificate.NotAfter || DateTime.Now < certificate.NotBefore)
                    {
                        Logger.WriteLine("Invalid certificate or certificate expired.");
                        return false;
                    }

                    // Verify the certificate chain
                    if (chain == null || chain.ChainStatus.Length != 0)
                    {
                        Logger.WriteLine("Certificate chain validation failed.");
                        return false;
                    }

                    // Compare the server's identity with the expected identity
                    string serverHostname = certificate.GetNameInfo(X509NameType.DnsName, false);
                    if (!string.Equals(ExpectedServerIdentity(), serverHostname, StringComparison.OrdinalIgnoreCase))
                    {
                        Logger.WriteLine($"Unexpected server identity. Expected: {ExpectedServerIdentity()}, Actual: {serverHostname}");
                        return false;
                    }

                    // Certificate validation successful
                    return true;
                }
            };
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Login(JSON.AuthLogin credentials, out string sessionToken)
        {
            VirtualizerSDK.VIRTUALIZER_LION_BLACK_START();

            sessionToken = null;

            string serializedAuth = JSON.SerializeAuthLogin(credentials);
            if (serializedAuth == null) return;

            foreach (var ipAddress in _ipAddresses)
            {
                Logger.WriteLine($"[AUTH] Attempting Login with IP: {ipAddress}");

                using HttpClient httpClient = new(GetHttpHandler());
                httpClient.DefaultRequestHeaders.Add("Accept", "*/*");

                // Custom DNS stuff
                var url = $"{protocol()}{ipAddress}/";
                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Host = DomainName();

                // Create the HTTP request message with the JSON data
                HttpRequestMessage request = new(HttpMethod.Post, "radar-login");
                request.Content = new StringContent(serializedAuth, Encoding.UTF8);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                // Send the HTTP request and get the response
                HttpResponseMessage response = httpClient.Send(request);

                // Check if the request was successful
                if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.OK)
                {
                    StreamReader streamReader = new(response.Content.ReadAsStream());
                    string responseContent = streamReader.ReadToEnd();

                    Logger.WriteLine($"[AUTH] Login Response: {responseContent}");

                    if (response.Headers.TryGetValues(SessionTokenHeaderName(), out var sessionTokenValues))
                    {
                        sessionToken = sessionTokenValues.FirstOrDefault();

                        Logger.WriteLine($"[AUTH] Login successful! Session Token: \"{sessionToken}\"");

                        break;
                    }
                    else
                    {
                        Logger.WriteLine($"[AUTH] Login Failed!");
                    }
                }
                else
                {
                    Logger.WriteLine($"[AUTH] Request failed with unexpected status code: {response.StatusCode}");
                }
            }

            VirtualizerSDK.VIRTUALIZER_LION_BLACK_END();
        }

        public struct CanLaunchResponse(bool canLaunch, string expiration, float runtime, string message)
        {
            public bool CanLaunch = canLaunch;
            public string Expiration = expiration;
            public float Runtime = runtime;
            public string Message = message;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void CanLaunch(string sessionToken, out CanLaunchResponse canLaunch)
        {
            VirtualizerSDK.VIRTUALIZER_LION_BLACK_START();

            canLaunch = new(false, null, 0, null);

            foreach (var ipAddress in _ipAddresses)
            {
                Logger.WriteLine($"[AUTH] Attempting CanLaunch with IP: {ipAddress}");

                using HttpClient httpClient = new(GetHttpHandler());
                httpClient.DefaultRequestHeaders.Add("Accept", "*/*");
                httpClient.DefaultRequestHeaders.Add(SessionTokenHeaderName(), sessionToken);

                // Custom DNS stuff
                var url = $"{protocol()}{ipAddress}/";
                httpClient.BaseAddress = new Uri(url);
                httpClient.DefaultRequestHeaders.Host = DomainName();

                // Create the HTTP request message with the JSON data
                HttpRequestMessage request = new(HttpMethod.Post, "can-launch");
                string hwidValue = HWID.GetHWID();
                Logger.WriteLine($"Using HWID: {hwidValue}");
                request.Content = new StringContent($"{{\"{productID()}\":\"{productID_UUID()}\", \"{hwid()}\":\"{hwidValue}\"}}", Encoding.UTF8);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                // Send the HTTP request and get the response
                HttpResponseMessage response = httpClient.Send(request);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    StreamReader streamReader = new(response.Content.ReadAsStream());
                    string responseContent = streamReader.ReadToEnd();

                    Logger.WriteLine($"[AUTH] CanLaunch Response: {responseContent}");

                    var canLaunch_base = JSON.DeserializeCanLaunch_Base(responseContent);
                    if (canLaunch_base != null && canLaunch_base.Success)
                    {
                        var message = JSON.DeserializeCanLaunch_Success(responseContent)?.Message;

                        canLaunch.CanLaunch = true;
                        canLaunch.Expiration = message.Expiration;
                        canLaunch.Runtime = message.Runtime;
                    }
                    else
                    {
                        canLaunch.Message = JSON.DeserializeCanLaunch_Error(responseContent)?.Message[0];
                    }

                    break;
                }
                else
                {
                    Logger.WriteLine($"[AUTH] Request failed with unexpected status code: {response.StatusCode}");
                }
            }

            VirtualizerSDK.VIRTUALIZER_LION_BLACK_END();
        }
    }
}
