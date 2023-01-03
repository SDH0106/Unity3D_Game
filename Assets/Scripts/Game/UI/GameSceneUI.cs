using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSceneUI : Singleton<GameSceneUI>
{
    [Header("HP")]
    [SerializeField] Text hpText;
    [SerializeField] Text maxHpText;
    [SerializeField] Slider hpBar;

    [Header("EXP")]
    [SerializeField] Text lvText;
    [SerializeField] Slider expBar;

    [Header("COIN")]
    [SerializeField] Text coinText;

    [Header("Time")]
    [SerializeField] Text timeText;

    [Header("Round")]
    [SerializeField] Text roundText;

    [Header("Sound")]
    [SerializeField] GameObject pauseUI;
    [SerializeField] Slider wholeSound;
    [SerializeField] Slider bgmSound;
    [SerializeField] Slider esSound;
    [SerializeField] GameObject wUnMark;
    [SerializeField] GameObject bUnMark;
    [SerializeField] GameObject sUnMark;

    [Header("Dash")]
    [SerializeField] GameObject dash;
    [SerializeField] Image dashImage;
    [SerializeField] Text dashCoolTime;
    [SerializeField] GameObject dashCountParent;
    [SerializeField] Text dashCount;
    [SerializeField] Text maxDashCount;

    [Header("Stat")]
    [SerializeField] GameObject statWindow;
    [SerializeField] Text maxHp;
    [SerializeField] Text reHp;
    [SerializeField] Text apHp;
    [SerializeField] Text def;
    [SerializeField] Text wAtk;
    [SerializeField] Text eAtk;
    [SerializeField] Text sAtk;
    [SerializeField] Text lAtk;
    [SerializeField] Text aSpd;
    [SerializeField] Text spd;
    [SerializeField] Text ran;
    [SerializeField] Text luk;
    [SerializeField] Text cri;

    [Header("Text")]
    [SerializeField] GameObject roundClearText;
    [SerializeField] GameObject gameOverUI;
    [SerializeField] GameObject gameOverText;
    [SerializeField] GameObject gameClearUI;
    [SerializeField] GameObject gameClearText;
    [SerializeField] GameObject statCardParent;
    [SerializeField] GameObject chestPassive;

    GameManager gameManager;
    Character character;
    SoundManager soundManager;

    bool muteWholeVolume;
    bool muteBgmVolume;
    bool muteSfxVolume;

    [HideInInspector] public int chestCount;

    private void Start()
    {
        gameManager = GameManager.Instance;
        character = Character.Instance;
        soundManager = SoundManager.Instance;
        soundManager.PlayBGM(1);
        roundClearText.SetActive(false);
        gameOverUI.SetActive(false);
        statCardParent.SetActive(false);
        pauseUI.SetActive(false);
        gameClearUI.SetActive(false);
        dash.SetActive(false);
        statWindow.SetActive(false);
        chestPassive.SetActive(false);
        muteWholeVolume = false;
        muteBgmVolume = false;
        muteSfxVolume = false;
        wUnMark.SetActive(false);
        bUnMark.SetActive(false);
        sUnMark.SetActive(false);
    }

    private void Update()
    {
        HpUI();
        ExpUI();
        CoinUI();
        RoundUI();
        TimeUI();
        DashUI();
        SettingStatText();

        wUnMark.SetActive(muteWholeVolume);
        bUnMark.SetActive(muteBgmVolume);
        sUnMark.SetActive(muteSfxVolume);

        if (!character.isDead)
        {
            if (gameManager.round != 30)
            {
                if (gameManager.isClear && gameManager.isBossDead)
                {
                    roundClearText.SetActive(true);
                }

                if (roundClearText.GetComponent<TypingText>().isOver == true)
                {
                    roundClearText.SetActive(false);

                    if (character.levelUpCount <= 0 && chestCount <= 0)
                    {
                        gameManager.ToShopScene();
                    }

                    if (character.levelUpCount > 0)
                    {
                        statCardParent.gameObject.SetActive(true);
                        statWindow.SetActive(true);
                    }

                    if (character.levelUpCount <= 0 && chestCount > 0)
                    {
                        statCardParent.gameObject.SetActive(false);
                        chestPassive.gameObject.SetActive(true);
                        statWindow.SetActive(true);
                    }
                }

                if (Input.GetKeyDown(KeyCode.Escape) && !gameManager.isPause)
                    PauseGame();

                else if (Input.GetKeyDown(KeyCode.Escape) && gameManager.isPause)
                    ReturnToGame();
            }

            else if(gameManager.round == 30)
            {
                if (gameManager.isClear && gameManager.isBossDead)
                {
                    gameClearUI.SetActive(true);
                }

                if (gameClearText.GetComponent<TypingText>().isOver == true)
                    SceneManager.LoadScene("End");
            }    
        }

        else if (character.isDead)
        {
            gameOverUI.SetActive(true);

            if (gameOverText.GetComponent<TypingText>().isOver == true)
                SceneManager.LoadScene("End");
        }
    }

    void SettingStatText()
    {
        maxHp.text = character.maxHp.ToString();
        reHp.text = gameManager.recoverHp.ToString("0.#");
        apHp.text = gameManager.absorbHp.ToString("0.#");
        def.text = gameManager.defence.ToString("0.#");
        wAtk.text = gameManager.physicDamage.ToString("0.#");
        eAtk.text = gameManager.elementDamage.ToString("0.#");
        sAtk.text = gameManager.shortDamage.ToString("0.#");
        lAtk.text = gameManager.longDamage.ToString("0.#");
        aSpd.text = gameManager.attackSpeed.ToString("0.#");
        spd.text = character.speed.ToString("0.#");
        ran.text = gameManager.range.ToString("0.#");
        luk.text = gameManager.luck.ToString("0.#");
        cri.text = gameManager.critical.ToString("0.#");
    }

    void DashUI()
    {
        if (gameManager.dashCount > 0)
        {
            dash.SetActive(true);
            Color color = dashImage.color;
            dashImage.fillAmount = 1;

            if (character.dashCount == 0)
            {
                color.a = 0.5f;
                dashImage.color = color;

                dashCountParent.gameObject.SetActive(false);

                dashCoolTime.gameObject.SetActive(true);
                dashImage.fillAmount = 1 - (character.dashCoolTime / character.initDashCoolTime);
                dashCoolTime.text = character.dashCoolTime.ToString("F2");
            }

            else if (character.dashCount > 0)
            {
                color.a = 1f;
                dashImage.color = color;

                dashCountParent.gameObject.SetActive(true);
                dashCount.text = character.dashCount.ToString();
                maxDashCount.text = gameManager.dashCount.ToString();

                dashCoolTime.gameObject.SetActive(false);
            }
        }
    }

    void HpUI()
    {
        if (gameManager.currentScene == "Game")
        {
            maxHpText.text = character.maxHp.ToString();
            hpText.text = character.currentHp.ToString("0.#");
            hpBar.value = 1 - (character.currentHp / character.maxHp);
        }
    }

    void ExpUI()
    {
        lvText.text = character.level.ToString();
        expBar.value = 1- (character.exp / character.maxExp);
    }

    void CoinUI()
    {
        coinText.text = gameManager.money.ToString();
    }

    void RoundUI()
    {
        roundText.text = gameManager.round.ToString();
    }

    void TimeUI()
    {
        if (gameManager.currentGameTime >= 1)
            timeText.text = ((int)gameManager.currentGameTime).ToString();

        else
        {
            timeText.color = Color.red;
            timeText.text = (gameManager.currentGameTime).ToString("F2");
        }
    }

    public void WholeVolumeChange()
    {
        soundManager.WholeVolume(1 - wholeSound.value);

        if (wholeSound.value == 1)
        {
            muteWholeVolume = true;
            muteBgmVolume = muteWholeVolume;
            muteSfxVolume = muteWholeVolume;
        }

        else
        {
            muteWholeVolume = false;
            if (bgmSound.value != 1)
                muteBgmVolume = muteWholeVolume;
            if (esSound.value != 1)
                muteSfxVolume = muteWholeVolume;
        }
    }

    public void OnOffWholeVolume()
    {
        muteWholeVolume = !muteWholeVolume;
        if (bgmSound.value != 1)
            muteBgmVolume = muteWholeVolume;
        if (esSound.value != 1)
            muteSfxVolume = muteWholeVolume;
        soundManager.WholeVolumeOnOff(muteWholeVolume);
    }

    public void BgmVolumeChange()
    {
        soundManager.BgmVolume(1 - bgmSound.value);

        if (bgmSound.value == 1)
        {
            muteBgmVolume = true;
        }

        else
        {
            if (!muteWholeVolume)
                muteBgmVolume = false;
        }
    }

    public void OnOffBgmVolume()
    {
        if (!muteWholeVolume)
        {
            muteBgmVolume = !muteBgmVolume;
            soundManager.BgmOnOff(muteBgmVolume);
        }
    }

    public void EsVolumeChange()
    {
        soundManager.EsVolume(1 - esSound.value);

        if (esSound.value == 1)
            muteSfxVolume = true;

        else
        {
            if (!muteWholeVolume)
                muteSfxVolume = false;
        }
    }

    public void OnOffSfxVolume()
    {
        if (!muteWholeVolume)
        {
            muteSfxVolume = !muteSfxVolume;
            soundManager.SfxOnOff(muteSfxVolume);
        }
    }


    public void PauseGame()
    {
        if (!character.isDead)
        {
            pauseUI.SetActive(true);
            gameManager.isPause = true;
            Time.timeScale = 0;
        }
    }

    public void TitleScene()
    {
        Destroy(gameManager.gameObject);
        Destroy(character.gameObject);
        Destroy(ItemManager.Instance.gameObject);
        Destroy(SoundManager.Instance.gameObject);
        SceneManager.LoadScene("StartTitle");
    }

    public void ReturnToGame()
    {
        pauseUI.SetActive(false);
        gameManager.isPause = false;
        Time.timeScale = 1;
    } 
}
