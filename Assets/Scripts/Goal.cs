using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Goal : MonoBehaviour
{
    static public bool goalMet = false;

    void OnTriggerEnter(Collider other)
    {
        Projectile p = other.GetComponent<Projectile>();
        if (p != null) {
            Goal.goalMet = true;
            Material mat = this.GetComponent<Renderer>().material;
            Color c = mat.color;
            c.a = 0.75f;
            mat.color = c;
        }
    }
}
