using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSlot : MonoBehaviour
{
    [SerializeField] GameObject sellUI;

    [HideInInspector] public int slotNum;

    WeaponInfo selectedWeapon;

    private void Start()
    {
        slotNum = transform.GetSiblingIndex();
    }

    public void ShowSellUI()
    {
        selectedWeapon = ItemManager.Instance.storedWeapon[slotNum];

        if (ItemManager.Instance.storedWeapon[slotNum] != null)
        {
            ShopManager.Instance.backgroundImage.SetActive(true);
            sellUI.gameObject.SetActive(true);
            sellUI.transform.position = Input.mousePosition;
            sellUI.GetComponent<SellCard>().Setting(slotNum);
        }
    }
}
