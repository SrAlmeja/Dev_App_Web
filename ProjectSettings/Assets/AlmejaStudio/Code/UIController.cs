using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    public TMP_Text tituloText;
    public TMP_Text trabalenguasText;
    
    [SerializeField] TMP_InputField id;
    [SerializeField] TMP_InputField tituloT;
    [SerializeField] TMP_InputField trabalenguas;
    long idItem;
    private UnityClient _unityClient;

    private void Start()
    {
        _unityClient = GetComponent<UnityClient>();
    }

    void StringToLong()
    {
       idItem = long.Parse(id.text);
    }

    public void GetRequestButton()
    {
        StartCoroutine(_unityClient.GetRequest("https://localhost:44363/api/todo"));
    }
    
    public void PostRequestButton()
    {
        StartCoroutine(_unityClient.PostRequest("https://localhost:44363/api/todo", idItem, tituloT.text, trabalenguas.text));
    }

    public void PutRequestButton()
    {
        StartCoroutine(_unityClient.PutRequest("https://localhost:44363/api/todo", idItem, tituloT.text, trabalenguas.text));
    }

    public void DeletButton()
    {
        StartCoroutine(_unityClient.DeleteRequest("https://localhost:44363/api/todo", idItem));
    }
    
}
