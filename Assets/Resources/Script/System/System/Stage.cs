
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class StageInformation
{
    public int stageNumber=99;
}
public class Stage : MonoBehaviour
{
    //=========================================
    string StagePath;
    //=========================================
    public Image[]ItemImage;
    //=========================================
    private StageInformation stageInformation = new StageInformation();
    //=========================================
    public List<StageInformation> AllItemList;
    //=========================================
    public Sprite ItemSPrite;
    public Sprite offsprite;
    //=========================================
    public Text StageTextInGame;
    private void Start()
    {
        StagePath = Application.persistentDataPath + "/StageInformation.txt";
        Load();
        for (int i = 0; i < stageInformation.stageNumber; i++)
        {
            ItemImage[i].sprite = ItemSPrite;
        }
    }

    private void Save()
    {
        string Stagejdata = JsonUtility.ToJson(stageInformation);
        byte[] Stagebytes = System.Text.Encoding.UTF8.GetBytes(Stagejdata);
        string StageCode = System.Convert.ToBase64String(Stagebytes);
        File.WriteAllText(StagePath, StageCode);
    }

    private void Load()
    {
        {
            if (!File.Exists(StagePath))
            {
                stageInformation.stageNumber = 1;
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
