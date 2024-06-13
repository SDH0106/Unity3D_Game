using UnityEngine;

public class ArrowTypeChange : MonoBehaviour
{
    [SerializeField] Sprite[] arrowImages;

    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ChangeArrowType(bool isCatch)
    {
        if (isCatch)
        {
            spriteRenderer.sprite = arrowImages[1];
            GetComponent<ProjectileObjectPool>().ChangeDamage();
        }

        else
        {
            spriteRenderer.sprite = arrowImages[0];
            GetComponent<ProjectileObjectPool>().BackToDefaultDamage();
        }
    }
}
