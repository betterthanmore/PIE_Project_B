using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public int stage;
    public int move;
    void Start()
    {
        GameManager.Instance.StageSetting(stage, move);
        GetComponent<GameController>().stage = stage;
    }
}
