using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Fire : Bullet
{
    [SerializeField] public GameObject explosion;

    protected IObjectPool<Explosion> exPool;

    public int grade;

    protected override void Awake()
    {
        base.Awake();
        exPool = new ObjectPool<Explosion>(CreateEx, OnGetEx, OnReleaseEx, OnDestroyEx, maxSize: 10);
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, initPos) > range)
        {
            DestroyBullet();
        }

        transform.position += new Vector3(dir.x, 0, dir.z) * speed * Time.deltaTime;

        // ÃÑ¾Ë °¢µµ
        angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(90, -angle, 0);
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Monster") && collision.collider.GetComponent<Monster>() != null)
        {
            Explosion();
        }

        base.OnCollisionEnter(collision);
    }

    void Explosion()
    {
        int rand = Random.Range(0, 100);
        if (rand <= 10 + gameManager.luck * 0.3f)
        {
            Explosion ex = exPool.Get();
            ex.grade = grade;
            ex.Setting();
        }
    }

    private Explosion CreateEx()
    {
        Explosion explosionPool = Instantiate(explosion, transform.position, transform.rotation).GetComponent<Explosion>();
        explosionPool.SetManagedPool(exPool);
        explosionPool.transform.SetParent(gameManager.bulletStorage);
        return explosionPool;
    }

    private void OnGetEx(Explosion exPool)
    {
        exPool.gameObject.SetActive(true);
    }

    private void OnReleaseEx(Explosion exPool)
    {
        exPool.gameObject.SetActive(false);
    }

    private void OnDestroyEx(Explosion exPool)
    {
        Destroy(exPool.gameObject);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        exPool.Clear();
    }
}
