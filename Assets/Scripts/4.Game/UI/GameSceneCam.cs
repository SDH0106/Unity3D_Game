using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneCam : MonoBehaviour
{
    Character character;

    void Start()
    {
        character = Character.Instance;
    }

    void Update()
    {
        if (!character.isDead)
        {
            transform.position = new Vector3(character.transform.position.x, transform.position.y, character.transform.position.z);
        }
    }
}
