using UnityEngine;
using UnityEngine.UI;

public class OpcionesManager : MonoBehaviour
{
    public Slider sliderMusica;
    public Slider sliderLluvia;
    public Slider sliderBrillo;
    
    public Image panelBrillo; // Un panel negro que cubre toda la pantalla
    public AudioSource audioMusica;
    public AudioSource audioLluvia;

    private void Start()
    {
        // Inicializar sliders con los valores actuales
        sliderMusica.value = audioMusica.volume;
        sliderLluvia.value = audioLluvia.volume;
        sliderBrillo.value = 1 - panelBrillo.color.a; // El valor inicial es inverso a la opacidad del panel

        // Agregar listeners
        sliderMusica.onValueChanged.AddListener(CambiarVolumenMusica);
        sliderLluvia.onValueChanged.AddListener(CambiarVolumenLluvia);
        sliderBrillo.onValueChanged.AddListener(CambiarBrillo);
    }

    void CambiarVolumenMusica(float valor)
    {
        audioMusica.volume = valor;
    }

    void CambiarVolumenLluvia(float valor)
    {
        audioLluvia.volume = valor;
    }

    void CambiarBrillo(float valor)
    {
        // Actualizar la transparencia del panel negro superpuesto
        // A más brillo, menos opaco debe ser el panel
        Color colorPanel = panelBrillo.color;
        colorPanel.a = 1 - valor; // Si valor es 1, alpha es 0 (más brillo)
        panelBrillo.color = colorPanel;
    }
}