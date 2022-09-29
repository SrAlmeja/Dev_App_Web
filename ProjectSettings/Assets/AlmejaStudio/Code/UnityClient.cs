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
    public TMP_Text Text;

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
                        Text.text = (pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    }
                    
                    break; 
            }

IEnumerator GetPost()
{
    ToDoTask myTask = new ToDoTask();
        myTask.name = "MyWork";
        myTask.isComplete = true;
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            string json = JsonUtility.ToJson(myTask);

        }
    }
    
}