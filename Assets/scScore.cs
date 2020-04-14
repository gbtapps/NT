using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class scScore : MonoBehaviour {

    public Text txtScore;
    public GameObject cubeBase;
    public float x_scale = 1.0f, x0 = -6f, y0 = -3f, ymax = 3f;
    // Use this for initialization
    void Start()
    {

        if (!scDataStore.debug_Start) scDataStore.Start();//- for debug. Should be done in Scene1

        int repeat = scDataStore.maxRepeatCount;
        int correct_count = 0;
        for (int i = 0; i < repeat; i++)
        {
            //Debug.Log(scDataStore.reactionEval[i+scDataStore.back_number]);
            if (scDataStore.reactionEval[i+scDataStore.back_number])
                correct_count++;
        }
        Debug.Log(correct_count.ToString());
        float scorePercent = (float)correct_count / repeat * 100f;
                
        //scorePercent = 90f;

        //- PlayerPrefs
        scDataStore.savePref_Score((int)scorePercent);
        int[] scores = scDataStore.loadPref_Scores();

        //- change level
        if (scorePercent > 85)
        {
            if (scDataStore.speed == 0)
            {
                scDataStore.speed = 1;
            }
            else {
                scDataStore.back_number++;
                scDataStore.speed = 0;
            }
            //PlayerPrefs.SetInt("back_number", scDataStore.back_number);
        } else if ( scorePercent < 65)
        {
            if (scDataStore.speed == 1)
            {
                scDataStore.speed = 0;
            }
            else {
                scDataStore.back_number--;
                if (scDataStore.back_number < 1) scDataStore.back_number = 1;
                scDataStore.speed = 1;
            }
            //PlayerPrefs.SetInt("back_number", scDataStore.back_number);
        }

        //- result
        txtScore.text = scorePercent.ToString("N0") + "%";
        int trial_Count = PlayerPrefs.GetInt("Trial_Count", 1)-1;
        if (trial_Count >= 18) trial_Count = 18;

        GameObject[] Obj = new GameObject[30];
        LineRenderer renderer = gameObject.GetComponent<LineRenderer>();
        // 線の幅
        // 頂点の数
        renderer.positionCount = trial_Count;
        Vector3 pos;
        for (int i = 0; i < trial_Count; i++)
        {
            pos = new Vector3(x0 + i * x_scale, y0 + scores[i] / 100f * ymax, -1);                
            Obj[i] = Instantiate(Resources.Load("Sphere"), pos, Quaternion.identity) as GameObject;            
            // 頂点を設定
            renderer.SetPosition(i, pos);
            renderer.startWidth = 0.1f;
            renderer.endWidth = 0.1f;
            renderer.startColor = Color.white;
            renderer.endColor = Color.white;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void button_callback()
    {
        SceneManager.LoadScene("1");
    }
}
