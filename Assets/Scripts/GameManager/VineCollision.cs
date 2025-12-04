using UnityEngine;
using UnityEngine.Tilemaps;

public class Vine : MonoBehaviour
{
    public static Vine instance { get; private set; }
    private string groundLayerName = "Ground";
    private string vineLayerName = "Vine";

    private int vineLayerID;
    private int groundLayerID;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        vineLayerID = LayerMask.NameToLayer(vineLayerName);
        groundLayerID = LayerMask.NameToLayer(groundLayerName);
        
        if (vineLayerID == -1 || groundLayerID == -1)
        {
            Debug.LogError("Layer không tồn tại!");
        }

        gameObject.layer = vineLayerID;
    }

    public void ChangeLayerToGround()
    {
        gameObject.layer = groundLayerID;
    }
    public void ChangeLayerToVine() => gameObject.layer = vineLayerID;
}