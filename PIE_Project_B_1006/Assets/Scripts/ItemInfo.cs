using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{//��� �ִ� ������ �������� ������ Ŭ����
    public int slotId;                    //����
    public int itemId;                    //������

   


    public void InitDummy(int slotId, int itemId)
    {//�μ��� ���� ������ Ŭ������ �Է�
        this.slotId = slotId;
        this.itemId = itemId;
        
    }
}
