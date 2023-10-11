using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{//잡고 있는 물건의 정보값을 가지는 클래스
    public int slotId;                    //슬롯
    public int itemId;                    //아이템

   


    public void InitDummy(int slotId, int itemId)
    {//인수로 받은 값들을 클래스에 입력
        this.slotId = slotId;
        this.itemId = itemId;
        
    }
}
