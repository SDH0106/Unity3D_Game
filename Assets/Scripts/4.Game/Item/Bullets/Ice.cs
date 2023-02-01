using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : Bullet
{
    bool isFreeze = false;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, initPos) > range)
        {
            DestroyBullet();
        }

        transform.position += new Vector3(dir.x, 0, dir.z) * speed * Time.deltaTime;

        // ÃÑ¾Ë °¢µµ
        angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(90, -angle, 0);
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Monster") && collision.collider.GetComponent<Monster>() != null)
        {
            Freezee();

            Instantiate(effectPrefab, transform.position, transform.rotation);

            if (gameManager.isReflect)
                Reflect(collision);

            else if (gameManager.onePenetrate)
                OnePenetrate();

            else if (gameManager.lowPenetrate)
                LowPenetrate();

            else if (!gameManager.isReflect && !gameManager.lowPenetrate && !gameManager.onePenetrate)
            {
                if (isDestroyed)
                    DestroyBullet();
            }

            if (damageUI.weaponDamage > collision.collider.GetComponent<Monster>().defence)
                damageUI.isMiss = false;

            else if (damageUI.weaponDamage <= collision.collider.GetComponent<Monster>().defence)
                damageUI.isMiss = true;

            if (gameManager.absorbHp > 0 && !damageUI.isMiss)
                Character.Instance.currentHp += gameManager.absorbHp;

            damageUI.realDamage = damageUI.weaponDamage - collision.collider.GetComponent<Monster>().defence;

            collision.collider.GetComponent<Monster>().OnDamaged(damageUI.realDamage, isFreeze);

            DamageUI pool = Instantiate(damageUI, transform.position, Quaternion.Euler(90, 0, 0)).GetComponent<DamageUI>();
            pool.gameObject.transform.SetParent(gameManager.damageStorage);
        }
    }

    void Freezee()
    {
        int rand = Random.Range(0, 100);

        if (rand <= 5 + gameManager.luck * 0.2)
            isFreeze = true;

        else
            isFreeze = false;
    }
}
