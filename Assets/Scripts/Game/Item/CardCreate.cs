using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCreate : MonoBehaviour
{
    WeaponCardUI cardUI;

    private void Start()
    {
        cardUI = GetComponent<WeaponCardUI>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            Instantiate(cardUI);
        }
    }
}
