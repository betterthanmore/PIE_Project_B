using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class GameController : MonoBehaviour
{
    public Slot[] slots;                        //슬롯 배열 관리

    private Vector3 _target;
    private ItemInfo carryingItem;               //들고있는 아이템 정보 관리

    private Dictionary<int, Slot> slotDictionary;  //슬롯id,클래스 관리하기 위한 자료구조

    private void Start()
    {
        slotDictionary = new Dictionary<int, Slot>(); //초기화 >> 이해 안됨

        for(int i  = 0; i < slots.Length; i++)
        {//각 슬롯의 아이디를 설정하고 딕셔너리에 추가
            slots[i].id = i;
            slotDictionary.Add(i, slots[i]);
        }
    }


    void Update()
    {
        if(Input.GetMouseButtonDown(0))             //마우스 누를 떄
        {
            SendRayCast();
        }

        if (Input.GetMouseButton(0) && carryingItem)      //잡고 이동시킬 때
        {
            SendRayCast();
        }

        if (Input.GetMouseButtonUp(0))       //마우스 버튼을 놓을때
        {
            SendRayCast();
        }

    }

    void SendRayCast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) )
        {
            var slot = hit.transform.GetComponent<Slot>(); //레이케스트를 통해 나온 슬롯 칸
            if(slot.state == Slot.SLOTSTATE.FULL && carryingItem == null)
            {
                //선택한 슬롯에서 아이템을 잡는다.
                string itemPath = "Prefabs/Item_Grabbed_" + slot.itemObject.id.ToString("000"); //?
                var itemGo = (GameObject)Instantiate(Resources.Load(itemPath)); //아이템 생성
                itemGo.transform.SetParent(this.transform);
                itemGo.transform.localPosition = Vector3.zero;
                itemGo.transform.localScale = Vector3.one * 2;

                carryingItem = itemGo.GetComponent<ItemInfo>();                 
                carryingItem = InitDummy(slot.id, slot.itemObject.id);

                slot.Grabbed();
            }
            else if(slot.state == Slot.SLOTSTATE.FULL && carryingItem != null)
            {
                slot.CreateItem(carryingItem.itemId);
                Destroy(carryingItem.gameObject);
            }
            else if(slot.state == Slot.SLOTSTATE.FULL && carryingItem != null)
            {
                if(slot.itemObject.id == carryingItem.itemId) 
                {
                    OnItemMergedWithTarget(slot.id);            //오브젝트 병합 함수 호출
                }
                else
                {
                    OnItemCarryFail();      //배치실패
                }
            }
        }
        else
        {
            if (!carryingItem) return;
            OnItemCarryFail();
        }
    }

    void OnItemMergedWithTarget(int targetSlotId)
    {//병합 함수
        var slot = GetSlotById(targetSlotId);
        Destroy(slot.itemObject.gameObject);
        slot.CreateItem(carryingItem.itemId + 1);
        Destroy(carryingItem.gameObject);

    }

    void OnItemSelected()
    {//아이템을 선택하고 아이템을 마우스 위치로 이동시키는 함수
        _target = Camera.main.ScreenToWorldPoint(Input.mousePosition);      //좌표변환
        _target.z = 0;
        var delta = 10 * Time.deltaTime;
        delta *= Vector3.Distance(transform.position, _target);
        carryingItem.transform.position = Vector3.MoveTowards(carryingItem.transform.position, _target, delta);
    }
    void OnItemCarryFail()
    {      //아이템 배치 실패
        var slot = GetSlotById(carryingItem.slotId);
        slot.CreateItem(carryingItem.itemId);
        Destroy(carryingItem.gameObject);
    }    

    Slot GetSlotById(int id)
    {
        return slotDictionary[id];
    }
}
