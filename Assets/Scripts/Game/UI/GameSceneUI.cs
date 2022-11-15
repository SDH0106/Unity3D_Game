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

    private void Update()
    {
        HpUI();
        ExpUI();
        CoinUI();
        RoundUI();
        TimeUI();
    }

    void HpUI()
    {
        maxHpText.text = GameManager.Instance.maxHp.ToString();
        hpText.text = GameManager.Instance.hp.ToString();
        hpBar.value = 1 - ((float)GameManager.Instance.hp / (float)GameManager.Instance.maxHp);
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
