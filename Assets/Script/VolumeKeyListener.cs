using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class VolumeKeyListener : MonoBehaviour
{
    public UnityEvent OnVolumeUpPressed;
    public UnityEvent OnVolumeDownPressed;

//private WebCamTexture webCamTexture;

    void Start()
    {
        // 确保挂载到名为"VolumeController"的GameObject
        if (gameObject.name != "VolumeController")
        {
            Debug.LogError("此脚本必须挂载到名为'VolumeController'的GameObject上！");
        }
        // 获取前置摄像头
        //WebCamDevice frontCam = WebCamTexture.devices.FirstOrDefault(d => d.isFrontFacing);
       // if (frontCam.name != null)
        //{
        //    webCamTexture = new WebCamTexture(frontCam.name);
        //    webCamTexture.Play(); // 启动摄像头
        //}
    }
    public TextMesh textMesh;

    // 由Android调用的方法
    public void OnVolumeKeyPressed(string keyCodeStr)
    {
        textMesh.text = keyCodeStr;
        if (keyCodeStr == "UP")
        {
            OnVolumeUpPressed.Invoke();
        }
        else if (keyCodeStr == "DOWN")
        {
            OnVolumeDownPressed.Invoke();
        }
        else if (keyCodeStr == "CLOSE")
        {
            //OnVolumeDownPressed.Invoke();
        }
        else if (keyCodeStr == "FAR")
        {
            //OnVolumeDownPressed.Invoke();
        }
        else if (keyCodeStr == "START")
        {
            Vibration.VibratePattern(new long[] { 100,100 }, -1);
        }
    }

    public void OnVoiceRecognitionFailed(string keyCodeStr)
    {
        textMesh.text = keyCodeStr;
    }

    public Magic magic;
    public void OnVoiceRecognitionResult(string keyCodeStr)
    {
        magic.Generate(keyCodeStr);
        textMesh.text = keyCodeStr;
        Vibration.VibratePattern(new long[] { 100,100 }, -1);

    }
}