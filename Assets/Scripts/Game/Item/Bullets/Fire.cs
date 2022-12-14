using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Bullet
{
    [SerializeField] GameObject explosion;

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
            int rand = Random.Range(0, 1);
            if (rand == 0)
                Instantiate(explosion, transform.position, transform.rotation);

            GameObject pool = Instantiate(damageUI, transform.position, Quaternion.Euler(90, 0, 0)).gameObject;
            pool.transform.SetParent(gameManager.damageStorage);
            other.GetComponent<Monster>().OnDamaged(damageUI.weaponDamage);
            if (gameManager.absorbHp > 0)
                gameManager.hp += gameManager.absorbHp;
            DestroyBullet();
            CancelInvoke("DestroyBullet");

            Instantiate(effectPrefab, transform.position, transform.rotation);
        }
    }
}
