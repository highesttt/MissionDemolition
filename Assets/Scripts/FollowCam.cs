using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{

    static public GameObject POI;

    [Header("Inscribed")]
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;

    [Header("Dynamic")]
    public float camZ;

    void Awake()
    {
        camZ = this.transform.position.z;
    }

    void FixedUpdate()
    {

        Vector3 destination = Vector3.zero;

        if (POI != null) {
            Rigidbody poiRigid = POI.GetComponent<Rigidbody>();

            if (poiRigid != null && poiRigid.IsSleeping()) {
                POI = null;
            }

        }

        if (POI != null) {
            destination = POI.transform.position;
        }

        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);

        destination = Vector3.Lerp(this.transform.position, destination, easing);

        destination.z = camZ;
        this.transform.position = destination;
    }
}
