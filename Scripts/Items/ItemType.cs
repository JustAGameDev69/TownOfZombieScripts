using UnityEngine;

public class ItemType : MonoBehaviour
{
    public enum TypeOfItem
    {
        flashlight,
        nightvision,
        lighter,
        rags,
        healthPack,
        pills,
        waterbottle,
        apple,
        flbattery,
        nvbattery,
        housekey,
        cabinkey,
        jerrycan
    }

    public TypeOfItem typeOfItem;
}
