using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class CorruptorFollow : MonoBehaviour
{
    public Transform player;
    public float speed = 5f;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public float flameCooldown = 10f;
    public float fireballCooldown = 10f;
    public float leftBoundaryX = -13f;
    public float rightBoundaryX = 9.5f;
    public GameObject fireballPrefab;
    public Transform fireballSpawnPoint;

    private float flameCooldownTimer = 0f;
    private float fireballCooldownTimer = 0f;
    private float initialScaleX;
    private bool isCastingFire = false;
    private NeuralNetwork aiDecisionMaker;

    // Training parameters
    private float previousBossHealth;
    private float previousPlayerHealth;
    private float episodeReward;
    private bool episodeEnded = false;

    // Constants for actions
    private enum Action
    {
        Follow = 0,
        CastFlame = 1,
        CastFireball = 2,
        Retreat = 3
    }

    void Start()
    {
        initialScaleX = Mathf.Abs(transform.localScale.x);
        aiDecisionMaker = new NeuralNetwork(inputSize: 6, hiddenSize: 12, outputSize: 4); // 4 actions
        LoadModel();

        previousBossHealth = BossHealth.health;
        previousPlayerHealth = PlayerHealth.health;
        episodeReward = 0f;
    }

    void Update()
    {
        if (episodeEnded)
            return;

        UpdateCooldowns();
        CheckFireCastingAnimation();

        if (!isCastingFire && !FinalBossVictory.victoryStarted)
        {
            MakeDecision();
        }

        // Check for end of episode
        if (BossHealth.health <= 0 || PlayerHealth.health <= 0 || Time.time >= 60f)
        {
            float finalReward = 0f;
            if (BossHealth.health > PlayerHealth.health && BossHealth.health > 0)
            {
                finalReward = 100f; // Victory
            }
            else
            {
                finalReward = -100f; // Defeat
                BossDeath();
            }

            episodeReward += finalReward;
            TrainAgent(finalReward);
            SaveModel();
        }
    }

    private void UpdateCooldowns()
    {
        if (flameCooldownTimer > 0)
        {
            flameCooldownTimer -= Time.deltaTime;
        }

        if (fireballCooldownTimer > 0)
        {
            fireballCooldownTimer -= Time.deltaTime;
        }
    }

    private void CheckFireCastingAnimation()
    {
        if (spriteRenderer.sprite.name.StartsWith("Attack_") || spriteRenderer.sprite.name.StartsWith("Move_") || spriteRenderer.sprite.name.StartsWith("Take Hit_"))
        {
            isCastingFire = true;
        }
        else
        {
            isCastingFire = false;
        }
    }

    private void MakeDecision()
    {
        if (player == null) return;

        float[] inputs = new float[]
        {
            transform.position.x,
            player.position.x,
            BossHealth.health / 100f, // Normalize health (assuming max health is 100)
            PlayerHealth.health / 100f,
            flameCooldownTimer / flameCooldown, // Normalize cooldowns
            fireballCooldownTimer / fireballCooldown
        };

        float[] qValues = aiDecisionMaker.Predict(inputs);
        Action selectedAction = SelectAction(qValues);

        ExecuteAction(selectedAction);
    }

    private Action SelectAction(float[] qValues)
    {
        // Epsilon-greedy strategy for exploration
        float epsilon = 0.1f;
        if (Random.value < epsilon)
        {
            int randomAction = Random.Range(0, qValues.Length);
            return (Action)randomAction;
        }
        else
        {
            float maxQ = float.MinValue;
            int actionIndex = 0;
            for (int i = 0; i < qValues.Length; i++)
            {
                if (qValues[i] > maxQ)
                {
                    maxQ = qValues[i];
                    actionIndex = i;
                }
            }
            return (Action)actionIndex;
        }
    }

    private void ExecuteAction(Action action)
    {
        switch (action)
        {
            case Action.Follow:
                MoveTowardsPlayer();
                break;
            case Action.CastFlame:
                if (flameCooldownTimer <= 0)
                {
                    CastFlameAttack();
                }
                else
                {
                    // If cooldown not ready, fallback to following
                    MoveTowardsPlayer();
                }
                break;
            case Action.CastFireball:
                if (fireballCooldownTimer <= 0)
                {
                    StartFireballLoadUp();
                }
                else
                {
                    // If cooldown not ready, fallback to following
                    MoveTowardsPlayer();
                }
                break;
            case Action.Retreat:
                RetreatFromPlayer();
                break;
            default:
                MoveTowardsPlayer();
                break;
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector3 moveHorizontal = new Vector3(player.position.x, transform.position.y, 0);
        transform.position = Vector3.MoveTowards(transform.position, moveHorizontal, speed * Time.deltaTime);

        FacePlayer(); // Always face the player when following
    }

    private void CastFlameAttack()
    {
        // Ensure boss faces the player before casting
        FacePlayer();

        animator.SetTrigger("CastFlame");
        flameCooldownTimer = flameCooldown;

        // Provide immediate reward for using flame attack appropriately
        float distance = Mathf.Abs(transform.position.x - player.position.x);
        float reward = (distance < 5f) ? 10f : -5f; // Reward if close, penalty if too far
        episodeReward += reward;
        TrainAgent(reward);
    }

    private void StartFireballLoadUp()
    {
        // Ensure boss faces the player before casting
        FacePlayer();

        animator.SetTrigger("CastFireball");
    }

    public void CastFireball()
    {
        fireballCooldownTimer = fireballCooldown;

        // Spawn the fireball from the fireballSpawnPoint's position
        GameObject fireballInstance = Instantiate(fireballPrefab, fireballSpawnPoint.position, Quaternion.identity);

        BossFireball bossFireball = fireballInstance.GetComponent<BossFireball>();

        if (bossFireball != null)
        {
            bossFireball.player = player;
            bossFireball.SetInitialDirection();
        }

        // Provide immediate reward for using fireball
        float distance = Mathf.Abs(transform.position.x - player.position.x);
        float reward = (distance >= 5f) ? 10f : -5f; // Reward if far, penalty if too close
        episodeReward += reward;
        TrainAgent(reward);
    }

    private void RetreatFromPlayer()
    {
        if (transform.position.x <= leftBoundaryX || transform.position.x >= rightBoundaryX)
        {
            FacePlayer();
            return;
        }

        Vector3 retreatHorizontal = new Vector3(transform.position.x, transform.position.y, 0);

        // Determine which direction to retreat
        if (player.position.x > transform.position.x)
        {
            // Retreat to the left
            retreatHorizontal.x = transform.position.x - 1f;
        }
        else
        {
            // Retreat to the right
            retreatHorizontal.x = transform.position.x + 1f;
        }

        transform.position = Vector3.MoveTowards(transform.position, retreatHorizontal, speed * Time.deltaTime);

        // Provide immediate reward for retreating when appropriate
        float reward = (BossHealth.health < PlayerHealth.health) ? 10f : -5f;
        episodeReward += reward;
        TrainAgent(reward);
    }

    private void BossDeath()
    {
        animator.SetTrigger("Death");
    }

    private void FacePlayer()
    {
        if (player.position.x > transform.position.x && !FinalBossVictory.victoryStarted)
        {
            transform.localScale = new Vector3(initialScaleX, transform.localScale.y, transform.localScale.z);
        }
        else if (player.position.x < transform.position.x && !FinalBossVictory.victoryStarted)
        {
            transform.localScale = new Vector3(-initialScaleX, transform.localScale.y, transform.localScale.z);
        }
    }

    private void TrainAgent(float reward)
    {
        // Define the next state
        float[] nextState = new float[]
        {
            transform.position.x,
            player.position.x,
            BossHealth.health / 100f,
            PlayerHealth.health / 100f,
            flameCooldownTimer / flameCooldown,
            fireballCooldownTimer / fireballCooldown
        };

        // Update the neural network with the reward
        aiDecisionMaker.UpdateWeights(reward, nextState);
    }

    private void SaveModel()
    {
        string path = Application.persistentDataPath + "/boss_model.txt";
        aiDecisionMaker.SaveWeights(path);
        Debug.Log("Model saved to " + path);
    }

    private void LoadModel()
    {
        string path = Application.persistentDataPath + "/boss_model.txt";
        if (File.Exists(path))
        {
            aiDecisionMaker.LoadWeights(path);
            Debug.Log("Model loaded from " + path);
        }
        else
        {
            Debug.Log("No saved model found. Starting fresh.");
        }
    }
}
