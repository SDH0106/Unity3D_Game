using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class GameSceneUI : MonoBehaviour
{
    [Header("HP")]
    [SerializeField] Text hpText;
    [SerializeField] Text maxHpText;
    [SerializeField] Slider hpBar;

    [Header("EXP")]
    [SerializeField] Slider expBar;

    [Header("COIN")]
    [SerializeField] Text coinText;

    [Header("Time")]
    [SerializeField] Text timeText;

    [Header("Round")]
    [SerializeField] Text roundText;

    [SerializeField] public Transform bulletStorage;
    [SerializeField] public Transform damageStorage;
    [SerializeField] GameObject clearText;
    [SerializeField] GameObject statCardParent;

    GameManager gameManager;

    private void Start()
    {
        SoundManager.Instance.PlayBGM(1);
        SoundManager.Instance.Volume(0.3f);
        gameManager = GameManager.Instance;
        clearText.SetActive(false);
        statCardParent.gameObject.SetActive(false);
    }

    private void Update()
    {
        HpUI();
        ExpUI();
        CoinUI();
        RoundUI();
        TimeUI();

        if (gameManager.currentGameTime <= 0)
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
    }

    void HpUI()
    {
        maxHpText.text = gameManager.maxHp.ToString();
        hpText.text = gameManager.hp.ToString();
        hpBar.value = 1 - (gameManager.hp / gameManager.maxHp);
    }

    void ExpUI()
    {
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
}
