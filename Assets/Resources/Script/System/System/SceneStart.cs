
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStart : MonoBehaviour
{
    public AudioSource audiodd;
    public AudioClip jumpSound;
    //=========================================
    public GameObject Store;
    public GameObject Character;
    public GameObject medal;
    public GameObject GameSrart;
    public GameObject GameSrartOn;
    public GameObject Preference;
    public GameObject Quit;
    //=========================================
    public static bool Starton = false;
    //=========================================
    string AudioPath;
    //=========================================
    private AudioSavein AudioControl = new AudioSavein();
    private void Start()
    {
        //=========================================
        AudioPath = Application.persistentDataPath + "/Audio.txt";
        //=========================================
        Load();
        this.audiodd = this.gameObject.AddComponent<AudioSource>();
        this.audiodd.clip = this.jumpSound;
        this.audiodd.volume = AudioControl.BackgroundSound;
        this.audiodd.Play();
        ActiveStart();
        
    }
    public void ActiveStart()
    {
        if (!Starton)
        {
            Store.SetActive(false);
            Character.SetActive(false);
            medal.SetActive(false);
            GameSrart.SetActive(true);
            GameSrartOn.SetActive(false);
            Quit.SetActive(false);
            //=========================================
            Starton = !Starton;
        }
        else
        {
            this.audiodd.Stop();
            Store.SetActive(true);
            Character.SetActive(true);
            medal.SetActive(true);
            GameSrartOn.SetActive(true);
            GameSrart.SetActive(false);
            Quit.SetActive(true);
            //=========================================
            Starton = true;
        }
    }

    private void Load()
    {
        if (!File.Exists(AudioPath))
        {
            AudioControl.BackgroundSound = 1;
            AudioControl.ClickSound = 1;
            AudioSave();
            return;
        }
        string AudioJdata = File.ReadAllText(AudioPath);
        AudioControl = JsonUtility.FromJson<AudioSavein>(AudioJdata);

    }

    public void quit()
    {
        Application.Quit();
    }

    public void AudioSave()
    {
        string AudioJdata = JsonUtility.ToJson(AudioControl);
        File.WriteAllText(AudioPath, AudioJdata);
    }
}
