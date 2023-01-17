using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonsBullet : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] DamageUI damageUIPreFab;

    float damage;

    Vector3 dir;
    float angle;

    private void Start()
    {
        Invoke("DestroyBullet", 5);
        damage = GameManager.Instance.round * 2;
    }

    private void Update()
    {
        transform.position += new Vector3(dir.x, 0, dir.z) * speed * Time.deltaTime;

        // ÃÑ¾Ë °¢µµ
        angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(90, -angle, 0);
    }

    public void Fire(Vector3 dir)
    {
        this.dir = dir.normalized;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Monster"))
        {
            CancelInvoke("DestroyBullet");
            DamageUI damageUI = Instantiate(damageUIPreFab, transform.position, damageUIPreFab.transform.rotation);
            damageUI.realDamage = damage;
            other.GetComponent<Monster>().PureOnDamaged(damage);
            Destroy(gameObject);
        }
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
