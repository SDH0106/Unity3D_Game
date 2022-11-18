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

    private void Start()
    {
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

        if (GameManager.Instance.currentGameTime <= 0)
        {
            clearText.SetActive(true);
        }

        if (clearText.GetComponent<TypingText>().isOver == true)
        {
            clearText.SetActive(false);

            if (GameManager.Instance.levelUpCount <= 0)
                GameManager.Instance.ToShopScene();

            if (GameManager.Instance.levelUpCount > 0)
                statCardParent.gameObject.SetActive(true);
        }
    }

    void HpUI()
    {
        maxHpText.text = GameManager.Instance.maxHp.ToString();
        hpText.text = GameManager.Instance.hp.ToString();
        hpBar.value = 1 - (GameManager.Instance.hp / GameManager.Instance.maxHp);
    }

    void ExpUI()
    {
        expBar.value = 1- (GameManager.Instance.exp / GameManager.Instance.maxExp);
    }

    void CoinUI()
    {
        coinText.text = GameManager.Instance.money.ToString();
    }

    void RoundUI()
    {
        roundText.text = GameManager.Instance.round.ToString();
    }

    void TimeUI()
    {
        if (GameManager.Instance.currentGameTime >= 1)
            timeText.text = ((int)GameManager.Instance.currentGameTime).ToString();
        else
        {
            timeText.color = Color.red;
            timeText.text = (GameManager.Instance.currentGameTime).ToString("F2");
        }
    }
}
