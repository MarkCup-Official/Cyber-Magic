using UnityEngine;

public class Vibration
{
    public static void VibratePattern(long[] pattern, int repeat)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");

            // 检查安卓版本
            AndroidJavaClass buildVersion = new AndroidJavaClass("android.os.Build$VERSION");
            int sdkInt = buildVersion.GetStatic<int>("SDK_INT");

            if (sdkInt >= 26) // 安卓8.0及以上
            {
                AndroidJavaClass vibrationEffectClass = new AndroidJavaClass("android.os.VibrationEffect");
                AndroidJavaObject effect = vibrationEffectClass.CallStatic<AndroidJavaObject>("createWaveform", pattern, repeat);
                vibrator.Call("vibrate", effect);
            }
            else // 安卓8.0以下
            {
                vibrator.Call("vibrate", pattern, repeat);
            }
        }
        else
        {
            Debug.Log("Vibration is not supported on this platform.");
        }
    }
}
