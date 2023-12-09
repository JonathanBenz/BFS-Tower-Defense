using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour
{

    [SerializeField] float maxHP = 5f;

    [Tooltip("Adds amount to maxHP when enemy dies")]
    [SerializeField] float difficultyRamp = 0.25f;
    float currentHP = 0;

    Enemy enemy;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
    }
    // Start is called before the first frame update
    void OnEnable()
    {
        currentHP = maxHP;
    }


    private void OnParticleCollision(GameObject collision)
    {
        currentHP--;
        if (currentHP <= 0)
        {
            gameObject.SetActive(false);
            maxHP += difficultyRamp;
            enemy.RewardGold();
        }
    }
}
