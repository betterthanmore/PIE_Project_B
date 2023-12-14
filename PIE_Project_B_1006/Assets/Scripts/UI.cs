using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{

    public void startButton()
    {
        SceneManager.LoadScene("CutScene");
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
    public void ReturnButton()
    {
        SceneManager.LoadScene("StageScene");
    }    
}
