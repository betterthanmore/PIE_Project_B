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
    public SLOTSTATE state = SLOTSTATE.EMPTY;                                //enum���� ����
    private void ChangeStateTo(SLOTSTATE targetState)
    {
        state = targetState;                                                 //�ش� ������ ���°��� ��ȯ�����ִ� �Լ�
    }

    public void Grabbed()
    {
        
    }    

    public void CreateItem(int id)
    {
        string itemPath = "Prefabs/Item_" + id.ToString("00");
        var itemGo = (GameObject)Instantiate(Resources.Load(itemPath));

    }
}
