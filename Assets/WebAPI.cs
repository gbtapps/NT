using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

class WebAPI : SingletonMonoBehaviour<WebAPI>
{
    //readonly string baseURL = "http://192.168.56.101";
    readonly string baseURL = "http://157.7.233.186";
    //  UTCとの差分
    TimeSpan localOffset = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now);

    public void TrainingResultPost(TrainingResultPostObject obj, Action<ResponseObject, UnityWebRequest> cb = null)
    {
        var json = ToJson(obj);
        StartCoroutine(Post("/TrainingResult", json, (response, www) => {
            ResponseObject resobj = new ResponseObject();
            cb?.Invoke(resobj, www);
        }));
    }

    public void TrainingResultGet()
    {
        StartCoroutine(Get("/TrainingResult", (response, www) => {
            ResponseObject resobj = new ResponseObject();
        }));
    }


    IEnumerator Post(string path, string postData, Action<string, UnityWebRequest> cb = null, string method = null, string url = null, bool token = true)
    {
        if (url == null)
        {
            url = baseURL;
        }
        UnityWebRequest www;
        if (method == UnityWebRequest.kHttpVerbPUT)
        {
            www = UnityWebRequest.Put(url + path, postData);
        }
        else
        {
            www = UnityWebRequest.Post(url + path, postData);
        }
        www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(postData));
        if (token)
        {
            //            www.SetRequestHeader("Authorization", "Bearer " + m_token);
        }
        if (method != null)
        {
            www.method = method;
        }
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        Debug.Log(www.method + " " + www.url);
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            Debug.Log(www.downloadHandler.text);
        }
        else
        {
            //            Debug.Log("Form upload complete!");
            Debug.Log(www.downloadHandler.text);
        }
        cb?.Invoke(www.downloadHandler.text, www);
    }

    IEnumerator Get(string path, Action<string, UnityWebRequest> cb = null, string url = null, bool token = true)
    {
        if (url == null)
        {
            url = baseURL;
        }
        var www = UnityWebRequest.Get(url + path);
        if (token)
        {
            //            www.SetRequestHeader("Authorization", "Bearer " + m_token);
        }
        www.SetRequestHeader("accept", "application/json");

        yield return www.SendWebRequest();

        Debug.Log(www.method + " " + www.url);
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            Debug.Log(www.downloadHandler.text);
        }
        else
        {
            //            Debug.Log("Form upload complete!");
            Debug.Log(www.downloadHandler.text);
        }
        cb?.Invoke(www.downloadHandler.text, www);
    }



    public class ResponseObject
    {
    }

    string ToJson(TrainingResultPostObject obj)
    {
        return JsonUtility.ToJson(obj);
    }

    [Serializable]
    public class TrainingResultPostObject
    {
        public string xb01id;
        public string training_id;
        public DateTime trained;
        public int time;
        public int score;
    }
}
