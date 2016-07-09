using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Specialized;
using System.Web.Script.Serialization;

namespace IntDevs.Upgrade
{
    public class RESTContext<T>
    {
        #region Fields

        private readonly string basicUrl;
        private HttpWebRequest httpRequest;
        private HttpWebResponse httpResponse;
        private Stream dataStream;
        private StreamReader streamReader;
        private FaultMessage errMsg = null;

        public FaultMessage ErrMsg
        {
            get { return errMsg; }
            set { errMsg = value; }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Message property
        /// </summary>
        public string StrMessage { get; private set; }

        #endregion

        #region Constructor

        public RESTContext(string url)
        {
            basicUrl = url + "/{0}/{1}";
        }

        #endregion

        #region Methods

        /// <summary>
        /// Send the request to WCF Service
        /// </summary>
        /// <param name="template">Object template like: User</param>
        /// <param name="action">Action like: Delete</param>
        /// <param name="method">Request method</param>
        /// <param name="t">Object like: User</param>
        private void SendRequest(string template, string action, HttpMethod method, T t, NameValueCollection NameValCol)
        {
            string jsonData = JsonHelp.JsonSerialize<T>(t);
            if (string.IsNullOrEmpty(jsonData))
                return;

            string responseData = string.Empty;

            byte[] data = UnicodeEncoding.UTF8.GetBytes(jsonData);

            //httpRequest = HttpWebRequest.CreateHttp(string.Format(basicUrl, template, action));
            httpRequest =(HttpWebRequest)HttpWebRequest.Create(string.Format(basicUrl, template, action));
            httpRequest.Method = method.ToString();
            httpRequest.ContentType = "application/json";
            httpRequest.ContentLength = data.Length;


            if (NameValCol != null)
            {
                httpRequest.Headers.Add(NameValCol);
            }

            try
            {
                using (dataStream = httpRequest.GetRequestStream())
                {
                    dataStream.Write(data, 0, data.Length);
                }

                using (httpResponse = httpRequest.GetResponse() as HttpWebResponse)
                {
                    dataStream = httpResponse.GetResponseStream();

                    using (streamReader = new StreamReader(dataStream))
                    {
                        responseData = streamReader.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {

                var errResp = ex.Response as HttpWebResponse;

                System.Diagnostics.Trace.WriteLine(string.Format("StatusCode:{0}({1})", errResp.StatusCode, (int)errResp.StatusCode));

                using (var stream = errResp.GetResponseStream())
                {
                    using (var sr = new StreamReader(stream))
                    {
                        string errData = sr.ReadToEnd();

                        System.Diagnostics.Trace.WriteLine(errData);


                        this.ErrMsg = JsonHelp.JsonDeserialize<FaultMessage>(errData);

                        System.Diagnostics.Trace.WriteLine(this.ErrMsg.Message);

                    }
                }

                StrMessage = ex.Message;
            }
        }

        /// <summary>
        /// Get the response from WCF Service
        /// </summary>
        /// <param name="template">Object template like: User</param>
        /// <param name="action">Action like: Delete</param>
        /// <param name="method">Request method</param>
        /// <returns>Return the result from WCF Service</returns>
        private string GetResponse(string template, string action, HttpMethod method, WebHeaderCollection headers)
        {
            string responseData = string.Empty;

            //httpRequest = HttpWebRequest.CreateHttp(string.Format(basicUrl, template, action));
            httpRequest = (HttpWebRequest)HttpWebRequest.Create(string.Format(basicUrl, template, action));
            httpRequest.Method = method.ToString();

            //httpRequest.KeepAlive = false;
            //httpRequest.ProtocolVersion = HttpVersion.Version11;

            httpRequest.Headers = headers;

            try
            {
                using (httpResponse = httpRequest.GetResponse() as HttpWebResponse)
                {
                    if ((int)httpResponse.StatusCode == 404)
                    {

                    }
                    dataStream = httpResponse.GetResponseStream();

                    using (streamReader = new StreamReader(dataStream))
                    {
                        responseData = streamReader.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                StrMessage = ex.Message;

                var errResp = ex.Response as HttpWebResponse;

                System.Diagnostics.Trace.WriteLine(string.Format("StatusCode:{0}({1})", errResp.StatusCode, (int)errResp.StatusCode));

                using (var stream = errResp.GetResponseStream())
                {
                    using (var sr = new StreamReader(stream))
                    {
                        string errData = sr.ReadToEnd();

                        System.Diagnostics.Trace.WriteLine(errData);


                        this.ErrMsg = JsonHelp.JsonDeserialize<FaultMessage>(errData);

                        System.Diagnostics.Trace.WriteLine(this.ErrMsg.Message);

                    }
                }

            }
            catch (ProtocolViolationException pve)
            {
                StrMessage = pve.Message;
            }

            return responseData;
        }

        /// <summary>
        /// Get all
        /// </summary>
        /// <returns>Return a object list like: List<User></returns>
        public List<T> GetAll(string template, string action, WebHeaderCollection headers)
        {
            //string data = GetResponse("User", "All", HttpMethod.GET);
            string data = GetResponse(template, action, HttpMethod.GET, headers);
            if (string.IsNullOrEmpty(data))
                return null;

            return JsonHelp.JsonDeserialize<List<T>>(data);
        }





        /// <summary>
        /// Create a object like: User
        /// </summary>
        /// <param name="t">Object like User</param>
        public void Create(string template, string action, T t, NameValueCollection headers)
        {
            //SendRequest("User", "Create", HttpMethod.POST, t);
            SendRequest(template, action, HttpMethod.POST, t, headers);
        }


        /// <summary>
        /// Update a object like: User
        /// </summary>
        /// <param name="t">Object like User</param>
        public void Update(string template, string action, T t, NameValueCollection headers)
        {
            //SendRequest("User", "Edit", HttpMethod.PUT, t);
            SendRequest(template, action, HttpMethod.PUT, t, headers);
        }

        /// <summary>
        /// Delete a object like: User
        /// </summary>
        /// <typeparam name="S">Type of object member like: int</typeparam>
        /// <param name="id">Object member like: User's id</param>
        public void Delete<S>(string template, string action, S id, WebHeaderCollection headers)
        {
            //GetResponse("User", string.Format("Delete/{0}", id), HttpMethod.DELETE);
            GetResponse(template, string.Format("{0}/{1}", action, id), HttpMethod.DELETE, headers);
        }

        #endregion
    }

    #region HttpMethod Class

    /// <summary>
    /// Class to simulate an enum
    /// </summary>
    class HttpMethod
    {
        private string method;

        public HttpMethod(string method)
        {
            this.method = method;
        }

        public static readonly HttpMethod GET = new HttpMethod("GET");
        public static readonly HttpMethod POST = new HttpMethod("POST");
        public static readonly HttpMethod PUT = new HttpMethod("PUT");
        public static readonly HttpMethod DELETE = new HttpMethod("DELETE");

        public override string ToString()
        {
            return method;
        }
    }
    public class FaultMessage
    {

        public string Message { get; set; }


        public int ErrorCode { get; set; }

    }
    internal class JsonHelp
    {
        private static JavaScriptSerializer jsonSerialize;

        #region Methods

        /// <summary>
        /// Serialize object to Json
        /// </summary>
        /// <typeparam name="T">Object like: User</typeparam>
        /// <param name="objList">Object list like: List<User></param>
        /// <returns>Return a json data</returns>
        internal static string JsonSerialize<T>(T objList)
        {
            jsonSerialize = new JavaScriptSerializer();

            return jsonSerialize.Serialize(objList);
        }

        /// <summary>
        /// DeSerialize json to an object
        /// </summary>
        /// <typeparam name="T">Object like: User</typeparam>
        /// <param name="strJson">Json string</param>
        /// <returns>Return an object</returns>
        internal static T JsonDeserialize<T>(string strJson)
        {
            jsonSerialize = new JavaScriptSerializer();

            return jsonSerialize.Deserialize<T>(strJson);
        }

        #endregion
    }

    #endregion
}
