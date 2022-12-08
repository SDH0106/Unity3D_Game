using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using static WeaponInfo;

[CreateAssetMenu(fileName = "new Item", menuName = "GameData/Item/Passive")]
public class PassiveInfo : ScriptableObject
{
    [SerializeField] Sprite itemSprite;
    [SerializeField] Grade itemGrade;
    [SerializeField] string itemName;
    [SerializeField] int itemPrice;
    [SerializeField] int maxCount;
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
    [SerializeField] float critical;
    [SerializeField] float salePercent;
    /*[SerializeField] float coinRange;
    [SerializeField] int coinCount;
    [SerializeField] int monsterCount;
    [SerializeField] float monsterSpeed;
    [SerializeField] int exp;
    [SerializeField] int waveCount;
    [SerializeField] int dashCount;
    [SerializeField] int fireCount;
    [SerializeField] int gradeCost;*/
    [SerializeField] string description;
    
    [HideInInspector] public float weight;


    public Sprite ItemSprite => itemSprite;
    public Grade ItemGrade => itemGrade;
    public string ItemName => itemName;
    public int MaxCount => maxCount;
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
    public int ItemPrice => itemPrice;
    public float Critical => critical;
    public float SalePercent => salePercent;
    public string Description => description;
}
