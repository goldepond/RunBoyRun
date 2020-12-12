using UnityEngine;

public class CameraShakeBB : MonoBehaviour
{
    //
    public float ShakeAmount; // 카메라 흔드는 힘
    private float ShakeTime = 0.0f; // 카메라 흔드는 시간
    private Vector3 initialPosition; // 진원, 카메라 원래 위치
    private readonly MainPlayer mainPlayer;


    public void ViberateForTime(float time)
    {
        ShakeTime = time;
    }
    private void Awake()
    {
        //mainPlayer = FindObjectOfType<MainPlayer>();
        // EventCorutine이라는 함수를 실행
        //initialPosition = new Vector3(0f, 0f, -5f); //카메라 기본 위치 설정 
    }

    // Update is called once per frame
    private void Update()
    {
        if (ShakeTime > 0)
        {
            transform.position = Random.insideUnitSphere * ShakeAmount + initialPosition;
            ShakeTime -= Time.deltaTime;
        }
        else
        {
            ShakeTime = 0.0f;
            transform.position = initialPosition;
        }

    }
}
