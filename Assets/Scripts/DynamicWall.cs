using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicWall : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 180.0f;

    void FixedUpdate()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
