using System.Collections;
using UnityEngine;

//=============================================================================================
// 기본몬스터 


//=============================================================================================
public abstract class MonsterScript : MonoBehaviour
{
    protected bool IsUnbeatTime = false; // 무적여부 체크 
    protected bool isDie = false; // 죽음 여부 체크
    //=============================================================================================
    private void Start()
    {
        IsUnbeatTime = false;
    }
    //=============================================================================================
    public void Die()
    {
        if (IsUnbeatTime)
        {

        }
        else if (!(IsUnbeatTime))
        {
            isDie = true;
            StopCoroutine("ChangeMovement"); //ChangeMovement코루틴을 시작합니다.
            SpriteRenderer renderer = gameObject.GetComponentInChildren<SpriteRenderer>();
            renderer.flipY = true;// Y축을 뒤집습니다.
            BoxCollider2D coll = gameObject.GetComponent<BoxCollider2D>();
            coll.enabled = false; // 
            _ = gameObject.GetComponent<Rigidbody2D>();
            //Vector2 dieVelocity = new Vector2(5f, 4f); // 위로 조금 튀어오름
            //rigd.AddForce(dieVelocity, ForceMode2D.Impulse);
            Destroy(gameObject, 3f); // 삭제
        }

    }
    public abstract void Jump();//플레이어에게 피격시 점프,이펙트 변화

    //=============================================================================================
    private void OnBecameVisible()
    {
        //enabled 스위치를 킨다.
        enabled = true;
    } // 
    //=============================================================================================
    private IEnumerator UnBeatTime()
    {
        int countTime = 0;
        while (countTime < 5)
        {
            yield return new WaitForSeconds(0.5f);
            countTime++;
        }
        IsUnbeatTime = false;

        yield return null;
    } //짧은 무적타임
}
public class ObjectMove : MonsterScript
{
    protected new SpriteRenderer renderer;
    //=============================================================================================
    /// MonsterType
    /// 0.  아무런 상호작용없음 /평균 속도 
    /// 1. 아무런 상호작용없음 / 빠른 이속
    /// 2.  플레이어 근처에 오면 /점프
    /// 3. 플레이어 근처에 오면 /급발진 돌진
    /// 4. 아무런 상호작용없음 / 느림
    //=============================================================================================


    protected int movementFlag = 0; // 기본이동방향
    private string dist = ""; //상,하,좌,우  판단
    //=============================================================================================
    private int ActRage = 0; // 
                             //=============================================================================================
    public int MonsterKeynum;
    private bool speedChange;
    private bool isTracing = false;
    private bool angery = false;
    private int monsterspeed;
    //=============================================================================================
    protected Vector3 movement;
    protected Vector3 moveVelocity = Vector3.zero;
    //=============================================================================================
    protected Animator animator;
    protected GameObject traceTarget;
    //=============================================================================================
    //=============================================================================================
    private readonly float DestroyxPos = -10;

    private bool NotNotice;

    //=============================================================================================
    private  Rigidbody2D rigid;

    private void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        Physics2D.IgnoreLayerCollision(11, 11);
        movementFlag = 1;
        animator = gameObject.GetComponentInChildren<Animator>(); // 애니매이터 사용
        MonsterCheck();
    }
   
    private void FixedUpdate()
    {
        
        Move();
        if (transform.position.x <= DestroyxPos)
        {
            // 미사일을 제거

            GetComponent<Collider2D>().enabled = false;
            gameObject.SetActive(false);
        }
    }
    //=============================================================================================
    private void OnBecameVisible()//enabled 스위치를 킨다.
    {
        MonsterCheck();
    }
    //=============================================================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case ("Player"):
                switch (MonsterKeynum)
                {
                    case (0):
                        break;
                    case (1):
                        break;
                    case (2):
                        Jump();
                        new WaitForSeconds(1f); // 1초 기다림
                        break;
                    case (3):
                        monsterspeed = 30;
                        animator.speed = 3;
                        break;
                    case (4):
                        break;
                }
                break;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (MonsterKeynum == 1)
            {

            }
            else if (gameObject.name == "MossyGiant")
            {
                isTracing = true; //isTracing == 추격하는 중
                animator.SetBool("isMoving", true);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (MonsterKeynum)
        {
            case (1):
                if (collision.tag == ("Player"))
                {
                    GetComponent<Collider2D>().enabled = false;
                    gameObject.SetActive(false);
                }
                break;
        }

    }

    //=============================================================================================

    private IEnumerator ChangeMovement()
    {
        if (angery == true)
        {
            animator.SetBool("AngryorNot", true);
            animator.SetBool("isMoving", true);
        }
        else
        {
            ActRage = UnityEngine.Random.Range(0, 3);
            movementFlag = UnityEngine.Random.Range(0, 3);
            if (movementFlag == 0)
            {
                animator.SetTrigger("StopMove");
                animator.SetBool("isMoving", false);

                if (ActRage == 0)
                {
                    animator.SetInteger("ChangeMovement", 0);
                }
                else if (ActRage == 1)
                {
                    animator.SetInteger("ChangeMovement", 1);
                }
                else if (ActRage == 2)
                {
                    animator.SetInteger("ChangeMovement", 2);
                }
            }
            else
            {
                animator.SetBool("isMoving", true);
            }
        }
        yield return new WaitForSeconds(3f);
        StartCoroutine(ChangeMovement());
    } //다양한 패턴의 애니매이터 조절을 위한 코루틴

    public IEnumerator BoostMonsterk() // 무적타임
    {
        int countTime = 0;
        while (countTime < 0.5f)
        {
            
            yield return new WaitForSeconds(0.5f);
            countTime++;
        }
      //  this.monsterspeed = 3;
        yield return null;
    }

    //=============================================================================================

    private void MonsterTRacing()
    {
        if (isTracing)
        {
            Vector3 playerPos = traceTarget.transform.position;
            if (playerPos.x < transform.position.x)
            {
                dist = "Left";
            }
            else if (playerPos.x > transform.position.x)
            {
                dist = "Right";
            }
        }
        else
        {
            if (movementFlag == 1)
            {
                dist = "Left";
            }
            else if (movementFlag == 2)
            {
                dist = "Right";
            }
        }
    }
    public void Move()
    {
        MonsterTRacing();
        transform.position += Vector3.left * monsterspeed * Time.deltaTime;
    }
  
    public override void Jump()
    {
        this.rigid.velocity = Vector2.up * 10;
    }
    private void MonsterCheck()
    {
        MonsterDataBaseScript data = GameObject.Find("GameManager").GetComponent<MonsterDataBaseScript>();
        
        for (int i = 0; i < data.AllItemList.Count; i++)
        {
            if (data.AllItemList[i].KeyNum == this.MonsterKeynum)
            {
               monsterspeed = data.AllItemList[i].Speed;
            }

        }
    }
    //=============================================================================================

}


