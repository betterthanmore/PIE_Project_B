using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class GameController : MonoBehaviour
{
    public Slot[] slots;                        //���� �迭 ����

    private Vector3 _target;
    private ItemInfo carryingItem;               //����ִ� ������ ���� ����

    private Dictionary<int, Slot> slotDictionary;  //����id,Ŭ���� �����ϱ� ���� �ڷᱸ��

    private void Start()
    {
        slotDictionary = new Dictionary<int, Slot>(); //�ʱ�ȭ >> ���� �ȵ�

        for(int i  = 0; i < slots.Length; i++)
        {//�� ������ ���̵� �����ϰ� ��ųʸ��� �߰�
            slots[i].id = i;
            slotDictionary.Add(i, slots[i]);
        }
    }


    void Update()
    {
        if(Input.GetMouseButtonDown(0))             //���콺 ���� ��
        {
            SendRayCast();
        }

        if (Input.GetMouseButton(0) && carryingItem)      //��� �̵���ų ��
        {
            SendRayCast();
        }

        if (Input.GetMouseButtonUp(0))       //���콺 ��ư�� ������
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
            var slot = hit.transform.GetComponent<Slot>(); //�����ɽ�Ʈ�� ���� ���� ���� ĭ
            if(slot.state == Slot.SLOTSTATE.FULL && carryingItem == null)
            {
                //������ ���Կ��� �������� ��´�.
                string itemPath = "Prefabs/Item_Grabbed_" + slot.itemObject.id.ToString("000"); //?
                var itemGo = (GameObject)Instantiate(Resources.Load(itemPath)); //������ ����
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
                    OnItemMergedWithTarget(slot.id);            //������Ʈ ���� �Լ� ȣ��
                }
                else
                {
                    OnItemCarryFail();      //��ġ����
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
    {//���� �Լ�
        var slot = GetSlotById(targetSlotId);
        Destroy(slot.itemObject.gameObject);
        slot.CreateItem(carryingItem.itemId + 1);
        Destroy(carryingItem.gameObject);

    }

    void OnItemSelected()
    {//�������� �����ϰ� �������� ���콺 ��ġ�� �̵���Ű�� �Լ�
        _target = Camera.main.ScreenToWorldPoint(Input.mousePosition);      //��ǥ��ȯ
        _target.z = 0;
        var delta = 10 * Time.deltaTime;
        delta *= Vector3.Distance(transform.position, _target);
        carryingItem.transform.position = Vector3.MoveTowards(carryingItem.transform.position, _target, delta);
    }
    void OnItemCarryFail()
    {      //������ ��ġ ����
        var slot = GetSlotById(carryingItem.slotId);
        slot.CreateItem(carryingItem.itemId);
        Destroy(carryingItem.gameObject);
    }    

    Slot GetSlotById(int id)
    {
        return slotDictionary[id];
    }
}
