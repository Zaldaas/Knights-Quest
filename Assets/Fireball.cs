using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed;
    public int startingPoint;
    public Transform[] points;
    private int i;
    [SerializeField] private GameObject fireball;
    
    // Start is called before the first frame update
    void Start()
    {
        i = startingPoint;
        transform.position = points[startingPoint].position;
        fireball.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, points[i].position) < .02f)
        {
            i++;
            if (i >= points.Length)
            {
                i = startingPoint;
                transform.position = points[startingPoint].position;
            }
        }

    }
}
