using UnityEngine;

public class CameraShakeBB : MonoBehaviour
{
    //
    public float ShakeAmount; // ī�޶� ���� ��
    private float ShakeTime = 0.0f; // ī�޶� ���� �ð�
    private Vector3 initialPosition; // ����, ī�޶� ���� ��ġ
    private readonly MainPlayer mainPlayer;


    public void ViberateForTime(float time)
    {
        ShakeTime = time;
    }
    private void Awake()
    {
        //mainPlayer = FindObjectOfType<MainPlayer>();
        // EventCorutine�̶�� �Լ��� ����
        //initialPosition = new Vector3(0f, 0f, -5f); //ī�޶� �⺻ ��ġ ���� 
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
