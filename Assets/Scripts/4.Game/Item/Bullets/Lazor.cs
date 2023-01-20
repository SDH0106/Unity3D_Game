using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazor : Bullet
{
    Vector3 initScale;

    private void Start()
    {
        gameManager = GameManager.Instance;
        initScale = transform.localScale;
    }

    private void Update()
    {
        // ÃÑ¾Ë °¢µµ
        angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(90, -angle, 0);

        if (gameManager.range <= 0)
            transform.localScale = new Vector3(initScale.x, initScale.y, initScale.z);

        else if (gameManager.range > 0)
            transform.localScale = new Vector3(initScale.x * (1 + gameManager.range * 0.05f), initScale.y, initScale.z);
    }

    public override void Shoot(Vector3 dir, float range)
    {
        this.dir = dir;

        Invoke("DestroyBullet", range);
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Monster" && collision.collider.GetComponent<Monster>())
        {
            collision.collider.GetComponent<Monster>().OnDamaged(damageUI.weaponDamage);

            if (damageUI.weaponDamage > collision.collider.GetComponent<Monster>().stat.monsterDefence * (1 + gameManager.round * 0.1f))
                damageUI.isMiss = false;

            else if (damageUI.weaponDamage <= collision.collider.GetComponent<Monster>().stat.monsterDefence * (1 + gameManager.round * 0.1f))
                damageUI.isMiss = true;

            damageUI.realDamage = damageUI.weaponDamage - collision.collider.GetComponent<Monster>().stat.monsterDefence * (1 + gameManager.round * 0.1f);

            DamageUI pool = Instantiate(damageUI, collision.contacts[0].point, Quaternion.Euler(90, 0, 0)).GetComponent<DamageUI>();
            pool.transform.SetParent(gameManager.damageStorage);

            if (gameManager.absorbHp > 0)
                Character.Instance.currentHp += gameManager.absorbHp;

            Instantiate(effectPrefab, collision.contacts[0].point, transform.rotation);
        }
    }
}
