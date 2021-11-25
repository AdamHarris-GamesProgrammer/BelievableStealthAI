using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableObjectOnTrigger : MonoBehaviour
{
    [SerializeField] GameObject _objectToShow = null;

    private void OnTriggerEnter(Collider other)
    {
        //Enables the object when the player is close
        if (other.CompareTag("Player"))
        {
            _objectToShow.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Disables the object when the player is close
        if (other.CompareTag("Player"))
        {
            _objectToShow.SetActive(false);
            //gameObject.SetActive(false);
        }
    }
}
