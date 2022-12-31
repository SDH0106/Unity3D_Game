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

    protected override void OnTriggerEnter(Collider other)
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Monster" && collision.collider.GetComponent<Monster>())
        {
            GameObject pool = Instantiate(damageUI, collision.contacts[0].point, Quaternion.Euler(90, 0, 0)).gameObject;
            pool.transform.SetParent(gameManager.damageStorage);
            collision.collider.GetComponent<Monster>().OnDamaged(damageUI.weaponDamage);
            if (gameManager.absorbHp > 0)
                Character.Instance.currentHp += gameManager.absorbHp;

            Instantiate(effectPrefab, collision.contacts[0].point, transform.rotation);
        }
    }
}
