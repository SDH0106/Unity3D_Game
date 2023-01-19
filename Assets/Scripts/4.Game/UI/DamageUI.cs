using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static WeaponInfo;

public class DamageUI : MonoBehaviour
{
    [SerializeField] public Text damageText;

    float printTime, initPrintTime;

    public float weaponDamage;
    public float realDamage;

    [HideInInspector] public bool isMiss;

    private void Start()
    {
        damageText.fontSize = 50;
        printTime = 1f;
        initPrintTime = printTime;

        if (!isMiss)
            damageText.text = realDamage.ToString("0.##");

        else if (isMiss)
        {
            damageText.color = Color.red;
            damageText.text = "Miss";
        }
    }

    private void ChangeAlpha(float alpha)
    {
        Color textXColor = damageText.color;        // 텍스트의 색상값
        textXColor.a = alpha;                       // 색상값의 알파값 변경(직접 변경 불가해서 빼옴)
        damageText.color = textXColor;              // 변경한 색상을 대입
    }

    private void Update()
    {
        transform.position += Vector3.forward * 0.5f * Time.deltaTime;

        printTime -= Time.deltaTime;

        if (printTime <= 0)
        {
            Destroy(gameObject);
        }

        ChangeAlpha(printTime / initPrintTime);
    }
}
