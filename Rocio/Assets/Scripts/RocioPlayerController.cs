using UnityEngine;

public class RocioPlayerController : MonoBehaviour
{
    public float velocidad;
    public float fuerzaSalto; // Este ya no se usa directamente, pero lo puedes dejar como referencia
    public LayerMask suelo;

    private Rigidbody2D rigidbody;
    private PolygonCollider2D boxCollider;
    private bool mirandoDerecha = true;
    private Animator animator;

    // Variables para la mecánica de salto tipo Jump King
    private bool estaCargandoSalto = false;
    private float tiempoCarga = 0f;
    private float cargaMaxima = 2f;
    private float fuerzaSaltoMin = 5f;
    private float fuerzaSaltoMax = 18f;
    private float direccionSalto = 0f;

    public PhysicsMaterial2D bounceMat;
    public PhysicsMaterial2D normalMat;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<PolygonCollider2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
{
    float inputMov = Input.GetAxisRaw("Horizontal");


    if (estaCargandoSalto)
    {
        tiempoCarga += Time.deltaTime;

        if (Input.GetKeyUp(KeyCode.Space) || tiempoCarga >= cargaMaxima)
        {
            EjecutarSalto();
        }
        else if (inputMov != 0f)
        {
            direccionSalto = inputMov < 0 ? -1f : 1f; // solo -1 o 1
        }
    }
    else
    {
        if (Input.GetKeyDown(KeyCode.Space) && estaEnSuelo())
        {
            estaCargandoSalto = true;
            tiempoCarga = 0f;
            direccionSalto = inputMov < 0 ? -1f : (inputMov > 0 ? 1f : 0f); // guarda dirección
        }
        else if (estaEnSuelo())
        {
            mover(inputMov); // solo si está en el suelo
        }
        else
        {
            animator.SetBool("Moviendose", false);
        }
    }

    if (rigidbody.linearVelocity.y <= -1)
    {
    rigidbody.sharedMaterial = normalMat;
    }
}


    bool estaEnSuelo()
    {
        RaycastHit2D rayCastHit2D = Physics2D.BoxCast(
            boxCollider.bounds.center,
            new Vector2(boxCollider.bounds.size.x, boxCollider.bounds.size.y),
            0f,
            Vector2.down,
            0.2f,
            suelo
        );
        return rayCastHit2D;
    }

void mover(float inputMov)
{
    if (inputMov != 0f)
    {
        animator.SetBool("Moviendose", true);
    }
    else
    {
        animator.SetBool("Moviendose", false);
    }

    rigidbody.linearVelocity = new Vector2(inputMov * velocidad, rigidbody.linearVelocity.y);
    cambiarDireccion(inputMov);
}


void EjecutarSalto()
{
    estaCargandoSalto = false;
    animator.SetBool("Saltando", true);

    float porcentajeCarga = Mathf.Clamp01(tiempoCarga / cargaMaxima);
    float fuerzaActual = Mathf.Lerp(fuerzaSaltoMin, fuerzaSaltoMax, porcentajeCarga);

    Vector2 direccion;

    // Si hay dirección horizontal, hacer el salto más largo horizontalmente
    if (direccionSalto != 0f)
    {
        direccion = new Vector2(direccionSalto * 0.75f, 1f);
    }
    else
    {
        direccion = new Vector2(0f, 1f);
    }

    rigidbody.sharedMaterial = bounceMat; // ← AÑADIDO
    rigidbody.linearVelocity = Vector2.zero;
    rigidbody.AddForce(direccion * fuerzaActual, ForceMode2D.Impulse);
}

  void cambiarDireccion(float movimiento)
    {
        if ((mirandoDerecha && movimiento < 0) || (!mirandoDerecha && movimiento > 0))
        {
            mirandoDerecha = !mirandoDerecha;
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
    }
}

