using UnityEngine;

public class MysteryBlock : MonoBehaviour
{
    public GameObject itemToSpawn; 
    public Sprite emptyBlockSprite; 
    public float bounceHeight = 0.2f; 
    public float bounceSpeed = 4f;
    public Color usedBlockColor = Color.gray;

    private bool _isUsed = false;
    private Vector3 _originalPosition;
    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        _originalPosition = transform.position;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (_isUsed || !collision.gameObject.CompareTag("Player")) return;

        ContactPoint2D contact = collision.GetContact(0);
        if (contact.normal.y > 0.5f) 
        {
            TriggerBlock();
        }
    }

    private void TriggerBlock()
    {
        _isUsed = true;
        _spriteRenderer.color = Color.gray; 

        GameObject spawnedItem = Instantiate(itemToSpawn, transform.position + Vector3.up, Quaternion.identity);

        Rigidbody2D itemRb = spawnedItem.GetComponent<Rigidbody2D>();
        if (itemRb != null)
        {
            itemRb.AddForce(new Vector2(2f, 4f), ForceMode2D.Impulse);
        }

        StartCoroutine(BounceEffect());
    }

    private System.Collections.IEnumerator BounceEffect()
    {
        Vector3 targetPos = _originalPosition + Vector3.up * bounceHeight;
        while (transform.position.y < targetPos.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, bounceSpeed * Time.deltaTime);
            yield return null;
        }

        while (transform.position.y > _originalPosition.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, _originalPosition, bounceSpeed * Time.deltaTime);
            yield return null;
        }
    }
}