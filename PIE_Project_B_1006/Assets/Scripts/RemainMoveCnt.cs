using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RemainMoveCnt : MonoBehaviour
{

    public TMP_Text text;
    int stage;

    public void Start()
    {
        stage = GameManager.Instance.stage;
        text = GetComponent<TMP_Text>();
        text.text = $"Move : {GameManager.Instance.stages[stage - 1].moveAmount}";
    }

    public void Update()
    {
        text.text = $"Move : {GameManager.Instance.stages[stage - 1].moveAmount}";

        if(GameManager.Instance.stages[stage - 1].moveAmount <= 0)
        {
            text.text = "move : 0 ";
        }
    }




}
