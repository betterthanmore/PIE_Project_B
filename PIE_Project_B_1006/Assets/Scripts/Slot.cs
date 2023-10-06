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
    public SLOTSTATE state = SLOTSTATE.EMPTY;                                //enum값을 선언
    private void ChangeStateTo(SLOTSTATE targetState)
    {
        state = targetState;                                                 //해당 슬롯의 상태값을 변환시켜주는 함수
    }

    public void Grabbed()
    {
        
    }    

    public void CreateItem(int id)
    {

    }
}
