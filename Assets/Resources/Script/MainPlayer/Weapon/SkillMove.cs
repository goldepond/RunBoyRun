using UnityEngine;
//-----------------------------------------------------------------------------------------
//이 스크립트는 적의 직선형 공격을 구현한 스크립트이다.
//-----------------------------------------------------------------------------------------
public class SkillMove : MonoBehaviour
{
    ///==========================================================s===================================
    public float MoveSpeed;     // 미사일이 날라가는 속도
    private Vector3 movedirection;

    ///=============================================================================================
    protected MainPlayer mainPlayer;
    ///=============================================================================================
    public Rigidbody2D rb;
    ///=============================================================================================
    private void Start()
    {
        mainPlayer = FindObjectOfType<MainPlayer>();
    }

    private void Update()
    {
        rb.velocity = movedirection * MoveSpeed * Time.deltaTime;
    }

    ///=============================================================================================
    private void OnBecameInvisible()
    {
        GetComponent<Collider2D>().enabled = false; //화면밖으로 나가 보이지 않게 되면 미사일이 비활성화한다.
    }
    ///=============================================================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 만약 Dead, Monster, Ground 의 테그를 가진 물체와 닿으면 미사일이 비활성화한다.
        if (collision.gameObject.tag == "Dead" || collision.gameObject.tag == "Ground")
        {
            GetComponent<Collider2D>().enabled = false;
            //|| collision.gameObject.tag == "Monster" 
        }
        else if (collision.gameObject.tag == "Monster")//&& isMonster == true
        {
            GetComponent<Collider2D>().enabled = false;
        }
    }


}

