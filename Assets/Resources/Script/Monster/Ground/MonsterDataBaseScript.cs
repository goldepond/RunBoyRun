using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterAll
{
    public MonsterAll(int _KeyNum, string _Type, string _Name, string _Explain, int _Speed, bool _isUsing)
    {
        KeyNum = _KeyNum;
        Type = _Type;
        Name = _Name;
        Explain = _Explain;
        Speed = _Speed;
        isUsing = _isUsing;
    }//»ý¼ºÀÚ

    public string Type, Name, Explain;
    public int Speed, KeyNum;
    public bool isUsing;

}
public class MonsterDataBaseScript : MonoBehaviour
{
    public TextAsset ItemDatabase;
    public List<MonsterAll> AllItemList, MyitemList, CurItemList;

    public GameObject[] MonsterSlot;
    //  public int SlotNumCloth;
    public int a;

    private void Start()
    {
       
        string[] line = ItemDatabase.text.Substring(0, ItemDatabase.text.Length).Split('\n');

        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');
            AllItemList.Add(new MonsterAll(Convert.ToInt32(row[0]), row[1], row[2], row[3], Convert.ToInt32(row[4]), row[5] == "TRUE"));
            a = AllItemList.Capacity;
        }
    }


}
