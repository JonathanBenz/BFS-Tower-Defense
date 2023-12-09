using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] int cost = 75;
    [SerializeField] float buildDelay = 1f;

    private void Start()
    {
        StartCoroutine(Build());
    }
    public bool CreateTower(Tower tower, Vector3 position)
    {
        Bank bank = FindObjectOfType<Bank>();
        if(bank == null)
        {
            return false;
        }

        if (bank.CurrentBalance >= cost)
        {
            Instantiate(tower.gameObject, position, Quaternion.identity);
            bank.Withdraw(cost);
            return true;
        }
        else return false;
    }

    IEnumerator Build()
    {
        // Disables all components of our ballista. 
        foreach(Transform child in this.transform)
        {
            child.gameObject.SetActive(false);
            foreach(Transform grandchild in child.transform)
            {
                grandchild.gameObject.SetActive(false);
            }
        }

        // Periodically builds up our ballista when we place it in the world. 
        foreach (Transform child in this.transform)
        {
            child.gameObject.SetActive(true);
            yield return new WaitForSeconds(buildDelay);
            // The grandchild in this case would be the projectile particle system. 
            foreach (Transform grandchild in child.transform)
            {
                grandchild.gameObject.SetActive(true);
            }
        }
    }
}
