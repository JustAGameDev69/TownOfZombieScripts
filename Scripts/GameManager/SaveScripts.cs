using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;

public class SaveScripts : MonoBehaviour
{
    public static bool gunUsed = false;
    public static bool inventoryOpen = false;
    public static bool hasSmashed = false;
    public static bool isHidden = false;
    public static bool generatorOn = false;
    public static bool gotVaccine = false;
    public static int zombiesInGameAmount = 0;
    public static int weaponID = 0;
    public static int itemID = 0;
    public static int health;
    public static float energy;
    public static float infection;

    public static GameObject doorObject;
    public static GameObject generator;
    public static GameObject vaccine;
    public static Vector3 bottlePos = new Vector3 (0, 0, 0);

    public static List<GameObject> zombiesChasing = new List<GameObject>();

    public static int[] weaponAmount = new int[9];                  //[0,0,0,0,0,0,0,0]
    public static int[] itemAmount = new int[13];
    public static int[] ammoAmount = new int[2];                        //Total ammo that player had
    public static int[] currentAmmo = new int[9];                       //Ammo that currently in the gun
    public static bool[] weaponPickup = new bool[9];
    public static bool[] itemPickup = new bool[13];

    public GameObject deathMessage;
    public GameObject fpsDisplay;
    private GameObject[] zombies;


    private void Start()
    {
        energy = FirstPersonController.playerEnergy;

        //Basic start info
        health = 100;
        infection = 0;
        inventoryOpen = false;
        weaponID = 0;
        itemID = 0;
        energy = 100;
        generatorOn = false;
        gotVaccine = false;


        weaponPickup[0] = true;

        itemPickup[0] = true;
        itemPickup[1] = true;

        itemAmount[0] = 1;
        itemAmount[1] = 1;

        weaponAmount[0] = 1;

        ammoAmount[0] = 12;
        ammoAmount[1] = 4;

        for(int i = 0; i < currentAmmo.Length; i++)
        {
            currentAmmo[i] = 4;
        }
        currentAmmo[4] = 12;
        currentAmmo[6] = 0;

        fpsDisplay.SetActive(false);
    }

    private void Update()
    {
        if (zombiesInGameAmount < 0)
            zombiesInGameAmount = 0;
        else if(zombiesInGameAmount > 100)
        {
            zombies = GameObject.FindGameObjectsWithTag("zombie");
            for(int i = 100; i < zombies.Length; i++)
                Destroy(zombies[i]);
        }

        if (FirstPersonController.inventorySwitchOn)
            inventoryOpen = true;
        else if (!FirstPersonController.inventorySwitchOn)
            inventoryOpen = false;

        PlayerEnergyLogic();

        if (infection < 50)
            infection += 0.1f * Time.deltaTime;
        else if (infection >= 50 && infection < 100)
            infection += 0.2f * Time.deltaTime;

        //Reduce energy per attack
        if(Input.GetMouseButtonDown(0) && energy > 20 && weaponID < 4 && !inventoryOpen)
        {
            FirstPersonController.playerEnergy -= 5;
            energy = FirstPersonController.playerEnergy;
        }
        //--------------------------------------------------------

        //-------------------FPS DISPLAY -------------------------//

        if (Input.GetKeyDown(KeyCode.P))
        {
            fpsDisplay.SetActive(true);
        }

        if(bottlePos != Vector3.zero)
        {
            if (!hasSmashed)
            {
                hasSmashed = true;
                StartCoroutine(ResetBottlePos());
            }
        }

        if(health <= 0 || infection >= 100)
        {
            deathMessage.SetActive(true);
            Cursor.visible = true;
            StartCoroutine(PauseTime());
        }

        if (FirstPersonController.isCrounching)
            energy = FirstPersonController.playerEnergy;

    }

    IEnumerator PauseTime()
    {
        yield return new WaitForSeconds(2);
        Time.timeScale = 0f;
    }

    IEnumerator ResetBottlePos()
    {
        yield return new WaitForSeconds(20);
        bottlePos = Vector3.zero;
        hasSmashed = false;
    }

    private void PlayerEnergyLogic()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetAxis("Vertical") != 0 && FirstPersonController.playerEnergy > 0.0f)
        {
            FirstPersonController.playerEnergy -= 10 * Time.deltaTime;
            energy = FirstPersonController.playerEnergy;
        }

        if (energy < 100)
        {
            FirstPersonController.playerEnergy += 3.35f * Time.deltaTime;
            energy = FirstPersonController.playerEnergy;
        }
        
        if (energy >= 100)
            FirstPersonController.playerEnergy = energy;            //Fix energy reduce from the point we using restore energy items. (Not from the amount after use)
    }

    public static void UpdateItemInventory()
    {
        for (int i = 2; i < itemAmount.Length; i++)
        {
            if (itemAmount[i] > 0)
            {
                itemPickup[i] = true;
            }
            else if (itemAmount[i] == 0)
            {
                itemPickup[i] = false;
            }
        }
    }

    public static void UpdateWeaponInventory()
    {
        for (int i = 1; i < weaponAmount.Length; i++)
        {
            if (weaponAmount[i] > 0)
            {
                weaponPickup[i] = true;
            }
            else if (weaponAmount[i] == 0)
            {
                weaponPickup[i] = false;
            }
        }
    }
}
