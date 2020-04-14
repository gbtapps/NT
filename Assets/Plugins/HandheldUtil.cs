using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandheldUtil
{
    private static AndroidJavaObject unityPlayer;
    private static AndroidJavaObject currentActivity;
    private static AndroidJavaObject vibrator;

    // 初期処理
    public static void Initialize()
    {
        return;


        unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
    }

    // バイブレーション機能呼び出し
    public static void Vibrate(long msec)
    {
        return;


        if (Application.platform == RuntimePlatform.Android) {
            vibrator.Call("vibrate", msec);
        }
        
    }

    // 終了処理
    public static void Destruct()
    {
        return;


        vibrator.Dispose();
        currentActivity.Dispose();
        unityPlayer.Dispose();
    }
}