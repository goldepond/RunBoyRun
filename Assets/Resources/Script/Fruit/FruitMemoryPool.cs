using System.Collections;
using UnityEngine;

//-----------------------------------------------------------------------------------------
// �޸� Ǯ Ŭ����
// �뵵 : Ư�� ���ӿ�����Ʈ�� �ǽð����� ������ �������� �ʰ�,
//     : �̸� ������ �� ���ӿ�����Ʈ�� ��Ȱ���ϴ� Ŭ�����Դϴ�.
//-----------------------------------------------------------------------------------------
//MonoBehaviour ��� �ȹ���. IEnumerable ��ӽ� foreach ��� ����
//System.IDisposable �������� �ʴ� �޸�(���ҽ�)�� ���� ��
public class FruitMemoryPool : IEnumerable, System.IDisposable
{
    //-------------------------------------------------------------------------------------
    // ������ Ŭ����

    //-------------------------------------------------------------------------------------
    private class Item
    {
        public bool active; // ������Ʈ�� ����ϰ� �ִ� ������ �Ǵ��ϴ� ����
        public GameObject gameObject; // ������ ������Ʈ
    }

    // ���� ������ Ŭ������ �迭�� ����(��, �������� �������� ���� ����)
    private Item[] table;

    //------------------------------------------------------------------------------------
    // ������ �⺻ ������(foreach���� ����ϴ� ���ε� �츮�� ������� ����, ������ ���߿�..)
    //------------------------------------------------------------------------------------
    public IEnumerator GetEnumerator()
    {
        if (table == null)    // ���� table�� ��üȭ ���� �ʾҴٸ�?
        {
            yield break;    // �Լ��� �׳� Ż��
        }

        // table�� �����ϸ� ���⼭���� ����   
        // count�� table�� ����(��, �迭�� ũ��)
        int count = table.Length;

        for (int i = 0; i < count; i++)    // �� count��ŭ �ݺ�
        {
            Item item = table[i];
            // item�� table�� i��ġ�� �ش�Ǵ� ��ü�� ����
            if (item.active) // item�� ������̸�
            {
                yield return item.gameObject; // �� item�� ������Ʈ�� ��ȯ
            }
        }
    }

    //-------------------------------------------------------------------------------------
    // �޸� Ǯ ����
    // original : �̸� ������ �� �����ҽ�
    // count : Ǯ �ְ� ����

    private bool AllClear = false;
    //-------------------------------------------------------------------------------------
    public void Create(GameObject[] original, int count)
    {
        Dispose();    // �޸�Ǯ �ʱ�ȭ
        table = new Item[count]; // count ��ŭ �迭�� ����

        for (int i = 0; i < count; i++) // count ��ŭ �ݺ�
        {
            Item item = new Item
            {
                active = false,
                gameObject = GameObject.Instantiate(original[Random.Range(0, original.Length)]) as GameObject
            };

            // original�� GameObject �������� item.gameObject�� ����
            item.gameObject.SetActive(false);
            // SetActive�� Ȱ��ȭ �Լ��ε� �޸𸮿��� �ø� ���̹Ƿ� ��Ȱ��ȭ ���·� ����
            table[i] = item;

        }
    }

    //-------------------------------------------------------------------------------------
    // �� ������ ��û - ���� �ִ� ��ü�� �ݳ��Ѵ�.
    //-------------------------------------------------------------------------------------
    public GameObject NewItem() // GetEnumerator()�� ���
    {
        if (table == null)
        {
            return null;
        }

        int count = table.Length;
        for (int i = 0; i < count; i++)
        {
            Item item = table[i];
            if (item.active == false)
            {
                item.active = true;
                item.gameObject.SetActive(true);
                return item.gameObject;
            }
        }

        return null;
    }

    //--------------------------------------------------------------------------------------
    // ������ ������� - ����ϴ� ��ü�� �����Ѵ�.
    // gameOBject : NewItem���� ����� ��ü
    //--------------------------------------------------------------------------------------
    public void RemoveItem(GameObject gameObject)
    {
        // table�� ��üȭ���� �ʾҰų�, �Ű������� ���� gameObject�� ���ٸ�
        if (table == null || gameObject == null)
        {
            return; // �Լ� Ż��
        }

        // table�� �����ϰų�, �Ű������� ���� gameObject�� �����ϸ� ���⼭���� ����
        // count�� table�� ����(��, �迭�� ũ��)
        int count = table.Length;

        for (int i = 0; i < count; i++)
        {
            Item item = table[i];
            // �Ű����� gameObject�� item�� gameObject�� ���ٸ�
            if (item.gameObject == gameObject)
            {
                // active ������ false��
                item.active = false;
                // �׸��� ���ӿ�����Ʈ�� ��Ȱ��ȭ ��Ų��.
                item.gameObject.SetActive(false);
                AllClear = false;
                break;
            }
        }
    }

    //--------------------------------------------------------------------------------------
    // ��� ������ ������� - ��� ��ü�� �����Ѵ�.
    //--------------------------------------------------------------------------------------
    public void ClearItem()
    {
        // table�� ��üȭ���� �ʾҴٸ�..
        if (table == null)
        {
            return;
        }

        // table�� �����ϸ�...
        // count�� table�� ����(��, �迭�� ũ��)
        int count = table.Length;

        for (int i = 0; i < count; i++)
        {
            Item item = table[i];
            // item�� ������� �ʰ�, Ȱ��ȭ�Ǿ� �ִٸ�
            if (item != null && item.active)
            {
                // ��Ȱ��ȭ ó���� �����մϴ�.
                item.active = false;
                item.gameObject.SetActive(false);
            }
        }
    }

    //--------------------------------------------------------------------------------------
    // �޸� Ǯ ����
    //--------------------------------------------------------------------------------------
    public void Dispose()
    {
        // table�� ��üȭ���� �ʾҴٸ�..
        if (table == null)
        {
            return;
        }

        // table�� �����ϸ�...
        // count�� table�� ����(��, �迭�� ũ��)
        int count = table.Length;

        for (int i = 0; i < count; i++)
        {
            Item item = table[i];
            GameObject.Destroy(item.gameObject);
            // �޸� Ǯ�� �����ϴ� ���̱� ������ ��� ������Ʈ�� Destroy �Ѵ�.
        }
        table = null;
    }

}