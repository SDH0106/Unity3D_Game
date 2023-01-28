using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] GameObject showPosUI;
    Animator anim;

    Vector3 dir;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        ChestOpen();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Character"))
        {
            anim.SetTrigger("isOpen");
            GameSceneUI.Instance.chestCount++;
            SoundManager.Instance.PlayES("Chest");
        }
    }

    void ChestOpen()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Open"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                Destroy(gameObject);
            }
        }
    }

    void ShowChestPos()
    {
        dir = transform.position - Character.Instance.transform.position;
        Instantiate(showPosUI);
        showPosUI.transform.position = Camera.main.WorldToViewportPoint(dir);
        showPosUI.transform.rotation.SetLookRotation(dir);

    }
}
