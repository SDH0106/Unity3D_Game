using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    //[SerializeField] Image[] weaponImages;
    [SerializeField] Image[] passiveImages;

    //[SerializeField] WeaponCardUI[] weapons;
    [SerializeField] PassiveCardUI[] passiveCards;

    int[] numArray;

    private void Awake()
    {
        /*numArray = new int[weapons[0].weaponInfo.Length];
        GetRandomCard(weapons.Length, weapons[0].weaponInfo.Length);*/

        numArray = new int[passiveCards[0].passiveInfo.Length];
        GetRandomCard(passiveCards.Length, passiveCards[0].passiveInfo.Length);
    }

    private void Update()
    {
        WeaponSlot();
        PassiveSlot();
    }

    void AlphaChange(int i, int a, Image[] image)
    {
        Color color = image[i].color;
        color.a = a;
        image[i].color = color;
    }

    void WeaponSlot()
    {
        /*for(int i = 0; i < weaponImages.Length; i++)
        {
            if (ItemManager.Instance.storedWeapon[i] != null)
            {
                AlphaChange(i, 1);
                weaponImages[i].sprite = ItemManager.Instance.storedWeapon[i].ItemSprite;
            }

            else if (ItemManager.Instance.storedWeapon[i] == null)
            {
                AlphaChange(i, 0);
            }
        }*/
    }

    void PassiveSlot()
    {
        for(int i = 0; i < passiveImages.Length; i++)
        {
            if (ItemManager.Instance.storedPassive[i] != null)
            {
                AlphaChange(i, 1, passiveImages);
                passiveImages[i].sprite = ItemManager.Instance.storedPassive[i].ItemSprite;
            }

            else if (ItemManager.Instance.storedPassive[i] == null)
            {
                AlphaChange(i, 0, passiveImages);
            }
        }
    }

    void GetRandomCard(int count, int length)
    {
        for (int i = 0; i < count; i++)
        {
            numArray[i] = UnityEngine.Random.Range(0, length);

            for (int j = 0; j < i; j++)
            {
                if (numArray[j] == numArray[i])
                {
                    i--;
                    break;
                }
            }
        }

        /*for (int i = 0; i < weapons.Length; i++)
            weapons[i].selectedWeapon = weapons[i].weaponInfo[numArray[i]];*/

        for (int i = 0; i < passiveCards.Length; i++)
        {
            passiveCards[i].selectedPassive = passiveCards[i].passiveInfo[numArray[i]];
        }

    }
}
