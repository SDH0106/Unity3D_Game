using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelect : MonoBehaviour
{
    [SerializeField] GameObject[] characters;

    public void SelectCharacter(int num)
    {
        GameObject character = Instantiate(characters[num], Vector3.zero, characters[num].transform.rotation);
        character.SetActive(false);
        character.GetComponent<Character>().characterNum = num;
        SceneManager.LoadScene("WeaponSelect");
    }
}
