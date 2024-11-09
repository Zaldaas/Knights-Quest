using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float speed;
    public int startingPoint;
    public Transform[] points;
    public bool onPlatform;
    public bool reachedDestination = false;
    private int i;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = points[startingPoint].position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, points[i].position) < .02f && onPlatform & !reachedDestination)
        {
            if (i < points.Length - 1)
            {
                i++;
            }
            else
            {
                reachedDestination = true;
            }
        }

        if (!reachedDestination)
        {
            transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(transform.position.y < collision.transform.position.y - .8f)
        {
            collision.transform.SetParent(transform);
            onPlatform = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.SetParent(null);
        onPlatform = false;
    }
}
