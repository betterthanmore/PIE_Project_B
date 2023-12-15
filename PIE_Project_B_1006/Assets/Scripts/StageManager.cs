using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            gm.StageSetting(3, 43, 5, 3);
            gm.StageSetting(4, 47, 5, 3);
            gm.StageSetting(5, 40, 5, 3);
        }
        else
        {
            Debug.Log("GameManager�� ã���� �����ϴ�");
        }

        if( SceneManager.GetActiveScene().name == "SceneName")
        {
         
        }
    }
}
