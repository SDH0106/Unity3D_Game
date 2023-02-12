using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : Bullet
{
    bool isFreeze = false;

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

        // ÃÑ¾Ë °¢µµ
        angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(90, -angle, 0);
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Monster") && collision.collider.GetComponent<Monster>() != null)
        {
            Monster monster = collision.collider.GetComponent<Monster>();

            Freeze();

            Instantiate(effectPrefab, transform.position, transform.rotation);
            
            DamageUI damage = pool.Get();

            damage.weaponDamage = bulletDamage;

            if (gameManager.isReflect)
                Reflect(collision);

            else if (gameManager.onePenetrate)
                OnePenetrate();

            else if (gameManager.lowPenetrate)
                LowPenetrate(damage);

            if (damage.weaponDamage > monster.defence)
                damage.isMiss = false;

            else if (damage.weaponDamage <= monster.defence)
                damage.isMiss = true;

            damage.realDamage = Mathf.Clamp(damage.weaponDamage - monster.defence, 0, damage.weaponDamage - monster.defence);
            damage.UISetting();
            damage.transform.position = transform.position;
            damage.gameObject.transform.SetParent(gameManager.damageStorage);

            monster.OnDamaged(damage.realDamage, isFreeze);

            if (gameManager.absorbHp > 0 && !damage.isMiss && !isAttack)
            {
                Character.Instance.currentHp += gameManager.absorbHp;
                isAttack = true;
            }

            if (!gameManager.isReflect && !gameManager.lowPenetrate && !gameManager.onePenetrate && !gameManager.penetrate)
            {
                if (!isDestroyed)
                    DestroyBullet();
            }
        }
    }

    void Freeze()
    {
        int rand = Random.Range(0, 100);

        if (rand <= 5 + gameManager.luck * 0.2)
            isFreeze = true;

        else
            isFreeze = false;
    }
}
