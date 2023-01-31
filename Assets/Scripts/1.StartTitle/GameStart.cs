using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    [SerializeField] GameObject optionPanel;
    [SerializeField] GameObject gameInfoPanel;
    [SerializeField] GameObject etcInfoPanel;
    SoundManager soundManager;

    private void Start()
    {
        optionPanel.SetActive(false);
        gameInfoPanel.SetActive(false);
        etcInfoPanel.SetActive(false);

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

    public void OnSelectSound()
    {
        soundManager.PlayES("SelectButton");
    }
}
