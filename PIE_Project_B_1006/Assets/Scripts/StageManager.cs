using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    GameManager gm;
    private void Start()
    {

        gm = GameManager.Instance;
        if (gm != null)
        {
            gm.StageSetting(1, 40, 5, 3);
            gm.StageSetting(2, 40, 5, 3);
            gm.StageSetting(3, 40, 5, 3);
            gm.StageSetting(4, 40, 5, 3);
            gm.StageSetting(5, 40, 5, 3);
        }
        else
        {
            Debug.Log("GameManager를 찾을수 없습니다");
        }
    }
}
