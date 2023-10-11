using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    

    public enum SLOTSTATE                                                    //슬롯 상태값
    {
        EMPTY,
        FULL
    }
    public int id;                                                           //슬롯 번호 ID
    public Item itemObject;
    public SLOTSTATE state = SLOTSTATE.EMPTY;                                //enum값을 선언

    public void GetSlotId(int id)
    {
        int x = id % 10;
        int y = id / 10;
    }


    
    

    
    
    private void ChangeStateTo(SLOTSTATE targetState)
    {
        state = targetState;                                                 //해당 슬롯의 상태값을 변환시켜주는 함수
    }

    public void Grabbed()
    {
        Destroy(itemObject.gameObject);
        ChangeStateTo(SLOTSTATE.EMPTY);
    }    

    public void CreateItem(int id)                                              //아이템 경로 Resources/Prefabs/Item_000 형태
    {
        string itemPath = "Prefabs/Item_" + id.ToString("000");
        var itemGo = (GameObject)Instantiate(Resources.Load(itemPath));
        itemGo.transform.SetParent(this.transform);
        itemGo.transform.localPosition = Vector3.zero;
        itemGo.transform.localScale = Vector3.one;

        //크기,위치 특정이 어려워 아직 작성하지 않음

        itemObject = itemGo.GetComponent<Item>();
        itemObject.Init(id, this);
        ChangeStateTo(SLOTSTATE.FULL);

    }
}
