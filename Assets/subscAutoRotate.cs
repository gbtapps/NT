using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class subscAutoRotate : MonoBehaviour {
    //public GameObject Obj;
    public float speed = 0.01f; 
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.RotateAroundLocal(Vector3.up, speed);
	}
}
