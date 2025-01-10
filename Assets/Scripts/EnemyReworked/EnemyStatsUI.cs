using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatsUI : MonoBehaviour
{
    [SerializeField] Image healthBar;

    [SerializeField] float maxHealth = 100;
    float health;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (health < 0) health = 0;
        BarLength();
    }

    void BarLength() {
        healthBar.transform.localScale = new Vector3(health / maxHealth, 0.8f, 1);
    }

    public float GetHealth() {
        return health;
    }

    public void SetHealth(float health) {
        this.health = health;
    }
}
