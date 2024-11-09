using UnityEngine;

public class CorruptorFollow : MonoBehaviour
{
    public Transform player;
    public float speed = 5f;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    private float initialScaleX;
    private bool isCastingFire = false;

    private void Start()
    {
        initialScaleX = Mathf.Abs(transform.localScale.x);
    }

    private void Update()
    {
        CheckFireCastingAnimation();
        if (!isCastingFire)
        {
            MoveTowardsPlayer();
        }
    }

    private void CheckFireCastingAnimation()
    {
        if (spriteRenderer.sprite.name.StartsWith("Attack_") || spriteRenderer.sprite.name.StartsWith("Move_"))
        {
            isCastingFire = true;
        }
        else
        {
            isCastingFire = false;
        }
    }

    private void MoveTowardsPlayer()
    {
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            Vector3 moveHorizontal = new Vector3(player.position.x, transform.position.y, 0);
            transform.position = Vector3.MoveTowards(transform.position, moveHorizontal, speed * Time.deltaTime);

            if (!isCastingFire)
            {
                if (player.position.x > transform.position.x)
                {
                    transform.localScale = new Vector3(initialScaleX, transform.localScale.y, transform.localScale.z);
                }
                else if (player.position.x < transform.position.x)
                {
                    transform.localScale = new Vector3(-initialScaleX, transform.localScale.y, transform.localScale.z);
                }
            }
        }
    }
}
