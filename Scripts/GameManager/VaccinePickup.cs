using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaccinePickup : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && SaveScripts.vaccine != null)
        {
            SaveScripts.gotVaccine = true;
            Destroy(gameObject);
        }
    }
}
