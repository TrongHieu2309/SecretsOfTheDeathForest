using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerPosition;
    [SerializeField] private Transform limitLeft;
    [SerializeField] private Transform limitRight;

    void LateUpdate()
    {
        if (playerPosition.position.x <= limitLeft.position.x)
        {
            transform.position = new Vector3(limitLeft.position.x, transform.position.y, transform.position.z);
            return;
        }
        if (playerPosition.position.x >= limitRight.position.x)
        {
            transform.position = new Vector3(limitRight.position.x, transform.position.y, transform.position.z);
            return;
        }

        transform.position = new Vector3(playerPosition.position.x, transform.position.y, transform.position.z);
    }
}
