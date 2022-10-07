using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using TMPro;
using UnityEngine.UI;

public class UnityClient : MonoBehaviour
{
    private UIController _uiController;
    [Serializable]
    public class ToDoTask
    {
        public long id;
        public string title;
        public string text;
    }

    void Start()
    {
        _uiController = GetComponent<UIController>();
        // A correct website page.
        StartCoroutine(GetRequest("https://localhost:44363/api/todo"));
        // A non-existing page.
        //StartCoroutine(GetRequest("https://error.html"));

    }

    

    public IEnumerator GetRequest(string uri)
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

                    foreach (var key in root.Keys)
                    {
                        if (root["title"] != null)
                        {
                            _uiController.tituloText.text = root["title"];    
                        }
                        else if (root["text"] != null)
                        {
                            _uiController.trabalenguasText.text = root["text"];
                        }
                    }
                    break;
            }
        }
    }
    
    public IEnumerator PostRequest(string uri, long id, string title, string text)
    {
        ToDoTask myTask = new ToDoTask();
        myTask.id = id;
        myTask.title = title;
        myTask.text = text;

        string json = JsonUtility.ToJson(myTask);
                
        Debug.Log(json);
                
        UnityWebRequest postRequest= UnityWebRequest.Put(uri,json);
        postRequest.method = "POST";
        postRequest.SetRequestHeader("Content-Type", "application/json");
        yield return postRequest.Send();

        if (postRequest.isNetworkError)
        {
            Debug.Log(postRequest.error);
        }
        else
        {
            Debug.Log(postRequest.downloadHandler.text);
        }
        postRequest.Dispose();
    }

    public IEnumerator PutRequest(string uri, long id, string titulo, string trabalenguas)
    {
        ToDoTask toDoTask = new ToDoTask();
        toDoTask.title = titulo;
        toDoTask.text = trabalenguas;
        
        string obj_JSON = JsonUtility.ToJson(toDoTask);
        UnityWebRequest put = UnityWebRequest.Put(uri + "/" + id, obj_JSON);
        put.SetRequestHeader("Content-Type", "application/json");
        yield return put.Send();
    
        if (put.isNetworkError)
        {
            Debug.Log(put.error);
        }
        else
        {
            Debug.Log(put.downloadHandler.text);
        }
    
        put.Dispose();
    }
            
    public IEnumerator DeleteRequest(string uri, long id)
    {
        UnityWebRequest www = UnityWebRequest.Delete(uri + "/" + id);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.responseCode);
            Debug.Log(www.error);
        }
        www.Dispose();
    }

    
    
}