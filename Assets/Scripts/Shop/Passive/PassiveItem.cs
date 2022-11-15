using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WeaponInfo;

public class PassiveItem : MonoBehaviour
{
    [HideInInspector] public string itemName;
    float hp;
    float recoverHp;
    float absorbHp;
    float defence;
    int weaponDamage;
    int elementDamage;
    float attackSpeed;
    float speed;
    float range;
    float luck;
    string itemPrice;

    SpriteRenderer spriteRenderer;

    PassiveInfo passiveInfo;

    private void Start()
    {
        passiveInfo = ItemManager.Instance.passiveCardItem;
        spriteRenderer = GetComponent<SpriteRenderer>();
        Setting();
    }

    void Setting()
    {
        spriteRenderer.sprite = passiveInfo.ItemSprite;
        itemName = passiveInfo.ItemName;
        hp = passiveInfo.Hp;
        recoverHp = passiveInfo.RecoverHp;
        absorbHp = passiveInfo.AbsorbHp;
        defence = passiveInfo.Defence;
        weaponDamage = passiveInfo.WeaponDamage;
        elementDamage = passiveInfo.ElementDamage;
        attackSpeed = passiveInfo.AttackSpeed;
        speed = passiveInfo.Speed;
        range = passiveInfo.Range;
        luck = passiveInfo.Luck;
        itemName = passiveInfo.ItemName;
    }
}
