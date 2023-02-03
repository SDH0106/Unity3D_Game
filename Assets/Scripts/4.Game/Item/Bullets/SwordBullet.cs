using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBullet : Bullet
{
    private void Start()
    {
        gameManager = GameManager.Instance;
        isAttack = false;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, initPos) > range)
        {
            DestroyBullet();
        }

        transform.position += new Vector3(dir.x, 0, dir.z) * speed * Time.deltaTime;

        // �Ѿ� ����
        angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(90, -angle, 0);
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Monster") && collision.collider.GetComponent<Monster>() != null)
        {
            Instantiate(effectPrefab, transform.position, transform.rotation);

            if (gameManager.isReflect)
                Reflect(collision);

            else if (gameManager.onePenetrate)
                OnePenetrate();

            else if (gameManager.lowPenetrate)
                LowPenetrate();

            else if (!gameManager.isReflect && !gameManager.lowPenetrate && !gameManager.onePenetrate && !gameManager.penetrate)
            {
                DestroyBullet();
            }

            if (damageUI.swordBulletDamage > collision.collider.GetComponent<Monster>().defence)
                damageUI.isMiss = false;

            else if (damageUI.swordBulletDamage <= collision.collider.GetComponent<Monster>().defence)
                damageUI.isMiss = true;

            if (gameManager.absorbHp > 0 && !damageUI.isMiss && !isAttack)
            {
                Character.Instance.currentHp += gameManager.absorbHp;
                isAttack = true;
            }

            damageUI.realDamage = damageUI.swordBulletDamage - collision.collider.GetComponent<Monster>().defence;

            collision.collider.GetComponent<Monster>().OnDamaged(damageUI.realDamage);

            DamageUI pool = Instantiate(damageUI, transform.position, Quaternion.Euler(90, 0, 0)).GetComponent<DamageUI>();
            pool.gameObject.transform.SetParent(gameManager.damageStorage);
        }
    }
}
