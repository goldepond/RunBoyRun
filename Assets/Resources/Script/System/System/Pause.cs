
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    //=========================================
    private bool pauseon;
    private bool Preferenceson = false;
    private bool CopyrightOn = false;
    private bool onceaTime;
    //=========================================
    public GameObject normalPanel;
    public GameObject pausePanel;
    public GameObject Preferences;
    public GameObject GameOver;
    public GameObject StageClear;
    public GameObject CopyRight;
    public GameObject[] MainPlayerTT;
    public GameObject newitem;
    //=========================================
    private readonly int a = 1;
    private readonly GameObject MainChar;
    //=========================================
    private Goal goal;
    private SlotNumCloth SlotNumCloth = new SlotNumCloth();
    private MainPlayer mainPlayer;
    private GameObject mainP;
    //=========================================
    private int checkerPlayer;
    //=========================================
    private AudioSource audio;
    private AudioSavein AudioControl = new AudioSavein();
    private Audio audioScript;
    public AudioClip jumpSound;
    //=========================================
    public Text ScoreTextInGame;
    //=========================================
    string AudioPath;
    string equipmentPath;
    string StagePath;
    //=========================================
    private StageInformation stageInformation;
    private GameStartSceneChange gameStartScene;


    private void Start()
    {
        //=========================================
        StagePath = Application.persistentDataPath + "/StageInformation.txt";
        AudioPath = Application.persistentDataPath + "/Audio.txt";
        equipmentPath = Application.persistentDataPath + "/equipItem.txt";

        //=========================================
        audioScript = GameObject.FindWithTag("UI").GetComponent<Audio>();
        gameStartScene = GameObject.FindWithTag("UI").GetComponent<GameStartSceneChange>();
        mainPlayer = GameObject.FindWithTag("Player").GetComponent<MainPlayer>();
        mainP = GameObject.FindWithTag("Player");
        this.audio = this.gameObject.AddComponent<AudioSource>();
        //=========================================

        //=========================================
        onceaTime = false;
       pauseon = false;

        Load();
        Time.timeScale = 1.0f;
        CharSelect();
        GameOver.SetActive(false);
        //=========================================
        this.audio.clip = jumpSound;

    }
    private void Update()
    {
        EndGame();
    }
    private void CharSelect()
    {
        for (int i = 0; i < 10; i++)
        {
            if (SlotNumCloth.charCloth == i)
            {
                checkerPlayer = i;
                MainPlayerTT[i].SetActive(true);
                break;
            }
        }
    }

    private void ResetItemClick()
    {
        SlotNumCloth.charCloth = 0;
        SlotNumCloth.charHat = 4;

        string equipmentJdata = JsonUtility.ToJson(SlotNumCloth);
        byte[] equipmentByte = System.Text.Encoding.UTF8.GetBytes(equipmentJdata);
        string equipmentCode = System.Convert.ToBase64String(equipmentByte);
        File.WriteAllText(equipmentPath, equipmentCode);
        Load();
    }

    public void PauseLoad()
    {
        string AudioJdata = File.ReadAllText(AudioPath);
        AudioControl = JsonUtility.FromJson<AudioSavein>(AudioJdata);
        this.audio.volume = AudioControl.ClickSound;

    }
    private void Load()
    {
        if ( !File.Exists(equipmentPath)) { ResetItemClick(); return; }
        string AudioJdata = File.ReadAllText(AudioPath);
        AudioControl = JsonUtility.FromJson<AudioSavein>(AudioJdata);

        string StageCode = File.ReadAllText(StagePath);
        byte[] Stagebytes = System.Convert.FromBase64String(StageCode);
        string Stagejdata = System.Text.Encoding.UTF8.GetString(Stagebytes);
        stageInformation = JsonUtility.FromJson<StageInformation>(Stagejdata);

        string equipmentCode = File.ReadAllText(equipmentPath);
        byte[] equipmentBytes = System.Convert.FromBase64String(equipmentCode);
        string equipmentJdata = System.Text.Encoding.UTF8.GetString(equipmentBytes);
        SlotNumCloth = JsonUtility.FromJson<SlotNumCloth>(equipmentJdata);
        this.audio.volume = AudioControl.ClickSound;
    }
    public void Activepause()
    {
         this.audio.Play();
        if (!pauseon)
        {
            Time.timeScale = 0;
            pausePanel.SetActive(true);
            normalPanel.SetActive(false);
            mainP.SetActive(false);

        }
        else
        {
            Time.timeScale = 1.0f;
            pausePanel.SetActive(false);
            normalPanel.SetActive(true);
            mainP.SetActive(true);
        }
       pauseon = !pauseon;
    }
    public void ActivePreferences()
    {
       this.audio.Play();
        if (!Preferenceson)
        {
            pausePanel.SetActive(false);
            Preferences.SetActive(true);
        }
        else
        {
            pausePanel.SetActive(true);
            Preferences.SetActive(false);
        }
        Preferenceson = !Preferenceson;
    }

    public void ActiveCopyRight()
    {
        this.audio.Play();
        if (!CopyrightOn)
        {
            pausePanel.SetActive(false);
            CopyRight.SetActive(true);
        }
        else
        {
            pausePanel.SetActive(true);
            CopyRight.SetActive(false);
        }
        CopyrightOn = !CopyrightOn;
    }
    public void EndGame()
    {
        if (mainPlayer.isDie)
        {
            ScoreTextInGame.text = "" + mainPlayer.Score;
            GameOver.SetActive(true);
            audioScript.EndMusic();
            if (mainPlayer.GoalClear && !onceaTime)
            {
                onceaTime = true;
                StageClear.SetActive(true);
                if (gameStartScene.stageNumber + 1 > stageInformation.stageNumber)
                {
                    stageInformation.stageNumber++;
                    newitem.SetActive(true);
                    Save();
                }

            }
        }
    }

    private void Save()
    {
        string Stagejdata = JsonUtility.ToJson(stageInformation);
        byte[] Stagebytes = System.Text.Encoding.UTF8.GetBytes(Stagejdata);
        string StageCode = System.Convert.ToBase64String(Stagebytes);
        File.WriteAllText(StagePath, StageCode);
    }

}
