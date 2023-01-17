using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Unity.VisualScripting;

public class OptionUI : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] Slider AllSound;
    [SerializeField] Slider bgmSound;
    [SerializeField] Slider sfxSound;
    [SerializeField] GameObject wUnMark;
    [SerializeField] GameObject bUnMark;
    [SerializeField] GameObject sUnMark;

    GameManager gameManager;
    SoundManager soundManager;

    bool muteAllVolume;
    bool muteBgmVolume;
    bool muteSfxVolume;

    private void Start()
    {
        panel.SetActive(false);

        gameManager = GameManager.Instance;
        soundManager = SoundManager.Instance;

        AllSound.value = 1 - PlayerPrefs.GetFloat("Sound_All");
        bgmSound.value = 1 - PlayerPrefs.GetFloat("Sound_Bgm");
        sfxSound.value = 1 - PlayerPrefs.GetFloat("Sound_Sfx");

        muteAllVolume = Convert.ToBoolean(PlayerPrefs.GetInt("Mute_All", 0));
        muteBgmVolume = Convert.ToBoolean(PlayerPrefs.GetInt("Mute_Bgm", 0));
        muteSfxVolume = Convert.ToBoolean(PlayerPrefs.GetInt("Mute_Sfx", 0));

        wUnMark.SetActive(muteAllVolume);
        bUnMark.SetActive(muteBgmVolume);
        sUnMark.SetActive(muteSfxVolume);
    }

    private void Update()
    {
        wUnMark.SetActive(muteAllVolume);
        bUnMark.SetActive(muteBgmVolume);
        sUnMark.SetActive(muteSfxVolume);

        if (Input.GetKeyDown(KeyCode.Escape)&& !gameManager.isPause)
            PauseGame();

        else if (Input.GetKeyDown(KeyCode.Escape) && gameManager.isPause)
            ReturnToGame();
    }

    public void ChangeWholeVolume()
    {
        if (panel.activeSelf)
        {
            if (AllSound.value == 1)
            {
                muteAllVolume = true;
                muteBgmVolume = muteAllVolume;
                muteSfxVolume = muteAllVolume;
            }

            else if (AllSound.value != 1 && !muteAllVolume)
            {
                muteAllVolume = false;

                if (!muteBgmVolume)
                    muteBgmVolume = muteAllVolume;

                if (!muteSfxVolume)
                    muteSfxVolume = muteAllVolume;
            }

            soundManager.WholeVolume((1 - AllSound.value), muteBgmVolume, muteSfxVolume);
        }
    }

    public void OnOffWholeVolume()
    {
        if (AllSound.value != 1)
            muteAllVolume = !muteAllVolume;

        if (bgmSound.value != 1)
            muteBgmVolume = muteAllVolume;

        if (sfxSound.value != 1)
            muteSfxVolume = muteAllVolume;

        soundManager.WholeVolumeOnOff(muteAllVolume);
    }

    public void ChangeBgmVolume()
    {
        if (panel.activeSelf)
        {
            if (bgmSound.value == 1)
            {
                muteBgmVolume = true;
            }

            else if (bgmSound.value != 1 && !muteBgmVolume)
            {
                if (!muteAllVolume)
                    muteBgmVolume = false;
            }

            soundManager.BgmVolume((1 - bgmSound.value), muteBgmVolume);
        }
    }

    public void OnOffBgmVolume()
    {
        muteBgmVolume = !muteBgmVolume;
        soundManager.BgmOnOff(muteBgmVolume);

        if (muteBgmVolume == false)
            muteAllVolume = false;
    }

    public void ChangeSfxVolume()
    {
        if (panel.activeSelf)
        {
            if (sfxSound.value == 1)
                muteSfxVolume = true;

            else if (sfxSound.value != 1 && !muteSfxVolume)
            {
                if (!muteAllVolume)
                    muteSfxVolume = false;
            }

            soundManager.EsVolume((1 - sfxSound.value), muteSfxVolume);
        }
    }

    public void OnOffSfxVolume()
    {
        muteSfxVolume = !muteSfxVolume;
        soundManager.SfxOnOff(muteSfxVolume);

        if (muteSfxVolume == false)
            muteAllVolume = false;
    }

    public void PauseGame()
    {
        if (!gameManager.isPause)
        {
            panel.SetActive(true);
            gameManager.isPause = true;
            Time.timeScale = 0;
        }
    }

    public void ReturnToGame()
    {
        if (gameManager.isPause)
        {
            panel.SetActive(false);
            gameManager.isPause = false;
            Time.timeScale = 1;
        }
    }

    public void TitleScene()
    {
        if(SceneManager.GetActiveScene().buildIndex > 1)
            Destroy(Character.Instance.gameObject);

        else if (SceneManager.GetActiveScene().buildIndex > 2)
            Destroy(ItemManager.Instance.gameObject);

        if (SoundManager.Instance.gameObject != null)
            Destroy(SoundManager.Instance.gameObject);

        if (gameManager.gameObject != null)
            Destroy(gameManager.gameObject);

        SceneManager.LoadScene("StartTitle");
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetInt("Mute_All", Convert.ToInt32(muteAllVolume));
        PlayerPrefs.SetInt("Mute_Bgm", Convert.ToInt32(muteBgmVolume));
        PlayerPrefs.SetInt("Mute_Sfx", Convert.ToInt32(muteSfxVolume));
    }
}