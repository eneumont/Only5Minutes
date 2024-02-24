using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Vector3 endPosition;
    [SerializeField] float rate = 1f;
    [SerializeField] VoidEvent resetEvent;

    // Start is called before the first frame update
    void Start()
    {
        resetEvent?.Subscribe(ResetMover);
        if(startPosition == null)
        {
            startPosition = transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (endPosition - transform.position).normalized * rate * Time.deltaTime;
    }

	private void ResetMover()
	{
        transform.position = startPosition;
	}
}
