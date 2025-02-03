using System.Collections;
using System.Collections.Generic;
using System.IO;
using TripoForUnity;
using UnityEngine;
using UnityEngine.UI;

public class VibrationSetting : MonoBehaviour
{
    public InputField inputField,inputField2; 
    private string filePath,filePath2;


    void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "savedVibration.txt");
        filePath2 = Path.Combine(Application.persistentDataPath, "savedVibration2.txt");
        LoadText();
    }

    public void SubmitText()
    {
        SaveText();
        if(long.TryParse(inputField.text,out long sl)){
        Dice.sleep=sl;
        }else{
            Dice.sleep=200;
        }
        if(long.TryParse(inputField2.text,out long sl2)){
        Dice.vibrateTime=sl2;
        }else{
            Dice.vibrateTime=100;
        }
    }

    void SaveText()
    {
        File.WriteAllText(filePath, inputField.text);
        File.WriteAllText(filePath2, inputField2.text);
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
        if (File.Exists(filePath2))
        {
            string savedText = File.ReadAllText(filePath2);
            inputField2.text = savedText;
            inputField2.onEndEdit.Invoke(savedText);
        }else{
            string savedText = "100";
            inputField2.text = savedText;
            inputField2.onEndEdit.Invoke(savedText);
        }
    }
}
