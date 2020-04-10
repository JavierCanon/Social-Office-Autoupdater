using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Updater.UpdatePlugin.Providers;

namespace Updater.UpdatePlugin.WebApiProvider
{
    internal sealed class WebApiSourceProvider : ISourceProvider
    {
        #region Retry constants

        private const int RetryCount = 3;
        private const int RetrySleepTime = 5000;

        #endregion

        private readonly ISecurityProvider _securityProvider;
        private const string AuthorizationHeader = "authorization";
        private const string CheckVersionMethod = "checkversion";
        private const string GetFileInfoMethod = "getfileinfo";
        private const string GetFileMethod = "getfile";
        private readonly string _host;
        private int _refreshTimeout = 10000;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public WebApiSourceProvider(ISecurityProvider securityProvider)
        {
            _securityProvider = securityProvider;
            _host = ConfigurationManager.AppSettings["updateUrl"];

            if (String.IsNullOrWhiteSpace(_host))
            {
                throw new ArgumentException("Config section 'updateUrl' missed");
            }
        }

        public bool CheckForUpdates()
        {
            var assembly = Assembly.GetEntryAssembly();
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            var attribute = (GuidAttribute)assembly.GetCustomAttributes(typeof(GuidAttribute),true)[0];
            var result = Execute<CheckVersionInVm>(CheckVersionMethod, new CheckVersionOutVm
            {
                AppGuid = Guid.Parse(attribute.Value),
                Version = fileVersionInfo.FileVersion
            });

            _refreshTimeout = result.RefreshTimeout;

            return result.NeedUpdate;
        }

        public event Action<ISourceProvider> HaveUpdates;

        public PackageInfo GetPackage()
        {
            var assembly = Assembly.GetEntryAssembly();
            var attribute = (GuidAttribute)assembly.GetCustomAttributes(typeof(GuidAttribute), true)[0];
            var appGuid = Guid.Parse(attribute.Value);

            return TryGetPackage(appGuid);
        }

        public void CheckInBackground()
        {
            Action repeatFunc = () =>
            {
                bool @continue = true;
                while (@continue)
                {
                    Thread.Sleep(_refreshTimeout);
                    if (CheckForUpdates() && HaveUpdates != null)
                    {
                        @continue = false;
                        HaveUpdates(this);
                    }
                }
            };

            Task.Factory.StartNew(repeatFunc, _cancellationTokenSource.Token);
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        #region Private methods

        private PackageInfo TryGetPackage(Guid appGuid, int count = 0)
        {
            var fileOutVm = new FileOutVm
            {
                AppGuid = appGuid,
            };
            var result = Execute<FileInVm>(GetFileInfoMethod, fileOutVm);
            var resultStream = GetStream(GetFileMethod, fileOutVm);

            if (GetHash(resultStream) == result.Hash)
            {
                resultStream.Position = 0;
                return new PackageInfo(result.AppName, resultStream);
            }

            if (count < RetryCount)
            {
                Thread.Sleep(RetrySleepTime);
                return TryGetPackage(appGuid, ++count);
            }

            return null;
        }

        private string GetHash(Stream stream)
        {
            MD5 md5 = MD5.Create();
            try
            {
                stream.Position = 0;
                return Encoding.UTF8.GetString(md5.ComputeHash(stream));
            }
            finally
            {
                md5.Dispose();
                stream.Position = 0;
            }
        }

        private T Execute<T>(string method, IVm vm)
            where T : IVm, new()
        {
            string url = String.Format("{0}/{1}", _host, method);
            var data = JsonConvert.SerializeObject(vm);
            var request = WebRequest.Create(url);
            request.ContentType = "application/json";
            request.Method = "POST";
            request.ContentLength = data.Length;
            request.Headers.Add(AuthorizationHeader, _securityProvider.GetToken());
            StreamWriter requestWriter = new StreamWriter(request.GetRequestStream());
            requestWriter.Write(data);
            requestWriter.Close();

            using (var response = request.GetResponse() as HttpWebResponse)
            {
                if (response == null || response.StatusCode != HttpStatusCode.OK)
                {
                    return new T();
                }

                using (var stream = response.GetResponseStream())
                {
                    if (stream == null)
                    {
                        return new T();
                    }

                    using (var reader = new StreamReader(stream))
                    {
                        return JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
                    }
                }
            }
        }

        private MemoryStream GetStream(string method, FileOutVm vm)
        {
            string url = String.Format("{0}/{1}?appGuid={2}", _host, method, vm.AppGuid);

            using (WebClient webClient = new WebClient())
            {
                webClient.Headers.Add("Authorization", _securityProvider.GetToken());
                byte[] response = webClient.DownloadData(url);
                return new MemoryStream(response);
            }
        }

        #endregion
    }
}