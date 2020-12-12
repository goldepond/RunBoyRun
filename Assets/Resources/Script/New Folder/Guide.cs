using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Allitem
{
    public Allitem(int _KeyNum, string _Type, string _Name, string _Explain, int _Number, bool _isUsing)
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
public class Guide : MonoBehaviour
{
    public Sprite TabIdleSprite, TabSelectSprite;
    string filePath;
    public string curType = "Cloth";
    //=========================================
    public List<Allitem> AllItemList, CurItemList;
    //=========================================
    public GameObject[] Slot, UsingImage;
    public Sprite[] ItemSPrite;
    public Image[] TabImage, ItemImage;
    //=========================================
    public TextAsset ItemDatabase;
    public Text GameGuide;
    public Text goldShow;
    //=========================================
    string AttributePath;
    //=========================================
    private void Start()
    {
        AttributePath = Application.persistentDataPath + "/AttributeBase.txt";
        string[] line = ItemDatabase.text.Substring(0, ItemDatabase.text.Length).Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');
            AllItemList.Add(new Allitem(Convert.ToInt32(row[0]), row[1], row[2], row[3], Convert.ToInt32(row[4]), row[5] == "TRUE"));
        }
        Load();
        TabClick("Hat"); 
    }
    public void SlotClick(int slotNum)
    {
        Allitem CurItem = CurItemList[slotNum];
        Allitem UsingItem = CurItemList.Find(x => x.isUsing == true);
        GameGuide.text = "" + CurItemList[slotNum].Explain;
        if (curType == "Cloth")
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
        CurItemList = AllItemList.FindAll(x => x.Type == tabName);
        for (int i = 0; i < Slot.Length; i++)
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
        int tabNum = 0;
        switch (tabName)
        {
            case "Cloth": tabNum = 0; break;
            case "Hat": tabNum = 1; break;
            case "Attribute": tabNum = 2; break;
            case "Item": tabNum = 3; break;
        }
        for (int i = 0; i < TabImage.Length; i++)
        {
            TabImage[i].sprite = i == tabNum ? TabSelectSprite : TabIdleSprite;
        }
    }

    private void Save()
    {
        string Attributejdata = JsonUtility.ToJson(new AttributeSeralization<Allitem>(AllItemList));
        byte[] AttributeBytes = System.Text.Encoding.UTF8.GetBytes(Attributejdata);
        string AttributeCode = Convert.ToBase64String(AttributeBytes);
        File.WriteAllText(AttributePath, AttributeCode);
        TabClick(curType);
    }
    private void Load()
    {
        if (!File.Exists(filePath))
        {
            return;
        }
        string AttributeCode = File.ReadAllText(AttributePath);
        byte[] AttributeBytes = System.Convert.FromBase64String(AttributeCode);
        string Attributejdata = System.Text.Encoding.UTF8.GetString(AttributeBytes);
        AllItemList = JsonUtility.FromJson<ItemSeralization<Allitem>>(Attributejdata).target;
        TabClick(curType);
    }
}
