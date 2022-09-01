using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using TMPro;

public class simpleRequests : MonoBehaviour
{
    [SerializeField]
    TMP_Text texto;
    [SerializeField]
    List<Persona> personas ;
    void Start()
    {
        // A correct website page.
        StartCoroutine(GetRequest("https://script.google.com/macros/s/AKfycbwjcnXqpD5nznRx0CTwcxIvxDDARvu2yNCatNfNO0kPow_FHzFGc7huD45RIpT_6mmu/exec"));

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
                    texto.text = webRequest.downloadHandler.text;
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    JSONNode root = JSONNode.Parse(webRequest.downloadHandler.text);

                    foreach (var key in root.Keys)
                    {
                        Debug.Log(key);
                        Persona a = ScriptableObject.CreateInstance<Persona>();
                        
                        a.nombre = key;
                        a.edad = root[key][0]["edad"];
                        a.color= root[key][0]["color"];
                        a.email = root[key] [0] ["email"];
                        a.comidas = root [key] [0] ["comidas"];
                        AssetDatabase.CreateAsset(a, "Assets/Personas/"+a.nombre+".asset");
                    }
                    break;
            }
        }
    }
}
