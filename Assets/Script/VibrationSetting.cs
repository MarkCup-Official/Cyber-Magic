using System.Collections;
using System.Collections.Generic;
using System.IO;
using TripoForUnity;
using UnityEngine;
using UnityEngine.UI;

public class VibrationSetting : MonoBehaviour
{
    public InputField inputField; 
    private string filePath;


    void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "savedVibration.txt");
        LoadText();
    }

    public void SubmitText()
    {
        SaveText(inputField.text);
        Dice.sleep=long.Parse(inputField.text);
        //Debug.Log(long.Parse(inputField.text));
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
        }else{
            string savedText = "200";
            inputField.text = savedText;
            inputField.onEndEdit.Invoke(savedText);
        }
    }
}
