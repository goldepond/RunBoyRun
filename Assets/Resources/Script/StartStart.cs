using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System;

public class StartStart : MonoBehaviour
{
    //=========================================
    string StagePath;
    string AttributePath;
    string AudioPath;
    string equipmentPath;
    string goldPath;
    //=========================================
    private StageInformation stageInformation = new StageInformation();
    private AudioSavein AudioControl = new AudioSavein();
    private SlotNumCloth SlotNumCloth = new SlotNumCloth();
    private Gold gold = new Gold();
    private bool OnceATime;
    //=========================================
    public List<StageInformation> AllItemList;

    public TextAsset AttributeDatabase;
    public List<Attribute> AttributeList, MyitemList, CurItemList;
    // Start is called before the first frame update
    void Start()
    {

        StagePath = Application.persistentDataPath + "/StageInformation.txt";
        equipmentPath = Application.persistentDataPath + "/equipItem.txt";
        goldPath = Application.persistentDataPath + "/gold.txt";
        AudioPath = Application.persistentDataPath + "/Audio.txt";
        AttributePath = Application.persistentDataPath + "/MyAttribute.txt";
        string[] line = AttributeDatabase.text.Substring(0, AttributeDatabase.text.Length - 1).Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');
            AttributeList.Add(new Attribute(row[0], row[1], row[2], Convert.ToInt32(row[3]), row[4] == "TRUE"));
        }
        Load();
    }
    private void Save()
    {
        OnceATime = true;
        print("한번만 실행");
        stageInformation.stageNumber = 1;

        AudioControl.BackgroundSound = 1;
        AudioControl.ClickSound = 1;

        SlotNumCloth.charCloth = 0;

        SlotNumCloth.charHat = 4;

        gold.Cha_Gold = 0;


        string Stagejdata = JsonUtility.ToJson(stageInformation);
        byte[] Stagebytes = System.Text.Encoding.UTF8.GetBytes(Stagejdata);
        string StageCode = System.Convert.ToBase64String(Stagebytes);
        File.WriteAllText(StagePath, StageCode);

        string AudioJdata = JsonUtility.ToJson(AudioControl);
        File.WriteAllText(AudioPath, AudioJdata);

        string equipmentJdata = JsonUtility.ToJson(SlotNumCloth);
        byte[] equipmentByte = System.Text.Encoding.UTF8.GetBytes(equipmentJdata);
        string equipmentCode = System.Convert.ToBase64String(equipmentByte);
        File.WriteAllText(equipmentPath, equipmentCode);

        string GoldJdata = JsonUtility.ToJson(gold);
        byte[] GoldtByte = System.Text.Encoding.UTF8.GetBytes(GoldJdata);
        string GoldCode = System.Convert.ToBase64String(GoldtByte);
        File.WriteAllText(goldPath, GoldCode);

        string Attributejdata = JsonUtility.ToJson(new AttributeSeralization<Attribute>(AttributeList));
        byte[] AttributeBytes = System.Text.Encoding.UTF8.GetBytes(Attributejdata);
        string AttributeCode = Convert.ToBase64String(AttributeBytes);
        File.WriteAllText(AttributePath, AttributeCode);

    }

    private void Load()
    {
        {
            if (!File.Exists(StagePath)&& !File.Exists(equipmentPath)&& !OnceATime )
            {
                Save();
                return;
            }
            string StageCode = File.ReadAllText(StagePath);
            byte[] Stagebytes = System.Convert.FromBase64String(StageCode);
            string Stagejdata = System.Text.Encoding.UTF8.GetString(Stagebytes);
            stageInformation = JsonUtility.FromJson<StageInformation>(Stagejdata);
        }
    }
}
