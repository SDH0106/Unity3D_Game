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
