using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] Texture2D cursorNormal;

    private void Start()
    {
        Vector2 cursorHotSpot = new Vector3(cursorNormal.width * 0.5f, cursorNormal.height * 0.5f);
        Cursor.SetCursor(cursorNormal, cursorHotSpot, CursorMode.ForceSoftware);

        SoundManager.Instance.PlayBGM(4, false);
    }
}
