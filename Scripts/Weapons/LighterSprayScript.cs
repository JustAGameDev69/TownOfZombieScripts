using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LighterSprayScript : MonoBehaviour
{
    public GameObject lighterObj;

    private void OnEnable()
    {
        lighterObj.SetActive(true);
    }

    private void OnDisable()
    {
        lighterObj.SetActive(false);
    }
}
