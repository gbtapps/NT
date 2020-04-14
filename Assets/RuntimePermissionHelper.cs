using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_ANDROID
public class RuntimePermissionHelper
{
	private RuntimePermissionHelper() { }

	// 実行中のActivityインスタンスを取得する
	private static AndroidJavaObject GetActivity()
	{
		using (var UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			return UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
		}
	}

	// Android M以上かどうか
	private static bool IsAndroidMOrGreater()
	{
		using (var VERSION = new AndroidJavaClass("android.os.Build$VERSION"))
		{
			return VERSION.GetStatic<int>("SDK_INT") >= 23;
		}
	}

    public static void UsePermission(string permission)
    {
        if (RuntimePermissionHelper.HasPermission(permission))
            RuntimePermissionHelper.RequestPermission(new string[] { permission });
    }

    // パーミッションを持っているかどうかを調べる
    public static bool HasPermission(string permission)
	{
		if (IsAndroidMOrGreater())
		{
			using (var activity = GetActivity())
			{
				return activity.Call<int>("checkSelfPermission", permission) == 0;
			}
		}

		return true;
	}

	// パーミッションが必要であることを説明するUIを出す必要があるか
	public static bool ShouldShowRequestPermissionRationale(string permission)
	{
		if (IsAndroidMOrGreater())
		{
			using (var activity = GetActivity())
			{
				return activity.Call<bool>("shouldShowRequestPermissionRationale", permission);
			}
		}

		return false;
	}

	// パーミッション許可ダイアログを表示する
	public static void RequestPermission(string[] permissiions)
	{
		if (IsAndroidMOrGreater())
		{
			using (var activity = GetActivity())
			{
				activity.Call("requestPermissions", permissiions, 0);
			}
		}
	}

	//　指定したintentを開く
	public static void CallIntent( string Action = "android.settings.LOCATION_SOURCE_SETTINGS")
	{
		// Find the UnityPlayer and get the static current activity
		AndroidJavaClass cUnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject oCurrentActivity = cUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

		// Get defenitions of Intent and it's constructor.
		AndroidJavaObject oIntent = new AndroidJavaObject("android.content.Intent");
		oIntent.Call<AndroidJavaObject>("setAction", Action);

		// Start the activity!
		oCurrentActivity.Call("startActivity", oIntent);

		//Dispose them. Not sure if I need to do it or not...
		oIntent.Dispose();
		oCurrentActivity.Dispose();
	}
}
#endif