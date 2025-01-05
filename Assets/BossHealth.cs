using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public static int health = 750;
    public static bool bossDefeated = false;
    public BossHealthBar healthBar;
    
    void Start()
    {
        health = 750;
        bossDefeated = false;
        healthBar.SetMaxHealth(750);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerSword") && transform.CompareTag("BossBody"))
        {
            TakeDamage(20);
        }
    }

    private void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.SetHealth(health);
        if (health <= 0 && !FinalBossDefeat.IsGameOver)
        {
            bossDefeated = true;
        }
    }
}
