using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.IO;

namespace LibCommonUtil
{
    sealed public class WebBrowserClient
    {
        private string _userAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-GB; rv:1.9.1.5) Gecko/20091102 Firefox/3.5.5 (.NET CLR 3.5.30729)";
        private string _acceptType = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
        private string _contentType = "application/x-www-form-urlencoded";
        private string _acceptLanguage = "en-us,en;q=0.5";
        private string _acceptCharset = "ISO-8859-1,utf-8;q=0.7,*;q=0.7";

        public HttpStatusCode HeadPage(Uri requestUri)
        {
            HttpWebRequest httpRequest = PrepareHeadRequest(requestUri, false, null, null);
            return ReadResponseStatusCode(SendHeadRequest(httpRequest));
        }

        public HttpStatusCode HeadPage(Uri requestUri, string referer, bool allowRedirect)
        {
            HttpWebRequest httpRequest = PrepareHeadRequest(requestUri, allowRedirect, null, referer);
            return ReadResponseStatusCode(SendHeadRequest(httpRequest));
        }

        public string GetPage(Uri requestUri)
        {
            HttpWebRequest httpRequest = PrepareGetRequest(requestUri, false, null, null);
            return ReadResponseContent(SendGetRequest(httpRequest));
        }

        public string GetPage(Uri requestUri, string referer)
        {
            HttpWebRequest httpRequest = PrepareGetRequest(requestUri, false, null, referer);
            return ReadResponseContent(SendGetRequest(httpRequest));
        }

        public string GetPage(Uri requestUri, string referer, bool allowRedirect)
        {
            HttpWebRequest httpRequest = PrepareGetRequest(requestUri, allowRedirect, null, referer);
            return ReadResponseContent(SendGetRequest(httpRequest));
        }

        public string GetPage(Uri requestUri, string referer, bool allowRedirect, CookieContainer cookieContainer)
        {
            HttpWebRequest httpRequest = PrepareGetRequest(requestUri, allowRedirect, cookieContainer, referer);
            return ReadResponseContent(SendGetRequest(httpRequest));
        }

        public WebResponse GetPageResponse(Uri requestUri, string referer, bool allowRedirect, CookieContainer cookieContainer)
        {
            HttpWebRequest httpRequest = PrepareGetRequest(requestUri, allowRedirect, cookieContainer, referer);
            return SendGetRequest(httpRequest);
        }

        public string PostSynchronous(Uri requestUri, Dictionary<string, string> postVariables)
        {
            HttpWebRequest httpRequest = PreparePostRequest(requestUri, false, null, null);
            string postString = dictionaryToPostString(postVariables);
            byte[] postData = Encoding.ASCII.GetBytes(postString);
            return ReadResponseContent(SendPostRequest(httpRequest, postData));
        }

        public string PostSynchronous(Uri requestUri, Dictionary<string, string> postVariables, string referer)
        {
            HttpWebRequest httpRequest = PreparePostRequest(requestUri, false, null, referer);
            string postString = dictionaryToPostString(postVariables);
            byte[] postData = Encoding.ASCII.GetBytes(postString);
            return ReadResponseContent(SendPostRequest(httpRequest, postData));
        }

        public string PostSynchronous(Uri requestUri, Dictionary<string, string> postVariables, string referer, bool allowRedirect)
        {
            HttpWebRequest httpRequest = PreparePostRequest(requestUri, allowRedirect, null, referer);
            string postString = dictionaryToPostString(postVariables);
            byte[] postData = Encoding.ASCII.GetBytes(postString);
            return ReadResponseContent(SendPostRequest(httpRequest, postData));
        }

        public string PostSynchronous(Uri requestUri, Dictionary<string, string> postVariables, string referer, bool allowRedirect, CookieContainer cookieContainer)
        {
            HttpWebRequest httpRequest = PreparePostRequest(requestUri, allowRedirect, cookieContainer, referer);
            string postString = dictionaryToPostString(postVariables);
            byte[] postData = Encoding.ASCII.GetBytes(postString);
            return ReadResponseContent(SendPostRequest(httpRequest, postData));
        }

        public WebResponse PostSynchronousResponse(Uri requestUri, Dictionary<string, string> postVariables, string referer, bool allowRedirect, CookieContainer cookieContainer)
        {
            HttpWebRequest httpRequest = PreparePostRequest(requestUri, allowRedirect, cookieContainer, referer);
            string postString = dictionaryToPostString(postVariables);
            byte[] postData = Encoding.ASCII.GetBytes(postString);
            return SendPostRequest(httpRequest, postData);
        }

        public string PostSynchronous(Uri requestUri, string postString)
        {
            HttpWebRequest httpRequest = PreparePostRequest(requestUri, false, null, null);
            byte[] postData = Encoding.ASCII.GetBytes(postString);
            return ReadResponseContent(SendPostRequest(httpRequest, postData));
        }

        public string PostSynchronous(Uri requestUri, string postString, string referer)
        {
            HttpWebRequest httpRequest = PreparePostRequest(requestUri, false, null, referer);
            byte[] postData = Encoding.ASCII.GetBytes(postString);
            return ReadResponseContent(SendPostRequest(httpRequest, postData));
        }

        public string PostSynchronous(Uri requestUri, string postString, string referer, bool allowRedirect)
        {
            HttpWebRequest httpRequest = PreparePostRequest(requestUri, allowRedirect, null, referer);
            byte[] postData = Encoding.ASCII.GetBytes(postString);
            return ReadResponseContent(SendPostRequest(httpRequest, postData));
        }

        public string PostSynchronous(Uri requestUri, string postString, string referer, bool allowRedirect, CookieContainer cookieContainer)
        {
            HttpWebRequest httpRequest = PreparePostRequest(requestUri, allowRedirect, cookieContainer, referer);
            byte[] postData = Encoding.ASCII.GetBytes(postString);
            return ReadResponseContent(SendPostRequest(httpRequest, postData));
        }

        public WebResponse PostSynchronousResponse(Uri requestUri, string postString, string referer, bool allowRedirect, CookieContainer cookieContainer)
        {
            HttpWebRequest httpRequest = PreparePostRequest(requestUri, allowRedirect, cookieContainer, referer);
            byte[] postData = Encoding.ASCII.GetBytes(postString);
            return SendPostRequest(httpRequest, postData);
        }

        private HttpWebRequest PrepareHeadRequest(Uri requestUri, bool allowRedirect, CookieContainer requestCookieContainer, string requestReferer)
        {
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(requestUri);

            httpRequest.UserAgent = _userAgent;
            httpRequest.Accept = _acceptType;
            httpRequest.Headers.Add("Accept-Language", _acceptLanguage);
            httpRequest.Headers.Add("Accept-Charset", _acceptCharset);
            httpRequest.AllowAutoRedirect = allowRedirect;
            httpRequest.Method = "HEAD";
            httpRequest.Timeout = 15000;

            if (requestCookieContainer != null)
                httpRequest.CookieContainer = requestCookieContainer;
            else
                httpRequest.CookieContainer = new CookieContainer();

            if (!string.IsNullOrEmpty(requestReferer))
                httpRequest.Referer = requestReferer;

            return httpRequest;
        }

        private HttpWebRequest PrepareGetRequest(Uri requestUri, bool allowRedirect, CookieContainer requestCookieContainer, string requestReferer)
        {
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(requestUri);

            httpRequest.UserAgent = _userAgent;
            httpRequest.Accept = _acceptType;
            httpRequest.Headers.Add("Accept-Language", _acceptLanguage);
            httpRequest.Headers.Add("Accept-Charset", _acceptCharset);
            httpRequest.AllowAutoRedirect = allowRedirect;
            httpRequest.Method = "GET";
            httpRequest.Timeout = 30000;

            if (requestCookieContainer != null)
                httpRequest.CookieContainer = requestCookieContainer;
            else
                httpRequest.CookieContainer = new CookieContainer();

            if (!string.IsNullOrEmpty(requestReferer))
                httpRequest.Referer = requestReferer;

            return httpRequest;
        }

        private HttpWebRequest PreparePostRequest(Uri requestUri, bool allowRedirect, CookieContainer requestCookieContainer, string requestReferer)
        {
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(requestUri);

            httpRequest.UserAgent = _userAgent;
            httpRequest.Accept = _acceptType;
            httpRequest.ContentType = _contentType;
            httpRequest.Headers.Add("Accept-Language", _acceptLanguage);
            httpRequest.Headers.Add("Accept-Charset", _acceptCharset);
            httpRequest.AllowAutoRedirect = allowRedirect;
            httpRequest.Method = "POST";
            httpRequest.Timeout = 30000;

            if (requestCookieContainer != null)
                httpRequest.CookieContainer = requestCookieContainer;
            else
                httpRequest.CookieContainer = new CookieContainer();

            if (!string.IsNullOrEmpty(requestReferer))
                httpRequest.Referer = requestReferer;

            return httpRequest;
        }

        private WebResponse SendHeadRequest(HttpWebRequest httpRequest)
        {
            try
            {
                return httpRequest.GetResponse();
            }
            catch (WebException ex)
            {
                if (ex.Response == null)
                    return null;
                else
                    return ex.Response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private WebResponse SendGetRequest(HttpWebRequest httpRequest)
        {
            try
            {
                return httpRequest.GetResponse();
            }
            catch (WebException ex)
            {
                return null;
                //return ex.Response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private WebResponse SendPostRequest(HttpWebRequest httpRequest, byte[] postData)
        {
            try
            {
                httpRequest.ContentLength = postData.Length;

                using (Stream postStream = httpRequest.GetRequestStream())
                {
                    postStream.Write(postData, 0, postData.Length);
                    postStream.Close();
                }
                return httpRequest.GetResponse();
            }
            catch
            {
                return null;
            }
        }

        private HttpStatusCode ReadResponseStatusCode(WebResponse response)
        {
            if (response == null)
                return HttpStatusCode.RequestTimeout;
            try
            {
                HttpWebResponse webResponse = (HttpWebResponse)response;
                return webResponse.StatusCode;
            }
            finally
            {
                response.Close();
            }
        }

        private string ReadResponseContent(WebResponse response)
        {
            if (response == null)
                return "error";

            string responseContent = "";

            using (Stream responseStream = response.GetResponseStream())
            {
                using (StreamReader responseStreamReader = new StreamReader(responseStream))
                {
                    responseContent = responseStreamReader.ReadToEnd();
                }
            }
            response.Close();
            return responseContent;
        }

        public string dictionaryToPostString(Dictionary<string, string> postVariables)
        {
            string postString = "";
            foreach (KeyValuePair<string, string> pair in postVariables)
            {
                postString += HttpUtility.UrlEncode(pair.Key) + "=" +
                    HttpUtility.UrlEncode(pair.Value) + "&";
            }

            return postString;
        }

        public Dictionary<string, string> postStringToDictionary(string postString)
        {
            char[] delimiters = { '&' };
            string[] postPairs = postString.Split(delimiters);

            Dictionary<string, string> postVariables = new Dictionary<string, string>();
            foreach (string pair in postPairs)
            {
                char[] keyDelimiters = { '=' };
                string[] keyAndValue = pair.Split(keyDelimiters);
                if (keyAndValue.Length > 1)
                {
                    postVariables.Add(HttpUtility.UrlDecode(keyAndValue[0]),
                        HttpUtility.UrlDecode(keyAndValue[1]));
                }
            }

            return postVariables;
        }
    }
}
