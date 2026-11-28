using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [SerializeField] private float _hpData;
    [SerializeField] private float playerHitTimer;
    [SerializeField] private GameObject self;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.TryGetComponent(out EnemyHPData attackedBy))
        {
            Debug.Log("collided with something");
            if (playerHitTimer < 0)
            {
                
                float value = attackedBy.enemyDamage;

                _hpData -= value;
                playerHitTimer = 0.5f;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        playerHitTimer -= Time.deltaTime;
        if (_hpData < 0)
        {
            //replace this with like, turning off a couple parts of the enemy
            self.SetActive(false);
        }
    }
}
