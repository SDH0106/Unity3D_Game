using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Bullet
{
    [SerializeField] public GameObject explosion;

    public int grade;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    void Update()
    {
        transform.position += new Vector3(dir.x, 0, dir.z) * speed * Time.deltaTime;

        // �Ѿ� ����
        angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(90, -angle, 0);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Monster" && other.GetComponent<Monster>() != null)
        {
            other.GetComponent<Monster>().OnDamaged(damageUI.weaponDamage);

            if (damageUI.weaponDamage > other.GetComponent<Monster>().stat.monsterDefence * (1 + gameManager.round * 0.1f))
                damageUI.isMiss = false;

            else if (damageUI.weaponDamage <= other.GetComponent<Monster>().stat.monsterDefence * (1 + gameManager.round * 0.1f))
                damageUI.isMiss = true;

            damageUI.realDamage = damageUI.weaponDamage - other.GetComponent<Monster>().stat.monsterDefence * (1 + gameManager.round * 0.1f);

            DamageUI pool = Instantiate(damageUI, transform.position, Quaternion.Euler(90, 0, 0)).GetComponent<DamageUI>();
            pool.gameObject.transform.SetParent(gameManager.damageStorage);

            int rand = Random.Range(0, 100);
            if (rand <= 5 + gameManager.luck * 0.2)
            {
                GameObject ex = Instantiate(explosion, transform.position, transform.rotation);
                ex.GetComponent<Explosion>().grade = grade;
            }

            if (gameManager.absorbHp > 0)
                Character.Instance.currentHp += gameManager.absorbHp;

            DestroyBullet();
            CancelInvoke("DestroyBullet");

            Instantiate(effectPrefab, transform.position, transform.rotation);
        }
    }
}