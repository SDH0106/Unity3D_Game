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
            transform.localScale = new Vector3(initScale.x * (1 + gameManager.range * 0.3f), initScale.y, initScale.z);
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
                        
            DamageUI damage = pool.Get();
            if (bulletDamage > collision.collider.GetComponent<Monster>().defence)
                damage.isMiss = false;
            else if (bulletDamage <= collision.collider.GetComponent<Monster>().defence)
                damage.isMiss = true;
            damage.realDamage = Mathf.Clamp(bulletDamage - collision.collider.GetComponent<Monster>().defence, 0, bulletDamage - collision.collider.GetComponent<Monster>().defence);
            damage.UISetting();
            damage.transform.position = collision.contacts[0].point;
            damage.gameObject.transform.SetParent(gameManager.damageStorage);

            collision.collider.GetComponent<Monster>().OnDamaged(damage.realDamage);

            if (gameManager.absorbHp > 0 && !damage.isMiss && !isAttack)
            {
                Character.Instance.currentHp += gameManager.absorbHp;
                isAttack = true;
            }
        }
    }

    public override void DestroyBullet()
    {
        dir = Vector3.zero;
        base.DestroyBullet();
    }
}
