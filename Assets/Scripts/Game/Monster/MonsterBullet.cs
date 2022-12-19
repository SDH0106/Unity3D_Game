using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MonsterBullet : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] public float bulletDamage;
    
    Collider coll;

    float angle;
    Vector3 dir;

    private void Start()
    {
        coll = GetComponent<Collider>();
        Invoke("DestroyBullet", 3f);
        dir = (Character.Instance.transform.position - transform.position).normalized;
    }

    void Update()
    {
        transform.position += new Vector3(dir.x, 0, dir.z) * speed * Time.deltaTime;

        // ÃÑ¾Ë °¢µµ
        angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(90, -angle, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Character")
        {
            other.GetComponent<Character>().OnDamaged(coll, bulletDamage);
            DestroyBullet();
            CancelInvoke("DestroyBullet");
        }
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
