using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using TMPro;

public class RequestApis : MonoBehaviour
{
    [SerializeField]
    TMP_Text texto;
    [SerializeField]
    List<Jokes> jokes ;
    private bool nextJoke;
    
    void Start()
    {
        // A correct website page.
        StartCoroutine(GetRequest("https://v2.jokeapi.dev/joke/Programming?lang=es"));
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
                        Jokes a = ScriptableObject.CreateInstance<Jokes>();

                        a.jokes = key;
                        a.setup = root[key][0]["setup"];
                        a.delivery = root[key][0]["delivery"];
                        AssetDatabase.CreateAsset(a, "Assets/Personas/" + a.jokes + ".asset");
                    }

                    break;
            }

        }
    }
    public void Buttonjoke()
    {
        if (nextJoke != null)
        {
            StartCoroutine(GetRequest("https://v2.jokeapi.dev/joke/Programming?lang=es"));
        }
    }
}