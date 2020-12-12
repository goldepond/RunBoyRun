using System.Collections;
using UnityEngine;
public class BoardManager : MonoBehaviour
{
    //=========================================
    public int colums = 120; // 가로 10
    //=========================================
    public GameObject[] floorTiles;
    //=========================================
    private Transform boardHolder;
    public Transform MissileLocation;
    //=========================================
    public int FireState;
    public int FireMemory = 10;
    public float FireDelay;
    //=========================================
    private void BoardSetup()
    {
        if (FireState > 0)
        {
            FireState--;
        }
    }
    private void Start()
    {
        boardHolder = new GameObject("Board").transform;
        StartCoroutine(FireCycleControl());
    }
    private void Update()
    {
        BoardSetup();
    }
    private IEnumerator FireCycleControl()
    {
        // 처음에 FireState를 false로 만들고
        if (FireState <= FireMemory)
        {
            yield return new WaitForSeconds(FireDelay);
            FireState++;
        }
        // StartCoroutine(FireCycleControl());
        // FireDelay초 후에
        // FireState를 true로 만든다.
    }

}