using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System;

public class GameController : MonoBehaviour
{
    public Slot[] slots;                                //���� ��Ʈ�ѷ������� Slot �迭�� ����

    private Vector3 _target;
    private ItemInfo carryingItem;                      //��� �ִ� ������ ���� �� ����

    private Dictionary<int, Slot> slotDictionary;       //Slot id, Slot class �����ϱ� ���� �ڷᱸ��

    public int stage;

    private void Start()
    {
        if (slots == null)
        {
            Slot[] _slots = FindObjectsOfType<Slot>();
            slots = _slots;
        }
        slotDictionary = new Dictionary<int, Slot>();   //�ʱ�ȭ

        for (int i = 0; i < slots.Length; i++)
        {                                               //�� ������ ID�� �����ϰ� ��ųʸ��� �߰�
            slots[i].id = i;
            slotDictionary.Add(i, slots[i]);
        }


        //placeItem ����Ͽ� ������ ��ġ 
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

        // Move Tile ���� ���� ����
        SetMoveTile(1, 3, 2, 'Y');
        SetMoveTile(3, 2, -1, 'X');
    }

    void Update()
    {


        if (Input.GetMouseButtonDown(0)) //���콺 ���� ��
        {
            SendRayCast();
        }

        if (Input.GetMouseButton(0) && carryingItem)    //��� �̵���ų ��
        {
            OnItemSelected();
        }

        if (Input.GetMouseButtonUp(0))  //���콺 ��ư�� ������
        {
            SendRayCast();
        }
    }

    //��ĭ �̵� ���� ��ũ��Ʈ �߰��ؾ��� Slot X Slot Y�� 1,-1 ���̳����� �̵�����
    void SendRayCast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            var slot = hit.transform.GetComponent<Slot>();          //Raycast�� ���� ���� Slotĭ
            if (slot.state == Slot.SLOTSTATE.FULL && carryingItem == null)
            {
                string itemPath = "Prefabs/Item_Grabbed_" + slot.itemObject.id.ToString("000");
                var itemGo = (GameObject)Instantiate(Resources.Load(itemPath));     //������ ����

                itemGo.transform.SetParent(this.transform);
                itemGo.transform.localPosition = Vector3.zero;
                itemGo.transform.localScale = Vector3.one * 2;

                carryingItem = itemGo.GetComponent<ItemInfo>();         //���� ���� �Է�
                carryingItem.InitDummy(slot.id, slot.itemObject.id, slot.x, slot.y);

                slot.ItemGrabbed();
            }
            else if (slot.state == Slot.SLOTSTATE.EMPTY && carryingItem != null)
            {//�� ���Կ� ������ ��ġ
                if ((carryingItem.slotX == slot.x || carryingItem.slotY == slot.y) && !(carryingItem.slotX == slot.x && carryingItem.slotY == slot.y))
                {
                    int x = carryingItem.slotX - slot.x;
                    int y = carryingItem.slotY - slot.y;
                    if (GameManager.Instance.stages[stage - 1].moveAmount > 0)
                    {
                        if (slot.isMoveTile == false)
                        {
                            slot.CreateItem(carryingItem.itemId);       //��� �ִ°� ���� ��ġ�� ����
                            Destroy(carryingItem.gameObject);           //��� �ִ°� �ı�
                        }
                        else
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
                        GameManager.Instance.stages[stage - 1].moveAmount -= Mathf.Abs(x + y);
                    }
                    else
                    {
                        GameOver();                             // ���� ���� �Լ� ȣ��
                    }

                }
                else
                {
                    OnItemCarryFail();  //������ ��ġ ����
                }
            }
            else if (slot.state == Slot.SLOTSTATE.FULL && carryingItem != null)
            {//Checking �� ����
                if ((carryingItem.slotX == slot.x || carryingItem.slotY == slot.y) && !(carryingItem.slotX == slot.x && carryingItem.slotY == slot.y))
                {
                    int x = carryingItem.slotX - slot.x;
                    int y = carryingItem.slotY - slot.y;

                    if (slot.itemObject.id == carryingItem.itemId)
                    {
                        if (GameManager.Instance.stages[stage - 1].moveAmount > 0)
                        {
                            if (slot.isMoveTile == false)
                            {
                                OnItemMergedWithTarget(slot.id);    //���� �Լ� ȣ��
                            }
                            else
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
                            GameManager.Instance.stages[stage - 1].moveAmount -= Mathf.Abs(x + y);
                        }
                        else
                        {
                            GameOver();                             // ���� ���� �Լ� ȣ��
                        }
                    }

                }
                else
                {
                    OnItemCarryFail();  //������ ��ġ ����
                }
            }
        }
        else
        {
            if (!carryingItem) return;
            OnItemCarryFail();  //������ ��ġ ����
        }

    }
    public void SetMoveTile(int x, int y, int moveAmount, char xOrY)            // SetMoveTile(1,2,3,X) 1,2�� �����ϴ� Ÿ���� x������ 3ĭ(����������) �̵��ϰ� ����
    {
        for (int i = 0; i < slotDictionary.Count; i++)
        {
            var slot = GetSlotById(i);
            if (slot.x == x && slot.y == y)
            {
                slot.isMoveTile = true;
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
                    Debug.Log("X�� Y�� �ƴմϴ�.");
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
                    OnItemMergedWithTarget(slot.id);     // ������ ����
                }
                else if (slot.state == Slot.SLOTSTATE.FULL)
                {
                    OnItemCarryFail();  //������ ��ġ ����
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
    {   //�������� �����ϰ� ���콺 ��ġ�� �̵� 
        _target = Camera.main.ScreenToWorldPoint(Input.mousePosition);  //��ǥ��ȯ
        _target.z = 0;
        var delta = 10 * Time.deltaTime;
        delta *= Vector3.Distance(transform.position, _target);
        carryingItem.transform.position = Vector3.MoveTowards(carryingItem.transform.position, _target, delta);
    }

    void OnItemMergedWithTarget(int targetSlotId)
    {
        var slot = GetSlotById(targetSlotId);
        Destroy(slot.itemObject.gameObject);            //slot�� �ִ� ��ü �ı�
        slot.CreateItem(carryingItem.itemId + 1);       //���Կ� ���� ��ȣ ��ü ����
        Destroy(carryingItem.gameObject);               //��� �ִ� ��ü �ı�
    }

    void OnItemCarryFail()
    {//������ ��ġ ���� �� ����
        var slot = GetSlotById(carryingItem.slotId);        //���� ��ġ Ȯ��
        slot.CreateItem(carryingItem.itemId);               //�ش� ���Կ� �ٽ� ����
        Destroy(carryingItem.gameObject);                   //��� �ִ� ��ü �ı�
    }

    public void PlaceItem(int x, int y, int itemId = 0)             //������ x��,y��,id��
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
    {//��� ������ ä���� �ִ��� Ȯ��
        foreach (var slot in slots)                       //foreach���� ���ؼ� Slots �迭�� �˻���
        {
            if (slot.state == Slot.SLOTSTATE.EMPTY)       //����ִ��� Ȯ��
            {
                return false;
            }
        }
        return true;
    }
    Slot GetSlotById(int id)
    {//���� ID�� ��ųʸ����� Slot Ŭ������ ����
        return slotDictionary[id];
    }

    void GameOver()
    {
        Debug.Log("GameOVER!!!!!!!");
        // ���ӿ��� �� �ߵ��ϴ� �Լ�
    }


}
