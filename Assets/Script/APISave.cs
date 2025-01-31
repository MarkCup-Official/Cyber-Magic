using System.Collections;
using System.Collections.Generic;
using System.IO;
using TripoForUnity;
using UnityEngine;
using UnityEngine.UI;

public class APISave : MonoBehaviour
{
    public InputField inputField; // ¹ØÁªUnity UIµÄInputField
    private string filePath;

    public TripoRuntimeCore tripoRuntimeCore;

    void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "savedAPI.txt");
        LoadText();
    }

    public void SubmitText()
    {
        SaveText(inputField.text);
        tripoRuntimeCore.set_api_key(inputField.text);
    }

    void SaveText(string text)
    {
        File.WriteAllText(filePath, text);
    }

    void LoadText()
    {
        if (File.Exists(filePath))
        {
            string savedText = File.ReadAllText(filePath);
            inputField.text = savedText;
            inputField.onEndEdit.Invoke(savedText);
        }
    }
}
