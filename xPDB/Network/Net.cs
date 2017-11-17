using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using xPDB.Utility;

namespace xPDB.Network
{
    public class Net
    {
        internal static T DownloadObject<T>(string url)
        {
            var task = DownloadObjectAsync<T>(url);
            task.Wait();
            return task.Result;
        }
        internal static byte[] GetBytes(string url)
        {
            var task = GetBytesAsync(url);
            task.Wait();
            return task.Result;
        }
        internal static async Task<T> DownloadObjectAsync<T>(string url)
        {
            try
            {
                string response = await GetStringAsync(url);
                string responseString = response;
                return JsonConvert.DeserializeObject<T>(responseString);
            }
            catch
            {
                return default(T);
            }
        }
        public static async Task<string> GetStringAsync(string url)
        {
            var request = WebRequest.CreateHttp(url);
            request.Method = "GET";

            var task = new TaskCompletionSource<WebResponse>();

            request.BeginGetResponse(ac =>
            {
                try
                {
                    task.SetResult(request.EndGetResponse(ac));
                }
                catch (Exception e)
                {
                    task.SetException(e);
                }
            }, null);

            using (var response = await task.Task)
            using (var stream = response.GetResponseStream())
            using (var output = new MemoryStream())
            {

                await stream.CopyToAsync(output);
                var array = output.ToArray();
                return Encoding.UTF8.GetString(array, 0, array.Length);
            }
        }
        public static async Task<byte[]> GetBytesAsync(string url)
        {
            var request = WebRequest.CreateHttp(url);
            request.Method = "GET";
            request.Timeout = 1000;

            var task = new TaskCompletionSource<WebResponse>();

            request.BeginGetResponse(ac =>
            {
                try
                {
                    task.SetResult(request.EndGetResponse(ac));
                }
                catch (Exception e)
                {
                    task.SetException(e);
                }
            }, null);

            using (var response = await task.Task)
            using (var stream = response.GetResponseStream())
            using (var output = new MemoryStream())
            {

                await stream.CopyToAsync(output);
                return output.ToArray();
            }
        }
        public static string GetStringAsyncPost(string url, string data, string ssid = null)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json; charset=UTF-8";
            httpWebRequest.Method = "POST";

            if(ssid != null)
            {
                CookieContainer _cookies = new CookieContainer();
                Cookie c = new Cookie();
                c.Domain = "p3.go-free.info";
                c.Expires = DateTime.Now.AddDays(1);
                c.Value = ssid;
                c.Name = "p_ssid";
                _cookies.Add(c);

                httpWebRequest.CookieContainer = _cookies;
            }

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                return streamReader.ReadToEnd();
            }
        }
    }
}
