using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    private void Start()
    {
        SoundManager.Instance.PlayBGM(4, false);
    }
}
