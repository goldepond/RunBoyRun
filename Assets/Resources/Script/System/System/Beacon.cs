using UnityEngine;
public class Beacon : MonoBehaviour
{
    ///=============================================================================================
    public float MoveSpeed;     // 미사일이 날라가는 속도
    private Vector3 movedirection;
    ///=============================================================================================
    private readonly MainPlayer mainPlayer;
    private readonly RangeSkill skill;
    ///=============================================================================================
    private void Update()
    {
        Move();
        if (transform.position.x <= -10)
        {
            // 미사일을 제거
            GetComponent<Collider2D>().enabled = false;
            gameObject.SetActive(false);
        }
    }
    ///=============================================================================================
    private void OnBecameInvisible()
    {
        GetComponent<Collider2D>().enabled = false; //화면밖으로 나가 보이지 않게 되면 미사일이 비활성화한다.
    }
    ///=============================================================================================
    private void Move()// 플레이어가 왼쪽으로 이동하는 함수. 
    {
        Vector3 moveVelocity = Vector3.left;
        transform.position += moveVelocity * MoveSpeed * Time.deltaTime;
    }
}

