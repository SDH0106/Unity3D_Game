using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WeaponInfo;

public class PassiveItem : MonoBehaviour
{
    public string itemName;
    float hp;
    float recoverHp;
    float absorbHp;
    int defence;
    int weaponDamage;
    int elementDamage;
    float attackSpeed;
    float range;
    float speed;
    int luck;

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
        hp = passiveInfo.HP;
        recoverHp = passiveInfo.RecoverHp;
        absorbHp = passiveInfo.AbsorbHp;
        defence = passiveInfo.Defence;
        weaponDamage = passiveInfo.WeaponDamage;
        elementDamage = passiveInfo.ElementDamage;
        attackSpeed = passiveInfo.AttackSpeed;
        range = passiveInfo.Range;
        speed = passiveInfo.Speed;
        luck = passiveInfo.Luck;
    }
}
