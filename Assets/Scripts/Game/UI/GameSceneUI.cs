using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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

    [Header("Dash")]
    [SerializeField] GameObject dash;
    [SerializeField] Image dashImage;
    [SerializeField] Text dashCoolTime;
    [SerializeField] GameObject dashCountParent;
    [SerializeField] Text dashCount;
    [SerializeField] Text maxDashCount;

    [Header("Text")]
    [SerializeField] GameObject roundClearText;
    [SerializeField] GameObject gameOverUI;
    [SerializeField] GameObject gameOverText;
    [SerializeField] GameObject gameClearUI;
    [SerializeField] GameObject gameClearText;
    [SerializeField] GameObject statCardParent;

    GameManager gameManager;
    Character character;

    private void Start()
    {
        SoundManager.Instance.PlayBGM(1);
        gameManager = GameManager.Instance;
        character = Character.Instance;
        roundClearText.SetActive(false);
        gameOverUI.SetActive(false);
        statCardParent.gameObject.SetActive(false);
        PauseUI.SetActive(false);
        gameClearUI.SetActive(false);
        dash.SetActive(false);
    }

    private void Update()
    {
        HpUI();
        ExpUI();
        CoinUI();
        RoundUI();
        TimeUI();
        DashUI();

        if (!character.isDead)
        {
            if (gameManager.round != 20)
            {
                if (gameManager.isClear && gameManager.isBossDead)
                {
                    roundClearText.SetActive(true);
                }

                if (roundClearText.GetComponent<TypingText>().isOver == true)
                {
                    roundClearText.SetActive(false);

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

            else if(gameManager.round == 20)
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

    void DashUI()
    {
        if (gameManager.dashCount > 0)
        {
            dash.SetActive(true);
            Color color = dashImage.color;

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
        if (!character.isDead)
        {
            PauseUI.SetActive(true);
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
        PauseUI.SetActive(false);
        gameManager.isPause = false;
        Time.timeScale = 1;
    } 
}
