using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WeaponInfo;

[CreateAssetMenu(fileName = "new Item", menuName = "GameData/Item/Passive")]
public class PassiveInfo : ScriptableObject
{
    [SerializeField] Sprite itemSprite;
    [SerializeField] string itemName;
    [SerializeField] float hp;
    [SerializeField] float recoverHp;
    [SerializeField] float absorbHp;
    [SerializeField] float defence;
    [SerializeField] int weaponDamage;
    [SerializeField] int elementDamage;
    [SerializeField] float attackSpeed;
    [SerializeField] float speed;
    [SerializeField] float range;
    [SerializeField] float luck;
    [SerializeField] int itemPrice;
    [SerializeField] string description;

    public Sprite ItemSprite => itemSprite;
    public string ItemName => itemName;
    public int WeaponDamage => weaponDamage;
    public int ElementDamage => elementDamage;
    public float Range => range;
    public float Defence => defence;
    public float Speed => speed;
    public float Luck => luck;
    public float Hp => hp;
    public float AbsorbHp => absorbHp;
    public float RecoverHp => recoverHp;
    public float AttackSpeed => attackSpeed;
    public string Description => description;
    public int ItemPrice => itemPrice;
}
