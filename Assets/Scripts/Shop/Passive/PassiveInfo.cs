using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WeaponInfo;

[CreateAssetMenu(fileName = "new Item", menuName = "GameData/Item/Passive")]
public class PassiveInfo : ScriptableObject
{
    [SerializeField] Sprite itemSprite;
    [SerializeField] Grade itemGrade;
    [SerializeField] string itemName;
    [SerializeField] float hp;
    [SerializeField] float recoverHp;
    [SerializeField] float absorbHp;
    [SerializeField] float defence;
    [SerializeField] float physicDamage;
    [SerializeField] float elementDamage;
    [SerializeField] float attackSpeed;
    [SerializeField] float speed;
    [SerializeField] float range;
    [SerializeField] float luck;
    [SerializeField] int itemPrice;
    [SerializeField] string description;

    public float weight;

    public Sprite ItemSprite => itemSprite;
    public Grade ItemGrade => itemGrade;
    public string ItemName => itemName;
    public float PhysicDamage => physicDamage;
    public float ElementDamage => elementDamage;
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
