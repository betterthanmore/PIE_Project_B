using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    

    public enum SLOTSTATE                                                    //���� ���°�
    {
        EMPTY,
        FULL
    }
    public int id;                                                           //���� ��ȣ ID
    public Item itemObject;
    public SLOTSTATE state = SLOTSTATE.EMPTY;                                //enum���� ����

    public void GetSlotId(int id)
    {
        int x = id % 10;
        int y = id / 10;
    }


    
    

    
    
    private void ChangeStateTo(SLOTSTATE targetState)
    {
        state = targetState;                                                 //�ش� ������ ���°��� ��ȯ�����ִ� �Լ�
    }

    public void Grabbed()
    {
        Destroy(itemObject.gameObject);
        ChangeStateTo(SLOTSTATE.EMPTY);
    }    

    public void CreateItem(int id)                                              //������ ��� Resources/Prefabs/Item_000 ����
    {
        string itemPath = "Prefabs/Item_" + id.ToString("000");
        var itemGo = (GameObject)Instantiate(Resources.Load(itemPath));
        itemGo.transform.SetParent(this.transform);
        itemGo.transform.localPosition = Vector3.zero;
        itemGo.transform.localScale = Vector3.one;

        //ũ��,��ġ Ư���� ����� ���� �ۼ����� ����

        itemObject = itemGo.GetComponent<Item>();
        itemObject.Init(id, this);
        ChangeStateTo(SLOTSTATE.FULL);

    }
}
