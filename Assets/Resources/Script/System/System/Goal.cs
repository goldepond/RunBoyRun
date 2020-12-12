using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour
{
    //=========================================
    string StagePath;
    private const string MethodName = "GoalTime";
    //=========================================
    public float startPosition;
    public float endPosition;
    //=========================================
    private int movespeed=0;
    //=========================================
    //=========================================
    private MainPlayer mainPlayer;
    private StageInformation stageInformation ;

    //=========================================
    private void Start()
    {

        //=========================================
        StagePath = Application.persistentDataPath + "/StageInformation.txt";
        //=========================================
        mainPlayer = GameObject.FindWithTag("Player").GetComponent<MainPlayer>();
    }
    private void Update()
    {
        Invisible();
    }
    private int a = 1;
    private void Invisible()
    {
        if (transform.position.x <= endPosition)
        {
            movespeed = 0;
            transform.Translate(-1 * (endPosition - startPosition), 0, 110);
            a++;
        }
        //=============================================================
        if ( mainPlayer.Score > 700 + 300*a)
        {
            movespeed = 5;
            transform.position += Vector3.left * movespeed * Time.deltaTime;
        }
    }
}
