using JetBrains.Annotations;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStartSceneChange : MonoBehaviour
{
    private bool Preferenceson = false;
    //=========================================
    private StageInformation stageInformation = new StageInformation();
    //=========================================
    public int stageNumber;
    //=========================================
    private AudioSource music1;
    private AudioSource audio;
    public AudioClip jumpSound;
    //=========================================
    string StagePath;
    //=========================================
    private void Start()
    {
        StagePath = Application.persistentDataPath + "/StageInformation.txt";
        //======================================
        Load();
        this.audio = this.gameObject.AddComponent<AudioSource>();
        this.audio.clip = this.jumpSound;
        //=========================================
    }
    public void StartButton()
    {
        SceneManager.LoadScene("gameselect"); 
    }
    //=========================================
    public void Stage1()
    {
            stageNumber = 1;
            //this.audio.Play();
            SceneManager.LoadScene("Scene_1");
    }
    public void Stage2()
    {
        if (stageInformation.stageNumber > 1)
        {
            stageNumber = 2;
            SceneManager.LoadScene("Scene_2");
        }
    }
    public void Stage3()
    {
        if (stageInformation.stageNumber > 2)
        {
            stageNumber = 3;
            SceneManager.LoadScene("Scene_3");
        }
    }
    public void Stage4()
    {
        if (stageInformation.stageNumber > 3)
        {
            stageNumber = 4;
            SceneManager.LoadScene("Scene_4");
        }
    }
    public void Stage5()
    {
        if (stageInformation.stageNumber > 4)
        {
            SceneManager.LoadScene("Scene_5");
        }
    }
    public void Stage6()
    {
        if (stageInformation.stageNumber > 5)
        {
            SceneManager.LoadScene("Scene_6");
        }
    }
    //=========================================
    public void CharacterButton()
    {
        SceneManager.LoadScene("CharacterScene");
        this.audio.Play();
    }
    public void AblityButton()
    {
        SceneManager.LoadScene("ablityScene");
        this.audio.Play();
    }
    public void BacktoMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void Guide()
    {
        SceneManager.LoadScene("GuideScene");
        this.audio.Play();
    }
    public void ActivePreferences()
    {
        if (!Preferenceson)
        {
        }
        else
        {
        }
        Preferenceson = !Preferenceson;
    }
    private void ResetItemClick()
    {
        stageInformation.stageNumber = 1;
        Save();
    }
    private void Load()
    {
        if (!File.Exists(StagePath))
        {
            ResetItemClick();
            return;
        }
        string StageCode = File.ReadAllText(StagePath);
        byte[] Stagebytes = System.Convert.FromBase64String(StageCode);
        string Stagejdata = System.Text.Encoding.UTF8.GetString(Stagebytes);
        stageInformation = JsonUtility.FromJson<StageInformation>(Stagejdata);
    }

    private void Save()
    {
        string Stagejdata = JsonUtility.ToJson(stageInformation);
        byte[] Stagebytes = System.Text.Encoding.UTF8.GetBytes(Stagejdata);
        string StageCode = System.Convert.ToBase64String(Stagebytes);
        File.WriteAllText(StagePath, StageCode);
    }
}
