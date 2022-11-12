using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassiveCardUI : MonoBehaviour
{
    [SerializeField] public PassiveInfo[] passiveInfo;

    [Header("decript")]
    [SerializeField] GameObject[] descriptPrefabs;
    [SerializeField] Transform decriptParent;

    [SerializeField] Image itemSprite;
    [SerializeField] Text itemName;

    Text hp;
    Text recoverHp;
    Text absorbHp;
    Text defence;
    Text weaponDamage;
    Text elementDamage;
    Text attackSpeed;
    Text speed;
    Text range;
    Text luck;

    int randNum;
    int selectNum;

    [HideInInspector] public PassiveInfo selectedPassive;

    float[] stats = new float[10];
    string[] statTypes = new string[10];

    private void Start()
    {
        Setting();
        StatArray();
        DescriptionInfo();
    }

    void Setting()
    {
        itemSprite.sprite = selectedPassive.ItemSprite;
        itemName.text = selectedPassive.ItemName;
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

        statTypes[0] = "최대 체력";
        statTypes[1] = "체력 회복";
        statTypes[2] = "체력 흡수";
        statTypes[3] = "방어력";
        statTypes[4] = "물리 대미지";
        statTypes[5] = "원소 대미지";
        statTypes[6] = "공격 속도";
        statTypes[7] = "이동 속도";
        statTypes[8] = "사거리";
        statTypes[9] = "행운";
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
        ItemManager.Instance.GetPassiveInfo(selectedPassive);
        Destroy(gameObject);
    }
}
