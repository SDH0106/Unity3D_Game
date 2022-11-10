using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{
    [Header("HP")]
    [SerializeField] Text hpText;
    [SerializeField] Text maxHpText;
    [SerializeField] Slider hpBar;
    [SerializeField] Slider playerHpBar;

    [Header("EXP")]
    [SerializeField] Slider expBar;

    [Header("COIN")]
    [SerializeField] Text coinText;

    Character character;

    private void Start()
    {
        character = Character.Instance;
    }

    private void Update()
    {
        HpUI();
        ExpUI();
        CoinUI();
    }

    void HpUI()
    {
        maxHpText.text = character.maxHp.ToString();
        hpText.text = character.hp.ToString();
        hpBar.value = 1 - ((float)character.hp / (float)character.maxHp);
        playerHpBar.value = hpBar.value;
    }

    void ExpUI()
    {
        expBar.value = 1- (character.exp / character.maxExp);
    }

    void CoinUI()
    {
        coinText.text = character.money.ToString();
    }
}
