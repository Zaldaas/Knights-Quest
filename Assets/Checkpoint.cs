using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameObject checkpointText;
    private BoxCollider2D platformCollider;
    private bool isActivated = false;

    void Start()
    {
        platformCollider = GetComponent<BoxCollider2D>();
        checkpointText.SetActive(false);
        InitializeCheckpoint();
    }

    void InitializeCheckpoint()
    {
        if (GameSession.Instance.IsCheckpointActivated(transform.position))
        {
            isActivated = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isActivated)
        {
            Bounds bounds = platformCollider.bounds;
            float platformTop = bounds.max.y;
            float playerBottom = other.bounds.min.y;

            if (playerBottom > platformTop)
            {
                GameSession.Instance.checkpointPosition = transform.position;
                GameSession.Instance.ActivateCheckpoint(transform.position);
                checkpointText.SetActive(true);
                Invoke("HideText", 2.5f);
                isActivated = true;
            }
        }
    }

    void HideText()
    {
        checkpointText.SetActive(false);
    }
}
