using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelect : MonoBehaviour
{
    [SerializeField] GameObject[] characters;
    [SerializeField] GameObject LockImage;

    bool[] characterClear;

    private void Start()
    {
        characterClear = new bool[(int)CHARACTER_NUM.Count];
        characterClear[(int)CHARACTER_NUM.Bagic] = Convert.ToBoolean(PlayerPrefs.GetInt("BagicClear", 0));
        LockImage.SetActive(!characterClear[(int)CHARACTER_NUM.Bagic]);
    }

    public void SelectCharacter(int num)
    {
        SoundManager.Instance.PlayES("SelectButton");
        GameObject character = Instantiate(characters[num], Vector3.zero, characters[num].transform.rotation);
        character.SetActive(false);
        character.GetComponent<Character>().characterNum = num;
        SceneManager.LoadScene("WeaponSelect");
    }
}
