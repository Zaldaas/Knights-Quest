using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    public static GameSession Instance;
    public Vector3 checkpointPosition = Vector3.zero;
    public List<Vector3> activatedCheckpoints = new List<Vector3>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool IsCheckpointActivated(Vector3 position)
    {
        return activatedCheckpoints.Contains(position);
    }

    public void ActivateCheckpoint(Vector3 position)
    {
        if (!activatedCheckpoints.Contains(position))
        {
            activatedCheckpoints.Add(position);
        }
    }

    public void ResetCheckpoints()
    {
        checkpointPosition = Vector3.zero;
        activatedCheckpoints.Clear();
    }
}
