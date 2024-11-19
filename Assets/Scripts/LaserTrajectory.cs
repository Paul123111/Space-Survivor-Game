using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTrajectory : MonoBehaviour
{
    [SerializeField] float lifeTime;
    [SerializeField] int damage;

    float currentTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > lifeTime) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) {
        Destroy(gameObject, 0.01f);
    }

    //private void OnTriggerEnter(Collider other) {
    //    if (other.gameObject.tag == "Enemy") {
    //        print("hi");
    //        Destroy(other.gameObject);
    //    }
    //}

    public int GetDamage() {
        return damage;
    }

    public void SetDamage(int damage) {
        this.damage = damage;
    }
}
