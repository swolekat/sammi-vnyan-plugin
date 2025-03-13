using System;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace SammiVnyanPlugin
{
    public class SammiVnyanPlugin : MonoBehaviour, VNyanInterface.ITriggerHandler, VNyanInterface.IButtonClickedHandler
    {
        [FormerlySerializedAs("windowPrefab")] public GameObject aboutWindow;

        private GameObject _aboutWindow;

        public void Awake()
        {
            try
            {
                VNyanInterface.VNyanInterface.VNyanUI.registerPluginButton("SAMMI", this);
                VNyanInterface.VNyanInterface.VNyanTrigger.registerTriggerListener(this);

                _aboutWindow = (GameObject)VNyanInterface.VNyanInterface.VNyanUI.instantiateUIPrefab(aboutWindow);

                if (_aboutWindow != null)
                {
                    _aboutWindow.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                    _aboutWindow.SetActive(false);

                    _aboutWindow.transform.Find("Panel/Version").GetComponent<Text>().text =
                        Assembly.GetExecutingAssembly().GetName().Version.ToString();
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error parsing expression: {e.Message}");
            }
        }

        public void pluginButtonClicked()
        {
            Application.OpenURL("https://github.com/swolekat/sammi-vnyan-plugin");
        }

        void httpRequest(string Content)
        {
            try
            {
                var request = new UnityWebRequest("http://localhost:9450/api", "POST");
                byte[] bodyRaw = Encoding.UTF8.GetBytes(Content);
                request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                request.SendWebRequest();
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
