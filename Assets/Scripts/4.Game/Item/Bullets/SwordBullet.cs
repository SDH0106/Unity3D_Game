using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBullet : Bullet
{
    public int criRand;

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

            DamageUI damage = pool.Get();
            if (bulletDamage > collision.collider.GetComponent<Monster>().defence)
                damage.isMiss = false;
            else if (bulletDamage <= collision.collider.GetComponent<Monster>().defence)
                damage.isMiss = true;
            damage.realDamage = Mathf.Clamp(bulletDamage - collision.collider.GetComponent<Monster>().defence, 0, bulletDamage - collision.collider.GetComponent<Monster>().defence);
            if (criRand <= gameManager.critical || gameManager.critical >= 100)
            {
                damage.damageText.color = new Color(0.9f, 0, 0.7f, 1);
                damage.damageText.fontSize = 65;
                Debug.Log("cri");
            }
            else if (criRand > gameManager.critical)
            {
                damage.damageText.color = Color.cyan;
                damage.damageText.fontSize = 50;
                Debug.Log("noCri");
            }
            damage.UISetting();
            damage.transform.position = transform.position;
            damage.gameObject.transform.SetParent(gameManager.damageStorage);

            collision.collider.GetComponent<Monster>().OnDamaged(damage.realDamage);

            if (gameManager.absorbHp > 0 && !damage.isMiss && !isAttack)
            {
                Character.Instance.currentHp += gameManager.absorbHp;
                isAttack = true;
            }

            if (gameManager.isReflect)
                Reflect(collision);

            else if (gameManager.onePenetrate)
                OnePenetrate();

            else if (gameManager.lowPenetrate)
                LowPenetrate(damage);

            else if (!gameManager.isReflect && !gameManager.lowPenetrate && !gameManager.onePenetrate && !gameManager.penetrate)
            {
                DestroyBullet();
            }
        }
    }
}
