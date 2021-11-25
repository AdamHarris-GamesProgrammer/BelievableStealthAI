using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harris.Saving;
using System;

public class SavingWrapper : MonoBehaviour
{
#if UNITY_EDITOR
    public void Update()
    {
        //Development test keys
        if (Input.GetKeyDown(KeyCode.O)) Save();
        if (Input.GetKeyDown(KeyCode.L)) Load();
        if (Input.GetKeyDown(KeyCode.Delete)) Delete();
    }
#endif 

    public void Delete()
    {
        GetComponent<SavingSystem>().Delete("save.sav");
    }

    public void Save()
    {
        GetComponent<SavingSystem>().Save("save.sav");
    }

    public void Load()
    {
        GetComponent<SavingSystem>().Load("save.sav");
    }
}
