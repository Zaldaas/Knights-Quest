using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformSync : MonoBehaviour
{
    public float baseSpeed; // The base speed of the platform.
    public float moveDuration = 2.0f; // The time it takes to move between points.
    public int startingPoint;
    public Transform[] points;
    private int i;
    private float speed; // Adjusted speed based on distance.

    // Start is called before the first frame update
    void Start()
    {
        transform.position = points[startingPoint].position;
        UpdateSpeed(); // Initial speed calculation.
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, points[i].position) < .02f)
        {
            i++;
            if (i == points.Length)
            {
                i = 0;
            }
            UpdateSpeed(); // Recalculate speed each time a new destination is set.
        }

        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
    }

    private void UpdateSpeed()
    {
        // Calculate the distance to the next point.
        float distance = Vector2.Distance(transform.position, points[i].position);
        // Adjust speed so the platform moves this distance over moveDuration.
        speed = distance / moveDuration;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(transform.position.y < collision.transform.position.y - .8f)
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.SetParent(null);
    }
}
