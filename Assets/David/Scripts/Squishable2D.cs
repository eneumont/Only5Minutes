using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Squishable2D : MonoBehaviour
{
    [SerializeField] float damage = 100;
    //[SerializeField] Character2D subject;
    [SerializeField] bool damageOverTime = true;
    [SerializeField] float maximumSquishAngle = 45;
    private List<Vector2> collisionPoints = new List<Vector2>();
    private List<Collider2D> colliders = new List<Collider2D>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < collisionPoints.Count - 1; i++)
        {
            for(int j = i + 1; j < collisionPoints.Count; j++)
            {
                bool a = collisionPoints[i] != collisionPoints[j];
                float a1 = Vector2.SignedAngle(Vector2.up, collisionPoints[i] - new Vector2(transform.position.x, transform.position.y));
                float a2 = Vector2.SignedAngle(Vector2.up, collisionPoints[j] - new Vector2(transform.position.x, transform.position.y));
                bool b = gameObject.TryGetComponent(out IDamagable subject);
				if ( a && Mathf.Abs(a1 + a2) < maximumSquishAngle && b)
                {
                    subject.ApplyDamage(damage * (damageOverTime ? Time.deltaTime : 1));
                }
            }
        }
    }
	private void OnCollisionEnter2D(Collision2D collision)
	{

		if(!colliders.Contains(collision.collider)) 
        {
			int index = 0;
			while (collision.GetContact(index).otherCollider.gameObject != gameObject && index < collision.contactCount - 1)
			{
				index++;
			}
			collisionPoints.Add(collision.GetContact(index).point);
            colliders.Add(collision.collider);
        }
	}
	private void OnCollisionStay2D(Collision2D collision)
	{
        int index = 0;
        while(collision.GetContact(index).otherCollider.gameObject != gameObject && index < collision.contactCount - 1)
        {
            index++;
        }
        collisionPoints[colliders.IndexOf(collision.collider)] = collision.GetContact(index).point;
	}
	private void OnCollisionExit2D(Collision2D collision)
	{
        collisionPoints.RemoveAt(colliders.IndexOf(collision.collider));
        colliders.Remove(collision.collider);
	}
}
