using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{
    [SerializeField] Text hpText;
    [SerializeField] Text maxHpText;
    [SerializeField] Slider hpBar;
    [SerializeField] Slider playerHpBar;

    Character character;

    private void Start()
    {
        character = Character.Instance;
    }

    private void Update()
    {
        HpUI();
    }

    void HpUI()
    {
        maxHpText.text = character.maxHp.ToString();
        hpText.text = character.hp.ToString();
        hpBar.value = 1 - ((float)character.hp / (float)character.maxHp);
        playerHpBar.value = hpBar.value;
    }
}
