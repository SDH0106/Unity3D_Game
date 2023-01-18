using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonsBullet : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected DamageUI damageUIPreFab;

    protected float damage;

    protected Vector3 dir;
    protected float angle;

    private void Start()
    {
        Invoke("DestroyBullet", 3);
        damage = GameManager.Instance.round * 2;
    }

    private void Update()
    {
        transform.position += new Vector3(dir.x, 0, dir.z) * speed * Time.deltaTime;

        // �Ѿ� ����
        angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(90, -angle, 0);
    }

    public void Fire(Vector3 dir)
    {
        this.dir = dir.normalized;
    }

    virtual protected void OnTriggerEnter(Collider other)
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

    protected void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
