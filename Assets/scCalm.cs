using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class scCalm : MonoBehaviour {
    public bool flagUpdate;
    public Canvas cvsMe;
    public Canvas cvsNext;
    public Button txtMSG;
    public float timeStart;
    public Canvas cvsStart;
    
    // Use this for initialization
    void Start () {
        flagUpdate = true;
        cvsMe = cvsMe.GetComponent<Canvas>();
        cvsNext = cvsNext.GetComponent<Canvas>();
        txtMSG = txtMSG.GetComponent<Button>();
        cvsStart = cvsStart.GetComponent<Canvas>();

        cvsMe.enabled = true;
        txtMSG.enabled = true;
        timeStart = Time.realtimeSinceStartup;
        txtMSG.transform.localPosition = new Vector3(0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
        if (!flagUpdate) return;

        Vector3 pos = txtMSG.transform.localPosition;
        pos.y += 10 * Time.deltaTime;
        txtMSG.transform.localPosition = pos;
        if (Time.realtimeSinceStartup - timeStart > 15) btnCallback();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("1");
            cvsMe.enabled = false;
        }
    }

    public void btnCallback()
    {
        flagUpdate = false;
        cvsMe.enabled = false;
        txtMSG.enabled = false;
        cvsNext.enabled = true;
        cvsNext.SendMessage("Start");
        //scDataStore.periodCalm = Time.realtimeSinceStartup - timeStart;

    }
}
