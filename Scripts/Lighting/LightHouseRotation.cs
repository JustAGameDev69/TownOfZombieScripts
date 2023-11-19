using UnityEngine;

public class LightHouseRotation : MonoBehaviour
{
    public float rotateSpeed = 7f;

    private void Update()
    {
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
    }
}
