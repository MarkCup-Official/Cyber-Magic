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
        // ȷ�����ص���Ϊ"VolumeController"��GameObject
        if (gameObject.name != "VolumeController")
        {
            Debug.LogError("�˽ű�������ص���Ϊ'VolumeController'��GameObject�ϣ�");
        }
        // ��ȡǰ������ͷ
        //WebCamDevice frontCam = WebCamTexture.devices.FirstOrDefault(d => d.isFrontFacing);
       // if (frontCam.name != null)
        //{
        //    webCamTexture = new WebCamTexture(frontCam.name);
        //    webCamTexture.Play(); // ��������ͷ
        //}
    }
    public TextMesh textMesh;

    // ��Android���õķ���
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