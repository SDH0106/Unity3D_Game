using UnityEngine;
using UnityEngine.UI;

public class FishingCard : MonoBehaviour
{
    [SerializeField] public PassiveInfo[] passiveInfo;

    [Header("decript")]
    [SerializeField] GameObject[] descriptPrefabs;
    [SerializeField] Transform decriptParent;

    [Header("Card")]
    [SerializeField] Image cardBack;
    [SerializeField] Image cardBackLine;
    [SerializeField] Image itemSprite;
    [SerializeField] Text itemName;
    [SerializeField] Text itemGrade;
    [SerializeField] Text itemCount;
    [SerializeField] Text sellPriceText;

    [HideInInspector] public PassiveInfo selectedPassive;

    float[] stats;
    string[] statTypes;

    int[] passiveIntVariables;
    float[] passiveFloatVariables;
    bool[] passiveBoolVariables;

    GameManager gameManager;
    ItemManager itemManager;
    Fishing fishing;

    int arrayCount;
    int sellPrice;

    private void Start()
    {
        gameManager = GameManager.Instance;
        itemManager = ItemManager.Instance;
        fishing = Fishing.Instance;

        Setting();
        CardImage();
        StatArray();
        DescriptionInfo();

        for (int i = 0; i < passiveInfo.Length; i++)
        {
            if (passiveInfo[i].ItemName == selectedPassive.ItemName)
            {
                arrayCount = i;
                break;
            }
        }
    }

    void Setting()
    {
        sellPrice = Mathf.CeilToInt(selectedPassive.ItemPrice * 0.7f);
        itemSprite.sprite = selectedPassive.ItemSprite;
        itemName.text = selectedPassive.ItemName;
        itemGrade.text = selectedPassive.ItemGrade.ToString();
        itemCount.text = selectedPassive.MaxCount.ToString();
        sellPriceText.text = sellPrice.ToString();
    }

    void CardImage()
    {
        if (selectedPassive.ItemGrade == Grade.�Ϲ�)
        {
            cardBack.color = new Color(0.142f, 0.142f, 0.142f, 1f);
            cardBackLine.color = Color.black;
            itemName.color = Color.white;
            itemGrade.color = Color.white;
        }

        else if (selectedPassive.ItemGrade == Grade.���)
        {
            cardBack.color = new Color(0f, 0.6f, 0.8f, 1f);
            cardBackLine.color = Color.blue;
            itemName.color = new Color(0.5f, 0.8f, 1f, 1f);
            itemGrade.color = new Color(0.5f, 0.8f, 1f, 1f);
        }

        else if (selectedPassive.ItemGrade == Grade.����)
        {
            cardBack.color = new Color(0.5f, 0.2f, 0.4f, 1f);
            cardBackLine.color = new Color(0.5f, 0f, 0.5f, 1f);
            itemName.color = new Color(0.8f, 0.4f, 1f, 1f);
            itemGrade.color = new Color(0.8f, 0.4f, 1f, 1f);
        }

        else if (selectedPassive.ItemGrade == Grade.��ȭ)
        {
            cardBack.color = new Color(0.7f, 0.1f, 0.1f, 1f);
            cardBackLine.color = Color.red;
            itemName.color = new Color(1f, 0.45f, 0.45f, 1f);
            itemGrade.color = new Color(1f, 0.45f, 0.45f, 1f);
        }
    }

    void StatArray()
    {
        stats = new float[15];
        stats[0] = selectedPassive.Hp;
        stats[1] = selectedPassive.RecoverHp;
        stats[2] = selectedPassive.AbsorbHp;
        stats[3] = selectedPassive.Defence;
        stats[4] = selectedPassive.PhysicDamage;
        stats[5] = selectedPassive.MagicDamage;
        stats[6] = selectedPassive.ShortDamage;
        stats[7] = selectedPassive.LongDamage;
        stats[8] = selectedPassive.AttackSpeed;
        stats[9] = selectedPassive.Speed;
        stats[10] = selectedPassive.Luck;
        stats[11] = selectedPassive.Range;
        stats[12] = selectedPassive.Critical;
        stats[13] = selectedPassive.PercentDamage;
        stats[14] = selectedPassive.Avoid;

        statTypes = new string[15];
        statTypes[0] = "�ִ� ü��";
        statTypes[1] = "ü�� ȸ��";
        statTypes[2] = "ü�� ����";
        statTypes[3] = "����";
        statTypes[4] = "���� ���ݷ�";
        statTypes[5] = "���� ���ݷ�";
        statTypes[6] = "�ٰŸ� ���ݷ�";
        statTypes[7] = "���Ÿ� ���ݷ�";
        statTypes[8] = "���� �ӵ�";
        statTypes[9] = "�̵� �ӵ�";
        statTypes[10] = "���";
        statTypes[11] = "��Ÿ�";
        statTypes[12] = "ũ��Ƽ��";
        statTypes[13] = "���ݷ� ����";
        statTypes[14] = "ȸ����";

        passiveIntVariables = new int[4];
        passiveIntVariables[0] = selectedPassive.DashCount;
        passiveIntVariables[1] = selectedPassive.BuffNum;
        passiveIntVariables[2] = selectedPassive.ExDmg;
        passiveIntVariables[3] = selectedPassive.IsedolCount;

        passiveFloatVariables = new float[7];
        passiveFloatVariables[0] = selectedPassive.CoinRange;
        passiveFloatVariables[1] = selectedPassive.IncreaseExp;
        passiveFloatVariables[2] = selectedPassive.MonsterSpeed;
        passiveFloatVariables[3] = selectedPassive.SalePercent;
        passiveFloatVariables[4] = selectedPassive.SummonASpd;
        passiveFloatVariables[5] = selectedPassive.SummonPDmg;
        passiveFloatVariables[6] = selectedPassive.MonsterDef;

        passiveBoolVariables = new bool[18];
        passiveBoolVariables[0] = selectedPassive.LuckCoin;
        passiveBoolVariables[1] = selectedPassive.LuckDamage;
        passiveBoolVariables[2] = selectedPassive.LuckCritical;
        passiveBoolVariables[3] = selectedPassive.DoubleShot;
        passiveBoolVariables[4] = selectedPassive.Revive;
        passiveBoolVariables[5] = selectedPassive.GgoGgo;
        passiveBoolVariables[6] = selectedPassive.Ilsoon;
        passiveBoolVariables[7] = selectedPassive.Wakgood;
        passiveBoolVariables[8] = selectedPassive.Ddilpa;
        passiveBoolVariables[9] = selectedPassive.Butterfly;
        passiveBoolVariables[10] = selectedPassive.SubscriptionFee;
        passiveBoolVariables[11] = selectedPassive.SpawnTree;
        passiveBoolVariables[12] = selectedPassive.Dotgu;
        passiveBoolVariables[13] = selectedPassive.IsReflect;
        passiveBoolVariables[14] = selectedPassive.OnePenetrate;
        passiveBoolVariables[15] = selectedPassive.LowPenetrate;
        passiveBoolVariables[16] = selectedPassive.Penetrate;
        passiveBoolVariables[17] = selectedPassive.Vamabsorb;
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
                if (stats[i] < 0)
                    descriptPrefabs[count].transform.GetChild(2).GetComponent<Text>().color = Color.red;
                descriptPrefabs[count].transform.GetChild(3).gameObject.SetActive(false);
                count++;
            }
        }

        for (int i = max - 1; i >= count; i--)
        {
            if (i == count)
            {
                descriptPrefabs[count].transform.GetChild(0).gameObject.SetActive(false);
                descriptPrefabs[count].transform.GetChild(1).gameObject.SetActive(false);
                descriptPrefabs[count].transform.GetChild(2).gameObject.SetActive(false);
                descriptPrefabs[count].transform.GetChild(3).gameObject.SetActive(true);
                descriptPrefabs[count].transform.GetChild(3).GetComponent<Text>().text = selectedPassive.Description;
            }

            else
                descriptPrefabs[i].gameObject.SetActive(false);
        }
    }

    public void Select()
    {
        SoundManager.Instance.PlayES("ChestSelect");

        if (itemManager.passiveCounts[arrayCount] > 0)
        {
            itemManager.GetPassiveInfo(selectedPassive);
            itemManager.passiveCounts[arrayCount]--;

            for (int i = 0; i < stats.Length; i++)
            {
                if (i == 13 || i == 9)
                    gameManager.stats[i] = Mathf.Round((gameManager.stats[i] + stats[i]) * 100) * 0.01f;

                else
                    gameManager.stats[i] = Mathf.Round((gameManager.stats[i] + stats[i]) * 10) * 0.1f;
            }

            for (int i = 0; i < passiveIntVariables.Length; i++)
            {
                if (i == 1 && passiveIntVariables[i] != 0) // ���� ����
                {
                    gameManager.passiveIntVariables[i] = passiveIntVariables[i];
                }

                else
                {
                    gameManager.passiveIntVariables[i] += passiveIntVariables[i];
                }

            }

            for (int i = 0; i < passiveFloatVariables.Length; i++)
            {
                gameManager.passiveFloatVariables[i] = Mathf.Round((gameManager.passiveFloatVariables[i] + passiveFloatVariables[i]) * 10) * 0.1f;
            }

            for (int i = 0; i < passiveBoolVariables.Length; i++)
            {
                if (passiveBoolVariables[i] == true)
                {
                    if (i == 14 || i == 15 || i == 16)
                    {
                        if (gameManager.isReflect)
                        {
                            passiveBoolVariables[i] = false;
                        }
                    }

                    gameManager.passiveBoolVariables[i] = passiveBoolVariables[i];

                    if (i == 8)
                    {
                        Ddilpa();
                        passiveBoolVariables[i] = false;
                    }

                    else if (i == 9)
                    {
                        Butterfly();
                        passiveBoolVariables[i] = false;
                    }

                    else if (i == 13)
                    {
                        gameManager.passiveBoolVariables[14] = false;
                        gameManager.passiveBoolVariables[15] = false;
                        gameManager.passiveBoolVariables[16] = false;
                    }

                    else if (i == 14)
                    {
                        gameManager.passiveBoolVariables[15] = false;
                        gameManager.passiveBoolVariables[16] = false;
                    }

                    else if (i == 15)
                    {
                        gameManager.passiveBoolVariables[14] = false;
                        gameManager.passiveBoolVariables[16] = false;
                    }

                    else if (i == 16)
                    {
                        gameManager.passiveBoolVariables[14] = false;
                        gameManager.passiveBoolVariables[15] = false;
                    }
                }
            }
        }

        GameSceneUI.Instance.chestCount--;

        if (GameSceneUI.Instance.chestCount > 0)
            ShowPassive.Instance.ShowRandomPassiveCard();

        fishing.Initialize();

        Destroy(gameObject);
    }

    void Ddilpa()
    {
        // ����(5) > ����(4)
        gameManager.stats[4] += Mathf.Round((gameManager.stats[5] / 2) * 10) * 0.1f;
        gameManager.stats[5] = 0;
    }

    void Butterfly()
    {
        // ����(4) > ����(5)
        gameManager.stats[5] += Mathf.Round((gameManager.stats[4] / 2) * 10) * 0.1f;
        gameManager.stats[4] = 0;
    }

    public void Sell()
    {
        SoundManager.Instance.PlayES("Coin");

        itemManager.passiveCounts[arrayCount]--;
        GameSceneUI.Instance.chestCount--;
        gameManager.money += sellPrice;

        if (GameSceneUI.Instance.chestCount > 0)
            ShowPassive.Instance.ShowRandomPassiveCard();

        fishing.Initialize();

        Destroy(gameObject);
    }

}
