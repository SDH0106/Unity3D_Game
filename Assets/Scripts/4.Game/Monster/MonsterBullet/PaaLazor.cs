using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PaaLazor : MonoBehaviour
{
    [SerializeField] float damage;

    Character character;
    Collider coll;
    SpriteRenderer rend;
    Animator anim;

    public bool isFlip;

    private void Start()
    {
        character = Character.Instance;
        coll = GetComponent<Collider>();
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        damage = damage + Mathf.Floor(GameManager.Instance.round / 5) * 2f;

        if (isFlip)
            rend.flipX = true;

        else
            rend.flipX = false;

        anim.SetBool("isFlip", isFlip);
    }

        private void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("RightPaaLazor") || anim.GetCurrentAnimatorStateInfo(0).IsName("LeftPaaLazor"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                Destroy(gameObject);
            }
        }
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            character.OnDamaged(coll, damage);
        }
    }
}
