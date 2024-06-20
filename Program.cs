using System;
using System.IO;
using System.Net;

namespace ConsoleApp2
{
    using System;
    using System.IO;
    using System.Net;

    class Program
    {
        static void Main()
        {
            try
            {
                string url = "https://www.banxico.org.mx/SieAPIRest/service/v1/series/SF61745/datos/2024-06-10/2024-06-25";
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Accept = "application/xml"; // Change to "application/xml" if the response is XML
                request.Headers["Bmx-Token"] = "23d490911e6dba3ac5c19a5bf941068b772463289a09d1da86a377f3bae5522f";

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            string responseText = reader.ReadToEnd();
                            Console.WriteLine("Response received from API:");
                            Console.WriteLine(responseText);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error: " + response.StatusDescription);
                    }
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine("An error occurred while sending the request:");
                Console.WriteLine(ex.Message);
                if (ex.Response != null)
                {
                    using (StreamReader reader = new StreamReader(ex.Response.GetResponseStream()))
                    {
                        string errorResponse = reader.ReadToEnd();
                        Console.WriteLine("Error response from server:");
                        Console.WriteLine(errorResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred:");
                Console.WriteLine(ex.Message);
            }
        }
    }

}
