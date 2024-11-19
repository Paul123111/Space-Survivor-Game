using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{

    [SerializeField] int damage = 20;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public int GetDamage() {
        return damage;
    }

    public void SetDamage(int damage) {
        this.damage = damage;
    }

}
