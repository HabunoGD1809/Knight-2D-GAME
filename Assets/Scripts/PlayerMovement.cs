using UnityEngine;

/// <summary>
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    [Tooltip("Velocidad de desplazamiento horizontal")]
    public float velocidadMovimiento = 5f;

    [Tooltip("Fuerza aplicada al saltar")]
    public float fuerzaSalto = 7f;

    [Header("Detección de suelo")]
    [Tooltip("Transform del objeto hijo colocado en los pies del personaje")]
    public Transform puntoSuelo;

    [Tooltip("Radio del círculo usado para detectar el suelo")]
    public float radioSuelo = 0.2f;

    [Tooltip("Capa (Layer) asignada a las plataformas/suelo")]
    public LayerMask capaSuelo;

    [Header("Sprites (opcional)")]
    [Tooltip("Sprite cuando está parado/caminando en el suelo")]
    public Sprite spriteIdle;

    [Tooltip("Sprite cuando está en el aire (saltando)")]
    public Sprite spriteJump;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool enSuelo;
    private float entradaHorizontal;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        entradaHorizontal = Input.GetAxisRaw("Horizontal");

        // Verifica si el personaje está tocando el suelo
        enSuelo = Physics2D.OverlapCircle(puntoSuelo.position, radioSuelo, capaSuelo);

        // Salto: solo si está en el suelo
        if (Input.GetButtonDown("Jump") && enSuelo)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
        }

        // Voltea el sprite según la dirección del movimiento
        if (entradaHorizontal > 0f)
            spriteRenderer.flipX = false;
        else if (entradaHorizontal < 0f)
            spriteRenderer.flipX = true;

        // Cambia el sprite según si está en el suelo o en el aire
        if (spriteIdle != null && spriteJump != null)
        {
            spriteRenderer.sprite = enSuelo ? spriteIdle : spriteJump;
        }
    }

    void FixedUpdate()
    {
        // Aplica el movimiento horizontal manteniendo la velocidad vertical (gravedad/salto)
        rb.linearVelocity = new Vector2(entradaHorizontal * velocidadMovimiento, rb.linearVelocity.y);
    }

    void OnDrawGizmosSelected()
    {
        if (puntoSuelo == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(puntoSuelo.position, radioSuelo);
    }
}
