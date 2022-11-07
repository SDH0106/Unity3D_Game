using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Monster : MonoBehaviour
{
    [SerializeField] SpriteRenderer rend;
    [SerializeField] Animator anim;
    [SerializeField] float speed;

    bool isRun = false;

    private IObjectPool<Monster> managedPool;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        anim.SetBool("isRun", isRun);
    }

    void Move()
    {
        Vector3 characterPos = CharacterMove.Instance.transform.position;
        Vector3 dir = characterPos - transform.position;

        transform.position = Vector3.MoveTowards(transform.position, characterPos, speed * Time.deltaTime);

        isRun = true;

        if (dir == Vector3.zero)
            isRun = false;

        if (dir.x < 0)
            rend.flipX = true;

        else if (dir.x >= 0)
            rend.flipX = false;
    }

    public void SetManagedPool(IObjectPool<Monster> pool)
    {
        managedPool = pool;
    }

    public void DestroyMonster()
    {
        managedPool.Release(this);
    }
}
