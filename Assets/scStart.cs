using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SimpleFileBrowser;

public class scStart : MonoBehaviour {
    public bool flagUpdate;
    public Canvas cvsMe;
    //public Canvas cvsNext;
    public AudioSource audStart;
    public Button btnStart;
    public Text txtTotal;
    public int flag_next;
    public float dy = 0;

    public Text str_back;
    public InputField input_N;
    public Slider slider_Speed;
    public Text timerText;

    public InputField input_RotationBlockSize;
//    public Text txtMessage;

    public InputField input_Challenge_Nbk;

    public InputField userID;
          
    // Use this for initialization
    void Start () {
        
        //if (checkExpired())
        //{
        //txtMessage.text = "アプリの利用期限が切れました。";
        //}

        cvsMe = cvsMe.GetComponent<Canvas>();
        //cvsNext = cvsNext.GetComponent<Canvas>();
        audStart = audStart.GetComponent<AudioSource>();
        btnStart = btnStart.GetComponent<Button>();
        txtTotal = txtTotal.GetComponent<Text>();


        switch (scDataStore.nback_mode)
        {
            case scDataStore.enumNbackMode.calc:
            case scDataStore.enumNbackMode.number:
                PlayerPrefs.SetInt("NB_back_number", scDataStore.back_number);
                break;
            case scDataStore.enumNbackMode.challenge:
                PlayerPrefs.SetInt("challenge_back_number", scDataStore.back_number);
                break;
        }


        scDataStore.back_number = PlayerPrefs.GetInt("NB_back_number", 1);
        input_N.text = scDataStore.back_number.ToString();
        slider_Speed.value = scDataStore.speed;

        scDataStore.challenge_back_number = PlayerPrefs.GetInt("challenge_back_number", 1);
        input_Challenge_Nbk.text = scDataStore.challenge_back_number.ToString();

        scDataStore.rotationBlockSize = PlayerPrefs.GetInt("rotation_block_size", 3);
        input_RotationBlockSize.text = scDataStore.rotationBlockSize.ToString();

        scDataStore.userID = PlayerPrefs.GetString("user_id", "default");
        userID.text = scDataStore.userID;
        
        //- debug
        //str_back.text = scDataStore.SD_card_pathname();

        setupStart();

        HandheldUtil.Initialize();//- for vibration        
    }

    private bool checkExpired()
    {
        return false;
        //DateTime expireDate = new DateTime(2021, 4, 1);
        //return expireDate <= DateTime.Now;
    }

    public void setupStart()
    {
        flag_next = 0;
        //btnStart.transform.localPosition = new Vector3(0, 0, 0);
        //cvsMe.enabled = true;
        //cvsNext.enabled = false;
        flagUpdate = true;
    }

    // Update is called once per frame
    void Update () {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            return;
        }

        
        if (!flagUpdate) return;
        //-timer
        if (scDataStore.debug_Start)
        {
            float dtime = Time.realtimeSinceStartup - scDataStore.unityTime;
            timerText.text = Mathf.Floor(dtime / 60).ToString("0") + ":" + (dtime % 60).ToString("00");
        }

        if (flag_next == 1)
        {
            Vector3 pos = btnStart.transform.localPosition;
            dy-= 100 * Time.deltaTime;
            pos.y += dy;
            btnStart.transform.localPosition = pos;
            if (pos.y < 0)
            {
                cvsMe.enabled = false;
                //cvsNext.enabled = true;
                //cvsNext.SendMessage("Start");
                flagUpdate = false;

                SceneManager.LoadScene("2");
            }
        }
        else if (flag_next==2)
        {
            Vector3 pos = btnStart.transform.localPosition;
            dy -= 100 * Time.deltaTime;
            pos.y += dy;
            btnStart.transform.localPosition = pos;
            if (pos.y < 0)
            {
                cvsMe.enabled = false;
                //cvsNext.enabled = true;
                //cvsNext.SendMessage("Start");
                flagUpdate = false;

                SceneManager.LoadScene("2_NBACK_Num");
            }
        }
        
    }

    public void onChange_Text_ID()
    {
        scDataStore.userID = userID.text;
        PlayerPrefs.SetString("user_id", scDataStore.userID);
    }
    public void onChange_Input_Challenge_Nbk()
    {
        scDataStore.challenge_back_number = int.Parse(input_Challenge_Nbk.text);
        PlayerPrefs.SetInt("challenge_back_number", scDataStore.challenge_back_number);
    }
    public void onClick_MarkButton()
    {
        scDataStore.saveToFile_Mark();
        HandheldUtil.Vibrate(100);
    }

    public void NextCanvas()//- N-back calc
    {
        if (checkExpired()) return;
        if (!scDataStore.debug_Start) scDataStore.Start();

        scDataStore.answerCount = 0;
        scDataStore.back_number = int.Parse(input_N.text);
        scDataStore.speed = (int)slider_Speed.value;
        scDataStore.isChallengeMode = false;
        //PlayerPrefs.SetInt("back_number", scDataStore.back_number);
        PlayerPrefs.SetInt("speed", scDataStore.speed);

        scDataStore.nback_mode = scDataStore.enumNbackMode.calc;

        audStart.Play();
        flag_next = 1;
    }

    public void NextCanvas_NBACK_NUM()//- N-back number
    {
        if (checkExpired()) return;
        if (!scDataStore.debug_Start) scDataStore.Start();

        scDataStore.answerCount = 0;
        scDataStore.back_number = int.Parse(input_N.text);
        scDataStore.speed = (int)slider_Speed.value;
        scDataStore.isChallengeMode = false;
        //PlayerPrefs.SetInt("back_number", scDataStore.back_number);
        PlayerPrefs.SetInt("speed", scDataStore.speed);

        scDataStore.nback_mode = scDataStore.enumNbackMode.number;

        audStart.Play();
        flag_next = 2;
    }

    public void startSpeedTask()
    {
        if (checkExpired()) return;
        cvsMe.enabled = false;
        flagUpdate = false;
        SceneManager.LoadScene("4speed");
    }

    public void startChallenge()
    {
        if (checkExpired()) return;
        if (!scDataStore.debug_Start) scDataStore.Start();

        //scDataStore.back_number = int.Parse(input_Challenge_Nbk.text);
        //PlayerPrefs.SetInt("back_number", scDataStore.back_number);
        //scDataStore.challenge_back_number = scDataStore.back_number;
        scDataStore.challenge_back_number = int.Parse(input_Challenge_Nbk.text);
        scDataStore.isChallengeMode = true;

        scDataStore.nback_mode = scDataStore.enumNbackMode.challenge;

        //- Bug fix -2019-07-29 TK
        scDataStore.answerCount = 0;
        scDataStore.back_number = int.Parse(input_Challenge_Nbk.text);
        //PlayerPrefs.SetInt("back_number", scDataStore.back_number);
        //---

        audStart.Play();

        cvsMe.enabled = false;
        flagUpdate = false;
        SceneManager.LoadScene("6Challenge");
    }
    public void start2D3D()
    {
        if (checkExpired()) return;
        if (!scDataStore.debug_Start) scDataStore.Start();

        scDataStore.rotationBlockSize = int.Parse(input_RotationBlockSize.text);

        audStart.Play();

        cvsMe.enabled = false;
        flagUpdate = false;
        SceneManager.LoadScene("8_2D3D");
    }

    public void onExportFileClick()
    {
        // フォルダ選択ダイアログを表示します

        StartCoroutine(ShowLoadDialogCoroutine());
    }

    IEnumerator ShowLoadDialogCoroutine()
    {
        // Show a load file dialog and wait for a response from user
        // Load file/folder: file, Initial path: default (Documents), Title: "Load File", submit button text: "Load"
        yield return FileBrowser.WaitForLoadDialog(true, null, "Select folder", "Select");

        // Dialog is closed
        // Print whether a file is chosen (FileBrowser.Success)
        // and the path to the selected file (FileBrowser.Result) (null, if FileBrowser.Success is false)
        Debug.Log(FileBrowser.Success + " " + FileBrowser.Result);
    }


    //===== saving ===
    /*
    public void saveStartTime(System.DateTime dt) { }
    public void saveAge(int ag) { DataST.age = ag; }
    public void saveSex(bool sx) { DataST.sex = sx; }
    public void saveFColor(int fc) { DataST.fColor = fc; }
    public void saveQScore(int qs) { DataST.Qscore = qs; }
    public void savePeriodCalm(float pc) { DataST.periodCalm = pc; }
    public void saveAnswerCount(int ac) { DataST.answerCount = ac; }
    public void saveAnswer(float tm, bool ans) { DataST.setAnswer(tm, ans); }
    */

    public Button test;
   
    public void onclicktest()
    {
        test = test.GetComponent<Button>();
        test.GetComponentInChildren<Text>().text = "hello";
    }





}
