using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProfile : MonoBehaviour
{
    [Header("Profile")]
    public TMP_InputField nameInputField;
    public TMP_InputField lastNameInputField;
    public TMP_InputField ageInputField;
    public Image countryflag;
    [Header("Resources")]
    public TMP_Text moneyText;
    private string defaultName;
    private string defaultLastName;
    private string defaultAge;

    void Start()
    {
        defaultName = PlayerPrefs.GetString("Name", "First Name");
        defaultLastName = PlayerPrefs.GetString("LastName", "Last Name");
        defaultAge = PlayerPrefs.GetString("Age", "Age");

        nameInputField.text = defaultName;
        lastNameInputField.text = defaultLastName;
        ageInputField.text = defaultAge;
    }

    public void Edit()
    {
        nameInputField.interactable = true;
        lastNameInputField.interactable = true;
        ageInputField.interactable = true;
    }

    public void Save()
    {
        PlayerPrefs.SetString("Name", nameInputField.text);
        PlayerPrefs.SetString("Last Name", lastNameInputField.text);
        PlayerPrefs.SetString("Age", ageInputField.text);

        nameInputField.interactable = false;
        lastNameInputField.interactable = false;
        ageInputField.interactable = false;
    }
}
