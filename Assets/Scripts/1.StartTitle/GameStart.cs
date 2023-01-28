using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    [SerializeField] GameObject optionPanel;
    SoundManager soundManager;

    private void Start()
    {
        soundManager = SoundManager.Instance;
        soundManager.PlayBGM(0, true);
    }

    public void ClickStart(string scene)
    {
        soundManager.PlayES("StartButton");
        SceneManager.LoadScene(scene);
    }

    public void ClickOption()
    {
        soundManager.PlayES("SelectButton");
        optionPanel.SetActive(true);
    }

    public void ClickExit()
    {
        soundManager.PlayES("SelectButton");
        Application.Quit();
    }
}
