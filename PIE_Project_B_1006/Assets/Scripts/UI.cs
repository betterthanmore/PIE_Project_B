using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public void startButton()
    {
        SceneManager.LoadScene("StageScene");
    }
    public void ExitButton()
    {
        Application.Quit();
    }
    public void StageButton(int stage)
    {
        GameManager.Instance.stage = stage;
        SceneManager.LoadScene("SampleScene");
    }
}