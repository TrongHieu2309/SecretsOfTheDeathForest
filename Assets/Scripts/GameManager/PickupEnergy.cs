using UnityEngine;

public class PickupEnergy : MonoBehaviour
{
    [SerializeField] private Transform destinationEnergy;
    private Transform targetUI;
    private Vector3 startPos;
    private float speed = 6f;
    private bool isFlying = false;

    public void FlyToUI()
    {
        targetUI = destinationEnergy;
        startPos = transform.position;
        isFlying = true;
    }

    void Update()
    {
        if (!isFlying) return;

        float t = speed * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, targetUI.position, t);
    }
}
