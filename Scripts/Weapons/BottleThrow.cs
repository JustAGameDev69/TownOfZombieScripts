using System.Collections.Generic;
using UnityEngine;

public class BottleThrow : MonoBehaviour
{
    public GameObject bottlePrefabs;
    public GameObject molotovPrefabs;
    public Transform throwPoint;
    public LayerMask collideLayer;

    [Header("Line Edit")]
    [SerializeField] private int linePoints;
    [SerializeField] private float linePointDistance;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float throwPower;
    [SerializeField] private Material whiteM, redM;         //White for the bottle itself and red for the bottle with rags.


    private LineRenderer throwLine;     //line


    private void Start()
    {
        throwLine = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (SaveScripts.inventoryOpen || SaveScripts.weaponID < 7)      //Fix rotation bug in inventory and disable aim line when unequip throwing items
            return;

        float horizontalRotation = Input.GetAxis("Mouse X") * 2;
        float verticalRotation = Input.GetAxis("Mouse Y") * 2;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, horizontalRotation * rotationSpeed, verticalRotation * rotationSpeed));

        if (Input.GetAxis("Mouse Y") > 0)
        {
            if (throwPower < 70)
            {
                throwPower += 6 * Time.deltaTime;
            }
        }
        else if (Input.GetAxis("Mouse Y") < 0)
        {
            if (throwPower > 20)
            {
                throwPower -= 12 * Time.deltaTime;
            }
        }

        throwLine.positionCount = linePoints;
        List<Vector3> points = new List<Vector3>();
        Vector3 startPos = throwPoint.position;
        Vector3 startVelocity = throwPoint.forward * throwPower;

        if (Input.GetMouseButton(1))
        {
            ChangeAimLineColor();

            for (float i = 0; i < linePoints; i += linePointDistance)
            {
                Vector3 newPoint = startPos + i * startVelocity;
                newPoint.y = startPos.y + startVelocity.y + i + Physics.gravity.y / 2f * i * i;
                points.Add(newPoint);

                if (Physics.OverlapSphere(newPoint, 0.01f, collideLayer).Length > 0)
                {
                    throwLine.positionCount = points.Count;
                    break;
                }
            }
            throwLine.SetPositions(points.ToArray());
        }
        else if (Input.GetMouseButtonUp(1))
        {
            throwLine.positionCount = 0;
        }

        if (WeaponsManager.emptyBottleThrow)
        {
            WeaponsManager.emptyBottleThrow = false;
            GameObject createBottle = Instantiate(bottlePrefabs, throwPoint.position, throwPoint.rotation);
            createBottle.GetComponentInChildren<Rigidbody>().velocity = throwPoint.transform.forward * throwPower;
            SaveScripts.weaponAmount[7]--;             //Reduce bottle
            SaveScripts.UpdateWeaponInventory();
        }

        if (WeaponsManager.molotovBottleThrow)
        {
            WeaponsManager.molotovBottleThrow = false;
            GameObject createBottle = Instantiate(molotovPrefabs, throwPoint.position, throwPoint.rotation);
            createBottle.GetComponentInChildren<Rigidbody>().velocity = throwPoint.transform.forward * throwPower;
            SaveScripts.weaponAmount[7]--;
            SaveScripts.itemAmount[3]--;                //Reduce rags
            SaveScripts.UpdateWeaponInventory();
        }

    }

    private void ChangeAimLineColor()
    {
        if (SaveScripts.weaponID == 7)
            throwLine.material = whiteM;
        else if (SaveScripts.weaponID == 8)
            throwLine.material = redM;
    }
}
