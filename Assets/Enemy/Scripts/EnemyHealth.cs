using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Enemy))]

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHitPoints = 5;
    int currentHitPoints = 0, hitPointsOfGun = 1;

    // Bank
    Enemy increaseGold;

    // Difficulty
    [Tooltip("Adds amount to maxHitPoints when enemy dies.")]
    [SerializeField] int difficultyRamp = 1;

    void Start()
    {
        increaseGold = GetComponent<Enemy>();
    }

    void OnEnable()
    {
        currentHitPoints = maxHitPoints;
    }

    void OnParticleCollision(GameObject other)
    {
        ProcessHit();
    }

    void ProcessHit()
    {
        currentHitPoints -= hitPointsOfGun;
        if (currentHitPoints < 1)
        {
            gameObject.SetActive(false);
            maxHitPoints += difficultyRamp;
            increaseGold.RewardGold();
        }
    }
}
