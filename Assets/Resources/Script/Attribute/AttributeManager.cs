using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class AttributeSeralization<T>
{
    public AttributeSeralization(List<T> _target) => target = _target;
    public List<T> target;
}
[System.Serializable]
public class Attribute
{
    public Attribute(string _Type, string _Name, string _Explain, int _Number, bool _isUsing)
    {
        Type = _Type;
        Name = _Name;
        Explain = _Explain;
        Number = _Number;
        isUsing = _isUsing;
    }//생성자

    public string Type, Name, Explain;
    public int Number;
    public bool isUsing;
}
public class AttributeManager : MonoBehaviour
{
    //=========================================
    string AttributePath;
    string goldPath;
    string StagePath;
    //==========================================
    public List<Attribute> AllAttributeList, MyitemList, CurItemList;
    public string curType = "Attribute";
    //==========================================
    public GameObject[] Slot, UsingImage;
    public Image[] TabImage, ItemImage;
    public Sprite[] ItemSPrite;
    public Sprite TabIdleSprite, TabSelectSprite;
    //==========================================
    private StageInformation stageInformation;
    private Gold gold = new Gold();
    //==========================================
    public TextAsset AttributeDatabase;
    public Text goldShow;
    //==========================================
    public GameObject Nogold;
    private void Start()
    {
        //==========================================
        AttributePath = Application.persistentDataPath + "/MyAttribute.txt";
        goldPath = Application.persistentDataPath + "/gold.txt";
        StagePath = Application.persistentDataPath + "/StageInformation.txt";
        //==========================================
        string[] line = AttributeDatabase.text.Substring(0, AttributeDatabase.text.Length - 1).Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');
            AllAttributeList.Add(new Attribute(row[0], row[1], row[2], Convert.ToInt32(row[3]), row[4] == "TRUE"));
        }
        
        Load();
        print(stageInformation.stageNumber);
        SlotClick(1);


    }
    private void Update()
    {
        goldShow.text = "" + gold.Cha_Gold;
    }
    public void GetItemClick()
    {
        if(gold.Cha_Gold > 49)
        {
            Nogold.SetActive(false);
            gold.Cha_Gold -= 50;
            Attribute curItem = MyitemList.Find(x => x.isUsing == true); 

            if (curItem != null)
            {
                curItem.Number = ((curItem.Number) + 1);
            }
            else
            {
                Attribute curAllAttribute = AllAttributeList.Find((x => x.isUsing == true));
                if (curAllAttribute != null)
                {
                    curAllAttribute.Number = 1;
                    MyitemList.Add(curAllAttribute);
                }
                else
                {
                    print("오류오류");
                }
            }
        }
        else
        {
            Nogold.SetActive(true);
            print("골드가 없다!");
        }
        Save();
        Load();
    }

    public void RemoveItemClick()
    {
        Attribute curItem = MyitemList.Find((x => x.isUsing == true));
        if (curItem != null)
        {
            int curNumber = (curItem.Number) - 1;

            if (curNumber <= 0)
            {
                MyitemList.Remove(curItem);
            }
            else
            {
                curItem.Number = curNumber;
            }
        }
        Save();
    }

    public void SlotClick(int slotNum)
    {
        Attribute CurItem = CurItemList[slotNum];
        Attribute UsingItem = CurItemList.Find(x => x.isUsing == true);
        if (curType == "Attribute")
        {
            if (UsingItem != null)
            {
                UsingItem.isUsing = false;
            }
            CurItem.isUsing = true;
        }
        else
        {
            CurItem.isUsing = !CurItem.isUsing;
            if (UsingItem != null)
            {
                UsingItem.isUsing = false;
            }

        }
        Save();
    }

    public void TabClick(string tabName)
    {
        curType = tabName;
        CurItemList = MyitemList.FindAll(x => x.Type == tabName);
        for (int i = 0; i < stageInformation.stageNumber; i++)
        {
            bool isExist = i < CurItemList.Count;
            Slot[i].SetActive(isExist);
            Slot[i].GetComponentInChildren<Text>().text = isExist ? CurItemList[i].Name + " : " + CurItemList[i].Number : "";
            if (isExist)
            {
                ItemImage[i].sprite = ItemSPrite[AllAttributeList.FindIndex(x => x.Name == CurItemList[i].Name)];
                UsingImage[i].SetActive(CurItemList[i].isUsing);
            }
        }
        int tabNum = 0;
        switch (tabName)
        {
            case "Attribute": tabNum = 0; break;
            case "Item": tabNum = 1; break;
        }
        for (int i = 0; i < TabImage.Length; i++)
        {
            TabImage[i].sprite = i == tabNum ? TabSelectSprite : TabIdleSprite;
        }
    }

    private void Save()
    {
        string Attributejdata = JsonUtility.ToJson(new AttributeSeralization<Attribute>(MyitemList));
        byte[] AttributeBytes = System.Text.Encoding.UTF8.GetBytes(Attributejdata);
        string AttributeCode = Convert.ToBase64String(AttributeBytes);
        File.WriteAllText(AttributePath, AttributeCode);

        string GoldJdata = JsonUtility.ToJson(gold);
        byte[] GoldtByte = System.Text.Encoding.UTF8.GetBytes(GoldJdata);
        string GoldCode = System.Convert.ToBase64String(GoldtByte);
        File.WriteAllText(goldPath, GoldCode);
        
        TabClick(curType);
    }
    private void ResetItemClick()
    {
        string Attributejdata = JsonUtility.ToJson(new AttributeSeralization<Attribute>(AllAttributeList));
        byte[] AttributeBytes = System.Text.Encoding.UTF8.GetBytes(Attributejdata);
        string AttributeCode = Convert.ToBase64String(AttributeBytes);
        File.WriteAllText(AttributePath, AttributeCode);

        Load();
    }
    private void Load()
    {
        if (!File.Exists(AttributePath))
        {
            ResetItemClick();
            return;
        }
        string AttributeCode = File.ReadAllText(AttributePath);
        byte[] AttributeBytes = System.Convert.FromBase64String(AttributeCode);
        string Attributejdata = System.Text.Encoding.UTF8.GetString(AttributeBytes);
        MyitemList = JsonUtility.FromJson<AttributeSeralization<Attribute>>(Attributejdata).target;

        string GoldCode = File.ReadAllText(goldPath);
        byte[] GoldtByte = System.Convert.FromBase64String(GoldCode);
        string GoldJdata = System.Text.Encoding.UTF8.GetString(GoldtByte);
        gold = JsonUtility.FromJson<Gold>(GoldJdata);

        string StageCode = File.ReadAllText(StagePath);
        byte[] Stagebytes = System.Convert.FromBase64String(StageCode);
        string Stagejdata = System.Text.Encoding.UTF8.GetString(Stagebytes);
        stageInformation = JsonUtility.FromJson<StageInformation>(Stagejdata);


        TabClick(curType);

    }

}
