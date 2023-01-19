using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonLazor : MonoBehaviour
{
    [SerializeField] DamageUI damageUIPreFab;

    public float damage;

    Vector3 dir;
    float angle;

    protected GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        damage = Mathf.Round(GameManager.Instance.round * 5 * (1 + GameManager.Instance.summonPDmg) * 10) * 0.1f;
    }

    private void Update()
    {
        // ÃÑ¾Ë °¢µµ
        angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(90, -angle, 0);
    }

    public void Fire(Vector3 dir)
    {
        this.dir = dir.normalized;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Monster") && collision.collider.GetComponent<Monster>())
        {
            collision.collider.GetComponent<Monster>().PureOnDamaged(damage);

            DamageUI damageUI = Instantiate(damageUIPreFab, collision.contacts[0].point, damageUIPreFab.transform.rotation);
            damageUI.realDamage = damage;
        }
    }

    public void BulletDestroy()
    {
        Destroy(gameObject);
    }
}
