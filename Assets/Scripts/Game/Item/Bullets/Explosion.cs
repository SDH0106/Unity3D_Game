using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] public float exDamage;
    [SerializeField] ExDamageUI damageUI;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Monster")
        {
            GameObject pool = Instantiate(damageUI, collision.contacts[0].point, Quaternion.Euler(90, 0, 0)).gameObject;
            pool.transform.SetParent(GameManager.Instance.damageStorage);
            collision.collider.GetComponent<Monster>().OnDamaged(exDamage * GameManager.Instance.round);
        }
    }
}
