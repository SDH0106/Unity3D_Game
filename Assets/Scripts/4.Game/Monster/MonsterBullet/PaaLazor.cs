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

    GameManager gameManager;

    bool isAttack;

    float realDamage;

    private void Start()
    {
        gameManager = GameManager.Instance;

        character = Character.Instance;
        coll = GetComponent<Collider>();
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        isAttack = false;

        realDamage = damage * (1 + Mathf.Floor(gameManager.round / 30)) + Mathf.Floor(gameManager.round / 5) * 2f;  // 트리거에도 있음

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

        if (gameManager.currentGameTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Character") && !isAttack)
        {
            realDamage = damage * (1 + Mathf.Floor(gameManager.round / 30)) + Mathf.Floor(gameManager.round / 5) * 2f;  // 트리거에도 있음
            character.OnDamaged(coll, realDamage);
            isAttack = true;
            StartCoroutine(IEInvincible());
        }
    }

    IEnumerator IEInvincible()
    {
        yield return new WaitForSeconds(0.6f);
        isAttack = false;
    }
}
