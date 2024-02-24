using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon2D : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] float damage = 10f;
    [SerializeField] FloatVariable health;
    public GameObject Target;

    public void Update()
    {
        //Vector2.MoveTowards(this.transform.position, Target.transform.position, speed);
        //transform.position += transform.right * Time.deltaTime * speed; 
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            health.value -= damage;
        }
        Destroy(gameObject);
    }

}
