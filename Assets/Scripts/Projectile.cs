using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    const int LOOKBACK_COUNT = 10;
    static List<Projectile> PROJ_LIST = new List<Projectile>();

    [SerializeField]
    private bool _awake = true;
    public bool awake {
        get { return _awake; }
        private set { _awake = value; }
    }

    private Vector3 prevPos;
    private List<float> deltas = new List<float>();
    private Rigidbody rigid;
    public bool hasExploded = false;

    void Start() {
        rigid = this.GetComponent<Rigidbody>();
        awake = true;
        prevPos = new Vector3(1000, 1000, 0);
        deltas.Add(1000);
        PROJ_LIST.Add(this);
    }

    void FixedUpdate() {
        if (rigid.isKinematic || !awake) return;

        Vector3 deltaV3 = this.transform.position - prevPos;
        deltas.Add(deltaV3.magnitude);
        prevPos = this.transform.position;

        while (deltas.Count > LOOKBACK_COUNT) {
            deltas.RemoveAt(0);
        }

        float maxDelta = 0;

        foreach (float delta in deltas) {
            if (delta > maxDelta) {
                maxDelta = delta;
            }
        }

        if (maxDelta <= Physics.sleepThreshold) {
            awake = false;
            rigid.Sleep();
        }
    }

    private void OnDestroy() {
        PROJ_LIST.Remove(this);
    }

    public static void DESTROY_PROJECTILES() {
        foreach (Projectile proj in PROJ_LIST) {
            Destroy(proj.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        Projectile otherProjectile = collision.gameObject.GetComponent<Projectile>();
        if (otherProjectile != null && otherProjectile != this && !this.hasExploded) {
            Rigidbody rb = GetComponent<Rigidbody>();
            Rigidbody otherRb = otherProjectile.GetComponent<Rigidbody>();

            if (rb != null && otherRb != null) {
                if (rb.velocity.magnitude > otherRb.velocity.magnitude) {
                    Explode(otherProjectile);
                } else {
                    otherProjectile.Explode(this);
                }
            }
        }
    }

    private void Explode(Projectile otherProjectile) {
        this.hasExploded = true;

        Destroy(otherProjectile.gameObject);

        float explosionRadius = 3.0f;
        float explosionForce = 4000.0f;
        Vector3 explosionPosition = otherProjectile.transform.position;

        Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);
        foreach (Collider hit in colliders) {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null && hit.CompareTag("Wall")) {
                rb.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);
            }
        }
    }
}
