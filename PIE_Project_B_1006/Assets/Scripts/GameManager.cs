using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : GenericSingleton<GameManager>
{
   public List<StageInfo> stages = new List<StageInfo>();
   public void GameOver()
    {
        Debug.Log("GAMEOVER");
    }

    public void StageSetting(int stageNum, int move)
    {
        StageInfo _stageInfo = new StageInfo
        {
            stage = stageNum,
            isCleared = false,
            moveAmount = move
        };
        stages.Add(_stageInfo);
    }
    public void StageClear(int stageNum)
    {
        StageInfo _stageInfo = stages.Find(stages => stages.stage == stageNum);

        if(_stageInfo != null)
        {
            _stageInfo.isCleared = true;
        }
        else
        {
            Debug.LogWarning($"{stageNum}단계 관련 클래스가 존재하지 않습니다.");
        }
    }
    [System.Serializable]
    public class StageInfo
    {
        public int stage;
        public bool isCleared = false;
        public int moveAmount;
    }

}
