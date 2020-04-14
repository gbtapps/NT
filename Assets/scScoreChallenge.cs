using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class scScoreChallenge : MonoBehaviour
{

    public Text txtScore;
    public GameObject cubeBase;
    public float x_scale = 1.0f, x0 = -6f, y0 = -3f, ymax = 3f;
    // Use this for initialization
    void Start()
    {
        if (!scDataStore.debug_Start) scDataStore.Start();//- for debug. Should be done in Scene1

        //- PlayerPrefs
        scDataStore.savePref_ChallengeScore(scDataStore.totalChallendedPeriod);
        int[] scores = scDataStore.loadPref_ChallengeScores();
        //- result
        txtScore.text = scDataStore.totalChallendedPeriod.ToString("0") + "  秒";
        int trial_Count = PlayerPrefs.GetInt("Challenge_Count", 1) - 1;
        if (trial_Count >= 18) trial_Count = 18;
        //trial_Count = 1;

        GameObject[] Obj = new GameObject[30];
        LineRenderer renderer = gameObject.GetComponent<LineRenderer>();
        // 線の幅
        // 頂点の数
        renderer.positionCount = trial_Count;
        Vector3 pos;
        for (int i = 0; i < trial_Count; i++)
        {
            pos = new Vector3(x0 + i * x_scale, y0 + (float)scores[i] / Mathf.Max(scores) * ymax * 0.9f, -1);
            Obj[i] = Instantiate(Resources.Load("Sphere"), pos, Quaternion.identity) as GameObject;
            // 頂点を設定
            renderer.SetPosition(i, pos);
            renderer.startWidth = 0.1f;
            renderer.endWidth = 0.1f;
            renderer.startColor = Color.white;
            renderer.endColor = Color.white;
        }
    }

    public void button_callback()
    {
        SceneManager.LoadScene("1");
    }
    // Update is called once per frame
    void Update()
    {

    }
}
