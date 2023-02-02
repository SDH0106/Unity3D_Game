using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SocialPlatforms;
using UnityEngine.SpatialTracking;

public class MonsterBullet : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] public float bulletDamage;
    
    protected Collider coll;

    protected Vector3 dir;

    [HideInInspector] public int randNum;
    [HideInInspector] public Vector3 monsPos;

    protected GameManager gameManager;

    protected float realDamage;

    private void Start()
    {
        gameManager = GameManager.Instance;
        coll = GetComponent<Collider>();
        Invoke("DestroyBullet", 3f);
        ShootDir();
        realDamage = bulletDamage + Mathf.Floor(GameManager.Instance.round / 5f) * 2f;  // 트리거에도 있음
        transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
    }

    void Update()
    {
        transform.position += dir * speed * Time.deltaTime;

        if (gameManager.currentGameTime <= 0)
        {
            CancelInvoke("DestroyBullet");
            DestroyBullet();
        }
    }

    void ShootDir()
    {
        dir = (Character.Instance.transform.position - transform.position).normalized;
        dir = new Vector3(dir.x, 0f, dir.z);
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Character")
        {
            realDamage = bulletDamage + Mathf.Floor(GameManager.Instance.round / 5f) * 2f;  // 트리거에도 있음
            other.GetComponent<Character>().OnDamaged(coll, realDamage);
            DestroyBullet();
            CancelInvoke("DestroyBullet");
        }
    }

    protected void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
