using UnityEngine;
using UnityEngine.UI;

public class PickupScript : MonoBehaviour
{
    public GameObject pickUpPanel;
    public Image image;
    public Sprite[] weaponIcons;
    public Sprite[] itemIcons;
    public Sprite[] ammoIcons;
    public Text mainTitle;
    public GameObject doorMessage;
    public GameObject generatorMessage;
    public GameObject vaccineMessage;
    public Text doorText;
    public AudioClip[] pickupSounds;

    private RaycastHit hit;
    private RaycastHit gunHit;
    private RaycastHit[] shotgunHits;
    private AudioSource audioPlayer;
    private int objID = 0;
    [SerializeField] private float shootRangeofShotgun;
    [SerializeField] private float raycastRadius;
    [SerializeField] private float raycastDistance;
    [SerializeField] private LayerMask pickupLayer;
    [SerializeField] private string[] weaponNames;
    [SerializeField] private string[] itemNames;
    [SerializeField] private string[] ammoNames;

    private void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
    }

    private void Update()
    {
        PlayerPickup();

        //-----------------------------------PISTOL-----------------------------------------------------------
        if(Physics.SphereCast(transform.position, 0.01f, transform.forward, out gunHit, 400))
        {
            if(gunHit.transform.gameObject.name == "Body" && SaveScripts.weaponID == 4)
            {
                if(Input.GetMouseButtonDown(0) && SaveScripts.currentAmmo[4] > 0)
                {
                    gunHit.transform.gameObject.GetComponent<ZombieDamageTaken>().SendGunDamage(gunHit.point);
                }
            }
        }
        //------------------------------------------------------------------------------------------------------

        if(SaveScripts.weaponID == 5)
        {
            shotgunHits = Physics.SphereCastAll(transform.position, shootRangeofShotgun, transform.forward, 40);

            for(int i = 0; i < shotgunHits.Length; i++)
            {
                if (shotgunHits[i].transform.gameObject.name == "Body")
                {
                    if (Input.GetMouseButtonDown(0) && SaveScripts.currentAmmo[5] > 0)
                    {
                        shotgunHits[i].transform.gameObject.GetComponent<ZombieDamageTaken>().SendGunDamage(shotgunHits[i].point);
                    }
                }
            }
        }

    }

    private void PlayerPickup()
    {
        if (Physics.SphereCast(transform.position, raycastRadius, transform.forward, out hit, raycastDistance, pickupLayer))
        {
            if (Vector3.Distance(transform.position, hit.transform.position) < raycastDistance)
            {
                if (hit.transform.gameObject.CompareTag("weapon"))
                {
                    objID = (int)hit.transform.gameObject.GetComponent<WeaponType>().typeOfWeapon;
                    ShowPickUpPanel(weaponIcons[objID], weaponNames[objID]);

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        PickupWeapon(objID);
                    }
                }
                else if (hit.transform.gameObject.CompareTag("item"))
                {
                    objID = (int)hit.transform.gameObject.GetComponent<ItemType>().typeOfItem;
                    ShowPickUpPanel(itemIcons[objID], itemNames[objID]);

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        PickupItem(objID);
                    }
                }
                else if (hit.transform.gameObject.CompareTag("ammo"))
                {
                    objID = (int)hit.transform.gameObject.GetComponent<AmmoType>().typeOfAmmo;
                    ShowPickUpPanel(ammoIcons[objID], ammoNames[objID]);

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        PickupAmmo(objID);
                    }
                }
                else if (hit.transform.gameObject.CompareTag("door"))
                {
                    SaveScripts.doorObject = hit.transform.gameObject;
                    objID = (int)hit.transform.gameObject.GetComponent<DoorType>().typeOfDoor;
                    DoorType door = hit.transform.gameObject.GetComponent<DoorType>();
                    if (door.locked)
                    {
                        if (!hit.transform.gameObject.GetComponent<DoorType>().electricDoor)
                            door.message = "Locked. Need " + door.typeOfDoor + " key to open";
                        else if (door.electricDoor && !SaveScripts.generatorOn)
                            door.message = "No power supply. Turn on the generator to open!";
                    }
                    else if (door.electricDoor && SaveScripts.generatorOn)
                    {
                        if (!door.opened)
                            door.message = "Power restore. Press E to open!";
                        else if (door.opened)
                            door.message = "Press E to close!";
                    }
                    else if (!door.locked && (objID == 1 || objID == 2))
                    {
                        if (!door.opened)
                            door.message = "Unlocked. Press E to open!";
                        else
                            door.message = "Press E to close!";
                    }

                    ShowDoorMessage(door.message);

                    if (Input.GetKeyDown(KeyCode.E) && !door.locked)
                    {
                        ToggleDoor(door);
                    }
                }
                else if (hit.transform.gameObject.CompareTag("generator"))
                {
                    SaveScripts.generator = hit.transform.gameObject;
                    if (!SaveScripts.generatorOn)
                    {
                        generatorMessage.SetActive(true);
                    }
                    else
                    {
                        generatorMessage.SetActive(false);
                    }
                }
                else if (hit.transform.gameObject.CompareTag("vaccine"))
                {
                    SaveScripts.vaccine = hit.transform.gameObject;
                    vaccineMessage.SetActive(true);
                }

            }
        }
        else
        {
            HidePickUpPanel();
            HideDoorMessage();
            SaveScripts.doorObject = null;

            generatorMessage.SetActive(false);
            vaccineMessage.SetActive(false);

            SaveScripts.vaccine = null;
            SaveScripts.generator = null;
        }
    }

    #region Pick up Function
    private void PlayPickupSound()
    {
        audioPlayer.clip = pickupSounds[3];
        audioPlayer.Play();
    }

    private void ShowPickUpPanel(Sprite icon, string title)
    {
        pickUpPanel.SetActive(true);
        image.sprite = icon;
        mainTitle.text = title;
    }

    private void PickupWeapon(int weaponID)
    {
        SaveScripts.weaponAmount[weaponID]++;
        PlayPickupSound();
        SaveScripts.UpdateWeaponInventory();
        Destroy(hit.transform.gameObject, 0.2f);
    }

    private void PickupItem(int itemID)
    {
        SaveScripts.itemAmount[itemID]++;
        PlayPickupSound();
        SaveScripts.UpdateItemInventory();
        Destroy(hit.transform.gameObject, 0.2f);
    }

    private void PickupAmmo(int ammoID)
    {
        if (ammoID == 0)
            SaveScripts.ammoAmount[ammoID] += 12;
        else if (ammoID == 1)
            SaveScripts.ammoAmount[ammoID] += 4;

        PlayPickupSound();
        SaveScripts.UpdateItemInventory();
        Destroy(hit.transform.gameObject, 0.2f);
    }
    private void HidePickUpPanel()
    {
        pickUpPanel.SetActive(false);
    }

    #endregion

    #region Door Function

    private void ShowDoorMessage(string message)
    {
        doorMessage.SetActive(true);
        doorText.text = message;
    }

    private void ToggleDoor(DoorType door)
    {
        audioPlayer.clip = pickupSounds[objID];
        audioPlayer.Play();

        if (!door.opened)
        {
            door.message = "Press E to close";
            door.opened = true;
            door.GetComponent<Animator>().SetTrigger("Open");
        }
        else
        {
            door.message = "Press E to open";
            door.opened = false;
            door.GetComponent<Animator>().SetTrigger("Close");
        }
    }

    private void HideDoorMessage()
    {
        doorMessage.SetActive(false);
    }

    #endregion
}
