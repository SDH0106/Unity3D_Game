using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSceneUI : MonoBehaviour
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
    [SerializeField] GameObject PauseUI;
    [SerializeField] Slider Sound;

    [Header("Text")]
    [SerializeField] GameObject clearText;
    [SerializeField] GameObject gameOverUI;
    [SerializeField] GameObject statCardParent;

    GameManager gameManager;

    private void Start()
    {
        SoundManager.Instance.PlayBGM(1);
        gameManager = GameManager.Instance;
        clearText.SetActive(false);
        gameOverUI.SetActive(false);
        statCardParent.gameObject.SetActive(false);
        PauseUI.SetActive(false);
    }

    private void Update()
    {
        HpUI();
        ExpUI();
        CoinUI();
        RoundUI();
        TimeUI();

        if (gameManager.hp > 0)
        {
            if (gameManager.isClear && gameManager.isBossDead)
            {
                clearText.SetActive(true);
            }

            if (clearText.GetComponent<TypingText>().isOver == true)
            {
                clearText.SetActive(false);

                if (gameManager.levelUpCount <= 0)
                    gameManager.ToShopScene();

                if (gameManager.levelUpCount > 0)
                    statCardParent.gameObject.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.Escape) && !gameManager.isPause)
                PauseGame();

            else if (Input.GetKeyDown(KeyCode.Escape) && gameManager.isPause)
                ReturnToGame();
        }

        else if (gameManager.hp <= 0)
            gameOverUI.SetActive(true);
    }

    void HpUI()
    {
        maxHpText.text = gameManager.maxHp.ToString();
        hpText.text = gameManager.hp.ToString();
        hpBar.value = 1 - (gameManager.hp / gameManager.maxHp);
    }

    void ExpUI()
    {
        lvText.text = gameManager.level.ToString();
        expBar.value = 1- (gameManager.exp / gameManager.maxExp);
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
    
    public void VolumeChange()
    {
        SoundManager.Instance.Volume(1 - Sound.value);
    }

    public void PauseGame()
    {
        if (gameManager.hp > 0)
        {
            PauseUI.SetActive(true);
            gameManager.isPause = true;
            Time.timeScale = 0;
        }
    }

    public void TitleScene()
    {
        Destroy(GameManager.Instance.gameObject);
        Destroy(Character.Instance.gameObject);
        Destroy(ItemManager.Instance.gameObject);
        Destroy(SoundManager.Instance.gameObject);
        SceneManager.LoadScene("StartTitle");
    }

    public void ReturnToGame()
    {
        PauseUI.SetActive(false);
        gameManager.isPause = false;
        Time.timeScale = 1;
    } 
}
