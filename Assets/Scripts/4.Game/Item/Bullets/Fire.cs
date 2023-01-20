using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Bullet
{
    [SerializeField] public GameObject explosion;

    public int grade;

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
        if (rand <= 5 + gameManager.luck * 0.2)
        {
            GameObject ex = Instantiate(explosion, transform.position, transform.rotation);
            ex.GetComponent<Explosion>().grade = grade;
        }
    }
}
