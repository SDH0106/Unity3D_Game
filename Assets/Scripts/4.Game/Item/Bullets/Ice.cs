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
        transform.position += new Vector3(dir.x, 0, dir.z) * speed * Time.deltaTime;

        // ÃÑ¾Ë °¢µµ
        angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(90, -angle, 0);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Monster" && other.GetComponent<Monster>() != null)
        {
            int rand = Random.Range(0, 100);
            if (rand <= 5 + gameManager.luck * 0.2)
                isFreeze = true;
            else
                isFreeze = false;

            other.GetComponent<Monster>().OnDamaged(damageUI.weaponDamage, isFreeze);

            if (damageUI.weaponDamage > other.GetComponent<Monster>().stat.monsterDefence * (1 + gameManager.round * 0.1f))
                damageUI.isMiss = false;

            else if (damageUI.weaponDamage <= other.GetComponent<Monster>().stat.monsterDefence * (1 + gameManager.round * 0.1f))
                damageUI.isMiss = true;

            damageUI.realDamage = damageUI.weaponDamage - other.GetComponent<Monster>().stat.monsterDefence * (1 + gameManager.round * 0.1f);

            DamageUI pool = Instantiate(damageUI, transform.position, Quaternion.Euler(90, 0, 0)).GetComponent<DamageUI>();
            pool.gameObject.transform.SetParent(gameManager.damageStorage);

            if (gameManager.absorbHp > 0)
                Character.Instance.currentHp += gameManager.absorbHp;
            DestroyBullet();
            CancelInvoke("DestroyBullet");

            Instantiate(effectPrefab, transform.position, transform.rotation);
        }
    }
}
