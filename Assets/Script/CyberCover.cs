using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CyberCover : MonoBehaviour
{
    public Text text;

    private bool cover = false;
    public string en, dis;
    private string filePath;

    void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "coverState.txt");
        LoadState();
    }

    public void Cover()
    {
        cover = !cover;
        SaveState();

        if (cover)
        {
            using (AndroidJavaClass jc = new AndroidJavaClass("com.MarkCup.Dice.VolumeKey"))
            {
                jc.CallStatic("CyberCover", "cover");
            }
            text.text = en;
        }
        else
        {
            using (AndroidJavaClass jc = new AndroidJavaClass("com.MarkCup.Dice.VolumeKey"))
            {
                jc.CallStatic("CyberCover", "uncover");
            }
            text.text = dis;
        }
    }

    void SaveState()
    {
        File.WriteAllText(filePath, cover ? "1" : "0");
    }

    void LoadState()
    {
        if (File.Exists(filePath))
        {
            string savedState = File.ReadAllText(filePath);
            cover = savedState == "1";

            if (cover)
            {
                using (AndroidJavaClass jc = new AndroidJavaClass("com.MarkCup.Dice.VolumeKey"))
                {
                    jc.CallStatic("CyberCover", "cover");
                }
                text.text = en;
            }
            else
            {
                using (AndroidJavaClass jc = new AndroidJavaClass("com.MarkCup.Dice.VolumeKey"))
                {
                    jc.CallStatic("CyberCover", "uncover");
                }
                text.text = dis;
            }
        }
        else
        {
            File.WriteAllText(filePath, "1");
            using (AndroidJavaClass jc = new AndroidJavaClass("com.MarkCup.Dice.VolumeKey"))
            {
                jc.CallStatic("CyberCover", "cover");
            }
            text.text = en;
        }
    }
}
