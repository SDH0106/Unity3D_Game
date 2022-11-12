using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    public void ClickStart(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void ClickOption()
    {

    }

    public void ClickExit()
    {
        Application.Quit();
    }
}
