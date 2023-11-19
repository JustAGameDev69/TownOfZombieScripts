using System.Collections;
using UnityEngine;

public class WeaponsManager : MonoBehaviour
{
    public enum weaponSelect
    {
        knife,
        cleaver,
        bat,
        axe,
        pistol,
        shotgun,
        sprayCan,
        bottle,
        bottleWithRags
    }

    public weaponSelect chosenWeapon;
    public GameObject[] weapons;
    public GameObject sprayCanPanel;
    public SprayCanScript sprayCanScript;
    public AudioClip[] reloadSound;
    public static bool emptyBottleThrow;
    public static bool molotovBottleThrow;

    private Animator animator;
    private AudioSource audioPlayer;
    private AnimatorStateInfo animatorInfo;
    private int currentWeaponID;
    private bool spraySoundOn = false;
    private bool canAttack = true;
    private bool sprayEmpty = false;
    private bool stopSpray = false;
    [SerializeField] private AudioClip[] weaponSound;

    private void Start()
    {
        SaveScripts.weaponID = (int)chosenWeapon;
        sprayCanScript = sprayCanPanel.GetComponent<SprayCanScript>();
        animator = GetComponent<Animator>();
        audioPlayer = GetComponent<AudioSource>();
        ChangeWeapon();
    }

    private void Update()
    {
        //----------------------FIX AUTO THROWN BOTTLE-----------------------//
        animatorInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (animatorInfo.IsTag("BottleThrown"))
            canAttack = false;
        else
            canAttack = true;

        //--------------------------------------------------------------------//


        if (SaveScripts.weaponID != currentWeaponID)
            ChangeWeapon();

        if (Input.GetMouseButtonDown(0) && canAttack && Time.timeScale == 1.0f)
        {
            if (SaveScripts.inventoryOpen)      //Fix can attack when inventory open
                return;

            if (SaveScripts.currentAmmo[SaveScripts.weaponID] > 0 && SaveScripts.energy > 20)
            {
                animator.SetTrigger("Attack");
                audioPlayer.clip = weaponSound[SaveScripts.weaponID];
                audioPlayer.Play();

                if (SaveScripts.weaponID == 4 || SaveScripts.weaponID == 5)
                {
                    SaveScripts.currentAmmo[SaveScripts.weaponID]--;            //Only reduce ammo when using pistol or shotgun
                    SaveScripts.gunUsed = true;
                }
            }
            else
            {
                if (SaveScripts.weaponID == 4 || SaveScripts.weaponID == 5)
                {
                    audioPlayer.clip = weaponSound[9];                  //Play gun out of ammo sound
                    audioPlayer.Play();
                }
            }
        }

        //------------------------------------SPRAY CAN-----------------------------------//
        if (SaveScripts.weaponID == 6 && !SaveScripts.inventoryOpen)
        {
            if (Input.GetMouseButton(0) && sprayCanScript.sprayAmount > 0.0f)
            {
                stopSpray = false;
                sprayEmpty = false;
                if (!spraySoundOn)
                {
                    stopSpray = true;
                    spraySoundOn = true;
                    animator.SetTrigger("Attack");
                    StartCoroutine(PlaySpraySound());
                }
            }
            else if (Input.GetMouseButtonUp(0) || sprayCanScript.sprayAmount <= 0.0f)
            {
                if (!stopSpray)
                {
                    stopSpray = true;
                    animator.SetTrigger("Release");
                    spraySoundOn = false;
                    audioPlayer.Stop();
                    audioPlayer.loop = false;
                }
            }

            if(sprayCanScript.sprayAmount <= 0.0f && !sprayEmpty)
            {
                sprayEmpty = true;
                SaveScripts.weaponAmount[6]--;
                if (SaveScripts.weaponAmount[6] == 0)
                    SaveScripts.weaponPickup[6] = false;
            }
        }

        //------------------------------------DONE SPRAY CAN-----------------------------------//

        //------------------------------------RELOAD AMMO--------------------------------------//

        if (SaveScripts.weaponID == 4 || SaveScripts.weaponID == 5)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (SaveScripts.ammoAmount[SaveScripts.weaponID - 4] <= 0)
                    return;

                SaveScripts.currentAmmo[SaveScripts.weaponID] += SaveScripts.ammoAmount[SaveScripts.weaponID - 4];      //Pistol at weaponID = 4, pistol ammo ammount = 0
                SaveScripts.ammoAmount[SaveScripts.weaponID - 4] = 0;
                animator.SetTrigger("Reload");
                audioPlayer.clip = reloadSound[SaveScripts.weaponID - 4];
                audioPlayer.Play();
            }
        }

    }

    private void ChangeHandPositionOnWeapon()                   //Better in-game feeling
    {
        switch (chosenWeapon)
        {
            case weaponSelect.knife:
                transform.localPosition = new Vector3(0.02f, -0.193f, 0.66f);
                break;
            case weaponSelect.cleaver:
                transform.localPosition = new Vector3(0.02f, -0.193f, 0.66f);
                break;
            case weaponSelect.bat:
                transform.localPosition = new Vector3(0.02f, -0.193f, 0.66f);
                break;
            case weaponSelect.axe:
                transform.localPosition = new Vector3(0.02f, -0.193f, 0.66f);
                break;
            case weaponSelect.pistol:
                transform.localPosition = new Vector3(0.02f, -0.193f, 0.66f);
                break;
            case weaponSelect.shotgun:
                transform.localPosition = new Vector3(0.02f, -0.193f, 0.4f);
                break;
            case weaponSelect.sprayCan:
                transform.localPosition = new Vector3(0.02f, -0.193f, 0.66f);
                break;
            case weaponSelect.bottle:
                transform.localPosition = new Vector3(0.02f, -0.193f, 0.66f);
                break;
        }

    }

    private void ChangeWeapon()
    {
        foreach (GameObject weapon in weapons)
        {
            weapon.SetActive(false);
        }
        weapons[SaveScripts.weaponID].SetActive(true);
        chosenWeapon = (weaponSelect)SaveScripts.weaponID;
        animator.SetInteger("WeaponID", SaveScripts.weaponID);
        animator.SetBool("WeaponChange", true);
        currentWeaponID = SaveScripts.weaponID;

        ChangeHandPositionOnWeapon();
        StartCoroutine(WeaponReset());
    }

    public void ThrowEmptyBottle() => emptyBottleThrow = true;
    public void ThrowMolotovBottle() => molotovBottleThrow = true;

    public void LoadAnotherBottle()
    {
        if (SaveScripts.weaponID == 7)
            ChangeWeapon();
    }

    public void LoadAnotherMolotov()
    {
        if (SaveScripts.weaponID == 8)
            ChangeWeapon();
    }


    IEnumerator WeaponReset()           //Pausing time weapon change
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("WeaponChange", false);
    }

    IEnumerator PlaySpraySound()
    {
        yield return new WaitForSeconds(0.3f);
        audioPlayer.clip = weaponSound[SaveScripts.weaponID];
        audioPlayer.Play();
        audioPlayer.loop = true;
    }
}
