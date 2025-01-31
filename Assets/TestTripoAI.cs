using System.Collections;
using System.Collections.Generic;
using TripoForUnity;
using UnityEngine;

public class TestTripoAI : MonoBehaviour
{
    public string prompt;
    private TripoRuntimeCore tripoCore;

    void Start()
    {
        tripoCore = gameObject.AddComponent<TripoRuntimeCore>();
        tripoCore.set_api_key("tsk_");
    }
    public void Generate()
    {
        tripoCore.textPrompt = prompt;
        tripoCore.OnDownloadComplete.AddListener(OnModelGenerated);
        tripoCore.Text_to_Model_func();
        Debug.Log("Start generate");
    }
    void OnModelGenerated(string modelPath)
    {
        Debug.Log($"Path: {modelPath}");
    }
}
