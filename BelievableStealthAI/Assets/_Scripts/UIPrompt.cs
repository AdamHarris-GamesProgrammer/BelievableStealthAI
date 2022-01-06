using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIPrompt : MonoBehaviour
{
    [SerializeField] Text _keyPrompt;
    [SerializeField] Text _promptDescription;
    [SerializeField] KeyCode _key;

    void Start()
    {
        _keyPrompt.text = _key.ToString();
    }

    public void SetText(string description)
    {
        _promptDescription.text = description;
    }
}
