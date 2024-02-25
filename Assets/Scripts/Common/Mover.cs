using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private Transform startPosition;
    [SerializeField] private Transform endPosition;
    [SerializeField] float rate = 1f;
    [SerializeField] private bool resetUponArrival = false;
    [SerializeField] VoidEvent[] resetEvents;
    [SerializeField] VoidEvent arrivedEvent;
    [SerializeField] VoidEvent[] startEvents;
    [SerializeField] bool startAutomatically = true;
    [NonSerialized] public bool going;
    [SerializeField] bool startOnContact = true;
    [SerializeField] bool reverseUponArrival = false;

    // Start is called before the first frame update
    void Start()
    {
        going = startAutomatically;
        if(resetUponArrival)
        {
            if(resetEvents.Length == 0) 
            {
                resetEvents = new VoidEvent[1];
                resetEvents[0] = new VoidEvent();
            }
            arrivedEvent = resetEvents[0];
		}
		foreach (VoidEvent evnt in resetEvents)
		{
			evnt?.Subscribe(ResetMover);
		}
        if (!startAutomatically)
        {
            foreach (VoidEvent evnt in startEvents)
            {
                evnt?.Subscribe(StartMover);
            }
        }
		if (startPosition == null)
        {
            startPosition = transform;
        }
	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (startOnContact)
		{
			StartMover();
		}
	}
	private void OnCollisionEnter(Collision collision)
	{
		if (startOnContact)
		{
			StartMover();
		}
	}
	// Update is called once per frame
	void Update()
    {
        if (going)
        {
            if (gameObject.TryGetComponent(out Rigidbody2D body2D))
            {
                body2D.velocity = (endPosition.position - transform.position).normalized * rate;
            }
            else if (gameObject.TryGetComponent(out Rigidbody body))
            {
                body.velocity = (endPosition.position - transform.position).normalized * rate;
            }
            else
            {
                transform.position += (endPosition.position - transform.position).normalized * rate * Time.deltaTime;
            }
            if (Vector3.Distance(transform.position, endPosition.position) < rate * 0.1)
			{
				if (reverseUponArrival)
				{
                    Vector3 destination = transform.position;
                    transform.position = endPosition.position;
                    endPosition.position = destination;
				}
				arrivedEvent?.RaiseEvent();
                going = startAutomatically && resetUponArrival;
            }
        }
    }

	private void ResetMover()
	{
        transform.position = startPosition.position;
        going = startAutomatically;
	}
    private void StartMover()
    {
        going = true;
    }
}
