using UnityEngine;

public class DashItem : MonoBehaviour
{
    [SerializeField] private string tagJugador = "Player";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(tagJugador))
        {
            RocioPlayerController controller = collision.GetComponent<RocioPlayerController>();
            if (controller != null && !controller.TieneItemActivo())
            {
                controller.ActivarDash();

                HUDController hud = FindObjectOfType<HUDController>();
                if (hud != null)
                {
                    hud.MostrarIconoDash();
                }

                Destroy(gameObject);
            }
        }
    }
}