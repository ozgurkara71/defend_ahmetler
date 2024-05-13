using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] Transform weapon;
    [SerializeField] Transform target;

    // Max shooting range.
    [SerializeField] float towerFireRange = 15f;
    [SerializeField] ParticleSystem projectileParticles;

    void Update()
    {
        FindClosestTarget();
        AimWeapon();
    }

    private void FindClosestTarget()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        Transform closestTarget = null;
        float maxDistance = Mathf.Infinity;

        foreach (Enemy enemy in enemies)
        {
            float targetDistance = Vector3.Distance(gameObject.transform.position, 
                enemy.transform.position);
            if (targetDistance < maxDistance)
            {
                closestTarget = enemy.transform;
                maxDistance = targetDistance;
            }
        }
        target = closestTarget;
        Debug.Log(target);
    }

    void AimWeapon()
    {
        if (target)
        {
            float targetDistance = Vector3.Distance(gameObject.transform.position,
            target.position);

            weapon.LookAt(target);
            if (targetDistance < towerFireRange)
                Attack(true);
            else
                Attack(false);
        }
    }

    void Attack(bool isInRange)
    {
        var emissionModule = projectileParticles.emission;

        emissionModule.enabled = isInRange;
    }
}
