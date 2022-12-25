using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Predator : MonoBehaviour
{
    [SerializeField] FoV FoV;
    [SerializeField] float regularSpeed = 3.0f;
    [SerializeField] int numberRays = 180;
    [SerializeField] float chaseSpeed = 4.0f;
    [SerializeField] float currentSpeed = 0.0f;
    float targetTime, elapsedTime;
    bool preyFound = false;
    RaycastHit hit;

    private void Start()
    {
        NewHeadingAndTime();
        currentSpeed = regularSpeed;
    }

    private void FixedUpdate()
    {
        // If prey is found, chase them and increase speed
        if (preyFound && hit.collider.tag == "Prey")
        {
            transform.LookAt(hit.transform.position);
            transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
            if(currentSpeed < chaseSpeed)
            {
                currentSpeed += 0.5f;
            }
            preyFound = false;
        }
        // If prey is lost, return to regular state and decrease speed
        // or if in regular state, do an update of frame
        // runs for target time chosen by newheadingandtime
        else if (targetTime >= elapsedTime)
        {
            transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
            if (currentSpeed > regularSpeed)
            {
                currentSpeed -= 0.5f;
            }
            elapsedTime += Time.deltaTime;
        }
        // Get new heading and time
        else
        {
            NewHeadingAndTime();
        }
        LookForPrey();
    }

    // Look for prey bia raycasts and tags
    private void LookForPrey()
    {
        for (int i = 0; i <= numberRays; i++)
        {
            Vector3 currentRay = FoV.angleToVector3Heading((-FoV.viewAngle / 2) + ((FoV.viewAngle / numberRays) * i));
            if (Physics.Raycast(FoV.transform.position, currentRay, out hit, FoV.viewRange))
            {
                if (hit.collider.tag == "Prey")
                {
                    preyFound = true;
                    break;
                }
                else
                {
                    preyFound = false;
                }
            }
        }
    }

    // Get random heading and time to wander
    private void NewHeadingAndTime()
    {
        elapsedTime = 0.0f;
        targetTime = UnityEngine.Random.Range(0.5f, 3.0f);
        transform.eulerAngles = new Vector3(0, UnityEngine.Random.Range(0, 360), 0);
    }

    // If collides with prey, destroy it. nom nom nom

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Prey")
        {
            preyFound = false;
            Destroy(other.gameObject);
            NewHeadingAndTime();
        }
    }

    // If collide with wall, avoid
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Wall")
        {
            Debug.Log("wall");
            transform.rotation = Quaternion.LookRotation(-transform.forward);
            //transform.eulerAngles = new Vector3(0, -transform.forward.y, 0);
            //transform.rotation = Quaternion.Inverse(transform.rotation);
        }
    }
}
