using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public GameObject healText;
    public GameObject healAvailableText;
    public int health = 100;
    public int maxHealth = 100;
    private float damageCooldown = 1f;
    private float healCooldown = 10f;
    private float lastDamageTime;
    private float lastHealTime;
    public static bool playerDefeated = false;
    public PlayerHealthBar healthBar;

    void Start()
    {
        playerDefeated = false;
        healText.SetActive(false);
        healAvailableText.SetActive(false);
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        if ((Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Alpha2)) && CanHeal())
        {
            Heal(15);
            healText.SetActive(true);
            Invoke("HideUsedText", 2.5f);
        }

        if (CanHeal())
        {
            healAvailableText.SetActive(true);
        }
        else
        {
            HideAvailableText();
        }
    }

    void HideUsedText()
    {
        healText.SetActive(false);
    }

    void HideAvailableText()
    {
        healAvailableText.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("PlayerSword") && Time.time > lastDamageTime + damageCooldown)
        {
            if (other.CompareTag("BossFlame"))
            {
                TakeDamage(20);
                lastDamageTime = Time.time;
            }
            else if(other.CompareTag("BossBody"))
            {
                TakeDamage(10);
                lastDamageTime = Time.time;
            }
        }
    }

    void TakeDamage(int damage)
    {
        health -= damage;
        health = Mathf.Max(health, 0);
        healthBar.SetHealth(health);
        Debug.Log($"Player Health: {health}");
        if (health <= 0 && !FinalBossVictory.isImmune)
        {
            Debug.Log("Player is dead!");
            playerDefeated = true;
        }
    }

    public void Heal(int amount)
    {
        if (CanHeal())
        {
            health += amount;
            health = Mathf.Min(health, maxHealth);
            healthBar.SetHealth(health);
            Debug.Log($"Player Health after healing: {health}");
            lastHealTime = Time.time;
        }
    }

    bool CanHeal()
    {
        return Time.time - lastHealTime >= healCooldown;
    }
}
