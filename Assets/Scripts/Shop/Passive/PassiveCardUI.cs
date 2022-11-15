using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PassiveCardUI : MonoBehaviour
{
    [SerializeField] public PassiveInfo[] passiveInfo;

    [SerializeField] Image lockBackImage;
    [SerializeField] Image lockImage;
    [SerializeField] Text lockText;

    [Header("decript")]
    [SerializeField] GameObject[] descriptPrefabs;
    [SerializeField] Transform decriptParent;

    [SerializeField] Image itemSprite;
    [SerializeField] Text itemName;

    [SerializeField] Text itemPrice;

    [HideInInspector] public PassiveInfo selectedPassive;

    float[] stats = new float[10];
    string[] statTypes = new string[10];

    Color LockImageColor;
    Color LockTextColor;

    [HideInInspector] public bool isLock = false;

    Color initPriceColor;

    private void Start()
    {
        initPriceColor = itemPrice.color;

        LockImageColor = lockBackImage.color;
        LockTextColor = lockText.color;

        Setting();
        StatArray();
        DescriptionInfo();
    }

    private void Update()
    {
        lockImage.gameObject.SetActive(isLock);

        if (GameManager.Instance.money < selectedPassive.ItemPrice)
            itemPrice.color = Color.red;

        else if (GameManager.Instance.money >= selectedPassive.ItemPrice)
            itemPrice.color = initPriceColor;

    }

    void Setting()
    {
        itemSprite.sprite = selectedPassive.ItemSprite;
        itemName.text = selectedPassive.ItemName;
        itemPrice.text = selectedPassive.ItemPrice.ToString();
    }

    void StatArray()
    {
        stats[0] = selectedPassive.Hp;
        stats[1] = selectedPassive.RecoverHp;
        stats[2] = selectedPassive.AbsorbHp;
        stats[3] = selectedPassive.Defence;
        stats[4] = selectedPassive.WeaponDamage;
        stats[5] = selectedPassive.ElementDamage;
        stats[6] = selectedPassive.AttackSpeed;
        stats[7] = selectedPassive.Speed;
        stats[8] = selectedPassive.Range;
        stats[9] = selectedPassive.Luck;

        statTypes[0] = "�ִ� ü��";
        statTypes[1] = "ü�� ȸ��";
        statTypes[2] = "ü�� ���";
        statTypes[3] = "����";
        statTypes[4] = "���� �����";
        statTypes[5] = "���� �����";
        statTypes[6] = "���� �ӵ�";
        statTypes[7] = "�̵� �ӵ�";
        statTypes[8] = "��Ÿ�";
        statTypes[9] = "���";
    }

    void DescriptionInfo()
    {
        int max = descriptPrefabs.Length;
        int count = 0;

        for (int i = 0; i < stats.Length; i++)
        {
            if (stats[i] != 0)
            {
                descriptPrefabs[count].transform.GetChild(0).GetComponent<Text>().text = statTypes[i];
                descriptPrefabs[count].transform.GetChild(2).GetComponent<Text>().text = stats[i].ToString();
                count++;
            }
        }

        for (int i = max - 1; i >= count; i--)
        {
            descriptPrefabs[i].gameObject.SetActive(false);
        }
    }

    public void Click()
    {
        if (GameManager.Instance.money >= selectedPassive.ItemPrice)
        {
            ItemManager.Instance.GetPassiveInfo(selectedPassive);
            Destroy(gameObject);
            isLock = false;
            GameManager.Instance.money -= selectedPassive.ItemPrice;
        }
    }

    public void Lock()
    {
        if (!isLock)
        {
            lockBackImage.color = Color.white;
            lockText.color = Color.black;
            isLock = true;
        }

        else if (isLock)
        {
            lockBackImage.color = LockImageColor;
            lockText.color = LockTextColor;
            isLock = false;
        }
    }
}
