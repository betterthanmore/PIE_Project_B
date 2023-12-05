using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneMove : MonoBehaviour
{
   public void SceneMovement(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
        
}
