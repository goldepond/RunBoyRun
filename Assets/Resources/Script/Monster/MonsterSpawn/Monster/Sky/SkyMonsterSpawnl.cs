
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

//-----------------------------------------------------------------------------------------
// 이것은 주인공이 발사하는 직선형 스킬을 구현한 스크립트 이다
//-----------------------------------------------------------------------------------------
public class SkyMonsterSpawnl : MonoBehaviour  // 이것은 주인공이 발사하는 직선형 스킬을 구현한 스크립트 이다.
{
    ///============================================================================================


    public GameObject[] Monster;    // 복제할 미사일 오브젝트
    ///=============================================================================================
    public Transform[] Sky_Location;   // 미사일이 발사될 위치   // 미사일이 발사될 위치
    public float MapDelay = 4;         // 미사일 발사 속도(미사일이 날라가는 속도x)

    ///=============================================================================================
    private bool FireState;              // 미사일 발사  제어할 변수
    ///=============================================================================================
    ///=============================================================================================
    public int MissileMaxPool;          // 메모리 풀에 저장할 미사일 개수
    private MonsterMemoryPool MPool;           // 메모리 풀
    private GameObject[] MissileArray;  // 메모리 풀과 연동하여 사용할 미사일 배열
    ///=============================================================================================
    //private readonly int MissileSpeed = 3;
    ///=============================================================================================
    protected MainPlayer mainPlayer;

    protected SkillMove skillMove;

    public List<Item> AllItemList, MyitemList, CurItemList;
    ///============================================================================================= 
    private void Start()
    {

        FireState = true;
        // 처음에 미사일을 발사할 수 있도록 제어변수를 true로 설정
        // 메모리 풀을 초기화합니다.
        MPool = new MonsterMemoryPool();
        // PlayerMissile을 MissileMaxPool만큼 생성합니다.
        MPool.Create(Monster, MissileMaxPool);
        // 배열도 초기화 합니다.(이때 모든 값은 null이 됩니다.)
        MissileArray = new GameObject[MissileMaxPool];

        mainPlayer = FindObjectOfType<MainPlayer>();
        skillMove = FindObjectOfType<SkillMove>();
    }

    private void Update()
    {
        // 매 프레임마다 미사일발사 함수를 체크한다.

        PlayerFire();
    }
    // 미사일을 발사하는 함수
    private void PlayerFire()
    {
        // 제어변수가 true일때만 발동
        if (FireState)
        {
            StartCoroutine(FireCycleControl());

            // 키보드의 "A"를 누르면
            // 코루틴 "FireCycleControl"이 실행되며
            // 미사일 풀에서 발사되지 않은 미사일을 찾아서 발사합니다.
            for (int i = 0; i < MissileMaxPool; i++)
            {
                // 만약 미사일배열[i]가 비어있다면
                if (MissileArray[i] == null)
                {
                    // 메모리풀에서 미사일을 가져온다.
                    MissileArray[i] = MPool.NewItem();
                    // 해당 미사일의 위치를 미사일 발사지점으로 맞춘다.

                    MissileArray[i].transform.position = Sky_Location[Random.Range(0, Sky_Location.Length)].transform.position;
                    // 발사 후에 for문을 바로 빠져나간다.
                    break;
                }
            }

        }

        // 미사일이 발사될때마다 미사일을 메모리풀로 돌려보내는 것을 체크한다.
        for (int i = 0; i < MissileMaxPool; i++)
        {
            // 만약 미사일[i]가 활성화 되어있다면
            if (MissileArray[i])
            {
                // 미사일[i]의 Collider2D가 비활성 되었다면
                if (MissileArray[i].GetComponent<Collider2D>().enabled == false)
                {
                    // 다시 Collider2D를 활성화 시키고
                    MissileArray[i].GetComponent<Collider2D>().enabled = true;
                    // 미사일을 메모리로 돌려보내고
                    MPool.RemoveItem(MissileArray[i]);
                    // 가리키는 배열의 해당 항목도 null(값 없음)로 만든다.
                    MissileArray[i] = null;
                }
            }
        }
    }

    // 코루틴 함수
    private IEnumerator FireCycleControl()
    {
        // 처음에 FireState를 false로 만들고
        FireState = false;
        // FireDelay초 후에
        //GameObject Monster =  MonsterAll [Random.Range(0, 5)];
        yield return new WaitForSeconds(MapDelay);
        // FireState를 true로 만든다.
        FireState = true;
        // FireDelay초 후에
        // FireState를 true로 만든다.
    }

}
