using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using JetBrains.Annotations;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{
    public Slot[] slots;                                //게임 컴트롤러에서는 Slot 배열을 관리

    private Vector3 _target;
    private ItemInfo carryingItem;                      //잡고 있는 아이템 정보 값 관리

    private Dictionary<int, Slot> slotDictionary;       //Slot id, Slot class 관리하기 위한 자료구조

    public int stage;

    

    private void Start()
    {
        if (slots == null)
        {
            Slot[] _slots = FindObjectsOfType<Slot>();
            slots = _slots;
        }
        slotDictionary = new Dictionary<int, Slot>();   //초기화

        for (int i = 0; i < slots.Length; i++)
        {                                               //각 슬롯의 ID를 설정하고 딕셔너리에 추가
            slots[i].id = i;
            slotDictionary.Add(i, slots[i]);
        }

        if(stage == 1)
        {
            //placeItem 사용하여 아이템 배치 
            PlaceItem(8, 1, 2);
            PlaceItem(4, 2, 2);
            PlaceItem(6, 2, 4);
            PlaceItem(2, 3, 4);
            PlaceItem(3, 4, 2);
            PlaceItem(2, 5, 2);
            PlaceItem(5, 5, 3);
            PlaceItem(7, 5, 3);
            PlaceItem(2, 6, 2);
            PlaceItem(3, 7, 4);
            PlaceItem(5, 7, 3);
            PlaceItem(7, 8, 1);
            PlaceItem(6, 9, 1);

            // Clear Tile 관련 세팅 예제
            SetClearTile(2, 1);
            SetClearTile(7, 3);
            SetClearTile(2, 8);

        }
        else if(stage == 2)
        {

            //placeItem 사용하여 아이템 배치 
            PlaceItem(3, 1, 3);
            PlaceItem(6, 1, 3);
            PlaceItem(9, 1, 4);
            PlaceItem(2, 2, 1);
            PlaceItem(4, 3, 3);
            PlaceItem(8, 3, 2);
            PlaceItem(5, 4, 2);
            PlaceItem(2, 5, 1);
            PlaceItem(7, 5, 2);
            PlaceItem(5, 6, 2);
            PlaceItem(8, 6, 2);
            PlaceItem(6, 7, 3);
            PlaceItem(3, 8, 4);
            PlaceItem(1, 9, 2);
            PlaceItem(5, 9, 2);

            // Clear Tile 관련 세팅 예제
            SetClearTile(3, 4);
            SetClearTile(3, 7);
            SetClearTile(7, 8);
        }




        // Move Tile 관련 세팅 예제
        /*        SetMoveTile(1, 3, 2, 'Y');
                SetMoveTile(3, 2, -1, 'X');*/
    }

    void Update()
    {


        if (Input.GetMouseButtonDown(0)) //마우스 누를 때
        {
            SendRayCast();
        }

        if (Input.GetMouseButton(0) && carryingItem)    //잡고 이동시킬 때
        {
            OnItemSelected();
        }

        if (Input.GetMouseButtonUp(0))  //마우스 버튼을 놓을때
        {
            SendRayCast();
        }

        
    }

     

    //한칸 이동 제한 스크립트 추가해야함 Slot X Slot Y가 1,-1 차이날때만 이동가능
    void SendRayCast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            var slot = hit.transform.GetComponent<Slot>();          //Raycast를 통해 나온 Slot칸
            if (slot == null)
                return;

            if (slot.state == Slot.SLOTSTATE.FULL && carryingItem == null)
            {
                string itemPath = "Prefabs/Item_Grabbed_" + slot.itemObject.id.ToString("000");
                var itemGo = (GameObject)Instantiate(Resources.Load(itemPath));     //아이템 생성

                itemGo.transform.SetParent(this.transform);
                itemGo.transform.localPosition = Vector3.zero;
                itemGo.transform.localScale = Vector3.one * 2;

                carryingItem = itemGo.GetComponent<ItemInfo>();         //슬롯 정보 입력
                carryingItem.InitDummy(slot.id, slot.itemObject.id, slot.x, slot.y);

                slot.ItemGrabbed();
            }
            else if (slot.state == Slot.SLOTSTATE.EMPTY && carryingItem != null)
            {//빈 슬롯에 아이템 배치
                if ((carryingItem.slotX == slot.x || carryingItem.slotY == slot.y) && !(carryingItem.slotX == slot.x && carryingItem.slotY == slot.y))
                {
                    int x = carryingItem.slotX - slot.x;
                    int y = carryingItem.slotY - slot.y;
                    if (GameManager.Instance.stages[stage - 1].moveAmount > 0)
                    {
                        if (slot.tileType == 0)
                        {
                            slot.CreateItem(carryingItem.itemId);       //잡고 있는것 슬롯 위치에 생성
                            Destroy(carryingItem.gameObject);           //잡고 있는것 파괴
                        }
                        else if (slot.tileType == 1)
                        {
                            if (slot.xOry == 1)
                            {
                                int _x = slot.x + slot.moveAmount;
                                UseMoveTile(_x, slot.y, slot.id);
                            }
                            else if (slot.xOry == 2)
                            {
                                int _y = slot.y + slot.moveAmount;
                                UseMoveTile(slot.x, _y, carryingItem.itemId);
                            }
                            Destroy(carryingItem.gameObject);
                        }
                        else if (slot.tileType == 2)
                        {
                            GameManager.StageInfo _stageInfo = GameManager.Instance.stages.Find(stages => stages.stage == GameManager.Instance.stage);
                            if (carryingItem.itemId == _stageInfo.clearItemId)
                            {
                                slot.CreateItem(carryingItem.itemId);       //잡고 있는것 슬롯 위치에 생성
                                slot.isCleared();
                                Destroy(carryingItem.gameObject);           //잡고 있는것 파괴
                            }
                            else
                            {
                                OnItemCarryFail();  //아이템 배치 실패
                            }
                        }
                        GameManager.Instance.stages[stage - 1].moveAmount -= Mathf.Abs(x + y);
                    }
                    else
                    {
                        GameOver();                             // 게임 오버 함수 호출
                    }

                }
                else
                {
                    OnItemCarryFail();  //아이템 배치 실패
                }
            }
            else if (slot.state == Slot.SLOTSTATE.FULL && carryingItem != null)
            {//Checking 후 병합
                if ((carryingItem.slotX == slot.x || carryingItem.slotY == slot.y) && !(carryingItem.slotX == slot.x && carryingItem.slotY == slot.y))
                {
                    int x = carryingItem.slotX - slot.x;
                    int y = carryingItem.slotY - slot.y;

                    if (slot.itemObject.id == carryingItem.itemId)
                    {
                        if (GameManager.Instance.stages[stage - 1].moveAmount > 0)
                        {
                            if (slot.tileType == 0)
                            {
                                OnItemMergedWithTarget(slot.id);    //병합 함수 호출
                            }
                            else if (slot.tileType == 1)
                            {
                                if (slot.xOry == 1)
                                {
                                    int _x = slot.x + slot.moveAmount;
                                    UseMoveTile(_x, slot.y, slot.id);
                                }
                                else if (slot.xOry == 2)
                                {
                                    int _y = slot.y + slot.moveAmount;
                                    UseMoveTile(slot.x, _y, carryingItem.itemId, true);
                                }
                            }
                            else if (slot.tileType == 2)
                            {
                                OnItemCarryFail();  //아이템 배치 실패
                            }
                            GameManager.Instance.stages[stage - 1].moveAmount -= Mathf.Abs(x + y);
                        }
                        else
                        {
                            GameOver();                             // 게임 오버 함수 호출
                        }
                    }

                }
                else
                {
                    OnItemCarryFail();  //아이템 배치 실패
                }
            }
        }
        else
        {
            if (!carryingItem) return;
            OnItemCarryFail();  //아이템 배치 실패
        }

    }

    public void SetClearTile(int x, int y)
    {
        for (int i = 0; i < slotDictionary.Count; i++)
        {
            var slot = GetSlotById(i);
            if (slot.x == x && slot.y == y)
            {
                slot.tileType = 2;
                
            }
        }
    }

    public void SetMoveTile(int x, int y, int moveAmount, char xOrY)            // SetMoveTile(1,2,3,X) 1,2에 존재하는 타일을 x축으로 3칸(오른쪽으로) 이동하게 만듬
    {
        for (int i = 0; i < slotDictionary.Count; i++)
        {
            var slot = GetSlotById(i);
            if (slot.x == x && slot.y == y)
            {
                slot.tileType = 1;
                slot.moveAmount = moveAmount;
                if (xOrY == 'X')
                {
                    slot.xOry = 1;
                }
                else if (xOrY == 'Y')
                {
                    slot.xOry = 2;
                }
                else
                {
                    Debug.Log("X나 Y가 아닙니다.");
                }
            }
        }
    }
    public void UseMoveTile(int x, int y, int itemId, bool isMerge = false)
    {
        for (int i = 0; i < slotDictionary.Count; i++)
        {
            var slot = GetSlotById(i);
            if (slot.x == x && slot.y == y)
            {

                if (slot.state == Slot.SLOTSTATE.FULL && carryingItem.itemId == slot.itemObject.id)
                {
                    OnItemMergedWithTarget(slot.id);     // 아이템 조합
                }
                else if (slot.state == Slot.SLOTSTATE.FULL)
                {
                    OnItemCarryFail();  //아이템 배치 실패
                }
                else
                {
                    if (isMerge == false)
                    {
                        slot.CreateItem(itemId);
                    }
                }
            }
        }
    }

    void OnItemSelected()
    {   //아이템을 선택하고 마우스 위치로 이동 
        _target = Camera.main.ScreenToWorldPoint(Input.mousePosition);  //좌표변환
        _target.z = -4;
        var delta = 10 * Time.deltaTime;
        delta *= Vector3.Distance(transform.position, _target);
        carryingItem.transform.position = Vector3.MoveTowards(carryingItem.transform.position, _target, delta);
    }

    void OnItemMergedWithTarget(int targetSlotId)
    {
        var slot = GetSlotById(targetSlotId);
        Destroy(slot.itemObject.gameObject);            //slot에 있는 물체 파괴
        slot.CreateItem(carryingItem.itemId + 1);       //슬롯에 다음 번호 물체 생성
        Destroy(carryingItem.gameObject);               //잡고 있는 물체 파괴
    }

    void OnItemCarryFail()
    {//아이템 배치 실패 시 실행
        var slot = GetSlotById(carryingItem.slotId);        //슬롯 위치 확인
        slot.CreateItem(carryingItem.itemId);               //해당 슬롯에 다시 생성
        Destroy(carryingItem.gameObject);                   //잡고 있는 물체 파괴
    }

    public void PlaceItem(int x, int y, int itemId = 0)             //슬롯의 x값,y값,id값
    {
        for (int i = 0; i < slotDictionary.Count; i++)
        {
            var slot = GetSlotById(i);
            if (slot.x == x && slot.y == y)
            {
                if (itemId != 0)
                {
                    slot.CreateItem(itemId);
                }
                else
                {
                    slot.CreateItem(0);
                }
            }
        }
    }


    bool AllSlotsOccupied()
    {//모든 슬롯이 채워져 있는지 확인
        foreach (var slot in slots)                       //foreach문을 통해서 Slots 배열을 검사후
        {
            if (slot.state == Slot.SLOTSTATE.EMPTY)       //비어있는지 확인
            {
                return false;
            }
        }
        return true;
    }
    Slot GetSlotById(int id)
    {//슬롯 ID로 딕셔너리에서 Slot 클래스를 리턴
        return slotDictionary[id];
    }

    void GameOver()
    {
        Debug.Log("GameOVER!!!!!!!");
        // 게임오버 시 발동하는 함수
    }


}
