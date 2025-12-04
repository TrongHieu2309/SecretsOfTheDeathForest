using System;
using UnityEngine;

public class Parallax : MonoBehaviour {
    private Transform cam;
    private Material mat;
    private float distance;

    [Range(0f, 0.05f)]
    [SerializeField] private float speed;

    void Awake()
    {
        cam = Camera.main.transform;
        mat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        BackMovement();
    }

    private void BackMovement()
    {
        distance = cam.position.x * speed;
        mat.SetTextureOffset("_MainTex", Vector2.right * distance);
    }
}