using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExDamageUI : MonoBehaviour
{
    [SerializeField] Text damageText;
    [SerializeField] GameObject explosion;

    public float damage;

    float printTime, initPrintTime;

    private void Start()
    {
        printTime = 1f;
        initPrintTime = printTime;
        damageText.text = damage.ToString();
    }

    private void ChangeAlpha(float alpha)
    {
        Color textXColor = damageText.color;        // �ؽ�Ʈ�� ����
        textXColor.a = alpha;                       // ������ ���İ� ����(���� ���� �Ұ��ؼ� ����)
        damageText.color = textXColor;              // ������ ������ ����
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
