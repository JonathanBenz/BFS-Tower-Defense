using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] Transform weapon;
    [SerializeField] ParticleSystem projectileParticles;
    [SerializeField] float range = 15f;
    Transform target;

    // Update is called once per frame
    void Update()
    {
        FindClosestTarget();
        AimWeapon();
    }

    void FindClosestTarget()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        Transform closestTarget = null;
        float maxDistance = Mathf.Infinity;

        foreach(Enemy e in enemies)
        {
            float distance = Vector3.Distance(e.transform.position, this.transform.position);
            if(distance < maxDistance)
            {
                closestTarget = e.transform;
                maxDistance = distance;
            }
        }
        target = closestTarget;
    }

    void AimWeapon()
    {
        float targetDistance = Vector3.Distance(target.position, this.transform.position);
        weapon.transform.LookAt(target);
        if (targetDistance <= range)
        {
            Attack(true);
        }
        else if (targetDistance > range)
        {
            Attack(false);
        }
    }

    void Attack(bool isActive)
    {
        var emissionModule = projectileParticles.emission;
        emissionModule.enabled = isActive;
    }
}
