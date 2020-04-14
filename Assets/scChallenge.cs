using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class scChallenge : MonoBehaviour
{
    public bool flagUpdate;
    public Text texObjQuestion;
    public Text texObjCounter;

    public Text txtObjReaction;
    public Text txtObjResult;
    public EventSystem eventStstem;
    public bool flag_Move;
    public bool flag_exit;
    public Canvas cvsMe;
    public Vector2 cvsCenter;
    //public Canvas cvsStart;
    //public Canvas cvsNext;
        
    public float posX = 200;
    public float posY = 200;
    public float posX_Counter = 200;
    public float sizeY = 0;

    public AudioSource soundYes;
    public AudioSource soundNo;

    public int calcAnswer = 0;

    public float timeStart;
    public float time_TaskStart;
    public Text timerText;
    public GameObject spriteTimerBar;

    float waitStart, waitDulation;
    bool isWaiting = false;

    public bool debug_this;
        
    // Use this for initialization
    void Start()
    {
        if (debug_this) scDataStore.Start();//- for debug. Scene-1 should do this.        
        if (!scDataStore.debug_Start) scDataStore.Start();//- for debug. Scene-1 should do this.        
        //if (Time.realtimeSinceStartup - scDataStore.unityTime > 60 * 60 * 12) //- check date
        //{
            scDataStore.startTime = System.DateTime.Now;
            scDataStore.unityTime = Time.realtimeSinceStartup;
            //scDataStore.NBACK_StartTime = Time.realtimeSinceStartup;
        //}

        scDataStore.ChallengeStartTime = (int)Time.realtimeSinceStartup;

        scDataStore.answerCount = 0;
        //scDataStore.challenge_back_number = 2;//- 2 back
        //scDataStore.challenge_back_number = 3;//- 3 back 2017-10-13
        scDataStore.speed = 1;//- fast mode=1
        scDataStore.isChallengeMode = true;
        PlayerPrefs.SetInt("speed", 1);

        //- Data store clear
        //if (scDataStore.NBACK_StartTime == 0f) scDataStore.NBACK_StartTime = Time.realtimeSinceStartup;
        //time_TaskStart = scDataStore.NBACK_StartTime;

        scDataStore.taskType = "Challenge task";     
        flagUpdate = true;
        cvsMe = cvsMe.GetComponent<Canvas>();
        texObjQuestion = texObjQuestion.GetComponent<Text>();
        texObjCounter = texObjCounter.GetComponent<Text>();
        flag_Move = false;
        flag_exit = false;
        cvsCenter = new Vector2(cvsMe.transform.position.x / 2, cvsMe.transform.position.y / 2);
        posX = texObjQuestion.transform.localPosition.x;
        posY = texObjQuestion.transform.localPosition.y;
        posX_Counter = texObjCounter.transform.localPosition.x;
        sizeY = texObjQuestion.preferredHeight;

        soundYes = soundYes.GetComponent<AudioSource>();
        soundNo = soundNo.GetComponent<AudioSource>();
        //cvsStart = cvsStart.GetComponent<Canvas>();
        //cvsNext = cvsNext.GetComponent<Canvas>();

        NewText();
    }

    // Update is called once per frame
    void Update()
    {
        if (!flagUpdate) return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("1");
            cvsMe.enabled = false;
        }
        //- wait
        if (isWaiting)
        {
            if (Time.realtimeSinceStartup - waitStart > waitDulation) isWaiting = false;
            else return;
        }
        //-timer
        float dtime = Time.realtimeSinceStartup - time_TaskStart;
        timerText.text = Mathf.Floor(dtime / 60).ToString("0") + ":" + (dtime % 60).ToString("00");
        //
        if (flag_Move)
        //- mode up the text
        {
            eventStstem.enabled = false;
            Color col = texObjQuestion.color;
            if (texObjQuestion.transform.localPosition.y < posY + sizeY / 4f)
            {
                float speed = 200;
                if (scDataStore.answerCount <= scDataStore.challenge_back_number) speed = 80;
                texObjQuestion.transform.localPosition += new Vector3(0, speed * Time.deltaTime, 0);
                texObjQuestion.color = new Color(col.r, col.g, col.b, 1f - (texObjQuestion.transform.localPosition.y - posY) / (60));
                texObjCounter.transform.localPosition += new Vector3(0, speed * Time.deltaTime, 0);
                texObjCounter.color = new Color(col.r, col.g, col.b, 1f - (texObjQuestion.transform.localPosition.y - posY) / (60));
            }
            else
            {
                eventStstem.enabled = true;
                flag_Move = false;
                texObjQuestion.transform.localPosition = new Vector3(posX, posY, 0);
                texObjQuestion.color = new Color(col.r, col.g, col.b, 1);
                texObjCounter.transform.localPosition = new Vector3(posX_Counter, posY, 0);
                texObjCounter.color = new Color(col.r, col.g, col.b, 1);
                NewText();
            }
        }
        else if (flag_exit)
        {
            eventStstem.enabled = false;
            if (Time.realtimeSinceStartup - timeStart < 1)
            {
                //cvsNext.enabled = true;
                //cvsNext.SendMessage("Start");
                //flag_exit = false;
            }
            else
            {
                cvsMe.enabled = false;                
                SceneManager.LoadScene("7score_challenge");                
            }
        }
        //- Skip waiting for the reaction in the Memory period
        else if (scDataStore.answerCount < scDataStore.challenge_back_number)
        {
            scDataStore.checkAnswer(0, -1);//- dummy
            //NewText();
            isWaiting = true;
            waitDulation = 2; waitStart = Time.realtimeSinceStartup;
            flag_Move = true;
        }
        //- Check time for fast mode
        else if (scDataStore.speed == 1)//- fast mode
        {
            float timePassed = Time.realtimeSinceStartup - timeStart;
            timerBarSetParcent( (2f - timePassed) / 2f *100f);
            if(timePassed > 2)
            {
                timeIsUp();        
            }
        }        
    }

    private void timeIsUp()
    {
        txtObjReaction.text = "";
        texObjQuestion.text = "Time is up.";
        texObjQuestion.color = new Color(1f, 0f, 0f);
        texObjCounter.text = "";
        startExit();
    }

    public void ButtonCallback_answer(int reactionKey)
    {
        if (scDataStore.answerCount < scDataStore.challenge_back_number | flag_Move | isWaiting) return;

        //Debug.Log(reactionKey.ToString());
        bool result = scDataStore.checkAnswer(Time.realtimeSinceStartup - timeStart, reactionKey);
        txtObjReaction.text = reactionKey.ToString();
    
        if (result)
        {
            txtObjResult.text = "O";
            txtObjResult.color = new Color(1f, 0f, 0f);
            soundYes.Play();
        }
        else
        {
            startExit();
            return;
        }

        flag_Move = true;

    }

    private void startExit()
    {
        int total = (int)(Time.realtimeSinceStartup - scDataStore.ChallengeStartTime);
        txtObjResult.text = (total).ToString("0.秒");
        scDataStore.totalChallendedPeriod = total;
        txtObjResult.color = new Color(0f, 0f, 1f);
        soundNo.Play();
        flag_Move = false;
        isWaiting = true;
        waitDulation = 2;
        waitStart = Time.realtimeSinceStartup;
        flag_exit = true;
        flag_Move = false;

        GotoNext();//- end the game
    }

    public void NewText()
    {
        /*if (scDataStore.answerCount - scDataStore.challenge_back_number + 1 > scDataStore.maxRepeatCount)
        {
                GotoNext();//- Repeat end
        }*/

        int r1 = 100, r2 = 100, eq = 100, ans = 100;
        while (ans < 0 || ans >= 10)
        {
            r1 = Random.Range(0, 10);
            r2 = Random.Range(0, 10);
            eq = Random.Range(0, 2);
            if (eq == 0) ans = r1 + r2;
            else ans = r1 - r2;
        }
        
        if (eq == 0)
            texObjQuestion.text = string.Concat(r1.ToString(), " + ", r2.ToString());
        else
            texObjQuestion.text = string.Concat(r1.ToString(), " - ", r2.ToString());

        texObjCounter.text = string.Concat("( ", (scDataStore.answerCount + 1).ToString("D2"), " )");

        calcAnswer = ans;
        scDataStore.addToQuestionList(calcAnswer);
        timeStart = Time.realtimeSinceStartup;
        //flag_Move = true;
        if (scDataStore.answerCount <= scDataStore.challenge_back_number - 1) texObjCounter.color = Color.blue;
        else texObjCounter.color = Color.white;
    }

    private void timerBarSetParcent(float val)
    {
        Vector2 scale = spriteTimerBar.transform.localScale;
        scale.x = 7f * val / 100f;
        spriteTimerBar.transform.localScale = scale;
    }
    private void timerBarReset()
    {
        timerBarSetParcent(100f);
    }
    public void GotoNext()
    {
        timeStart = Time.realtimeSinceStartup;
        timerBarReset();
        flag_exit = true;

        if (debug_this) return;

        scDataStore.saveToFile_Challenge();
        //cvsMe.enabled = false;

        string[] resultStr = scDataStore.resultToString_Challenge();
        WebAPI.TrainingResultPostObject obj = new WebAPI.TrainingResultPostObject();
        //obj.xb01id = SystemInfo.deviceUniqueIdentifier + "," + resultStr[0]; //scDataStore.userID;
        obj.xb01id = resultStr[0]; //scDataStore.userID;
        obj.training_id = resultStr[1];
        obj.time = 0;
        obj.score = 0;

        WebAPI.Instance.TrainingResultPost(obj);
    }
}
