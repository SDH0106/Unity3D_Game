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

    private void Update()
    {
        if (!character.isDead)
        {
            transform.position = new Vector3(Mathf.Clamp(character.transform.position.x, -14f, 17f), transform.position.y, Mathf.Clamp(character.transform.position.z, -49f, -31f));
            //transform.position = Vector3.Lerp(transform.position, new Vector3(character.transform.position.x, transform.position.y, character.transform.position.z), 5*Time.deltaTime);
        }
    }
}
