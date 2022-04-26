using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIPrompt : MonoBehaviour
{
    [Header("UI Options")]
    [SerializeField] Text _keyPrompt;
    [SerializeField] Text _promptDescription;
    [SerializeField] KeyCode _key;

    void Start()
    {
        //Set the text for the prompt to the key 
        _keyPrompt.text = _key.ToString();
    }

    //Sets the text for the description of the prompt. e.g. "Open Door", "Close Door", "Assassinate Enemy"
    public void SetText(string description)
    {
        _promptDescription.text = description;
    }
}
