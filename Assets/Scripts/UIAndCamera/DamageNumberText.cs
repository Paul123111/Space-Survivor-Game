using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageNumberText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI damageNumber;
    [SerializeField] float lifeTime;
    float alpha = 1f;

    private void Start() {
        transform.position = new Vector3(transform.position.x + (1 * Random.Range(-1f, 1f)), transform.position.y + (1 * Random.Range(-1f, 1f)), transform.position.z + (1 * Random.Range(-1f, 1f)));
        Destroy(gameObject, lifeTime);
    }

    private void Update() {
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f * Time.deltaTime, transform.position.z);
        alpha -= (1.0f/lifeTime) * Time.deltaTime;
        damageNumber.alpha = alpha;
    }

    public void SetDamageText(float damage) {
        damageNumber.text = "-" + Mathf.Floor(damage);
    }
}
