using UnityEngine;
using System.Collections;


public class RocioPlayerController : MonoBehaviour
{
    public float velocidad = 5f;
    public LayerMask suelo;
    
    // Configuración de salto
    [Header("Configuración de Salto")]
    [Tooltip("Fuerza mínima de salto")]
    public float fuerzaSaltoMin = 5f;
    
    [Tooltip("Fuerza máxima de salto")]
    public float fuerzaSaltoMax = 20f;
    
    [Tooltip("Tiempo máximo de carga del salto (segundos)")]
    public float cargaMaxima = 1.0f; // Reducido de 1.5f a 1.0f para carga más rápida
    
    [Tooltip("Factor de fuerza para saltos diagonales")]
    public float factorDiagonalFuerza = 1.3f; // Multiplicador para saltos diagonales
    
    [Tooltip("Componente horizontal para saltos diagonales")]
    public float factorDiagonalX = 1.5f; // Componente X más fuerte
    
    [Tooltip("Componente vertical para saltos diagonales")]
    public float factorDiagonalY = 0.7f; // Componente Y más débil

    private Rigidbody2D rigidbody;
    private PolygonCollider2D boxCollider;
    private bool mirandoDerecha = true;
    private Animator animator;

    // Doble salto
    private bool dobleSaltoDisponible = false;

    //Dash
    [Header("Dash Config")]
    public float fuerzaDash = 12f;
    public float duracionDash = 0.2f;

    private bool estaDasheando = false;
    private float gravedadOriginal;

    private bool dashDisponible = false;



    // Variables para la mecánica de salto tipo Jump King
    private bool estaCargandoSalto = false;
    private float tiempoCarga = 0f;
    private float direccionSalto = 0f;
    private bool saltando = false;
    private bool direccionCambiada = false; // Para evitar cambios múltiples de dirección

    // Puntos de verificación de suelo
    [SerializeField] private Transform checkPointLeft;
    [SerializeField] private Transform checkPointRight;
    public float rayLength = 0.2f;

    public PhysicsMaterial2D bounceMat;
    public PhysicsMaterial2D normalMat;

    void Awake()
    {
        // Crear los puntos de verificación si no existen
        CrearPuntosVerificacion();
    }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<PolygonCollider2D>();
        animator = GetComponent<Animator>();

        gravedadOriginal = rigidbody.gravityScale;
    }

    // Método para crear los puntos de verificación
    private void CrearPuntosVerificacion()
    {
        // Si no existe el punto izquierdo, crearlo
        if (checkPointLeft == null)
        {
            GameObject leftCheck = new GameObject("LeftCheck");
            leftCheck.transform.SetParent(transform);
            leftCheck.transform.localPosition = new Vector3(-0.4f, -0.9f, 0f);
            checkPointLeft = leftCheck.transform;
        }

        // Si no existe el punto derecho, crearlo
        if (checkPointRight == null)
        {
            GameObject rightCheck = new GameObject("RightCheck");
            rightCheck.transform.SetParent(transform);
            rightCheck.transform.localPosition = new Vector3(0.4f, -0.9f, 0f);
            checkPointRight = rightCheck.transform;
        }
    }

    void Update()
    {
        // Verificar que los puntos de verificación existan
        if (checkPointLeft == null || checkPointRight == null)
        {
            CrearPuntosVerificacion();
            return; // Salir del update si acabamos de crear los puntos
        }

        float inputMov = Input.GetAxisRaw("Horizontal");
        bool enSuelo = estaEnSuelo();

        // Resetear material cuando está cayendo
        if (rigidbody.linearVelocity.y <= -1)
        {
            rigidbody.sharedMaterial = normalMat;
        }

        // Manejo de la carga del salto
        if (estaCargandoSalto)
        {
            // No permitir movimiento durante la carga
            rigidbody.linearVelocity = new Vector2(0f, rigidbody.linearVelocity.y);
            
            tiempoCarga += Time.deltaTime;

            // Capturar la dirección durante la carga (solo una vez)
            if (inputMov != 0f && !direccionCambiada)
            {
                direccionSalto = inputMov < 0 ? -1f : 1f;
                direccionCambiada = true;
                
                // Girar el personaje en la dirección del salto
                if ((mirandoDerecha && direccionSalto < 0) || (!mirandoDerecha && direccionSalto > 0))
                {
                    mirandoDerecha = !mirandoDerecha;
                    transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
                }
            }

            // Limitar el tiempo de carga o ejecutar al soltar
            if (Input.GetKeyUp(KeyCode.Space) || tiempoCarga >= cargaMaxima)
            {
                EjecutarSalto();
            }
        }
        else if (!saltando) // Solo permitir iniciar salto si no está en medio de uno
        {
            if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
            {
                IniciarCargaSalto(inputMov);

            }
            else if (enSuelo)
            {
                mover(inputMov);
                animator.SetBool("Saltando", false);

            }
            else
            {
                animator.SetBool("Moviendose", false);
            }
        }

        if (!enSuelo && dobleSaltoDisponible && Input.GetKeyDown(KeyCode.Space))
        {
        IniciarDobleSalto();
        }

        if (dashDisponible && Input.GetKeyDown(KeyCode.E))
        {
        UsarDash();
        }

    }

    void IniciarCargaSalto(float inputMov)
    {
        estaCargandoSalto = true;
        tiempoCarga = 0f;
        direccionCambiada = false;
        animator.SetBool("Cargando", true);
        animator.SetBool("Saltando", false);
        animator.SetBool("Moviendose", false);
        // Capturar dirección inicial si ya se está moviendo
        direccionSalto = inputMov < 0 ? -1f : (inputMov > 0 ? 1f : 0f);
        
        // Detener movimiento horizontal al iniciar la carga
        rigidbody.linearVelocity = new Vector2(0f, rigidbody.linearVelocity.y);
    }

    void IniciarDobleSalto()
    {
    // Mantener la velocidad horizontal, solo añadir impulso vertical
    float fuerzaDobleSalto = (fuerzaSaltoMin + fuerzaSaltoMax) / 2f;

    // Aplicar impulso hacia arriba sin cancelar velocidad actual
    rigidbody.AddForce(Vector2.up * fuerzaDobleSalto, ForceMode2D.Impulse);

    dobleSaltoDisponible = false;
    saltando = true;
    animator.SetBool("Saltando", true);
    animator.SetBool("Moviendose", false);
    animator.SetBool("Cargando", false);

        HUDController hud = FindObjectOfType<HUDController>();
    if (hud != null)
    {
    hud.OcultarIcono();
    }

    if (InGameAudioManager.Instance != null)
    {
        InGameAudioManager.Instance.PlayDobleSalto();
    }
    }

void UsarDash()
{
    float inputDir = Input.GetAxisRaw("Horizontal");
    if (inputDir == 0 || estaDasheando) return;

    StartCoroutine(EjecutarDash(inputDir));
}



private IEnumerator EjecutarDash(float direccion)
{
    estaDasheando = true;

    // Cancelar toda velocidad actual y suspender gravedad
    rigidbody.linearVelocity = Vector2.zero;
    rigidbody.gravityScale = 0f;

    // Aplicar impulso horizontal
    rigidbody.AddForce(new Vector2(direccion, 0f) * fuerzaDash, ForceMode2D.Impulse);

    // Opcional: activar animación
    animator.SetTrigger("Dash");

    // Desactivar dash en HUD
    HUDController hud = FindObjectOfType<HUDController>();
    if (hud != null)
    {
        hud.OcultarIcono();
    }

    // Esperar duración del dash
    yield return new WaitForSeconds(duracionDash);

    // Restaurar gravedad
    rigidbody.gravityScale = gravedadOriginal;

    estaDasheando = false;
    dashDisponible = false;

    if (InGameAudioManager.Instance != null)
    {
        InGameAudioManager.Instance.PlayDash();
    }
}


    void EjecutarSalto()
{
    estaCargandoSalto = false;
    saltando = true;
    direccionCambiada = false;
    animator.SetBool("Saltando", true);
    animator.SetBool("Moviendose", false);
    animator.SetBool("Cargando", false);

    float porcentajeCarga = Mathf.Clamp01(tiempoCarga / cargaMaxima);
    float porcentajeCuadratico = porcentajeCarga * porcentajeCarga;
    float fuerzaActual = Mathf.Lerp(fuerzaSaltoMin, fuerzaSaltoMax, porcentajeCuadratico);

    rigidbody.sharedMaterial = bounceMat;
    rigidbody.linearVelocity = Vector2.zero;

    Vector2 direccion;

    if (direccionSalto != 0f)
    {
        direccion = new Vector2(direccionSalto * factorDiagonalX, factorDiagonalY);
        // Sin normalizar
    }
    else
    {
        direccion = Vector2.up;
    }

    Debug.Log($"Salto: Dir={direccionSalto}, Fuerza={fuerzaActual}, Vector={direccion}");
    Debug.DrawRay(transform.position, direccion * fuerzaActual, Color.cyan, 1f);

    rigidbody.AddForce(direccion * fuerzaActual, ForceMode2D.Impulse);

    if (InGameAudioManager.Instance != null)
    {
        InGameAudioManager.Instance.PlaySalto();
    }

}


    bool estaEnSuelo()
    {
        // Verificar que los puntos de verificación existan
        if (checkPointLeft == null || checkPointRight == null)
        {
            CrearPuntosVerificacion();
            return false;
        }

        // Comprobar con dos rayos en lugar de boxcast para mayor precisión
        RaycastHit2D leftCheckHit = Physics2D.Raycast(checkPointLeft.position, Vector2.down, rayLength, suelo);
        RaycastHit2D rightCheckHit = Physics2D.Raycast(checkPointRight.position, Vector2.down, rayLength, suelo);
        
        bool groundDetected = leftCheckHit || rightCheckHit;
        
        // Si está en el suelo y estaba saltando, resetear el estado de salto
        if (groundDetected && saltando)
        {
            saltando = false;
        }
        
        return groundDetected;
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

    void cambiarDireccion(float movimiento)
    {
        if ((mirandoDerecha && movimiento < 0) || (!mirandoDerecha && movimiento > 0))
        {
            mirandoDerecha = !mirandoDerecha;
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
    }
    
    // Método para visualizar los rayos de detección de suelo en el editor
    private void OnDrawGizmos()
    {
        // Solo dibujar si los puntos existen
        if (checkPointLeft != null && checkPointRight != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(checkPointLeft.position, checkPointLeft.position + Vector3.down * rayLength);
            Gizmos.DrawLine(checkPointRight.position, checkPointRight.position + Vector3.down * rayLength);
        }
    }

    public void ActivarDobleSalto()
    {
    dobleSaltoDisponible = true;
    }

    public void ActivarDash()
    {
    dashDisponible = true;
    }

    public bool TieneDobleSaltoActivo()
    {
    return dobleSaltoDisponible;
    }

    public bool TieneItemActivo()
{
    return dobleSaltoDisponible || dashDisponible;
}




}

