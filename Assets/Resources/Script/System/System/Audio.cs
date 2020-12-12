
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class AudioSavein
{
    public float BackgroundSound;
    public float ClickSound;
}
public class Audio : MonoBehaviour
{
    string AudioPath;
    //=========================================
    public Slider backVolume;
    public Slider SystemVolume;
    //=========================================
    public Text BackGroundSound;
    public Text ClickSound;
    public TextAsset AudioSaveScript;
    //=========================================
    private GameStartSceneChange GameStartSceneChange;
    //=========================================
    private MainPlayer goald;
    private Pause pause;
    //=========================================
    public AudioClip badEndSound; 
    public AudioClip GoodEndSound;
    public AudioClip music1;
    public AudioClip music2;
    public AudioClip music3;
    public AudioClip music4;
    //=========================================
    private AudioSavein AudioControl;
    private AudioSource audio;

    private bool onetime;
    void Awake()
    {
        onetime = true;
        //=========================================
        AudioPath = Application.persistentDataPath + "/Audio.txt";
        //=======================================================
        pause = GameObject.FindWithTag("UI").GetComponent<Pause>();
        goald  = GameObject.FindWithTag("Player").GetComponent<MainPlayer>();
        GameStartSceneChange = FindObjectOfType<GameStartSceneChange>();
        this.audio = this.gameObject.AddComponent<AudioSource>();
        Load();
        goald.GoalClear = false;
        BackMusic();
    }
    void Update()
    {
        BackGroundSound.text = "" + (Math.Truncate(backVolume.value*100)/100 );
        ClickSound.text = "" + (Math.Truncate(SystemVolume.value* 100) / 100);
        SoundSliderBack();
        SoundSliderClick();
    }
    public void AudioSave()
    {
        print(audio.volume);
         audio.volume = backVolume.value;
         AudioControl.BackgroundSound = backVolume.value;

        SystemVolume.value = AudioControl.ClickSound;
        string AudioJdata = JsonUtility.ToJson(AudioControl);
        File.WriteAllText(AudioPath, AudioJdata);
        Load();
        goald.MainLoad();
        pause.PauseLoad();
        backVolume.value = AudioControl.BackgroundSound;
    }
    private void Load()
    {
        if (!File.Exists(AudioPath))
        {
            AudioControl.BackgroundSound =1;
            AudioControl.ClickSound = 1;
            AudioSave();
            return;
        }
         string AudioJdata = File.ReadAllText(AudioPath);
        AudioControl = JsonUtility.FromJson<AudioSavein>(AudioJdata);
         
          this.audio.volume = AudioControl.BackgroundSound;
          backVolume.value = AudioControl.BackgroundSound;
        SystemVolume.value = AudioControl.ClickSound;
    }
    public void SoundSliderBack()
    {
         
    }
    public void SoundSliderClick()
    {
        AudioControl.ClickSound = SystemVolume.value;
    }
    private void BackMusic()
    {
        switch(GameStartSceneChange.stageNumber)
        {
            case (1):
                this.audio.clip = this.music1;
                this.audio.Play();
                break;
            case (2):
                this.audio.clip = this.music1;
                this.audio.Play();
                break;
            case (3):
                this.audio.clip = this.music2;
                 this.audio.Play();
                break;
            case (4):
                this.audio.clip = this.music2;
                this.audio.Play();
                break;
            case (5):
                this.audio.clip = this.music3;
                this.audio.Play();
                break;
            case (6):
                this.audio.clip = this.music4;
                this.audio.Play();
                break;


        }
    }
    public void EndMusic()
    {
        if(onetime)
        {
            onetime = false;
            if (goald.GoalClear)
            {
                this.audio.clip = this.GoodEndSound;
                this.audio.Play();
            }
            else
            {
                this.audio.clip = this.badEndSound;
                this.audio.Play();
            }
        }

    }

    public void GoalGoal()
    {
        goald.GoalClear = true;
    }

}
