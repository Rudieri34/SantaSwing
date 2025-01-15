using UnityEngine;

public class GhostEffect : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    private Color initialColor;
    public float fadeSpeed = 2f; // Velocidade da transpar�ncia

    void Start()
    {
        initialColor = spriteRenderer.color;
    }

    void Update()
    {
        // Gradualmente reduz a opacidade
        initialColor.a -= fadeSpeed * Time.deltaTime;
        spriteRenderer.color = initialColor;

        // Destroi o ghost quando totalmente invis�vel
        if (initialColor.a <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetSprite(Sprite sprite, Color color, bool flipX)
    {
        spriteRenderer.sprite = sprite;
        spriteRenderer.color = color;

        spriteRenderer.flipX = flipX;
    }
}