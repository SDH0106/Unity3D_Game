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
        if (gameManager.range <= 0)
            transform.localScale = new Vector3(initScale.x, initScale.y, initScale.z);

        else if (gameManager.range > 0)
        {
            transform.localScale = new Vector3(initScale.x * (1 + gameManager.range * 0.05f), initScale.y, initScale.z);
        }
    }

    public override void Shoot(Vector3 dir, Vector3 initPos, float range)
    {
        this.dir = dir;
        gameManager = GameManager.Instance;

        Invoke("DestroyBullet", 0.3f);

        angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(90, -angle, 0);
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Monster" && collision.collider.GetComponent<Monster>())
        {
            Instantiate(effectPrefab, collision.contacts[0].point, transform.rotation);

            if (damageUI.weaponDamage > collision.collider.GetComponent<Monster>().defence)
                damageUI.isMiss = false;

            else if (damageUI.weaponDamage <= collision.collider.GetComponent<Monster>().defence)
                damageUI.isMiss = true;

            if (gameManager.absorbHp > 0 && !damageUI.isMiss)
                Character.Instance.currentHp += gameManager.absorbHp;

            damageUI.realDamage = damageUI.weaponDamage - collision.collider.GetComponent<Monster>().defence;

            DamageUI pool = Instantiate(damageUI, collision.contacts[0].point, Quaternion.Euler(90, 0, 0)).GetComponent<DamageUI>();
            pool.transform.SetParent(gameManager.damageStorage);

            collision.collider.GetComponent<Monster>().OnDamaged(damageUI.realDamage);
        }
    }

    public override void DestroyBullet()
    {
        dir = Vector3.zero;
        base.DestroyBullet();
    }
}
