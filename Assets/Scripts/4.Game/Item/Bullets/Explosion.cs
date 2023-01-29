using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] float exDamage;
    [SerializeField] ExDamageUI damageUI;
    public int grade;

    private void Start()
    {
        SoundManager.Instance.PlayES("Explosion");
        exDamage = exDamage + GameManager.Instance.exDmg;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Monster")
        {
            GameObject pool = Instantiate(damageUI, collision.contacts[0].point, Quaternion.Euler(90, 0, 0)).gameObject;
            pool.GetComponent<ExDamageUI>().damage = exDamage * grade;
            pool.transform.SetParent(GameManager.Instance.damageStorage);
            collision.collider.GetComponent<Monster>().PureOnDamaged(exDamage * grade);
        }
    }
}
