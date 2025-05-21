using UnityEngine;

public class JugadorData : MonoBehaviour
{
    public static JugadorData Instancia;
    public string nombreJugador = "";

    private void Awake()
    {
        if (Instancia == null)
        {
            Instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
