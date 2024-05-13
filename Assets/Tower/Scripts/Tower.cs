using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] int cost = 75;
    float buildDelay = 1f;

    void Start()
    {
        StartCoroutine(Build());   
    }

    public bool CreateTower(Tower tower, Vector3 position)
    {
        Bank bank = FindObjectOfType<Bank>();

        if (!bank)
            return false;
        if(bank.CurrentBalance >= cost)
        {
            Instantiate(tower, position, Quaternion.identity);
            bank.Withdraw(cost);
            return true;
        }
        return false;
    }

    IEnumerator Build()
    {
        // Set disable tower
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
            foreach (Transform grandChild in child)
            {
                grandChild.gameObject.SetActive(false);
            }
        }

        // Set enable tower back
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
            yield return new WaitForSeconds(buildDelay);
            foreach (Transform grandChild in child)
            {
                grandChild.gameObject.SetActive(true);
            }
        }
    }
}