using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using TMPro;
using UnityEngine.UI;

public class RequestApis : MonoBehaviour
{
    public TMP_Text setup;
    public TMP_Text  delivery;
    
    //public Buttom 
    
    void Start()
    {
        // A correct website page.
        StartCoroutine(GetRequest("https://v2.jokeapi.dev/joke/Any?lang=es"));
        // A non-existing page.
        //StartCoroutine(GetRequest("https://error.html"));
    }
   
    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    JSONNode root = JSONNode.Parse(webRequest.downloadHandler.text);

                    Debug.Log(root["setup"]);
                    
                    foreach (var obj in root.Keys)
                    {
                        if (root["setup"] != null)
                        {
                            setup.text = root["setup"];
                            delivery.text = root["delivery"];
                        }
                        else
                        if(root["joke"] != null)
                        {
                            setup.text = root["joke"];
                            delivery.text = root[""];
                        }
                    }
                    break; 
            }

        }
    }
    public void Buttonjoke()
    {
        StartCoroutine(GetRequest("https://v2.jokeapi.dev/joke/Programming?lang=es"));
    }
}