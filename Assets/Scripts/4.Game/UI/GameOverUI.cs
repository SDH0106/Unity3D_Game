using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] Camera cam;

    bool isActive;

    private void Start()
    {
        SoundManager.Instance.PlayBGM(4, false);
        cam.transform.position = Camera.main.transform.position;
    }
}
