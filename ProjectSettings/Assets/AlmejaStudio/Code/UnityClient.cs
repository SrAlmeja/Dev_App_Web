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
    public TMP_Text itemName;
    public TMP_Text idText;
    string nameItem;
    public bool boolItem;
    public int itemID;
    
    public TMP_Text Titulo;
    public TMP_Text Trabalenguas;

    //public Buttom 
    [Serializable]
    public class ToDoTask
    {
        public string name;
        public bool isComplete;
    }

    void Start()
    {
        // A correct website page.
        StartCoroutine(GetRequest("https://localhost:44363/api/todo"));
        // A non-existing page.
        //StartCoroutine(GetRequest("https://error.html"));

    }

    private void Update()
    {
        nameItem = itemName.text;
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

                    foreach (var key in root.Keys)
                    {
                        Titulo.text = (pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                        Trabalenguas.text = (pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    }

                    break;
            }
        }
    }
    
    IEnumerator PostRequest(string uri)
    {
        ToDoTask myTask = new ToDoTask();
        myTask.name = nameItem;
        myTask.isComplete = boolItem;

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

    IEnumerator PutRequest(string uri, int id, string name, bool boolItem)
    {
        UnityWebRequest put = UnityWebRequest.Put(uri + "/" + id, {\"Id\":"id",\"Name\":\"" name"\", \"BoolTem\":\""boolItem"\"}");
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
            
    // IEnumerator DeleteRequest(string uri)
    // {
    //     
    // }

    public void IsEmptyPage(bool tog)
    {
        boolItem = tog;
    }
    
    public void GetRequestButton()
    {
        StartCoroutine(GetRequest("https://localhost:44363/api/todo"));
    }
    
    public void PostRequestButton()
    {
        StartCoroutine(PostRequest("https://localhost:44363/api/todo"));
    }

    // public void PutRequestButton()
    // {
    //     StartCoroutine(PutRequest("https://localhost:44363/api/todo", itemID, nameItem, boolItem));
    // }

    public void DeletButton()
    {
        
    }
    
}