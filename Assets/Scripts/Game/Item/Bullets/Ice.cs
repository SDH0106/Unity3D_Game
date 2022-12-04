using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : Bullet
{
    bool isFreeze = false;

    void Update()
    {
        transform.position += new Vector3(dir.x, 0, dir.z) * speed * Time.deltaTime;

        // ÃÑ¾Ë °¢µµ
        angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(90, -angle, 0);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Monster" && other.GetComponent<Monster>() != null)
        {
            int rand = Random.Range(0, 1);
            if (rand == 0)
                isFreeze = true;
            else
                isFreeze = false;

            GameObject pool = Instantiate(damageUI, transform.position, Quaternion.Euler(90, 0, 0)).gameObject;
            pool.transform.SetParent(GameManager.Instance.damageStorage);
            other.GetComponent<Monster>().OnDamaged(damageUI.weaponDamage, isFreeze);
            GameManager.Instance.hp += GameManager.Instance.absorbHp;
            DestroyBullet();
            CancelInvoke("DestroyBullet");

            Instantiate(effectPrefab, transform.position, transform.rotation);
        }
    }
}
