using UnityEngine;
using UnityEngine.UI;

public class player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    [SerializeField] private float maxHp = 1000f;
    private float currentHp;
    [SerializeField] private Image hpBar;
    [SerializeField] GameManager gameManager;
    public Joystick joystick; // Tham chiếu tới Joystick UI

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        currentHp = maxHp;
        UpdateHpBar();
    }

    void Update()
    {
        MovePlayer();
        UpdateHpBar();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameManager.PauseGameMenu();
        }
    }
   

 void MovePlayer()
{
    Vector2 moveInput = new Vector2(joystick.Horizontal, joystick.Vertical);
    rb.linearVelocity = moveInput.normalized * moveSpeed;

    if (moveInput.x < 0) spriteRenderer.flipX = true;
    else if (moveInput.x > 0) spriteRenderer.flipX = false;

    animator.SetBool("isRun", moveInput != Vector2.zero);
}


    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);
        UpdateHpBar();

        if (currentHp <= 0)
        {
            Die();
        }
    }

    public void Heal(float healValue)
    {
        if (currentHp < maxHp)
        {
            currentHp += healValue;
            currentHp = Mathf.Min(currentHp, maxHp);
            UpdateHpBar();
        }
    }

    private void Die()
    {
        gameManager.GameOverMenu();
    }

    private void UpdateHpBar()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = currentHp / maxHp;
        }
    }
}
