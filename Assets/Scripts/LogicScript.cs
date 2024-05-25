using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogicScript : MonoBehaviour
{
    public Text timeText;
    private float time = 0.0f;

    private Dictionary<int, bool> checkPoints = new Dictionary<int, bool>();

    //// Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 4; i++)
        {
            checkPoints.Add(i, false);
        }
    }

    //// Update is called once per frame
    public void CrossFinishLine()
    {
        bool canFinish = true;
        for( int i = 0;i < 4;i++)
        {
            if (!checkPoints[i]) 
            {
                canFinish = false;
                break;
            }
        }
        if(canFinish)
        {
            for (int i = 0; i < 4; i++)
            {
                checkPoints[i] = false;
            }
            time = 0.0f;
        }
    }

    public void CrossCheckPoint(int id)
    {
        checkPoints[id] = true;
    }
    void Update()
    {
        time += Time.deltaTime;
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        timeText.text = timeSpan.ToString(@"mm\:ss\:ff");
    }
}
