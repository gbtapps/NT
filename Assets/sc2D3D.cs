using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;

public class sc2D3D : MonoBehaviour
{
    public AudioSource audClick;
    public Canvas cvs;
    public Text txtTime;
    public GameObject blockPrefab;
    public GameObject blockPrefabAns;
    float startTime;
    float preClickTime;
    private int sx=4, sy=4, sz=4;
    public GameObject[] parentObj = new GameObject[4];
    public GameObject[] ansObj = new GameObject[3];
    private bool[,,] f, ansF;
    private GameObject[,] objBlocks;
    private int ansNum;
    public GameObject timerSprite;
    private Vector3 timerSprite_DefaultScale;
    public int totalTrainingPeriodinSec = 60;
    public Text answerCounterText;


    // Use this for initialization
    void Start()
    {
        scDataStore.Start();//- for debug. Scene-1 should do this.        
            scDataStore.startTime = System.DateTime.Now;
            scDataStore.unityTime = Time.realtimeSinceStartup;
            scDataStore.NBACK_StartTime = Time.realtimeSinceStartup;
        scDataStore.taskType = "2D3D TASK";

        PlayerPrefs.SetInt("rotation_block_size", scDataStore.rotationBlockSize);
        sx = scDataStore.rotationBlockSize;
        sy = sx;sz = sx;

        startTime = Time.realtimeSinceStartup;
        preClickTime = startTime;

        ansF = new bool[sx, sy, sz];        
        f = new bool[sx, sy, sz];
        objBlocks = new GameObject[7, sx * sy * sz];
        
        float x = sx;
        float y = sy;
        float z = sz;

        float scu = 0.6f;
        for (int i = 0; i < 4; i++)        
            prepareBlocks(scu, parentObj[i], blockPrefab, i);            
        for (int i = 0; i < 3; i++)
            prepareBlocks(scu, ansObj[i], blockPrefabAns, i+4);

        prepareGame(f,ansF);

        timerSprite_DefaultScale = timerSprite.transform.localScale;
        HandheldUtil.Initialize();//- for vibration        
    }

    public void resetRotationAngle()
    {
        for (int i = 0; i < 4; i++)
        {
            parentObj[i].transform.rotation =
                Quaternion.Euler(new Vector3(-15f, 20f, 0));
        }
    }

    private void prepareGame(bool[,,] f, bool[,,] ansF)
    {
        ansNum = (int)UnityEngine.Random.Range(0f, 4f);        
        for (int i = 0; i < 4; i++)
        {
            f = makeBinalyModel(sx, sy, sz);
            GameObject[] tmp=makeObject(f, parentObj[i], blockPrefab, i);
            
            if (ansNum == i)
            {
                Array.Copy(f, ansF, f.Length);
            }
            parentObj[i].transform.rotation =
                Quaternion.Euler(new Vector3(-15f, 20f, 0));
        }
        Vector3[] rot = new Vector3[3];
        rot[0] = new Vector3(-90, 0, 0);
        rot[1] = new Vector3(0, 0, 0);
        rot[2] = new Vector3(0, 90, 0);
        for (int i = 0; i < 3; i++)
        {
            makeObject(ansF, ansObj[i], blockPrefabAns, i+4);
            ansObj[i].transform.rotation =  Quaternion.Euler(rot[i]);
            //ansObj[i].transform.Rotate(rot[i]);            
        }
    }
    
    private void destroyArrayGameObject(GameObject[] tmp)
    {
        for (int i = 0; i < tmp.Length; i++) Destroy(tmp[i]);

    }
    private GameObject[] makeObject(bool[,,] f, GameObject parentObj, GameObject blockPrefab, int targetArea)
    {
        Vector3 sc = parentObj.transform.localScale;
        int sx = f.GetLength(0), sy = f.GetLength(1), sz = f.GetLength(2);
        Vector3 scDiv = sc;
        scDiv.x /= (float)sx * 2f;
        scDiv.y /= (float)sy * 2f;
        scDiv.z /= (float)sz * 2f;
        
        Vector3 p0 = parentObj.transform.position;
        Vector3 pSc = parentObj.transform.localScale;
        GameObject[] Obj = new GameObject[sx * sy * sz];
        int counter = 0;        
        for (int i = 0; i < sx; i++)
        {
            for (int j = 0; j < sy; j++)
            {
                for (int k = 0; k < sz; k++)
                {
                    if (f[i, j, k])
                        objBlocks[targetArea, counter].SetActive(true);
                    else
                        objBlocks[targetArea, counter].SetActive(false);
                    counter++;                    
                }
            }
        }

        return Obj;
    }
    private void prepareBlocks(float scu, GameObject parentObj, GameObject blockPrefab, int targetArea)
    {
        int counter = 0;
        int i1 = targetArea;
        for (int i = 0; i < sx; i++)
        {
            for (int j = 0; j < sy; j++)
            {
                for (int k = 0; k < sz; k++)
                {
                    objBlocks[i1, counter] = Instantiate(blockPrefab) as GameObject;
                    objBlocks[i1, counter].transform.parent = parentObj.transform;
                    objBlocks[i1, counter].transform.localScale = new Vector3(
                        1f / sx, 1f / sy, 1f / sz) * scu * 0.95f;
                    Vector3 p1 = objBlocks[i1, counter].transform.localScale;
                    objBlocks[i1, counter].transform.localPosition = new Vector3(
                    -0.5f * scu + p1.x / 2f + (float)i * scu / (float)sx,
                    -0.5f * scu + p1.y / 2f + j * scu / (float)sy,
                    -0.5f * scu + p1.z / 2f + k * scu / (float)sz);
                    objBlocks[i1, counter].name = (i + 1).ToString();
                    counter++;
                }
            }
        }
    }    
    public bool[,,] makeBinalyModel(int x, int y, int z)
    {
        bool[,,] f = new bool[x, y, z];
        bool[,] a = make2D(x, y);
        bool[,] b = make2D(x, y);

        for (int i = 0; i < z; i++)
        {
            while (!check2D(a, b))
            {
                b = make2D(x, y);
            }
            for (int j = 0; j < x; j++)
            {
                for (int k = 0; k < y; k++)
                {
                    f[j, k, i] = b[j, k];
                    a[j, k] = b[j, k];
                }
            }
            b = make2D(x, y);
        }
        return f;
    }
    private bool[,] make2D(int x, int y)
    {
        bool[,] e = new bool[x, y];
        while (true)
        {
            int count = 0;
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (UnityEngine.Random.Range(0f, 1f) > 0.5f)
                    {
                        e[i, j] = true;
                        count++;
                    }
                    else e[i, j] = false;
                }
            }
            if (count > 3) break;
        }
        return e;
    }
    private bool check2D(bool[,] a, bool[,] b)
    {
        int x = a.GetLength(0), y = a.GetLength(1);
        bool ret = false;
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                if (a[i, j] && b[i, j]) ret = true;
            }
        }
        return ret;
    }

    public void button_click(int num)
    {
        cvs.enabled = false;
        updateDataStore(num);
        prepareGame(f,ansF);
        cvs.enabled = true;
    }

    private void updateDataStore(int inputNum)
    {
        scDataStore.reactionDuration[scDataStore.answerCount] = Time.realtimeSinceStartup - preClickTime;
        scDataStore.reactionEval[scDataStore.answerCount] = inputNum == ansNum;
        scDataStore.answerCount++;
        if (inputNum == ansNum)
        {
            scDataStore.correctCount++;
            HandheldUtil.Vibrate(20);
        }
        else
        { HandheldUtil.Vibrate(1000); }

        preClickTime = Time.realtimeSinceStartup;
        Debug.Log(scDataStore.reactionDuration[scDataStore.answerCount - 1].ToString() + 
            "/" + scDataStore.reactionEval[scDataStore.answerCount - 1].ToString() + "/" + ansNum.ToString());
        answerCounterText.text = scDataStore.correctCount.ToString() +"/"+scDataStore.answerCount.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("1");
            cvs.enabled = false;
        }

        //-timer
        if (Time.realtimeSinceStartup - startTime > totalTrainingPeriodinSec)
        {
            //finish the training
            scDataStore.saveToFile_2D3D();
            SceneManager.LoadScene("9_2D3D_score");
            Handheld.Vibrate();
            HandheldUtil.Destruct();//- for vibration
            cvs.enabled = false;
        }
        else
        {
            float scR = 1 - (Time.realtimeSinceStartup - startTime)/totalTrainingPeriodinSec;
            Vector3 sc = timerSprite_DefaultScale;
            sc.x *= scR;
            timerSprite.transform.localScale = sc;
        }
        
    }
}
