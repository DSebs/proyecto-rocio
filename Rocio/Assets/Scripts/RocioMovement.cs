using UnityEngine;
using System.Threading;


public class RocioMovement : MonoBehaviour
{
    public float velocidad;
    public float fuerzaSalto;
    public LayerMask suelo;
    private Rigidbody2D rigidbody;
    private PolygonCollider2D boxCollider;
    private bool mirandoDerecha = true;
    private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<PolygonCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float mov = mover();

    }
    bool estaEnSuelo()
    {
        RaycastHit2D rayCastHit2D = Physics2D.BoxCast(boxCollider.bounds.center, new Vector2(boxCollider.bounds.size.x, boxCollider.bounds.size.y), 0f, Vector2.down, 0.2f, suelo);
        return rayCastHit2D;
    }

    float mover()
    {
        float InputMov = Input.GetAxis("Horizontal");
        if (InputMov != 0f)
        {
            animator.SetBool("Moviendose",true);
        }
        else
        {
            animator.SetBool("Moviendose",false);
        }
        saltar(InputMov);
        rigidbody.velocity = new Vector2(InputMov * velocidad, rigidbody.velocity.y);
        cambiarDireccion(InputMov);
        return InputMov;
    }
    void saltar(float mov)
    {
        if (Input.GetKeyDown(KeyCode.Space) && estaEnSuelo())
        {
            if (mov == 0f)
            {
                animator.SetBool("Saltando", true);
            }
            rigidbody.AddForce(Vector2.up*fuerzaSalto,ForceMode2D.Impulse);
        }
        else if (estaEnSuelo())
        {
            animator.SetBool("Saltando", false);
        }
    }

    void cambiarDireccion(float movimiento)
    {

        if ((mirandoDerecha == true && movimiento < 0) || (mirandoDerecha == false && movimiento > 0))
        {
            mirandoDerecha = !mirandoDerecha;
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
    }


}