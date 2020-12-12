using System.Collections;
using UnityEngine;

//-----------------------------------------------------------------------------------------
// �̰��� ���ΰ��� �߻��ϴ� ������ ��ų�� ������ ��ũ��Ʈ �̴�
//-----------------------------------------------------------------------------------------
public class FruitSpawnl : MonoBehaviour  // �̰��� ���ΰ��� �߻��ϴ� ������ ��ų�� ������ ��ũ��Ʈ �̴�.
{
    public GameObject[] Monster;    // ������ �̻��� ������Ʈ
    ///=============================================================================================
    
    public Transform[] MapLocation;   // �̻����� �߻�� ��ġ
    public float MapDelay = 4;         // �̻��� �߻� �ӵ�(�̻����� ���󰡴� �ӵ�x)

    ///=============================================================================================
    private bool FireState;              // �̻��� �߻�  ������ ����
    ///=============================================================================================
    ///=============================================================================================
    public int MissileMaxPool;          // �޸� Ǯ�� ������ �̻��� ����
    private FruitMemoryPool MPool;           // �޸� Ǯ
    private GameObject[] MissileArray;  // �޸� Ǯ�� �����Ͽ� ����� �̻��� �迭
    ///=============================================================================================
    private readonly int MissileSpeed = 3;

    ///=============================================================================================
    ///
    private MainPlayer mainPlayer;
    private SkillMove skillMove;
    ///============================================================================================= 
    ///
    private void OnApplicationQuit()
    {
        // �޸� Ǯ�� ���ϴ�.
        MPool.Dispose();
    }

    private void Start()
    {
        FireState = true;
        // ó���� �̻����� �߻��� �� �ֵ��� ������� true�� ����
        // �޸� Ǯ�� �ʱ�ȭ�մϴ�.
        MPool = new FruitMemoryPool();
        // PlayerMissile�� MissileMaxPool��ŭ �����մϴ�.
        MPool.Create(Monster, MissileMaxPool);
        // �迭�� �ʱ�ȭ �մϴ�.(�̶� ��� ���� null�� �˴ϴ�.)
        MissileArray = new GameObject[MissileMaxPool];

        mainPlayer = FindObjectOfType<MainPlayer>();
        skillMove = FindObjectOfType<SkillMove>();

    }

    private void Update()
    {
        // �� �����Ӹ��� �̻��Ϲ߻� �Լ��� üũ�Ѵ�.
        PlayerFire();
    }

    // �̻����� �߻��ϴ� �Լ�
    private void PlayerFire()
    {
        // ������� true�϶��� �ߵ�
        if (FireState)
        {
            StartCoroutine(FireCycleControl());
            // �ڷ�ƾ "FireCycleControl"�� ����Ǹ�
            // �̻��� Ǯ���� �߻���� ���� �̻����� ã�Ƽ� �߻��մϴ�.
            for (int i = 0; i < MissileMaxPool; i++)
            {
                // ���� �̻��Ϲ迭[i]�� ����ִٸ�
                if (MissileArray[i] == null)
                {
                    // �޸�Ǯ���� �̻����� �����´�.
                    MissileArray[i] = MPool.NewItem();
                    // �ش� �̻����� ��ġ�� �̻��� �߻��������� �����.

                    MissileArray[i].transform.position = MapLocation[Random.Range(0, MapLocation.Length)].transform.position;
                    // �߻� �Ŀ� for���� �ٷ� ����������.
                    break;
                }
            }

        }

        // �̻����� �߻�ɶ����� �̻����� �޸�Ǯ�� ���������� ���� üũ�Ѵ�.
        for (int i = 0; i < MissileMaxPool; i++)
        {
            // ���� �̻���[i]�� Ȱ��ȭ �Ǿ��ִٸ�
            if (MissileArray[i])
            {
                // �̻���[i]�� Collider2D�� ��Ȱ�� �Ǿ��ٸ�
                if (MissileArray[i].GetComponent<Collider2D>().enabled == false)
                {
                    // �ٽ� Collider2D�� Ȱ��ȭ ��Ű��
                    MissileArray[i].GetComponent<Collider2D>().enabled = true;
                    // �̻����� �޸𸮷� ����������
                    MPool.RemoveItem(MissileArray[i]);
                    // ����Ű�� �迭�� �ش� �׸� null(�� ����)�� �����.
                    MissileArray[i] = null;
                }
            }
        }
    }

    // �ڷ�ƾ �Լ�
    private IEnumerator FireCycleControl()
    {
        // ó���� FireState�� false�� �����
        FireState = false;
        // FireDelay�� �Ŀ�
        //GameObject Monster =  MonsterAll [Random.Range(0, 5)];
        yield return new WaitForSeconds(MapDelay);
        // FireState�� true�� �����.
        FireState = true;
        // FireDelay�� �Ŀ�
        // FireState�� true�� �����.
    }

}