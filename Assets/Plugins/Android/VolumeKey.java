
package com.MarkCup.Dice;

import android.content.Intent;
import android.media.AudioManager;
import android.view.KeyEvent;
import com.unity3d.player.UnityPlayer;
import com.unity3d.player.UnityPlayerActivity;
import android.os.Bundle;
import android.app.Activity;
import android.content.Context;
import android.hardware.Sensor;
import android.hardware.SensorEvent;
import android.hardware.SensorEventListener;
import android.hardware.SensorManager;
import android.speech.RecognizerIntent;
import android.speech.SpeechRecognizer;
import android.widget.Toast;

import java.util.ArrayList;

public class VolumeKey extends UnityPlayerActivity {

    private AudioManager audioManager;
    private SensorManager sensorManager;
    private Sensor proximitySensor;
    private SensorEventListener proximitySensorListener;

    private boolean covered;
    private float thresholdDistance; // 阈值

    private SpeechRecognizer speechRecognizer;
    private boolean isRecording = false; // 标记是否正在录音

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        // 获取传感器管理器
        sensorManager = (SensorManager) getSystemService(Context.SENSOR_SERVICE);
        if (sensorManager != null) {
            // 获取距离传感器
            proximitySensor = sensorManager.getDefaultSensor(Sensor.TYPE_PROXIMITY);
            if (proximitySensor != null) {
                // 计算阈值（最大范围的一半）
                thresholdDistance = proximitySensor.getMaximumRange() / 2;
                if (thresholdDistance > 1) {
                    thresholdDistance = 1;
                }
                // 创建监听器
                proximitySensorListener = new SensorEventListener() {
                    @Override
                    public void onSensorChanged(SensorEvent event) {
                        float distance = event.values[0];
                        UnityPlayer.UnitySendMessage("VolumeController", "OnVolumeKeyPressed",
                                String.valueOf(distance));
                        if (distance > thresholdDistance) {
                            covered = false;
                        } else {
                            covered = true;
                        }
                    }

                    @Override
                    public void onAccuracyChanged(Sensor sensor, int accuracy) {
                        // 这里可以留空，但必须实现
                    }
                };

                // 注册监听器
                sensorManager.registerListener(proximitySensorListener, proximitySensor,
                        SensorManager.SENSOR_DELAY_FASTEST);
            } else {
                UnityPlayer.UnitySendMessage("VolumeController", "OnVolumeKeyPressed", "None Sensor");
            }
        }

        // 初始化语音识别器
        speechRecognizer = SpeechRecognizer.createSpeechRecognizer(this);
        speechRecognizer.setRecognitionListener(new android.speech.RecognitionListener() {
            @Override
            public void onReadyForSpeech(Bundle params) {
            }

            @Override
            public void onBeginningOfSpeech() {
            }

            @Override
            public void onRmsChanged(float rmsdB) {
            }

            @Override
            public void onBufferReceived(byte[] buffer) {
            }

            @Override
            public void onEndOfSpeech() {
            }

            @Override
            public void onError(int error) {
                UnityPlayer.UnitySendMessage("VolumeController", "OnVoiceRecognitionFailed", "Error: " + error);
            }

            @Override
            public void onResults(Bundle results) {
                ArrayList<String> recognizedText = results.getStringArrayList(SpeechRecognizer.RESULTS_RECOGNITION);
                if (recognizedText != null && !recognizedText.isEmpty()) {
                    String result = recognizedText.get(0);
                    UnityPlayer.UnitySendMessage("VolumeController", "OnVoiceRecognitionResult", result);
                }
            }

            @Override
            public void onPartialResults(Bundle partialResults) {
            }

            @Override
            public void onEvent(int eventType, Bundle params) {
            }
        });
    }

    @Override
    public boolean onKeyDown(int keyCode, KeyEvent event) {
        if (cyberCover) {
            switch (keyCode) {
                case KeyEvent.KEYCODE_VOLUME_UP: // 按下音量上键开始录音
                    if (!isRecording) {
                        startVoiceRecognition();
                        isRecording = true;
                        UnityPlayer.UnitySendMessage("VolumeController", "OnVolumeKeyPressed", "START");
                    }
                    return true;
                case KeyEvent.KEYCODE_VOLUME_DOWN: // 降低音量时
                    UnityPlayer.UnitySendMessage("VolumeController", "OnVolumeKeyPressed", "DOWN");
                    return true;
                default:
                    return super.onKeyDown(keyCode, event);
            }
        } else if (covered) {
            switch (keyCode) {
                case KeyEvent.KEYCODE_VOLUME_UP: // 按下音量上键开始录音
                    if (!isRecording) {
                        startVoiceRecognition();
                        isRecording = true;
                        UnityPlayer.UnitySendMessage("VolumeController", "OnVolumeKeyPressed", "START");
                    }
                    return true;
                case KeyEvent.KEYCODE_VOLUME_DOWN: // 降低音量时
                    UnityPlayer.UnitySendMessage("VolumeController", "OnVolumeKeyPressed", "DOWN");
                    return true;
                default:
                    return super.onKeyDown(keyCode, event);
            }
        } else {
            return super.onKeyDown(keyCode, event);
        }
    }

    static boolean cyberCover = false;

    public static void CyberCover(String message) {
        // System.out.println("Unity says: " + message);
        //UnityPlayer.UnitySendMessage("VolumeController", "OnVolumeKeyPressed", message);
        if (message.equals("cover")) {
            cyberCover = true;
        } else if (message.equals("uncover")) {
            cyberCover = false;
        }
    }

    @Override
    public boolean onKeyUp(int keyCode, KeyEvent event) {
        if (covered) {
            switch (keyCode) {
                case KeyEvent.KEYCODE_VOLUME_UP: // 松开音量上键结束录音
                    if (isRecording) {
                        stopVoiceRecognition();
                        isRecording = false;
                    }
                    return true;
                default:
                    return super.onKeyUp(keyCode, event);
            }
        } else {
            return super.onKeyUp(keyCode, event);
        }
    }

    private void startVoiceRecognition() {
        Intent intent = new Intent(RecognizerIntent.ACTION_RECOGNIZE_SPEECH);
        intent.putExtra(RecognizerIntent.EXTRA_LANGUAGE_MODEL, RecognizerIntent.LANGUAGE_MODEL_FREE_FORM);
        intent.putExtra(RecognizerIntent.EXTRA_PROMPT, "请开始说话...");
        startActivityForResult(intent, 1001);
    }

    private void stopVoiceRecognition() {
        speechRecognizer.stopListening(); // 停止录音
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        if (requestCode == 1001 && resultCode == RESULT_OK && data != null) {
            ArrayList<String> results = data.getStringArrayListExtra(RecognizerIntent.EXTRA_RESULTS);
            if (results != null && !results.isEmpty()) {
                String recognitionResult = results.get(0);
                UnityPlayer.UnitySendMessage("VolumeController", "OnVoiceRecognitionResult", recognitionResult);
            }
        }
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        // 取消注册监听器，防止内存泄漏
        if (sensorManager != null && proximitySensorListener != null) {
            sensorManager.unregisterListener(proximitySensorListener);
        }

        // 释放语音识别器
        if (speechRecognizer != null) {
            speechRecognizer.destroy();
        }
    }
}

/*
 * package com.MarkCup.Dice;
 * 
 * import android.content.Intent;
 * import android.media.AudioManager;
 * import android.view.KeyEvent;
 * import com.unity3d.player.UnityPlayer;
 * import com.unity3d.player.UnityPlayerActivity;
 * import android.os.Bundle;
 * import android.app.Activity;
 * 
 * import android.content.Context;
 * import android.hardware.Sensor;
 * import android.hardware.SensorEvent;
 * import android.hardware.SensorEventListener;
 * import android.hardware.SensorManager;
 * 
 * public class VolumeKey extends UnityPlayerActivity {
 * 
 * private AudioManager audioManager;
 * 
 * private SensorManager sensorManager;
 * private Sensor proximitySensor;
 * private SensorEventListener proximitySensorListener;
 * 
 * private boolean covered;
 * private float thresholdDistance; // 阈值
 * 
 * 
 * @Override
 * protected void onCreate(Bundle savedInstanceState) {
 * super.onCreate(savedInstanceState);
 * 
 * // 获取音量管理器
 * // audioManager = (AudioManager) getSystemService(AUDIO_SERVICE);
 * 
 * // textView = new TextView(this);
 * // setContentView(textView);
 * 
 * // 获取传感器管理器
 * sensorManager = (SensorManager) getSystemService(Context.SENSOR_SERVICE);
 * if (sensorManager != null) {
 * // 获取距离传感器
 * proximitySensor = sensorManager.getDefaultSensor(Sensor.TYPE_PROXIMITY);
 * if (proximitySensor != null) {
 * // 计算阈值（最大范围的一半）
 * thresholdDistance = proximitySensor.getMaximumRange() / 2;
 * if(thresholdDistance>1){
 * thresholdDistance=1;
 * }
 * // 创建监听器
 * proximitySensorListener = new SensorEventListener() {
 * 
 * @Override
 * public void onSensorChanged(SensorEvent event) {
 * float distance = event.values[0];
 * //UnityPlayer.UnitySendMessage("VolumeController",
 * "OnVolumeKeyPressed",String.valueOf(distance));
 * if (distance > 2.5) {
 * covered = false;
 * // UnityPlayer.UnitySendMessage("VolumeController", "OnVolumeKeyPressed",
 * // "FAR");
 * } else {
 * covered = true;
 * // UnityPlayer.UnitySendMessage("VolumeController", "OnVolumeKeyPressed",
 * // "CLOSE");
 * }
 * }
 * 
 * @Override
 * public void onAccuracyChanged(Sensor sensor, int accuracy) {
 * // 这里可以留空，但必须实现
 * }
 * };
 * 
 * // 注册监听器
 * // sensorManager.registerListener(proximitySensorListener, proximitySensor,
 * // SensorManager.SENSOR_DELAY_NORMAL);
 * // 注册监听器，增加传感器更新频率到最快（SENSOR_DELAY_FASTEST）
 * // 需要注意，频率越高，电池消耗也越大
 * sensorManager.registerListener(proximitySensorListener, proximitySensor,
 * SensorManager.SENSOR_DELAY_FASTEST); // 或 SENSOR_DELAY_UI，SENSOR_DELAY_GAME
 * 
 * } else {
 * UnityPlayer.UnitySendMessage("VolumeController", "OnVolumeKeyPressed",
 * "None Sensor");
 * }
 * }
 * }
 * 
 * @Override
 * public boolean onKeyDown(int keyCode, KeyEvent event) {
 * if (covered) {
 * switch (keyCode) {
 * case KeyEvent.KEYCODE_VOLUME_UP: // 增加音量
 * // if (audioManager != null) {
 * // audioManager.adjustStreamVolume(AudioManager.STREAM_MUSIC,
 * // AudioManager.ADJUST_RAISE,
 * // AudioManager.FLAG_SHOW_UI);
 * // }
 * UnityPlayer.UnitySendMessage("VolumeController", "OnVolumeKeyPressed", "UP");
 * 
 * return true;
 * case KeyEvent.KEYCODE_VOLUME_DOWN: // 降低音量
 * // if (audioManager != null) {
 * // audioManager.adjustStreamVolume(AudioManager.STREAM_MUSIC,
 * // AudioManager.ADJUST_LOWER,
 * // AudioManager.FLAG_SHOW_UI);
 * // }
 * UnityPlayer.UnitySendMessage("VolumeController", "OnVolumeKeyPressed",
 * "DOWN");
 * return true;
 * default:
 * return super.onKeyDown(keyCode, event);
 * }
 * } else {
 * return super.onKeyDown(keyCode, event);
 * }
 * }
 * 
 * @Override
 * protected void onDestroy() {
 * super.onDestroy();
 * // 取消注册监听器，防止内存泄漏
 * if (sensorManager != null && proximitySensorListener != null) {
 * sensorManager.unregisterListener(proximitySensorListener);
 * }
 * }
 * }
 */