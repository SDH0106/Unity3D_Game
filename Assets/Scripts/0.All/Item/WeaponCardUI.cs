using UnityEngine;
using UnityEngine.UI;
using static WeaponInfo;

public class WeaponCardUI : MonoBehaviour
{
    [SerializeField] public WeaponInfo[] weaponInfo;

    [Header("Lock")]
    [SerializeField] Image lockBackImage;
    [SerializeField] Image lockImage;
    [SerializeField] Text lockText;

    [Header("Card")]
    [SerializeField] Image cardBack;
    [SerializeField] Image cardBackLine;
    [SerializeField] Image itemSprite;
    [SerializeField] Text weaponName;
    [SerializeField] Text type;
    [SerializeField] Text attackTypes;
    [SerializeField] Text weaponDamage;
    [SerializeField] Text magicDamage;
    [SerializeField] Text attackDelay;
    [SerializeField] Text bulletSpeed;
    [SerializeField] Text weaponRange;
    [SerializeField] Text weaponPrice;
    [SerializeField] Text weaponGrade;
    [SerializeField] Text description;
    [SerializeField] GameObject combineCheckPanel;
    [SerializeField] Text combineMoney;
    [SerializeField] GameObject cantCombinePanel;

    Color LockImageColor;
    Color LockTextColor;

    [HideInInspector] public WeaponInfo selectedWeapon;

    [HideInInspector] public bool isLock;

    Color initPriceColor;

    GameManager gameManager;
    ItemManager itemManager;
    Character character;

    int price;

    public int cardNum;

    int combineNum;

    int num;

    bool isOver = false;

    public Grade selectGrade;

    private void Start()
    {
        gameManager = GameManager.Instance;
        itemManager = ItemManager.Instance;
        character = Character.Instance;

        isOver = false;

        combineCheckPanel.SetActive(false);
        cantCombinePanel.SetActive(false);

        initPriceColor = weaponPrice.color;
        LockImageColor = new Color(0.17f, 0.17f, 0.17f);
        LockTextColor = Color.white;

        num = transform.parent.GetSiblingIndex();

        isLock = itemManager.cardLocks[num];

        Setting();
        CardColor();
        StartLockColor();
    }

    private void Update()
    {
        if (!isOver)
        {
            if (Input.GetMouseButton(0))
            {
                combineCheckPanel.SetActive(false);
                cantCombinePanel.SetActive(false);
            }
        }

        lockImage.gameObject.SetActive(isLock);

        if (gameManager.money < price)
            weaponPrice.color = Color.red;

        else if (gameManager.money >= price)
            weaponPrice.color = initPriceColor;
    }

    void Setting()
    {
        if (isLock)
        {
            selectedWeapon = itemManager.lockedWeaCards[num];
            selectGrade = itemManager.selectedGrades[num];
        }

        int grade = (int)selectGrade;

        price = Mathf.CeilToInt(selectedWeapon.WeaponPrice * (grade * 2f + 1) * (1 - gameManager.salePercent));
        itemSprite.sprite = selectedWeapon.ItemSprite;
        weaponName.text = selectedWeapon.WeaponName.ToString();
        type.text = selectedWeapon.Type.ToString();
        weaponDamage.text = (selectedWeapon.WeaponDamage * (grade + 1)).ToString();
        magicDamage.text = (selectedWeapon.MagicDamage * (grade + 1)).ToString();
        attackDelay.text = (selectedWeapon.AttackDelay - (grade * 0.1f)).ToString("0.##");
        bulletSpeed.text = selectedWeapon.BulletSpeed.ToString();
        weaponRange.text = selectedWeapon.WeaponRange.ToString();
        weaponPrice.text = price.ToString();
        weaponGrade.text = selectGrade.ToString();
        description.text = selectedWeapon.Description.ToString();
        combineMoney.text = Mathf.CeilToInt(price * 0.5f).ToString();

        if (selectedWeapon.Type == WEAPON_TYPE.��)
            attackTypes.text = "(����/�ٰŸ�)";

        else if (selectedWeapon.Type == WEAPON_TYPE.��)
            attackTypes.text = "(����/���Ÿ�)";

        else if (selectedWeapon.Type == WEAPON_TYPE.������)
            attackTypes.text = "(����/���Ÿ�)";
    }

    void CardColor()
    {
        if (selectGrade == Grade.�Ϲ�)
        {
            cardBack.color = new Color(0.142f, 0.142f, 0.142f, 0.8235f);
            cardBackLine.color = Color.black;
            weaponName.color = Color.white;
            weaponGrade.color = Color.white;
        }

        else if(selectGrade == Grade.���)
        {
            cardBack.color = new Color(0f, 0.6f, 0.8f, 0.8235f);
            cardBackLine.color = Color.blue;
            weaponName.color = new Color(0.5f, 0.8f, 1f, 1f);
            weaponGrade.color = new Color(0.5f, 0.8f, 1f, 1f);
        }

        else if (selectGrade == Grade.����)
        {
            cardBack.color = new Color(0.5f, 0.2f, 0.4f, 0.8235f);
            cardBackLine.color = new Color(0.5f, 0f, 0.5f, 1f);
            weaponName.color = new Color(0.8f, 0.4f, 1f, 1f);
            weaponGrade.color = new Color(0.8f, 0.4f, 1f, 1f);
        }

        else if (selectGrade == Grade.��ȭ)
        {
            cardBack.color = new Color(0.7f, 0.1f, 0.1f, 0.8235f);
            cardBackLine.color = Color.red;
            weaponName.color = new Color(1f, 0.45f, 0.45f, 1f);
            weaponGrade.color = new Color(1f, 0.45f, 0.45f, 1f);
        }
    }

    public void Click()
    {
        bool canSwordBuy = false;

        if (selectedWeapon.Type == WEAPON_TYPE.��)
        {
            if (itemManager.fullCount < 5 && gameManager.money >= price)
            {
                canSwordBuy = true;
                SoundManager.Instance.PlayES("WeaponSelect");
                gameManager.money -= price;
                itemManager.fullCount++;
                itemManager.GetWeaponInfo(selectedWeapon);
                itemManager.weaponGrade[itemManager.weaponCount] = selectGrade;
                isLock = false;
                itemManager.cardLocks[num] = isLock;
                itemManager.lockedWeaCards[num] = null;
                character.Equip();
                Destroy(gameObject);
            }

            else if (itemManager.fullCount >= 5 && gameManager.money >= price)
            {
                for (int i = 0; i < itemManager.storedWeapon.Length; i++)
                {
                    if (itemManager.storedWeapon[i] != null && selectGrade != Grade.��ȭ)
                    {
                        if ((selectedWeapon.WeaponName == itemManager.storedWeapon[i].WeaponName) && (selectGrade == itemManager.weaponGrade[i]))
                        {
                            canSwordBuy = true;
                            combineNum = i;
                            combineCheckPanel.SetActive(true);
                            break;
                        }
                    }
                }
            }

            if(!canSwordBuy)
                SoundManager.Instance.PlayES("CantBuy");
        }

        // �˽�ĳ���ʹ� �˿ܿ� ���� ���ϰ�
        else if (selectedWeapon.Type != WEAPON_TYPE.��)
        {
            bool canBuy = false;
            if (character.characterNum != (int)CHARACTER_NUM.Legendary)
            {
                if (itemManager.fullCount < 5 && gameManager.money >= price)
                {
                    canBuy = true;
                    SoundManager.Instance.PlayES("WeaponSelect");
                    gameManager.money -= price;
                    itemManager.fullCount++;
                    itemManager.GetWeaponInfo(selectedWeapon);
                    itemManager.weaponGrade[itemManager.weaponCount] = selectGrade;
                    if (selectedWeapon.WeaponName == "���� ������")
                    {
                        if(character.thunderCount == 0)
                        {
                            character.thunderMark.SetActive(true);
                        }

                        character.thunderCount++;
                    }
                    isLock = false;
                    itemManager.cardLocks[num] = isLock;
                    itemManager.lockedWeaCards[num] = null;
                    character.Equip();
                    Destroy(gameObject);
                }

                else if (itemManager.fullCount >= 5 && gameManager.money >= price)
                {
                    for (int i = 0; i < itemManager.storedWeapon.Length; i++)
                    {
                        if (itemManager.storedWeapon[i] != null && selectGrade != Grade.��ȭ)
                        {
                            if ((selectedWeapon.WeaponName == itemManager.storedWeapon[i].WeaponName) && (selectGrade == itemManager.weaponGrade[i]))
                            {
                                canBuy = true;
                                combineNum = i;
                                combineCheckPanel.SetActive(true);
                                break;
                            }
                        }
                    }
                }

                if (!canBuy)
                    SoundManager.Instance.PlayES("CantBuy");
            }

            else if (character.characterNum == (int)CHARACTER_NUM.Legendary)
                SoundManager.Instance.PlayES("CantBuy");
        }
    }

    public void Combine()
    {
        if (gameManager.money - price >= Mathf.CeilToInt(price * 0.5f))
        {
            SoundManager.Instance.PlayES("WeaponSelect");
            gameManager.money -= Mathf.CeilToInt(price * 0.5f);
            gameManager.money -= price;
            itemManager.weaponGrade[combineNum]++;
            isLock = false;
            itemManager.cardLocks[num] = isLock;
            itemManager.lockedWeaCards[num] = null;
            Destroy(gameObject);
        }

        else
        {
            SoundManager.Instance.PlayES("CantBuy");
            cantCombinePanel.SetActive(true);
        }
    }

    public void Lock()
    {
        if (!isLock)
        {
            lockBackImage.color = Color.white;
            lockText.color = Color.black;
            itemManager.lockedWeaCards[num] = selectedWeapon;
            itemManager.selectedGrades[num] = selectGrade;
            isLock = true;
        }

        else if (isLock)
        {
            lockBackImage.color = LockImageColor;
            lockText.color = LockTextColor;
            itemManager.lockedWeaCards[num] = null;
            isLock = false;
        }

        itemManager.cardLocks[num] = isLock;
    }

    void StartLockColor()
    {
        if (isLock)
        {
            lockBackImage.color = Color.white;
            lockText.color = Color.black;
        }

        else if (!isLock)
        {
            lockBackImage.color = LockImageColor;
            lockText.color = LockTextColor;
        }
    }

    public void PointerEnter()
    {
        isOver = true;
    }

    public void PointerExit()
    {
        isOver = false;
    }
}
