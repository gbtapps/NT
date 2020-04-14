using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class scSpeedTask : MonoBehaviour {
    public AudioSource audClick;
    public Canvas cvs;
    public Text txtTime;
    float startTime;
    float preClickTime;
    int counter = 1;

    public ParticleSystem particle;
    public Camera camera;

	// Use this for initialization
	void Start () {
        if (!scDataStore.debug_Start) scDataStore.Start();//- for debug. Scene-1 should do this.        
        scDataStore.startTime = System.DateTime.Now;
        scDataStore.taskType = "SPEED TASK";
        startTime = Time.realtimeSinceStartup;
        preClickTime = startTime;
        GameObject[] Obj = new GameObject[30];
        float x = cvs.transform.GetComponent<RectTransform>().sizeDelta.x;
        float y = cvs.transform.GetComponent<RectTransform>().sizeDelta.y;
        int sx = 6,sy = 5;
        bool[,] f = new bool[sx,sy];
        int p, q;
        for (int i = 0; i < sx*sy; i++)
        {
            p = Random.Range(0, sx);q = Random.Range(0, sy);
            while (f[p, q])
            {
                p = Random.Range(0, sx); q = Random.Range(0, sy);
            }
            f[p, q] = true;
            Obj[i] = Instantiate(Resources.Load("Button"),
                new Vector3(-x/2+(p+1)*x/(sx+1), -y/2+(q+1)*y/(sy+1), -1),
                Quaternion.identity) as GameObject;
            Obj[i].transform.SetParent(cvs.transform,false);
            Obj[i].transform.Find("Text").GetComponent<Text>().text = (i+1).ToString();
            //Obj[i].GetComponent<Button>().onClick.AddListener(button_click);
            //Obj[i].tag = i.ToString();
            Obj[i].name = (i+1).ToString();
            var trigger = Obj[i].AddComponent<EventTrigger>();
            trigger.triggers = new List<EventTrigger.Entry>();

            // PointerEnter(マウスオーバー)時のイベントを設定してみる
            var entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown; // 他のイベントを設定したい場合はここを変える
            entry.callback.AddListener(OnPointerClickLevelButton);
            trigger.triggers.Add(entry);
        }

        HandheldUtil.Initialize();//- for vibration  

        //Input.backButtonLeavesApp = true;//- for back button

        particle.Simulate(0.0f, true, true);
        //particle.Play();


    }
    public void OnPointerClickLevelButton(BaseEventData eventData)
    {
        string s = eventData.selectedObject.name;

        if (counter== int.Parse(s))
        {
            var pos = camera.ScreenToWorldPoint(Input.mousePosition + camera.transform.forward * 10);
            var part = Instantiate(particle, pos, Quaternion.identity);
            //particle.Simulate(0.0f, true, true);
            //particle.transform.position = pos;
            //particle.Emit(1);
            //particle.Play();
            part.transform.position = pos;
            part.Emit(1);
            part.Play();

            eventData.selectedObject.SetActive(false);
            scDataStore.speed_ReactionTimes[counter-1] = Time.realtimeSinceStartup - preClickTime;
            preClickTime = Time.realtimeSinceStartup;
            //audClick.Play();
            HandheldUtil.Vibrate(100);
            counter++;
        }

        if (counter > 30)
        {
            scDataStore.saveToFile_SPEED();

            //- Upload log
            string[] resultStr = scDataStore.resultToString_SPEED();

            WebAPI.TrainingResultPostObject obj = new WebAPI.TrainingResultPostObject();
            obj.xb01id = resultStr[0]; //scDataStore.userID;
            obj.training_id = resultStr[1];
            obj.time = 0;
            obj.score = 0;
            WebAPI.Instance.TrainingResultPost(obj);


            SceneManager.LoadScene("5score");
            Handheld.Vibrate();
            //HandheldUtil.Destruct();//- for vibration
            cvs.enabled = false;
        }
        //Debug.Log(s + "が、選択されました！");
    }

    public void button_click()
    {
        Button obj = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        //Debug.Log(obj.GetComponent<Text>().text);
    }
	// Update is called once per frame
	void Update () {
        //-timer
        float dtime = Time.realtimeSinceStartup - startTime;
        txtTime.text = Mathf.Floor(dtime / 60).ToString("0") + ":" + (dtime % 60).ToString("00.000");
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {            
            SceneManager.LoadScene("1");
            cvs.enabled = false;
        }        
    }
}
