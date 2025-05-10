using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject panelPausa;
    //public GameObject panelOpciones;
    private bool juegoPausado = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
                AlternarPausa();
        }
    }

    public void AlternarPausa()
    {
        juegoPausado = !juegoPausado;

        panelPausa.SetActive(juegoPausado);
        Time.timeScale = juegoPausado ? 0f : 1f;
    }

    public void VolverAlMenu()
    {
        Time.timeScale = 1f; // por si acaso
        SceneManager.LoadScene("PantallaInicio"); // usa el nombre exacto de tu escena
    }

    public void AbrirOpciones()
    {
       // panelOpciones.SetActive(true);
       // panelPausa.SetActive(false);
    }

    public void CerrarOpciones()
    {
        //panelOpciones.SetActive(false);
        panelPausa.SetActive(true);
    }
}
