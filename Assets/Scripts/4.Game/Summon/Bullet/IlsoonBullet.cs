using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class IlsoonBullet : SummonsBullet
{
    private void Start()
    {
        gameManager = GameManager.Instance;
        damage = Mathf.Round(GameManager.Instance.longDamage * 2 * (1 + GameManager.Instance.summonPDmg) * 10) * 0.1f;
    }
    private void Update()
    {
        transform.position += new Vector3(dir.x, 0, dir.z) * speed * Time.deltaTime;

        // ÃÑ¾Ë °¢µµ
        angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(90, -angle, 0);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            DamageUI damageUI = Instantiate(damageUIPreFab, transform.position, damageUIPreFab.transform.rotation);
            damageUI.realDamage = damage;
            other.GetComponent<Monster>().PureOnDamaged(damage);
        }
    }
}
