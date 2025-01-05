using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFireball : MonoBehaviour
{
    public Transform player;
    public float speed = 5f;
    public float rotationOffset = 0f;

    private Vector3 spawnPosition;
    private Vector3 moveDirection;

    private void Start()
    {
        spawnPosition = transform.position;
        SetInitialDirection();
    }

    private void Update()
    {
        MoveInDirection();
    }

    public void SetInitialDirection()
    {
        if (player != null)
        {
            moveDirection = (player.position - transform.position).normalized;
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            angle += rotationOffset;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void MoveInDirection()
    {
        transform.position += moveDirection * speed * Time.deltaTime;
    }
}
