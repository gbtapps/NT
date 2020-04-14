using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class scScoring : MonoBehaviour {

    public Slider sliderAge;
    public Slider sliderSex;
    public Slider sliderFCol;
    public Slider sliderQSc;
    public Text t1, t2, t3, t4,t5;

    public Canvas cvsMe;
    public Canvas cvsStart;

    // Use this for initialization
    void Start () {
        sliderSex = sliderSex.GetComponent<Slider>();
        sliderAge = sliderAge.GetComponent<Slider>();
        sliderFCol = sliderFCol.GetComponent<Slider>();
        sliderQSc = sliderQSc.GetComponent<Slider>();
        t1 = t1.GetComponent<Text>();
        t2 = t2.GetComponent<Text>();
        t3 = t3.GetComponent<Text>();
        t4 = t4.GetComponent<Text>();
        t5 = t5.GetComponent<Text>();

        cvsMe = cvsMe.GetComponent<Canvas>();
        cvsStart = cvsStart.GetComponent<Canvas>();

        t5.text = Application.persistentDataPath;

    }

    // Update is called once per frame
    void Update () {
	
	}

    public void cbAge()
    {
        t2.text = sliderAge.value.ToString();
        //scDataStore.age = (int)sliderAge.value;
    }

    public void cbSex()
    {
        if (sliderSex.value == 0)
        {
            t1.text = "M";
        }
        else
        {
            t1.text = "F";
        }
        //scDataStore.sex = (int)sliderSex.value;
    }

    public void cbFCol()
    {
        //scDataStore.fColor = (int)sliderFCol.value;
        if (sliderFCol.value == 0) { t3.text = "dark"; }
        if (sliderFCol.value == 1) { t3.text = "nomal"; }
        if (sliderFCol.value == 2) { t3.text = "brite"; }

        
    }
    public void cbQSc()
    {
        //scDataStore.Qscore = (int)sliderQSc.value;
        t4.text = sliderQSc.value.ToString();
    }

    public void saveToFile()
    {
        //scDataStore.saveToFile();
        cvsMe.enabled = false;
        cvsStart.enabled = true;
        cvsStart.SendMessage("Start");
    }
}
