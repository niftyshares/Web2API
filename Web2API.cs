using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json;
using NLog.Internal;

namespace Web2API
{
    public class email : CommonFunctions
    {
        [FunctionName("email")]
        public static async Task<HttpResponseMessage> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            string name = req.Query["email"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            //Emails();
            //SelectData();

            if(name != null)
            al = SmartTechnique(name);

            string responseMessage = string.IsNullOrEmpty(name)
                ? "Query string was missing."
                : "\n\n" + EmailParser(name);

            Stream s = new MemoryStream(Encoding.UTF8.GetBytes(responseMessage ?? ""));


            response.Content = new StreamContent(s);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;

        }

    }


    public class NiftyValue : CommonFunctions
    {
        [FunctionName("NiftyValue")]
        public static async Task<HttpResponseMessage> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            string name = req.Query["city"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "Query string was missing."
                : "\n\n" + PullData(name);

            Stream s = new MemoryStream(Encoding.UTF8.GetBytes(responseMessage ?? ""));


            response.Content = new StreamContent(s);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;

        }

    }

    public class IndianCityDetails : CommonFunctions
    {
        [FunctionName("IndianCityDetails")]
        public static async Task<HttpResponseMessage> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            string name = req.Query["city"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "Query string was missing."
                : "\n\n" + PullData(name);

            Stream s = new MemoryStream(Encoding.UTF8.GetBytes(responseMessage ?? ""));


            response.Content = new StreamContent(s);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;

        }

    }

    public class YouTubeTopFive : CommonFunctions
    {
        [FunctionName("YouTubeTopFive")]
        public static async Task<HttpResponseMessage> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log,ExecutionContext context)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            string q = req.Query["q"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            q = q ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(q)
                ? "Query string was missing."
                : "\n\n" + PullDataYoutube(q,context);

            Stream s = new MemoryStream(Encoding.UTF8.GetBytes(responseMessage ?? ""));


            response.Content = new StreamContent(s);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;

        }


}

    public class CommonFunctions {

        public static ArrayList SmartTechnique(string emailField)
        {
            string payload = emailField.Substring(0, emailField.LastIndexOf("."));
            string DomainExtension = emailField.Substring(emailField.LastIndexOf("."));
            ArrayList validDomains = new ArrayList();
            string builtDomain = string.Empty;


            StringBuilder sb = new StringBuilder("");

            // http requests check
            foreach (char c in payload.Reverse().ToArray())
            {
                
                sb.Insert(0, c);
                if (sb.Length >= 3)
                {
                    builtDomain = sb.ToString() + DomainExtension;
                    WebRequest request = WebRequest.Create("http://www." + builtDomain);

                    request.Method = "GET";
                    try
                    {
                        var response = request.GetResponse();
                        HttpStatusCode statusCode = ((HttpWebResponse)response).StatusCode;

                        if (statusCode.Equals(HttpStatusCode.OK))
                        {
                            if (!string.IsNullOrEmpty(builtDomain) && !validDomains.Contains(builtDomain) && checkMXRecord(builtDomain)) validDomains.Add(builtDomain);
                        }
                    }
                    catch (Exception ee)
                    {
                        if (ee.Message.Equals("No such host is known. No such host is known."))
                        {
                            continue;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(builtDomain) && !validDomains.Contains(builtDomain) && checkMXRecord(builtDomain)) validDomains.Add(builtDomain);
                        }
                    }
                }

            }

            // https requests check
            foreach (char c in payload.Reverse().ToArray())
            {
                
                sb.Insert(0, c);
                if (sb.Length >= 3)
                {
                    builtDomain = sb.ToString() + DomainExtension;
                    WebRequest request = WebRequest.Create("https://" + builtDomain);

                    request.Method = "GET";
                    try
                    {
                        var response = request.GetResponse();
                        HttpStatusCode statusCode = ((HttpWebResponse)response).StatusCode;

                        if (statusCode.Equals(HttpStatusCode.OK))
                        {
                            if (!string.IsNullOrEmpty(builtDomain) && !validDomains.Contains(builtDomain) && checkMXRecord(builtDomain)) validDomains.Add(builtDomain);
                        }
                    }
                    catch (Exception ee)
                    {
                        if (ee.Message.Equals("No such host is known. No such host is known."))
                        {
                            continue;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(builtDomain) && !validDomains.Contains(builtDomain) && checkMXRecord(builtDomain)) validDomains.Add(builtDomain);
                        }
                    }
                }

            }

            return validDomains;
        }


        public static void SelectData()
        {
                
            SqlConnection cn = new SqlConnection(ConfigurationManager.AppSettings["connectionString"]);
            
            SqlCommand cmd = new SqlCommand("SELECT DISTINCT domain FROM[dbo].[emailDomains]", cn);
            
            cn.Open();
            
            SqlDataReader rdr = cmd.ExecuteReader();
            
            while (rdr.Read())
            {
              
                if(!al.Contains(rdr[0])) al.Add(rdr[0]);
            }
            
            rdr.Close();
            // Close the connection by calling close() method  
            cn.Close();
        }

        public static void Emails()
        {
            al.Add("gmail.com");
            al.Add("outlook.com");
            al.Add("yahoo.com");
            al.Add("protonmail.com");
            al.Add("live.com");
            
        }
        public static string EmailParser(string emailField) {
            string output = string.Empty;

            if (!string.IsNullOrEmpty(emailField) && !emailField.Contains("@")) {

                foreach (string emailProvider in al) {

                    foreach (int index in emailField.AllIndexesOf(emailProvider))
                    {
                        if (index != -1 && !string.IsNullOrEmpty(emailField.Substring(0, emailField.IndexOf(emailProvider))))
                        {
                            output += emailField.Substring(0, emailField.IndexOf(emailProvider)) + "@" + emailProvider+"<br/>";
                        }
                    }
                }
            }
            return output != string.Empty ? output : emailField;
        }

        public static ArrayList al = new ArrayList();


        public static bool checkMXRecord(string domain)
        {

            WebRequest request = WebRequest.Create(ConfigurationManager.AppSettings["NSUrl"] + domain);
            WebResponse response = request.GetResponse();
            string MXEntryInPage = "";

            using (Stream dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                MXEntryInPage = reader.ReadToEnd();



            }
            return !MXEntryInPage.Contains("No mail servers found.");
        }

            public static string PullData(string city)
        {

            WebRequest request = WebRequest.Create(ConfigurationManager.AppSettings["LatLonIndianCities"] + city);
            WebResponse response = request.GetResponse();
            string coordinates = "";

            using (Stream dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                coordinates = reader.ReadToEnd();
                int startPostion = coordinates.IndexOf("<title>") + 7;
                int endPostion = coordinates.IndexOf("</title>");
                coordinates = coordinates.Substring(startPostion, endPostion - startPostion);
                // Display the content.


            }


            // Close the response.
            response.Close();
            return coordinates;



        }

        public static string PullDataYoutube(string youtubeQuery, ExecutionContext context)
        {
            string output = string.Empty;
            WebRequest request = WebRequest.Create("https://www.youtube.com/results?search_query=" + youtubeQuery);
            ArrayList al = new ArrayList();
            string staticURLPortion = string.Empty;


            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream dataStream = response.GetResponseStream())
                {
                    // Open the stream using a StreamReader for easy access.
                    StreamReader reader = new StreamReader(dataStream);
                    // Read the content.
                    string websiteRawResponse = reader.ReadToEnd();
                    int tracker = 0;
                    string trackerString = string.Empty;
                    string searchString = "videoId\":\"";
                    string videoId = "";
                    staticURLPortion = File.ReadAllText(Path.Combine(context.FunctionAppDirectory, "HTMLPage.html"));


                    for (int i = 0; i < websiteRawResponse.Length; i++)
                    {
                        tracker = websiteRawResponse.IndexOf(searchString);
                        videoId = websiteRawResponse.Substring(tracker + 10, 11);

                        if (!al.Contains(videoId))
                        {
                            al.Add(videoId);
                            if (al.Count == 5)
                            {
                                break;
                            }
                        }
                        websiteRawResponse = websiteRawResponse.Substring(tracker + 10);

                    }

                }
            }

            return string.Format(staticURLPortion, al[0].ToString(), al[1].ToString(), al[2].ToString(), al[3].ToString(), al[4].ToString());



        }
    }

    public static class extensionKLiye {
        public static List<int> AllIndexesOf(this string str, string value)
        {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentException("the string to find may not be empty", "value");
            List<int> indexes = new List<int>();
            for (int index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index);
                if (index == -1)
                    return indexes;
                indexes.Add(index);
            }
        }
    }

}

