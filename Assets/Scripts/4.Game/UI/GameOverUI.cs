using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] Camera cam;

    private void Start()
    {
        SoundManager.Instance.PlayBGM(3);
        SoundManager.Instance.audioSource.loop = false;
        cam.transform.position = Camera.main.transform.position;
    }
}
