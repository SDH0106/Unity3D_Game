using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ChamelTongue : MonoBehaviour
{
    [SerializeField] Monster mainBody;

    Character character;
    Collider coll;

    private void Start()
    {
        character = Character.Instance;
        coll = GetComponent<Collider>();
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            character.OnDamaged(coll, mainBody.stat.monsterDamage);
        }
    }
}
