using UnityEngine;

public class RocioMovement : MonoBehaviour
{
    public float velocidad;
    public float fuerzaSalto;
    public LayerMask suelo;
    private Rigidbody2D rigidbody;
    private BoxCollider2D boxCollider;
    private bool mirandoDerecha = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        mover();
        saltar();

    }
    bool estaEnSuelo()
    {
        RaycastHit2D rayCastHit2D = Physics2D.BoxCast(boxCollider.bounds.center, new Vector2(boxCollider.bounds.size.x, boxCollider.bounds.size.y), 0f, Vector2.down, 0.2f, suelo);
        return rayCastHit2D;
    }

    void mover()
    {
        float InputMov = Input.GetAxis("Horizontal");

        rigidbody.velocity = new Vector2(InputMov * velocidad, rigidbody.velocity.y);
        cambiarDireccion(InputMov);
    }
    void saltar()
    {
        if (Input.GetKeyDown(KeyCode.Space) && estaEnSuelo())
        {
            rigidbody.AddForce(Vector2.up*fuerzaSalto,ForceMode2D.Impulse);
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