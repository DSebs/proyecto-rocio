using UnityEngine;
using UnityEngine.UI;

public class InGameAudioManager : MonoBehaviour
{
    public static InGameAudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicaSource;
    public AudioSource efectosSource;

    [Header("Efectos de sonido")]
    public AudioClip saltoSFX;
    public AudioClip dashSFX;
    public AudioClip dobleSaltoSFX;
    public AudioClip caidaFuerteSFX;

    [Header("Sliders de volumen")]
    public Slider sliderMusica;
    public Slider sliderEfectos;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        musicaSource.Play();

        if (sliderMusica != null)
        {
            sliderMusica.onValueChanged.AddListener(SetVolumenMusica);
            sliderMusica.value = musicaSource.volume;
        }

        if (sliderEfectos != null)
        {
            sliderEfectos.onValueChanged.AddListener(SetVolumenEfectos);
            sliderEfectos.value = efectosSource.volume;
        }
    }

    public void SetVolumenMusica(float value)
    {
        musicaSource.volume = value;
    }

    public void SetVolumenEfectos(float value)
    {
        efectosSource.volume = value;
    }

    public void PlaySalto()
    {
        efectosSource.PlayOneShot(saltoSFX);
    }

    public void PlayDobleSalto()
    {
        efectosSource.PlayOneShot(dobleSaltoSFX);
    }

    public void PlayDash()
    {
        efectosSource.PlayOneShot(dashSFX);
    }

    public void PlayCaidaFuerte()
    {
        efectosSource.PlayOneShot(caidaFuerteSFX);
    }
}
