using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Vector3 endPosition;
    [SerializeField] float rate = 1f;
    [SerializeField] private bool resetUponArrival = false;
    [SerializeField] VoidEvent[] resetEvents;
    [SerializeField] VoidEvent arrivedEvent;
    [SerializeField] VoidEvent[] startEvents;
    [SerializeField] bool startAutomatically = true;
    [NonSerialized] public bool going;

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
            startPosition = transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (going)
        {
            transform.position += (endPosition - transform.position).normalized * rate * Time.deltaTime;
            if (Vector3.Distance(transform.position, endPosition) < rate * 0.1)
            {
                arrivedEvent?.RaiseEvent();
                going = false;
            }
        }
    }

	private void ResetMover()
	{
        transform.position = startPosition;
        going = startAutomatically;
	}
    private void StartMover()
    {
        going = true;
    }
}
