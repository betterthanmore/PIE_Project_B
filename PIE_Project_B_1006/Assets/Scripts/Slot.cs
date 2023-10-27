using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public enum SLOTSTATE       //���Ի��°�
    {
        EMPTY,
        FULL
    }

    public int id;                              //���� ��ȣ ID
    public Item itemObject;                     //������ Ŀ���� Class ID
    public SLOTSTATE state = SLOTSTATE.EMPTY;   //Enum �� ����
    public int x, y;
    public int tileType = 0;
    public int moveAmount;
    public int xOry;
    public int clearId;

    private void ChangeStateTo(SLOTSTATE targetState)
    {//�ش� ������ ���°��� ��ȯ �����ִ� �Լ�
        state = targetState;
    }

    public void ItemGrabbed()
    {//RayCast�� ���ؼ� �������� ����� ��
        Destroy(itemObject.gameObject);         //���� �������� ����
        ChangeStateTo(SLOTSTATE.EMPTY);         //������ �� ����
        if (tileType == 2)
        {
            isCleared(false);
        }
    }
    public void isCleared(bool y = true)        //�׳� ���� Ŭ���� false���� Ŭ���� ���� Ƚ�� ����
    {
        GameManager.StageInfo _stageInfo = GameManager.Instance.stages.Find(stages => stages.stage == GameManager.Instance.stage);          // ���Ӹ޴������� �� �������� ���� Ŭ������ ����Ʈ���� �ҷ���
        if (y == true)
        {
            if (itemObject.id == _stageInfo.clearItemId)
            {
                if (_stageInfo != null)
                {
                    _stageInfo.clearAmount -= 1;            // �� ����Ʈ���� Ŭ���� ���� Ƚ���� ������
                    if (_stageInfo.clearAmount == 0)
                    {
                        GameManager.Instance.StageClear(GameManager.Instance.stage);
                    }
                }
            }

        }
        else
        {
            if (_stageInfo != null)
            {
                _stageInfo.clearAmount += 1;            // �� ����Ʈ���� Ŭ���� ���� Ƚ���� ������
            }
        }

    }
    public void CreateItem(int id)
    {
        //������ ��δ� (Resources/Prefabs/Item_000)
        string itemPath = "Prefabs/Item_" + id.ToString("000");
        var itemGo = (GameObject)Instantiate(Resources.Load(itemPath));

        itemGo.transform.SetParent(this.transform);
        itemGo.transform.localPosition = Vector3.zero;
        itemGo.transform.localScale = Vector3.one;
        //������ �� ������ �Է�
        itemObject = itemGo.GetComponent<Item>();
        itemObject.Init(id, this); //�Լ��� ���� �� �Է�(this -> Slot Class)

        ChangeStateTo(SLOTSTATE.FULL);

    }
}