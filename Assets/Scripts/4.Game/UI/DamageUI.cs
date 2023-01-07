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

    private void Start()
    {
        damageText.fontSize = 50;
        printTime = 1f;
        initPrintTime = printTime;
    }

    private void ChangeAlpha(float alpha)
    {
        Color textXColor = damageText.color;        // �ؽ�Ʈ�� ����
        textXColor.a = alpha;                       // ������ ���İ� ����(���� ���� �Ұ��ؼ� ����)
        damageText.color = textXColor;              // ������ ������ ����
    }

    private void Update()
    {
        damageText.text = weaponDamage.ToString("0.##");

        transform.position += Vector3.forward * 0.5f * Time.deltaTime;

        printTime -= Time.deltaTime;

        if (printTime <= 0)
        {
            Destroy(gameObject);
        }

        ChangeAlpha(printTime / initPrintTime);
    }
}
