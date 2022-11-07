using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : Singleton<CharacterMove>
{
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] SpriteRenderer rend;
    [SerializeField] Animator anim;
    [SerializeField] float speed;

    bool isRun = false;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
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
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        transform.position += new Vector3(x, 0, z) * speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            rend.flipX = true;
            isRun = true;
        }

        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            rend.flipX = false;
            isRun = true;
        }

        else
            isRun = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("2");
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("1");
    }
}
