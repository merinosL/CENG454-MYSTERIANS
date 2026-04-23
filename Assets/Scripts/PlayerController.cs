using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 8f;

    [Header("Jump")]
    public float jumpForce = 14f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.15f;
    public LayerMask groundLayer;

    private Rigidbody2D _rb;
    private bool _isGrounded;
    private bool _jumpRequested;
    
    public enum PlayerPowerState { Normal, Super, Fire, Invisible }
    
    [Header("Power-Ups")]
    public bool isSuper = false;
    public bool isFire = false;
    public bool isInvisible = false;

    public GameObject projectilePrefab;
    private Color _originalColor;

    private Coroutine _superCoroutine;
    private Coroutine _fireCoroutine;
    private Coroutine _invisibleCoroutine;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _originalColor = GetComponent<SpriteRenderer>().color;
    }

    void Update()
    {
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
            _jumpRequested = true;
        
        if (Input.GetKeyDown(KeyCode.F) && isFire)
        {
            Shoot();
        }
    }

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        _rb.linearVelocity = new Vector2(horizontalInput * speed, _rb.linearVelocity.y);

        if (_jumpRequested)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
            _jumpRequested = false;
        }

        float currentX = Mathf.Abs(transform.localScale.x); 
        float currentY = transform.localScale.y;

        if (horizontalInput > 0.01f)       
            transform.localScale = new Vector3(currentX, currentY, 1);
        else if (horizontalInput < -0.01f) 
            transform.localScale = new Vector3(-currentX, currentY, 1);
    }

    public void ApplyPowerUp(PlayerPowerState powerType)
    {
        switch (powerType)
        {
            case PlayerPowerState.Super:
                if (_superCoroutine != null) StopCoroutine(_superCoroutine);
                _superCoroutine = StartCoroutine(SuperRoutine()); 
                break;
                
            case PlayerPowerState.Fire:
                if (_fireCoroutine != null) StopCoroutine(_fireCoroutine);
                _fireCoroutine = StartCoroutine(FireRoutine());
                break;
                
            case PlayerPowerState.Invisible:
                if (_invisibleCoroutine != null) StopCoroutine(_invisibleCoroutine);
                _invisibleCoroutine = StartCoroutine(InvisibilityRoutine());
                break;
                
            case PlayerPowerState.Normal:
                isSuper = false;
                isFire = false;
                isInvisible = false;
                UpdateAppearance();
                break;
        }
    }

    private void UpdateAppearance()
    {
        float sign = Mathf.Sign(transform.localScale.x);
        if (isSuper) transform.localScale = new Vector3(2f * sign, 3f, 1f);
        else transform.localScale = new Vector3(1f * sign, 1f, 1f);

        Color targetColor = isFire ? Color.red : _originalColor;
        
        targetColor.a = isInvisible ? 0.5f : 1f;
        
        GetComponent<SpriteRenderer>().color = targetColor;
    }


    private System.Collections.IEnumerator SuperRoutine()
    {
        isSuper = true;
        UpdateAppearance(); 

        yield return new WaitForSeconds(15f); 

        isSuper = false;
        UpdateAppearance(); 
    }

    private System.Collections.IEnumerator FireRoutine()
    {
        isFire = true;
        UpdateAppearance();

        yield return new WaitForSeconds(20f); 

        isFire = false;
        UpdateAppearance(); 
    }

    private System.Collections.IEnumerator InvisibilityRoutine()
    {
        isInvisible = true;
        UpdateAppearance();

        yield return new WaitForSeconds(10f); 

        isInvisible = false;
        UpdateAppearance(); 
    }

    void Shoot()
    {
        GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        float direction = Mathf.Sign(transform.localScale.x);
        proj.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(direction * 12f, 0);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = _isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}