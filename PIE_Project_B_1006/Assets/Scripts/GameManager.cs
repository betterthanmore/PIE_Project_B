using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : GenericSingleton<GameManager>
{
    public List<StageInfo> stages = new List<StageInfo>();
    public int stage;
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log(scene.name);
        Debug.Log(mode);
        if (scene.name == "SampleScene")
        {
            SelectStage();
        }
    }
    public void GameOver()
    {
        Debug.Log("GAMEOVER");
    }

    public void StageSetting(int stageNum, int move, int itemId, int amount)
    {
        StageInfo _stageInfo = new StageInfo
        {
            stage = stageNum,
            isCleared = false,
            moveAmount = move,
            clearItemId = itemId,
            clearAmount = amount
        };
        stages.Add(_stageInfo);
    }
    public void StageClear(int stageNum)
    {
        StageInfo _stageInfo = stages.Find(stages => stages.stage == stageNum);

        if (_stageInfo != null)
        {
            _stageInfo.isCleared = true;
        }
        else
        {
            Debug.LogWarning($"{stageNum}단계 관련 클래스가 존재하지 않습니다.");
        }
    }

    public void SelectStage()
    {
        GameObject newObject = new GameObject("GameController");

        GameController gameController = newObject.AddComponent<GameController>();

        gameController.stage = stage;
    }

    [System.Serializable]
    public class StageInfo
    {
        public int stage;
        public bool isCleared = false;
        public int moveAmount;
        public int clearItemId;
        public int clearAmount;
    }

}
