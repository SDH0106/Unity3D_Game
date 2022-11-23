using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    SoundManager soundManager;

    private void Start()
    {
        soundManager = SoundManager.Instance;
        soundManager.PlayBGM(0);
    }

    public void ClickStart(string scene)
    {
        soundManager.PlayES("Start");
        SceneManager.LoadScene(scene);
    }

    public void ClickOption()
    {
        soundManager.PlayES("SelectButton");
    }

    public void ClickExit()
    {
        soundManager.PlayES("SelectButton");
        Application.Quit();
    }
}
