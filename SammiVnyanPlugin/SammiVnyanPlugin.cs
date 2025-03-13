using System;
using System.Text;
using System.Net.Http;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Networking;

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
                client.DefaultRequestHeaders.ExpectContinue = false;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error parsing expression: {e.Message}");
            }
        }

        void httpRequest(string Content)
        {
            try
            {
                //var jsonData = new StringContent(Content, Encoding.ASCII);
                //jsonData.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var request = new UnityWebRequest("http://localhost:9450/api", "POST");
                byte[] bodyRaw = Encoding.UTF8.GetBytes(Content);
                request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                request.SendWebRequest();
                //client.PostAsync(URL, jsonData);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error parsing expression: {e.Message}");
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
                    httpRequest("{\"request\": \"triggerButton\", \"buttonID\":\"" + buttonId + "\"}");

                }
                catch (Exception e)
                {
                    Debug.LogError($"Error parsing expression: {e.Message}");
                }

                return;
            }
            if (triggerName == "SammiSetButtonVariable")
            {
                var buttonId = text1;
                var variableName = text2;
                var stringValue = text3;
                var numberValue = value1;

                try
                {
                    if (String.IsNullOrEmpty(stringValue)) {
                        httpRequest("{\"request\": \"setVariable\", \"buttonID\":\"" + buttonId + "\", \"name\": \""+variableName+"\", \"value\":"+numberValue+"}");
                        return;
                    }
                    httpRequest("{\"request\": \"setVariable\", \"buttonID\":\"" + buttonId + "\", \"name\": \"" + variableName + "\", \"value\":\"" + stringValue + "\"}");
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error parsing expression: {e.Message}");
                }

                return;
            }


        }

        public void Start() { 
        
        }

    }
}
