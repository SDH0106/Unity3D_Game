using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum KeyAction { UP, DOWN ,LEFT, RIGHT, DASH, COUNT}

public static class KeySetting { public static Dictionary<KeyAction, KeyCode> keys = new Dictionary<KeyAction, KeyCode>(); }

public class TitleOption : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] Slider AllSound;
    [SerializeField] Slider bgmSound;
    [SerializeField] Slider sfxSound;
    [SerializeField] GameObject wUnMark;
    [SerializeField] GameObject bUnMark;
    [SerializeField] GameObject sUnMark;

    SoundManager soundManager;

    bool muteAllVolume;
    bool muteBgmVolume;
    bool muteSfxVolume;

    KeyCode[] defaultKeys = new KeyCode[] { KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D, KeyCode.Space };

    int key = -1;

    [SerializeField] Text[] keyTexts;

    private void Start()
    {
        panel.SetActive(false);

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

        for (int i = 0; i < (int)KeyAction.COUNT; i++)
        {
            if (!KeySetting.keys.ContainsKey((KeyAction)i))                 // Dictionary keys에 값이 없다면
            {
                KeySetting.keys.Add((KeyAction)i, defaultKeys[i]);          // keys값에 default값 대입
                
            }
        }

        for (int i = 0; i < keyTexts.Length; i++)
        {
            keyTexts[i].text = KeySetting.keys[(KeyAction)i].ToString();    // keyText에 keys의 keyCode값 대입
        }
    }

    private void Update()
    {
        wUnMark.SetActive(muteAllVolume);
        bUnMark.SetActive(muteBgmVolume);
        sUnMark.SetActive(muteSfxVolume);

        for (int i = 0; i < keyTexts.Length; i++)
        {
            keyTexts[i].text = KeySetting.keys[(KeyAction)i].ToString();
        }
    }

    private void OnGUI()
    {
        Event keyEvent = Event.current;     // 지금 진행되고 있는 이벤트 확인
        if (keyEvent.isKey)                 // 키보드 입력이 있다면 
        {
            KeySetting.keys[(KeyAction)key] = keyEvent.keyCode;     // 지금 입력된 키를 받아와 KeySetting 클래스의 Dictionary형 변수 keys의 key번째에 등록.
            key = -1;
        }
    }

    public void ChangeKey(int num)
    {
        key = num;      // 버튼에 함수를 등록시키고 num값을 받아와 key값에 num값을 대입
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

    public void BackTitle()
    {
        PlayerPrefs.SetInt("Mute_All", Convert.ToInt32(muteAllVolume));
        PlayerPrefs.SetInt("Mute_Bgm", Convert.ToInt32(muteBgmVolume));
        PlayerPrefs.SetInt("Mute_Sfx", Convert.ToInt32(muteSfxVolume));
        panel.SetActive(false);
    }
}
