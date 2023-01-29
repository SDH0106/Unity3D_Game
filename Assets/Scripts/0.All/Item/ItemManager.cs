using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    [SerializeField] public DamageUI[] damageUI;
    [SerializeField] public Transform coinStorage;
    [SerializeField] PassiveCardUI passiveCard;

    [HideInInspector] public PassiveInfo passiveCardItem;

    [HideInInspector] public WeaponInfo[] storedWeapon;
    [HideInInspector] public PassiveInfo[] storedPassive;
    [HideInInspector] public int[] storedPassiveCount;

    [HideInInspector] public int weaponCount;
    int passiveItemCount;

    [HideInInspector] public bool isFool;
    [HideInInspector] public int foolCount;

    [HideInInspector] public Grade[] weaponGrade;

    [HideInInspector] public PassiveInfo[] lockedPassCards;
    [HideInInspector] public WeaponInfo[] lockedWeaCards;
    [HideInInspector] public bool[] cardLocks;
    [HideInInspector] public Grade[] cardGrades;

    [HideInInspector] public int[] passiveCounts;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        weaponGrade = new Grade[6];
        passiveCounts = new int[passiveCard.passiveInfo.Length];

        for (int i = 0; i < passiveCounts.Length; i++)
        {
            passiveCounts[i] = passiveCard.passiveInfo[i].MaxCount;
        }
        isFool = false;
        foolCount = 0;
        weaponCount = 0;
        passiveItemCount = 0;
        storedWeapon = new WeaponInfo[6];
        storedPassive = new PassiveInfo[90];
        storedPassiveCount = new int[storedPassive.Length];
        lockedPassCards = new PassiveInfo[4];
        lockedWeaCards = new WeaponInfo[4];
        cardLocks = new bool[4] { false, false, false, false };
        cardGrades = new Grade[4];
    }

    public void GetWeaponInfo(WeaponInfo weaponInfo)
    {
        if (foolCount > 5)
        {
            isFool = true;
        }

        else if (foolCount <= 5)
            isFool = false;

        if (isFool == false)
        {
            for (int i = 0; i < storedWeapon.Length; i++)
            {
                if (storedWeapon[i] == null)
                {
                    weaponCount = i;
                    break;
                }
            }

            storedWeapon[weaponCount] = weaponInfo;
        }
    }

    public void GetPassiveInfo(PassiveInfo passiveInfo)
    {
        passiveCardItem = passiveInfo;
        storedPassive[passiveItemCount] = passiveInfo;

        if (passiveItemCount == 0)
        {
            storedPassiveCount[passiveItemCount]++;
        }

        else if (passiveItemCount > 0)
        {
            CheckItemEquel();
        }

        passiveItemCount++;
    }

    void CheckItemEquel()
    {
        for (int i = 0; i < passiveItemCount; i++)
        {
            if (storedPassive[passiveItemCount] == storedPassive[i])
            {
                storedPassive[passiveItemCount] = null;
                storedPassiveCount[i]++;
                passiveItemCount--;
                break;
            }

            else
            {
                if (i == passiveItemCount - 1)
                    storedPassiveCount[passiveItemCount]++;
            }
        }
    }
}
