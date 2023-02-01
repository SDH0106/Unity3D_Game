using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SocialPlatforms;
using UnityEngine.SpatialTracking;

public class MonsterBullet : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] public float bulletDamage;
    
    Collider coll;

    float angle;
    Vector3 dir;

    [HideInInspector] public int randNum;
    [HideInInspector] public Vector3 monsPos;

    private void Start()
    {
        coll = GetComponent<Collider>();
        Invoke("DestroyBullet", 3f);
        ShootDir();
        speed = (randNum == 0) ? 6f : 3f;
        bulletDamage = bulletDamage + Mathf.Floor(GameManager.Instance.round / 5f) * 2f;  // 트리거에도 있음
        transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
    }

    void Update()
    {
        //transform.Translate(new Vector3(dir.x, dir.z, 0) * speed * Time.deltaTime);
        if (randNum == 0)
            transform.position += dir * speed * Time.deltaTime;

        else if(randNum == 1)
        {
            //transform.Translate(new Vector3(dir.x, 0f, dir.z) * speed * Time.deltaTime, Space.Self);
            transform.position += dir * speed * Time.deltaTime;
            transform.RotateAround(monsPos, Vector3.up, 120f * Time.deltaTime);
        }
    }

    public void ShootDir()
    {
        if (randNum == 0)
        {
            dir = (Character.Instance.transform.position - transform.position).normalized;
            dir = new Vector3(dir.x, 0f, dir.z);
        }

        else if (randNum == 1)
        {
            dir = (transform.position - monsPos).normalized;
            dir = new Vector3(dir.x, 0f, dir.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Character")
        {
            bulletDamage = bulletDamage + Mathf.Floor(GameManager.Instance.round / 5) * 2;
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
