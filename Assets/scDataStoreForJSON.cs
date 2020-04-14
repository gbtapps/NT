using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;


[SerializeField]
public class scDataStoreForJSON  {
    public string taskType;
    public string startTime;
    public string endTime;
    public int back_number;
    public int speed;
    public int answerCount;
    public int maxRepeatCount;
    public float[] reactionDuration;
    public bool[] reactionEval;
    public int[] questionList;
    public int[] reactionList;
   
}
