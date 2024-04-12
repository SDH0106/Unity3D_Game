using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectSceneCard : WeaponCard
{
    [SerializeField] GameObject lockPanel;

    private void Start()
    {
        itemManager = ItemManager.Instance;
        gameManager = GameManager.Instance;
        character = Character.Instance;

        Setting();
        CardColor();

        if (character.characterNum == 0)
            lockPanel.SetActive(false);

        else if (character.characterNum == 1)
        {
            if (selectedWeapon.Type == WeaponInfo.WEAPON_TYPE.°Ë)
                lockPanel.SetActive(false);

            else
                lockPanel.SetActive(true);
        }
    }

    protected override void Setting()
    {
        price = 0;
        base.Setting();
    }

    public override void Select()
    {
        base.Select();
        character.gameObject.SetActive(true);
        gameManager.ToNextScene("Game");
    }
}
