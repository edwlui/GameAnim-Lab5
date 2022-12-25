using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;
using static UnityEngine.GraphicsBuffer;

public class FoV : MonoBehaviour
{   
    [Range(0, 360)][SerializeField] public float viewAngle;
    [Range(0, 100)][SerializeField] public float viewRange = 10.0f;
    [SerializeField] public int numberRaysDebug = 180;
    public Vector3 angleToVector3Heading(float viewAngle)
    {
        // Makes it local space so it rotates as the character rotates
        viewAngle += transform.eulerAngles.y;

        // Returns vector of forward heading + view angle
        // Note self x is cos and y is sin but for some reason it is 90 degrees wrong. Flipping it resolved the issue
        // Also don't even try using transform forward or you get epilepsy mode and waste another 3 hours
        return new Vector3(Mathf.Sin(viewAngle * Mathf.Deg2Rad), 0, Mathf.Cos(viewAngle * Mathf.Deg2Rad));
    }
}


// Can be commented out, fov for debugging
[CustomEditor(typeof(FoV))]
public class DrawFoV : Editor
{
    private void OnSceneGUI()
    {
        // Setup
        FoV FoV = (FoV)target;
        Handles.color = Color.red;

        // Make left and right bounding angle note: (/2 allows correct number of raycasts)
        Vector3 angleLeftBound = FoV.angleToVector3Heading(-FoV.viewAngle / 2);
        Vector3 angleRightBound = FoV.angleToVector3Heading(FoV.viewAngle / 2);

        // Draw left and right bounding angle
        Handles.DrawLine(FoV.transform.position, FoV.transform.position + (angleLeftBound * FoV.viewRange));
        Handles.DrawLine(FoV.transform.position, FoV.transform.position + (angleRightBound * FoV.viewRange));

        // Draw left half and right half of cone end
        Handles.DrawWireArc(FoV.transform.position, Vector3.up, angleLeftBound, FoV.viewAngle, FoV.viewRange);
        Handles.DrawWireArc(FoV.transform.position, Vector3.up, angleRightBound, -FoV.viewAngle, FoV.viewRange);

        // Draw ray casts
        Handles.color = Color.green;
        for (int i = 0; i <= FoV.numberRaysDebug; i++)
        {
            Vector3 currentRay = FoV.angleToVector3Heading((-FoV.viewAngle / 2) + ((FoV.viewAngle / FoV.numberRaysDebug) * i));
            Handles.DrawLine(FoV.transform.position, FoV.transform.position + currentRay * FoV.viewRange);
        }
    }
}

// SAVE: Old way in case new way breaks
//FoV FoV = (FoV)target;
//Handles.color = Color.red;

//// Make left and right bounding angle
//Vector3 angleA = FoV.angleToVector3Heading(-FoV.viewAngle);
//Vector3 angleB = FoV.angleToVector3Heading(FoV.viewAngle);

//// Draw left and right bounding angle
//Handles.DrawLine(FoV.transform.position, FoV.transform.position + angleA * FoV.viewRange);
//Handles.DrawLine(FoV.transform.position, FoV.transform.position + angleB * FoV.viewRange);

//// Draw left half and right half of cone end
//Handles.DrawWireArc(FoV.transform.position, Vector3.up, angleA, FoV.viewAngle, FoV.viewRange);
//Handles.DrawWireArc(FoV.transform.position, Vector3.up, angleB, -FoV.viewAngle, FoV.viewRange);

//// Draw ray casts
//Handles.color = Color.green;
//for (int i = 0; i <= FoV.numberRaysDebug; i += 2)
//{
//    Vector3 currentRay = FoV.angleToVector3Heading((-FoV.viewAngle) + ((FoV.viewAngle / FoV.numberRaysDebug) * i));
//    Handles.DrawLine(FoV.transform.position, FoV.transform.position + currentRay * FoV.viewRange);

//    currentRay = FoV.angleToVector3Heading((FoV.viewAngle) + ((-FoV.viewAngle / FoV.numberRaysDebug) * i));
//    Handles.DrawLine(FoV.transform.position, FoV.transform.position + currentRay * FoV.viewRange);
//}
