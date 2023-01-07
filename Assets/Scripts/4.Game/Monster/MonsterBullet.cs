using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SpatialTracking;

public class MonsterBullet : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] public float bulletDamage;
    
    Collider coll;

    float angle;
    Vector3 dir;

    public int randNum;
    [HideInInspector] public Vector3 monsPos;

    private void Start()
    {
        coll = GetComponent<Collider>();
        Invoke("DestroyBullet", 3f);
        ShootDir();
        speed = (randNum == 0) ? 6 : 4;
    }

    void Update()
    {
        if (randNum == 0)
            transform.position += new Vector3(dir.x, 0, dir.z) * speed * Time.deltaTime;

        if(randNum == 1)
        {
            transform.Translate(new Vector3(dir.x, dir.z, 0) * speed * Time.deltaTime);
            transform.RotateAround(monsPos, Vector3.up, 120 * Time.deltaTime);
        }
    }

    public void ShootDir()
    {
        if (randNum == 0)
        {
            dir = (Character.Instance.transform.position - transform.position).normalized;

            // ÃÑ¾Ë °¢µµ
            angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(90, -angle, 0);
        }

        else if (randNum == 1)
        {
            dir = (transform.position - monsPos).normalized;
        }
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
