
using UnityEngine;

public class EnemyHPData : MonoBehaviour
{
    [SerializeField] private float _hpData;
    [SerializeField] private float enemyHitTimer;
    [SerializeField] private GameObject self;
    public float enemyDamage = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (enemyHitTimer < 0)
        {
            attackBasic attackedBy = other.GetComponent<attackBasic>();
            if (attackedBy != null)
            {
                float value = attackedBy.damage;

                _hpData -= value;
                enemyHitTimer = 0.25f;
            }
        }
    }
        // Update is called once per frame
        void Update()
    {
        enemyHitTimer -= Time.deltaTime;
        if (_hpData < 0)
        {
            //replace this with like, turning off a couple parts of the enemy
            self.SetActive(false);
        }
    }
}
