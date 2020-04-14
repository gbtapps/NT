using UnityEngine;
using System.Collections;
using System;
using System.IO;

[Serializable]

    public class scDataStore{

    public static string taskType;
    public static System.DateTime startTime;

    public static float NBACK_StartTime = 0f;

    public static int answerCount;
    public static int correctCount;

    public static int maxRepeatCount = 25;

    public static float[] reactionDuration;
    public static bool[] reactionEval;

    public static int[] questionList;
    public static int[] reactionList;
    public static int back_number = 2;
    public static int speed=0;
    public static enumNbackMode nback_mode = enumNbackMode.none;
    public enum enumNbackMode
    {
        calc,number,challenge, none
    }
    

    public static int rotationBlockSize = 3;

    public static float[] speed_ReactionTimes;

    public static bool debug_Start = false;

    public static bool isChallengeMode = false;
    public static int totalChallendedPeriod = 0;
    public static int ChallengeStartTime = 0;
    public static int challenge_back_number = 1;

    public static float unityTime;

    public static string userID = "default";

    /*
    public static scDataStore Instance;

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    */

    // Use this for initialization
    public static void Start () {
        startTime = System.DateTime.Now;
        unityTime = Time.realtimeSinceStartup;
        //reactionDuration = new float[512];
        //reactionEval = new bool[512];
        answerCount = 0;
        correctCount = 0;
        //totalSubjectNumber = 0;

        reactionDuration = new float[1000];
        reactionEval = new bool[1000];
        questionList = new int[1000];
        reactionList = new int[1000];

        speed_ReactionTimes = new float[1000];

        debug_Start = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public static bool checkAnswer(float dur, int reactionKey)
    {
        bool ev;
        if (reactionKey >= 0)
        {            
            int tg = answerCount - back_number;            
            int ans = questionList[tg];            
            ev = ans == reactionKey;
            Debug.Log("A:" + ans.ToString() + " R:" + reactionKey.ToString());
        }
        else
        {
            ev = false;
        }        

        reactionDuration[answerCount] = dur;
        reactionEval[answerCount] = ev;
        reactionList[answerCount] = reactionKey;        
        answerCount++;
        Debug.Log(ev.ToString());
        return ev;
    }   

    public static void addToQuestionList(int answer)
    {
        questionList[answerCount] = answer;
    }

    public static string SD_card_pathname()
    {
        string path = "";
        if (Application.platform == RuntimePlatform.Android)
        {
            using (AndroidJavaClass jcEnvironment = new AndroidJavaClass("android.os.Environment"))
            using (AndroidJavaObject joExDir = jcEnvironment.CallStatic<AndroidJavaObject>("getExternalStorageDirectory"))
            {
                path = joExDir.Call<string>("toString") + "/WMT_Log/";
            }
        }
        else
        {
            path = Application.persistentDataPath;
        }
        return path;
    }

    public static FileInfo getFileInfo(string filename)
    {
        //string path = Application.persistentDataPath;
        //string path = "/sdcard/logtest";
        string path = SD_card_pathname();
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        path += PlayerPrefs.GetString("user_id") + "_" + filename;        
        FileInfo fi = new FileInfo(path);
        return fi;
    }
    public static void saveToFile_Mark()
    {
        FileInfo fi = getFileInfo("LOG_Mark.txt");
        StreamWriter sw = fi.AppendText();

        scDataStoreForJSON_Mark dsJ = new scDataStoreForJSON_Mark();
        dsJ.taskType = "Mark";
        dsJ.startTime = System.DateTime.Now.ToString();
        
        string jsonStr = JsonUtility.ToJson(dsJ);
        sw.Write(jsonStr);
        sw.WriteLine("");

        sw.Close();
    }
    public static void saveToFile_NBACK()
    {
        FileInfo fi = getFileInfo("LOG_NBACK.txt");
        StreamWriter sw = fi.AppendText();        
        Debug.Log(fi.ToString());
        Debug.Log(sw.ToString());

        scDataStoreForJSON dsJ = new scDataStoreForJSON();
        dsJ.taskType = taskType;
        dsJ.startTime = startTime.ToString();
        dsJ.endTime = System.DateTime.Now.ToString();
        dsJ.answerCount = answerCount;
        dsJ.back_number = back_number;
        dsJ.maxRepeatCount = maxRepeatCount;
        dsJ.questionList = (int[])questionList.Clone();
        dsJ.reactionDuration = reactionDuration;
        dsJ.reactionEval = reactionEval;
        dsJ.reactionList = reactionList;
               
        string jsonStr = JsonUtility.ToJson(dsJ);
        sw.Write(jsonStr);
        sw.WriteLine("");

        sw.Close();
    }
    public static void saveToFile_Challenge()
    {
        FileInfo fi = getFileInfo("LOG_Challenge.txt");
        StreamWriter sw = fi.AppendText();
        Debug.Log(fi.ToString());
        Debug.Log(sw.ToString());

        scDataStoreForJSON_Challenge dsJ = new scDataStoreForJSON_Challenge();
        dsJ.taskType = taskType;
        dsJ.startTime = startTime.ToString();
        dsJ.endTime = System.DateTime.Now.ToString();
        dsJ.answerCount = answerCount;
        dsJ.back_number = challenge_back_number;
        dsJ.maxRepeatCount = maxRepeatCount;
        dsJ.reactionDuration = reactionDuration;
        dsJ.totalChallengedPeriod = totalChallendedPeriod;

        string jsonStr = JsonUtility.ToJson(dsJ);
        sw.Write(jsonStr);
        sw.WriteLine("");

        sw.Close();
    }
    public static void saveToFile_SPEED()
    {
        FileInfo fi = getFileInfo("LOG_SPEED.txt");
        StreamWriter sw = fi.AppendText();

        Debug.Log(fi.ToString());
        Debug.Log(sw.ToString());

        scDataStoreForJSON_Speed dsJ = new scDataStoreForJSON_Speed();
        dsJ.taskType = taskType;
        dsJ.startTime = startTime.ToString();
        dsJ.endTime = System.DateTime.Now.ToString();
        dsJ.speed_ReactionTimes = speed_ReactionTimes;

        string jsonStr = JsonUtility.ToJson(dsJ);
        sw.Write(jsonStr);
        sw.WriteLine("");

        sw.Close();
    }
    public static void saveToFile_2D3D()
    {
        FileInfo fi = getFileInfo("LOG_2D3D.txt");
        StreamWriter sw = fi.AppendText();

        Debug.Log(fi.ToString());
        Debug.Log(sw.ToString());

        scDataStoreForJSON_2D3D dsJ = new scDataStoreForJSON_2D3D();
        dsJ.taskType = taskType;
        dsJ.startTime = startTime.ToString();
        dsJ.endTime = System.DateTime.Now.ToString();
        dsJ.answerCount = answerCount;
        dsJ.reactionDuration = reactionDuration;
        dsJ.reactionEval = reactionEval;

        string jsonStr = JsonUtility.ToJson(dsJ);
        sw.Write(jsonStr);
        sw.WriteLine("");

        sw.Close();
    }

    public static string[] resultToString_NBACK()
    {
        string[] str = new string[2];
        float totalTime=0;        
        string rt="", cf="";
        for (int i = 0; i < answerCount; i++)
        {
            totalTime += reactionDuration[i];
            if (reactionDuration[i]<256)
                rt += ((int)Math.Round(reactionDuration[i] * 10)).ToString("X2");
            else
                rt += "GG";

            if (reactionEval[i])
                cf += "T";
            else
                cf += "F";
        }
        str[0] = IdentificationTag() +","+ taskType + "," + back_number.ToString() + "," + answerCount.ToString() + "," + totalTime.ToString("F1") + "," + cf;
        str[1] = rt;
        return str;
    }
    public static string[] resultToString_Challenge()
    {
        string[] str = new string[2];
        float totalTime = 0;
        string rt = "", cf = "";
        for (int i = 0; i < answerCount; i++)
        {
            totalTime += reactionDuration[i];
            if (reactionDuration[i] < 256)
                rt += ((int)Math.Round(reactionDuration[i] * 10)).ToString("X2");
            else
                rt += "GG";

            if (reactionEval[i])
                cf += "T";
            else
                cf += "F";
        }
        str[0] = IdentificationTag() + "," + taskType + "," + challenge_back_number.ToString() + "," + answerCount.ToString() + "," + totalTime.ToString("F1") + "," + cf;
        str[1] = rt;
        return str;
    }
    public static string[] resultToString_SPEED()
    {
        string[] str = new string[2];
        float totalTime = 0;
        string rt = "";
        for (int i = 0; i < 30; i++)
        {
            totalTime += speed_ReactionTimes[i];
            if (speed_ReactionTimes[i] < 256)
                rt += ((int)Math.Round(speed_ReactionTimes[i] * 10)).ToString("X2");
            else
                rt += "GG";
        }
        str[0] = IdentificationTag() + "," + taskType + "," + totalTime.ToString("F1");
        str[1] = rt;
        return str;
    }
       
    public static void savePref_Score(int score)
    {
        int trialCount = PlayerPrefs.GetInt("Trial_Count",1);
        if (trialCount > 18)
        {
            for (int i = 0; i < 18; i++)
            {                
                PlayerPrefs.SetInt(string.Format("score{0}", i),
                    PlayerPrefs.GetInt(string.Format("score{0}", i + 1), 0));
            }
            PlayerPrefs.SetInt("score18", score);
        }
        else
        {
            PlayerPrefs.SetInt(string.Format("score{0}", trialCount), score);
        }
        trialCount++;
        PlayerPrefs.SetInt("Trial_Count", trialCount);
    }
    public static int[] loadPref_Scores()
    {
        int[] ret = new int[18];
        for (int i = 0; i < 18; i++)
        {
            ret[i] = PlayerPrefs.GetInt(string.Format("score{0}", i + 1), 0);
        }
        return ret;
    }

    public static void savePref_SpeedScore(float score)
    {
        int trialCount = PlayerPrefs.GetInt("Speed_Trial_Count", 1);
        if (trialCount > 18)
        {
            for (int i = 0; i < 18; i++)
            {
                PlayerPrefs.SetFloat(string.Format("speedscore{0}", i),
                    PlayerPrefs.GetFloat(string.Format("speedscore{0}", i + 1), 0));
            }
            PlayerPrefs.SetFloat("speedscore18", score);
        }
        else
        {
            PlayerPrefs.SetFloat(string.Format("speedscore{0}", trialCount), score);
        }
        trialCount++;
        PlayerPrefs.SetInt("Speed_Trial_Count", trialCount);
    }
    public static float[] loadPref_SpeedScores()
    {
        float[] ret = new float[18];
        for (int i = 0; i < 18; i++)
        {
            ret[i] = PlayerPrefs.GetFloat(string.Format("speedscore{0}", i + 1), 0);
        }
        return ret;
    }

    public static void savePref_ChallengeScore(int score)
    {
        int trialCount = PlayerPrefs.GetInt("Challenge_Count", 1);
        //trialCount = 1;
        if (trialCount > 18)
        {
            for (int i = 0; i < 18; i++)
            {
                PlayerPrefs.SetInt(string.Format("challenge_score{0}", i),
                    PlayerPrefs.GetInt(string.Format("challenge_score{0}", i + 1), 0));
            }
            PlayerPrefs.SetInt("challenge_score18", score);
        }
        else
        {
            PlayerPrefs.SetInt(string.Format("challenge_score{0}", trialCount), score);
        }
        trialCount++;
        PlayerPrefs.SetInt("Challenge_Count", trialCount);
    }
    public static int[] loadPref_ChallengeScores()
    {
        int[] ret = new int[18];
        for (int i = 0; i < 18; i++)
        {
            ret[i] = PlayerPrefs.GetInt(string.Format("challenge_score{0}", i + 1), 0);
        }
        return ret;
    }

    public static void savePref_2D3DScore(int score)
    {
        int trialCount = PlayerPrefs.GetInt("2D3D_Count", 1);
        //trialCount = 1;
        if (trialCount > 18)
        {
            for (int i = 0; i < 18; i++)
            {
                PlayerPrefs.SetInt(string.Format("2D3D_score{0}", i),
                    PlayerPrefs.GetInt(string.Format("2D3D_score{0}", i + 1), 0));
            }
            PlayerPrefs.SetInt("2D3D_score18", score);
        }
        else
        {
            PlayerPrefs.SetInt(string.Format("2D3D_score{0}", trialCount), score);
        }
        trialCount++;
        PlayerPrefs.SetInt("2D3D_Count", trialCount);
    }
    public static int[] loadPref_2D3DScores()
    {
        int[] ret = new int[18];
        for (int i = 0; i < 18; i++)
        {
            ret[i] = PlayerPrefs.GetInt(string.Format("2D3D_score{0}", i + 1), 0);
        }
        return ret;
    }

    public static string getAndroidIMEI()
    {
#if UNITY_ANDROID
        RuntimePermissionHelper.UsePermission("android.permission.READ_PHONE_STATE");
        AndroidJavaObject TM = new AndroidJavaObject("android.telephony.TelephonyManager");
        return TM.Call<string>("getDeviceId");
#else
        return "";
#endif
    }

    private static string IdentificationTag()
    {
        //return getAndroidIMEI();
        return SystemInfo.deviceUniqueIdentifier+","+userID;
    }
}