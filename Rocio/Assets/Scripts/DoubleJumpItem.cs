using UnityEngine;

public class DoubleJumpItem : MonoBehaviour
{
    // Tag para identificar al jugador
    [SerializeField] private string tagJugador = "Player";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(tagJugador))
        {
            // Obtener referencia al controlador del jugador
            RocioPlayerController controller = collision.GetComponent<RocioPlayerController>();
            if (controller != null)
            {
                controller.ActivarDobleSalto(); // Método que implementaremos en el player
            }

            // Notificar al HUD (más adelante)
            HUDController hud = FindObjectOfType<HUDController>();
            if (hud != null)
            {
                hud.MostrarIconoDobleSalto(); // Lo implementaremos luego
            }

            // Destruir el ítem
            Destroy(gameObject);
        }
    }
}
