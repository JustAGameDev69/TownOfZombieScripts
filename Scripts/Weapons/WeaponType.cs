using UnityEngine;

public class WeaponType : MonoBehaviour
{
    public enum TypeOfWeapon
    {
        knife,
        cleaver,
        bat,
        axe,
        pistol,
        shotgun,
        sprayCan,
        bottle,
        molotov
    }

    public TypeOfWeapon typeOfWeapon;
}
