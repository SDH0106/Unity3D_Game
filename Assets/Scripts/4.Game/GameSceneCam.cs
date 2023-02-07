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

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(character.transform.position.x, transform.position.y, character.transform.position.z);
    }
}
