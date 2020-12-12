using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class ItemEffect : MonoBehaviour
{
    public int itemID;
    //=========================================
    string AttributePath;
    string AudioPath;
    //=========================================
    private Transform trans;
    //=========================================
    private Rigidbody2D thisRb;
    //=========================================
    public List<Attribute> MyitemList;
    //=========================================
    public SpriteRenderer renderer;
    //=========================================
    private AudioSource audio;
    private AudioSavein AudioControl = new AudioSavein();
    //=========================================
    private void Awake()
    {
        //=========================================
        AttributePath = Application.persistentDataPath + "/MyAttribute.txt";
        AudioPath = Application.persistentDataPath + "/Audio.txt";
        //=========================================
        trans = transform;
        thisRb = trans.GetComponent<Rigidbody2D>();
        this.audio = this.gameObject.GetComponentInChildren<AudioSource>();
        Load();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        MainPlayer mainPlayer = GameObject.Find("Mainplayer").GetComponent<MainPlayer>();
        switch (collision.gameObject.tag)
        {
            case ("Player"):
                {
                    this.audio.volume = AudioControl.ClickSound;
                    this.audio.Play();
                    renderer.enabled = false;
                    switch (itemID)
                    {
                        case (1):
                            mainPlayer.JumpCountUP(MyitemList[0].Number);
                            //점프 횟수 증가 
                            break;
                        case (2):
                            mainPlayer.BoostItem(MyitemList[1].Number);
                            //돌진
                            break;
                        case (3):
                            mainPlayer.JumpJump100(MyitemList[2].Number);
                            //무한 점프
                            break;
                        case (4):
                            mainPlayer.Case01(MyitemList[3].Number);
                            //소소한 체력증가
                            break;
                        case (5):
                            mainPlayer.Case02(MyitemList[4].Number);
                            //대단한 체력 증가 
                            break;
                        case (6):
                            mainPlayer.unbeat(MyitemList[5].Number);
                            //무적시간 
                            break;
                        case (7):
                            mainPlayer.JumpOnce(MyitemList[6].Number);
                            //즉시 점프
                            break;
                        case (8):
                            mainPlayer.GetGold((MyitemList[7].Number));
                            //골드 증가
                            break;
                    }
                    StartCoroutine("CountTime");
                }
                break;
        }
    }
    IEnumerator CountTime()
    {
        yield return new WaitForSeconds(1);
        GetComponent<SpriteRenderer>().enabled = true;
    }
    void Load()
    {
        if (!File.Exists(AudioPath) || !File.Exists(AttributePath))
        {
            AudioControl.BackgroundSound = 0;
            AudioControl.ClickSound = 0;
           
            return;
        }
        string AttributeCode = File.ReadAllText(AttributePath);
        byte[] AttributeBytes = System.Convert.FromBase64String(AttributeCode);
        string Attributejdata = System.Text.Encoding.UTF8.GetString(AttributeBytes);
        MyitemList = JsonUtility.FromJson<AttributeSeralization<Attribute>>(Attributejdata).target;


        string AudioJdata = File.ReadAllText(AudioPath);
        AudioControl = JsonUtility.FromJson<AudioSavein>(AudioJdata);
    }

    public void MainLoad()
    {
        Load();
    }
    void Save()
    {
        string Attributejdata = JsonUtility.ToJson(new AttributeSeralization<Attribute>(MyitemList));
        byte[] AttributeBytes = System.Text.Encoding.UTF8.GetBytes(Attributejdata);
        string AttributeCode = Convert.ToBase64String(AttributeBytes);
        File.WriteAllText(AttributePath, AttributeCode);

        string AudioJdata = JsonUtility.ToJson(AudioControl);
        File.WriteAllText(AudioPath, AudioJdata);
    }
    private void Update()
    {
        this.audio.volume = AudioControl.ClickSound;
    }
}