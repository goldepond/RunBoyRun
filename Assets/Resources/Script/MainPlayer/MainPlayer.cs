
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.Video;
using System;

///=============================================================================================
// 주인공의 이동과 점프를 구현한 스크립트 이다.
///=============================================================================================
public partial class MainPlayer : MonoBehaviour  // 주인공의 이동과 점프를 구현한 스크립트 이다.
{
    private float MAxhpd;
    private float MaxJumpCount;
    private float JumpPower;
    public float hp;
    public float checkRadius;  // 발바닥 크기
    ///=============================================================================================
    public Image curHealthBar;
    ///=============================================================================================
    public int Score;
    public static int JumpCount;
    private const string UnBeatTimeON = "UnBeatTime";
    ///=============================================================================================
    public Transform groundcheck; // 발바닥 생성
    ///=============================================================================================
    public LayerMask whatIsGround; // 무엇을 바닥으로 설정할것인가.
    ///=============================================================================================
    public bool isGrounded;   // 땅의 유무를 체크하는 변수
    public bool IsUnbeatTime;//피격뒤 잠시 무적.
    public bool stop;
    public bool notdead = true;
    public bool isDie = false;
    private bool undead;
    public bool DoubleJump;

    public bool GoalClear;
    ///=============================================================================================
    private CameraShakeBB cameraShakeBB;
    private Rigidbody2D rigid;
    private SpriteRenderer renderer;
    private SlotNumCloth SlotNumCloth;
    private Gold gold;
    private Audio audio;

    private AudioSource MaineoCRSound;
    private AudioSavein AudioControl = new AudioSavein();

    public AudioClip damage;
    public AudioClip JumpSound;
    ///=============================================================================================
    public Text ScoreTextInGame;
    public Text HPShow;
    public Text jumpCountText;
    public Text goldShow;
    ///=============================================================================================
    private static int boostTime;
    private static int JumpBoostTime;
    private static int unbeatTime;
    //=========================================
    private int startundeadTime;
    private int startBoostTime;
    private int startjumpPack;
    private int startUnbeatTime;
    private int Gold;
    //=========================================
    private float startgravityScale;
    private int startgoalCutLine;
    private int startJumpDouble;
    private int startMsxhpPlus;
    private int startJumpCountUP;
    //=========================================
    string goldPath;
    string ItemPath;
    string AttributePath;
    string AudioPath;

    public List<Attribute> MyAttributeList;
    ///============================================================================================ 
    private void StartSetting()
    {

        goldPath = Application.persistentDataPath + "/gold.txt";
        ItemPath = Application.persistentDataPath + "/equipItem.txt";
        AttributePath = Application.persistentDataPath + "/MyAttribute.txt";
        AudioPath = Application.persistentDataPath + "/Audio.txt";


        MAxhpd = 9000 + startMsxhpPlus; JumpPower = 11; MAxhpd = hp;
        curHealthBar.fillAmount = hp / hp;
        rigid = gameObject.GetComponent<Rigidbody2D>();// 물리엔진 사용
        renderer = gameObject.GetComponentInChildren<SpriteRenderer>();//스프라이트렌더러 사용
        cameraShakeBB = GameObject.FindWithTag("MainCamera").GetComponent<CameraShakeBB>();
        audio = FindObjectOfType<Audio>();

        this.MaineoCRSound = this.gameObject.AddComponent<AudioSource>();

    }
    private void Load()
    {
        if (!File.Exists(goldPath) && !File.Exists(ItemPath))
        {
            return;
        }
        string GoldCode = File.ReadAllText(goldPath);
        byte[] GoldtByte = System.Convert.FromBase64String(GoldCode);
        string GoldJdata = System.Text.Encoding.UTF8.GetString(GoldtByte);
        gold = JsonUtility.FromJson<Gold>(GoldJdata);

        string itemCode = File.ReadAllText(ItemPath);
        byte[] itembytes = System.Convert.FromBase64String(itemCode);
        string itemjdata = System.Text.Encoding.UTF8.GetString(itembytes);
        SlotNumCloth = JsonUtility.FromJson<SlotNumCloth>(itemjdata);

        string AudioJdata = File.ReadAllText(AudioPath);
        AudioControl = JsonUtility.FromJson<AudioSavein>(AudioJdata);

        if (File.Exists(AttributePath))
        {
            string AttributeCode = File.ReadAllText(AttributePath);
            byte[] AttributeBytes = System.Convert.FromBase64String(AttributeCode);
            string Attributejdata = System.Text.Encoding.UTF8.GetString(AttributeBytes);
            MyAttributeList = JsonUtility.FromJson<AttributeSeralization<Attribute>>(Attributejdata).target;
        }

    }
    private void Awake()
    {
        StartSetting();
        Load();
        StartiTem();
        if (File.Exists(AttributePath))
        {
            AttributeUpgrade();
        }
        gravity();
    }
    private void Update()
    {
        Jump();
        GroundCheck();
        HP();
        ScoreBoard();
    }
    ///============================================================================================ 
    private void OnTriggerEnter2D(Collider2D col) //플레이어와 충돌했을때를 위한 함수
    {
        switch (col.gameObject.tag)
        {
            case ("PlayerIsDead"): // Dead테그의 물체와 충돌했다면...
                if (!IsUnbeatTime)
                {
                    this.MaineoCRSound.volume = AudioControl.ClickSound;
                    this.MaineoCRSound.clip = this.damage;
                    this.MaineoCRSound.Play();
                    cameraShakeBB.ViberateForTime(0.1f);
                    _ = Vector2.zero;
                    _ = new Vector2(-5f, 5f);
                    rigid.AddForce(Vector2.zero, ForceMode2D.Impulse);
                    hp -= 1000;
                    if (hp > 0)
                    {
                        IsUnbeatTime = true;
                        StartCoroutine("UnBeatTime");
                    }
                }
                break;
            case ("DoubleJump"): //Jumprocket테그의 아이템을 먹었다면...
                gold.Cha_Gold++;
                JumpCount ++; // 2단 점프!
                break;

            case ("Coin"): //코인에 먹었다
                break;

            case ("AutoJump"): //자동으로 점프

                gold.Cha_Gold++;
                rigid.velocity = Vector2.up * JumpPower;
                break;

            case ("PlayerDie"): //w죽음
                Die();
                break;
            case ("Goal"): //w죽음

                GoalClear = true;
                audio.GoalGoal();
                Die();
                break;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch ((collision.gameObject.tag))
        {
            case ("Monster"): // Monster테그의 물체와 충돌했다면...
                gold.Cha_Gold++;
                rigid.velocity = Vector2.up * 12;
                JumpCount++;
                break;
        }
    }
    ///============================================================================================ 
    public IEnumerator UnBeatTime() // 무적타임
    {
        int countTime = 0;
        while (countTime < unbeatTime)
        {
            stop = true;
            yield return new WaitForSeconds(0.5f);
            countTime++;
        }
        IsUnbeatTime = false;
        stop = false;
        yield return null;
    }
    public IEnumerator Boost() // 무적타임
    {
        IsUnbeatTime = true;
        int countTime = 0;
        while (countTime < boostTime)
        {
            stop = true;
            Time.timeScale = 4;
            yield return new WaitForSeconds(0.5f);
            countTime++;
        }
        IsUnbeatTime = false;
        stop = false;
        Time.timeScale = 1;
        yield return null;
    }
    public IEnumerator Jumppack() // 무적타임
    {
        int countTime = 0;
        while (countTime < JumpBoostTime)
        {
            JumpCount = 100;
            yield return new WaitForSeconds(0.5f);
            countTime++;
        }
        JumpCount = 1;
        yield return null;
    }
    ///============================================================================================ 
    private void OnBecameInvisible()
    {
        //화면밖으로 나가 보이지 않게 되면 호출이 된다. 
        // 어딘가에 걸려 넘어져 카메라의 속도에 반응하지 못하고 뒤쳐지면 게임 오버!.
        Die();
    }
    ///============================================================================================ 
    public static void RestartStage()
    {
        Time.timeScale   = 0f;
    }
    private void ScoreBoard()
    {
        if (!isDie)
        {
            Score++;
        }
        ScoreTextInGame.text = "Score : " + Score;
        jumpCountText.text = "Jump : " + JumpCount;
        goldShow.text = "Gold : " + gold.Cha_Gold;
        HPShow.text = "HP : " + hp;
    }
    private void SaveGold()
    {
        string GoldJdata = JsonUtility.ToJson(gold);
        byte[] GoldtByte = System.Text.Encoding.UTF8.GetBytes(GoldJdata);
        string GoldCode = System.Convert.ToBase64String(GoldtByte);
        File.WriteAllText(goldPath, GoldCode);
        Load();
    }
    ///============================================================================================ 
    ///============================================================================================ 
    ///============================================================================================ 
    ///============================================================================================ 
    ///============================================================================================ 
    private void Jump() // 플레이어가 점프하는 것을 구현한 함수
    {
        isGrounded = Physics2D.OverlapCircle(groundcheck.position, checkRadius, whatIsGround);
        // ㄴPhysics2D.OverlapCircle( groundcheck의 위치, groundcheck의 반경, whatIsGround에 들어가는 녀석에 반응) 
        if (Input.touchCount > 0 && notdead && JumpCount > 0)//Input.touchCount > 0
        {
            this.MaineoCRSound.volume = (AudioControl.ClickSound/10);
            print(this.MaineoCRSound.volume);
            this.MaineoCRSound.clip = this.JumpSound;
            this.MaineoCRSound.Play();
            if (Input.touchCount > 0 && JumpCount > 0)//Input.touchCount > 0 // Input.GetKeyDown(KeyCode.UpArrow) 
            {
                //만약 플레이어가 위키를 누루고 있으면서 JumpCount가 0초과 라면
                rigid.velocity = Vector2.up * (JumpPower);// 위로 점프! && JumpCount > 0
                DoubleJump = true;
                JumpCount--;
            }
            else
            {
                rigid.velocity = Vector2.up * JumpPower;
            }

        }
    }


    public void MainLoad()
    {
        Load();
    }
    private void GroundCheck()
    {
        if (isGrounded == true)// 만약 isGrounded의 스위치가 켜졌다면(
        {
            DoubleJump = false;
            if (JumpCount <= 0)
            {
                JumpCount += 1+startJumpDouble;
            }
        }
    }
    public void Die()
    {
        SaveGold();
        isDie = true;
        ///===============================================
        SpriteRenderer renderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        BoxCollider2D coll = gameObject.GetComponent<BoxCollider2D>();
        Rigidbody2D rigd = gameObject.GetComponent<Rigidbody2D>();
        ///===============================================
        renderer.flipY = true;// Y축을 뒤집습니다.
        coll.enabled = false; //
        rigid.velocity = Vector2.up * (JumpPower);
    }
    private void HP()
    {
        curHealthBar.fillAmount = ((hp - 1) / (MAxhpd));
        if (Time.deltaTime == 0) { }
        else
        {
            hp--;
        }
        //  Debug.Log(hp);
        if (hp < 0 && !isDie && !undead)
        {
            Die();
        }
        else if (hp > MAxhpd)
        {
            hp = MAxhpd;
        }
        else if (hp < 0 && undead)
        {
            hp += (((MAxhpd/2) + startundeadTime));
            unbeat(5);
            undead = false;
        }

        if (this.gameObject.transform.position.y < -200)
        {
            Die();
        }
    }
    public void autoJump()
    {
        rigid.velocity = Vector2.up * (JumpPower);// 위로 점프! && JumpCount > 0
    }
    ///============================================================================================ 
    ///============================================================================================ 
    public void JumpCountUP(int i)
    {
        JumpCount = +i;
    }//1 점프 횟수 증가
    public void BoostItem(int i)
    {
        boostTime = 10 + i;
        StartCoroutine("Boost");

    }//2 돌진 
    public void JumpJump100(int i)
    {
        StartCoroutine("Jumppack");
        JumpBoostTime = 5 + i;

    }//3 잠시동안 무한 점프
    public void Case01(int i)
    {
        hp += (500 + (i*100));
    }//4 소소한 체력 증가 
    public void Case02(int i)
    {
        hp += (1000 + (i * 500));
    }//5 대단한 체력 증가 
    public void unbeat(int i)
    {
        StartCoroutine("UnBeatTime");
        unbeatTime = 3 + i;
    }//6 무적 타임
    public void JumpOnce(int i)
    {
        rigid.velocity = Vector2.up * i *10;// 위로 점프! && JumpCount > 0
    }//7 즉시 점프 
    public void GetGold(int i)
    {
        gold.Cha_Gold = gold.Cha_Gold + i;
    } // 골드 증가 

    ///============================================================================================ 
    ///============================================================================================ 
    private void StartiTem()
    {
        switch (SlotNumCloth.charHat)
        {
            case (4):
                JumpCountUP(10);
                break;
            case (5):
                //"부활"
                undead = true; // 부활 
                break;
            case (6):
                BoostItem(startBoostTime); //돌진
                //"SPeedBoost"
                break;
            case (7):
                JumpJump100(startUnbeatTime); //무한점프 
                //"JumpPack"
                break;
            case (8):
                JumpOnce(startjumpPack);
                break;
        }
    }
    private void gravity()
    {
        //rigid.gravityScale = startgravityScale;
    }

    ///============================================================================================ 
    private void AttributeUpgrade()
    {
        startUnbeatTime = MyAttributeList[0].Number;//1 d 
        startgoalCutLine = MyAttributeList[1].Number;//2
        startgravityScale = MyAttributeList[2].Number * 0.9f; //3 d 
        startundeadTime = MyAttributeList[3].Number; //4d
        startMsxhpPlus = MyAttributeList[4].Number;//5d
        startBoostTime = MyAttributeList[5].Number; //6 d 
        startjumpPack = MyAttributeList[6].Number; //7 d 
        startJumpDouble = (MyAttributeList[7].Number / 3);//8d
    }

    ///============================================================================================ 
}
public class Gold
{
    public int Cha_Gold;
}



