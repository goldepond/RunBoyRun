
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ItemSeralization<T>
{
    public ItemSeralization(List<T> _target) => target = _target;
    public List<T> target;
}
[System.Serializable]
public class Item
{
    public Item(int _KeyNum, string _Type, string _Name, string _Explain, int _Number, bool _isUsing)
    {
        KeyNum = _KeyNum;
        Type = _Type;
        Name = _Name;
        Explain = _Explain;
        Number = _Number;
        isUsing = _isUsing;
    }//»ý¼ºÀÚ

    public string Type, Name, Explain;
    public int Number, KeyNum;
    public bool isUsing;

}
public class SlotNumCloth
{
    public int charHat;
    public int charCloth;
}
public class ItemManager : MonoBehaviour
{
    //============================================
    string ItemPath;
    string equipmentPath;
    string StagePath;
    //============================================
    public string curType = "Cloth";
    //============================================
    public List<Item> AllItemList, MyitemList, CurItemList;
    //============================================
    public GameObject[] Slot, UsingImage;  
    public Image[] TabImage, ItemImage;
    public Sprite[] ItemSPrite;
    //============================================
    public Sprite TabIdleSprite, TabSelectSprite;
    //============================================
    public GameObject CharShow_Hat;
    public GameObject CharShow_Cloth;
    //============================================
    private SlotNumCloth SlotNumCloth = new SlotNumCloth();
    private StageInformation stageInformation;
    //============================================
    public TextAsset ItemDatabase;
    public Text goldShow;
    //============================================

    private void Start()
    {
        //=========================================
        ItemPath = Application.persistentDataPath + "/MyItemText.txt";
        equipmentPath = Application.persistentDataPath + "/equipItem.txt";
        StagePath = Application.persistentDataPath + "/StageInformation.txt";
        //=========================================
        string[] line = ItemDatabase.text.Substring(0, ItemDatabase.text.Length).Split('\n');
      //  Save();
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');
            AllItemList.Add(new Item(Convert.ToInt32(row[0]), row[1], row[2], row[3], Convert.ToInt32(row[4]), row[5] == "TRUE"));
        }
       Load();
    }

    public void CharCloth(int i)
    {
        CharShow_Cloth.GetComponentInChildren<Image>().sprite = ItemSPrite[i];
    }
    public void SlotClick(int slotNum)
    {
        Item CurItem = CurItemList[slotNum];
        Item UsingItem = CurItemList.Find(x => x.isUsing == true);
        if (curType == "Cloth")
        {
            SlotNumCloth.charCloth = CurItem.KeyNum;
            CharCloth(SlotNumCloth.charCloth);
            if (UsingItem != null)
            {
                UsingItem.isUsing = false;
                
            }
            CurItem.isUsing = true;
        }
        else
        {
            CurItem.isUsing = !CurItem.isUsing;
          SlotNumCloth.charHat = CurItem.KeyNum;
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
        CurItemList = AllItemList.FindAll(x => x.Type == tabName);
        
        for (int i = 0; i < Slot.Length; i++)
        {
            if (i < stageInformation.stageNumber)
            {
                bool isExist = i < CurItemList.Count;
                Slot[i].SetActive(isExist);
                Slot[i].GetComponentInChildren<Text>().text = isExist ? CurItemList[i].Name: "";
                if (isExist)
                {
                    ItemImage[i].sprite = ItemSPrite[AllItemList.FindIndex(x => x.Name == CurItemList[i].Name)];
                    UsingImage[i].SetActive(CurItemList[i].isUsing);
                }
            }
        }
        int tabNum = 0;
        switch (tabName)
        {
            case "Cloth": tabNum = 0; break;
            case "Hat": tabNum = 1; break;
        }
        for (int i = 0; i < TabImage.Length; i++)
        {
            TabImage[i].sprite = i == tabNum ? TabSelectSprite : TabIdleSprite;
        }
    }
    public void Save()
    {
        string itemjdata = JsonUtility.ToJson(new ItemSeralization<Item>(AllItemList));
        byte[] itemBytes = System.Text.Encoding.UTF8.GetBytes(itemjdata);
        string itemCode = Convert.ToBase64String(itemBytes);
        File.WriteAllText(ItemPath, itemCode);

        string equipmentJdata = JsonUtility.ToJson(SlotNumCloth);
        byte[] equipmentByte = System.Text.Encoding.UTF8.GetBytes(equipmentJdata);
        string equipmentCode = System.Convert.ToBase64String(equipmentByte);
        File.WriteAllText(equipmentPath, equipmentCode);
        TabClick(curType);
    }

    private void  ResetItemClick()
    {
        string itemjdata = JsonUtility.ToJson(new ItemSeralization<Item>(AllItemList));
        byte[] itemBytes = System.Text.Encoding.UTF8.GetBytes(itemjdata);
        string itemCode = Convert.ToBase64String(itemBytes);
        File.WriteAllText(ItemPath, itemCode);

        string equipmentJdata = JsonUtility.ToJson(SlotNumCloth);
        byte[] equipmentByte = System.Text.Encoding.UTF8.GetBytes(equipmentJdata);
        string equipmentCode = System.Convert.ToBase64String(equipmentByte);
        File.WriteAllText(equipmentPath, equipmentCode);
        Load();
    }
    private void Load()
    {
        if(!File.Exists(ItemPath) ||!File.Exists(equipmentPath)){ResetItemClick();return;}

        string itemCode = File.ReadAllText(ItemPath);
        byte[] itembytes = System.Convert.FromBase64String(itemCode);
        string itemjdata = System.Text.Encoding.UTF8.GetString(itembytes);
        MyitemList = JsonUtility.FromJson<ItemSeralization<Item>>(itemjdata).target;

        string equipmentCode = File.ReadAllText(equipmentPath);
        byte[] equipmentBytes = System.Convert.FromBase64String(equipmentCode);
        string equipmentJdata = System.Text.Encoding.UTF8.GetString(equipmentBytes);
        SlotNumCloth = JsonUtility.FromJson<SlotNumCloth>(equipmentJdata);

        string StageCode = File.ReadAllText(StagePath);
        byte[] Stagebytes = System.Convert.FromBase64String(StageCode);
        string Stagejdata = System.Text.Encoding.UTF8.GetString(Stagebytes);
        stageInformation = JsonUtility.FromJson<StageInformation>(Stagejdata);


        TabClick(curType);
        
    }
}
