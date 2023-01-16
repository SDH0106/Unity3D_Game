using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Experimental.XR.Interaction;
using static WeaponInfo;

[CreateAssetMenu(fileName = "new Item", menuName = "GameData/Item/Passive")]
public class PassiveInfo : ScriptableObject
{
    [SerializeField] Sprite itemSprite;
    [SerializeField] Grade itemGrade;
    [SerializeField] string itemName;
    [SerializeField] int itemPrice;
    [SerializeField] int maxCount;

    [Header("Stat")]
    [SerializeField] float hp;
    [SerializeField] float recoverHp;
    [SerializeField] float absorbHp;
    [SerializeField] float defence;
    [SerializeField] float physicDamage;
    [SerializeField] float elementDamage;
    [SerializeField] float shortDamage;
    [SerializeField] float longDamage;
    [SerializeField] float attackSpeed;
    [SerializeField] float speed;
    [SerializeField] float range;
    [SerializeField] float luck;
    [SerializeField] float critical;
    [SerializeField] float percentDamage;
    [SerializeField] float avoid;

    [Header("Item")]
    [SerializeField] float coinRange;
    [SerializeField] float increaseExp;
    [SerializeField] float monsterSpeed;
    [SerializeField] float salePercent;
    [SerializeField] int dashCount;
    [SerializeField] bool luckCoin;
    [SerializeField] bool luckDamage;
    [SerializeField] bool luckCritical;
    [SerializeField] bool doubleShot;
    [SerializeField] bool revive;
    [SerializeField] string description;

    [HideInInspector] public float weight;

    public Sprite ItemSprite => itemSprite;
    public Grade ItemGrade => itemGrade;
    public string ItemName => itemName;
    public int MaxCount => maxCount;
    public float PhysicDamage => physicDamage;
    public float ElementDamage => elementDamage;
    public float ShortDamage => shortDamage;
    public float LongDamage => longDamage;
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
    public float MonsterSpeed => monsterSpeed;
    public float SalePercent => salePercent;
    public int DashCount => dashCount;
    public float CoinRange => coinRange;
    public float IncreaseExp => increaseExp;
    public bool LuckCoin => luckCoin;
    public bool LuckDamage => luckDamage;
    public bool LuckCritical => luckCritical;
    public bool DoubleShot => doubleShot;
    public bool Revive => revive;
    public string Description => description;
    public float PercentDamage => percentDamage;
    public float Avoid => avoid;
}
