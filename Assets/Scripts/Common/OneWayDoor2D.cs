using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class OneWayDoor2D : MonoBehaviour
{
	//simple door script. opens door if you approach from side a, blocks if you do not. Minor issue of any approach from a will open even from elsewhere
	[SerializeField] Collider2D doorCollider;
	private void OnTriggerEnter2D(Collider2D collision)
	{
		doorCollider.enabled = false;
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		doorCollider.enabled = true;
	}
}
