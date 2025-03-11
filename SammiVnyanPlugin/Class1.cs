using System;
using System.Text;
using System.Net.Http;
using UnityEngine;
using System.Threading.Tasks;

namespace SammiVnyanPlugin
{
    public class SammiVnyanPlugin : MonoBehaviour, VNyanInterface.ITriggerHandler
    {
        private static HttpClient client = new HttpClient();

        public void Awake()
        {
            try
            {
                VNyanInterface.VNyanInterface.VNyanTrigger.registerTriggerListener(this);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error parsing expression: {e.Message}");
            }
        }

        async Task<string> httpRequest(string Method, string URL, string Content)
        {
            try
            {
                var jsonData = new StringContent(Content, Encoding.ASCII);
                jsonData.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                string Response = "";
                int httpStatus = 0;

                switch (Method)
                {
                    case "POST":
                        var PostResult = await client.PostAsync(URL, jsonData);
                        Response = PostResult.Content.ReadAsStringAsync().Result;
                        httpStatus = ((int)PostResult.StatusCode);
                        Console.WriteLine(PostResult.ToString());
                        PostResult.Dispose();
                        break;
                    case "PUT":
                        var PutResult = await client.PutAsync(URL, jsonData);
                        Response = PutResult.Content.ReadAsStringAsync().Result;
                        httpStatus = ((int)PutResult.StatusCode);
                        PutResult.Dispose();
                        break;
                    case "GET":
                        var GetResult = await client.GetAsync(URL);
                        Response = GetResult.Content.ReadAsStringAsync().Result;
                        httpStatus = ((int)GetResult.StatusCode);
                        GetResult.Dispose();
                        break;
                    case "PATCH":
                        var request = new HttpRequestMessage(new HttpMethod("PATCH"), URL);
                        request.Content = jsonData;
                        var PatchResult = await client.SendAsync(request);
                        Response = PatchResult.Content.ReadAsStringAsync().Result;
                        httpStatus = ((int)PatchResult.StatusCode);
                        PatchResult.Dispose();
                        break;
                }
                return Response;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error parsing expression: {e.Message}");
                return "";
            }
        }


        public void triggerCalled(string triggerName, int value1, int value2, int value3, string text1, string text2,
    string text3)
        {
            if (triggerName == "SammiTriggerButton")
            {
                var buttonId = text1;

                try
                {
                    httpRequest("POST", "http://localhost:9450/api", "{\"request\": \"triggerButton\", \"buttonID\":\"" + buttonId + "\"}");

                }
                catch (Exception e) {
                    Debug.LogError($"Error parsing expression: {e.Message}");
                }

                return;
            }


        }

        public void Start() { 
        
        }

    }
}
