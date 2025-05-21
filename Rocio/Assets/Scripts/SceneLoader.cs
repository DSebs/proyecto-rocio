using UnityEngine;
using System.Collections;
using TMPro;


public class SceneLoader : MonoBehaviour
{
    public GameObject panelCreditos; // Asignar desde el inspector
    private CanvasGroup canvasGroup;
    [SerializeField] private GameObject panelNombreJugador;
    [SerializeField] private TMP_InputField nombreInputField;
    [SerializeField] private CanvasGroup canvasGroupNombre;

    [SerializeField] private GameObject fondoOscuro;
    [SerializeField] private GameObject panelOpciones;
    [SerializeField] private CanvasGroup canvasGroupOpciones;

    private void Start()
    {
        if (panelCreditos != null)
        {
            canvasGroup = panelCreditos.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void LoadGame()
    {
    AudioManager.Instance.PlayClick();

    canvasGroupNombre = panelNombreJugador.GetComponent<CanvasGroup>();
    canvasGroupNombre.alpha = 0f;
    canvasGroupNombre.interactable = false;
    canvasGroupNombre.blocksRaycasts = false;

    StopAllCoroutines();
    StartCoroutine(FadeInNombreJugador());
    }

    public void ConfirmarNombreYComenzar()
    {
    string nombre = nombreInputField.text.Trim();

    if (!string.IsNullOrEmpty(nombre))
    {
        JugadorData.Instancia.nombreJugador = nombre;
        UnityEngine.SceneManagement.SceneManager.LoadScene("PrimeraPantalla");
    }
    }

    public void MostrarCreditos()
    {
        AudioManager.Instance.PlayClick();
        StopAllCoroutines(); // Cancela si ya hay una animación corriendo
        StartCoroutine(FadeInCreditos());
    }

    public void OcultarCreditos()
    {
        AudioManager.Instance.PlayClick(); 
        StopAllCoroutines();
        StartCoroutine(FadeOutCreditos());
    }

    public void MostrarOpciones()
{
    AudioManager.Instance.PlayClick();
    StopAllCoroutines();
    StartCoroutine(FadeInOpciones());
}

public void OcultarOpciones()
{
    AudioManager.Instance.PlayClick();
    StopAllCoroutines();
    StartCoroutine(FadeOutOpciones());
}

public void OcultarNombreJugador()
{
    AudioManager.Instance.PlayClick();
    StopAllCoroutines();
    StartCoroutine(FadeOutNombreJugador());
}
private IEnumerator FadeInNombreJugador()
{
    canvasGroupNombre.interactable = true;
    canvasGroupNombre.blocksRaycasts = true;
    fondoOscuro.SetActive(true);

    float duration = 0.3f;
    float time = 0f;

    panelNombreJugador.transform.localScale = Vector3.zero;

    while (time < duration)
    {
        float t = time / duration;
        canvasGroupNombre.alpha = Mathf.Lerp(0f, 1f, t);
        panelNombreJugador.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
        time += Time.deltaTime;
        yield return null;
    }

    canvasGroupNombre.alpha = 1f;
    panelNombreJugador.transform.localScale = Vector3.one;
}

private IEnumerator FadeOutNombreJugador()
{
    canvasGroupNombre.interactable = false;
    canvasGroupNombre.blocksRaycasts = false;
    fondoOscuro.SetActive(false);

    float duration = 0.3f;
    float time = 0f;
    Vector3 initialScale = Vector3.one;

    while (time < duration)
    {
        float t = time / duration;
        canvasGroupNombre.alpha = Mathf.Lerp(1f, 0f, t);
        panelNombreJugador.transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, t);
        time += Time.deltaTime;
        yield return null;
    }

    canvasGroupNombre.alpha = 0f;
    panelNombreJugador.transform.localScale = Vector3.zero;
}

private IEnumerator FadeInCreditos()
{
    canvasGroup.interactable = true;
    canvasGroup.blocksRaycasts = true;
    fondoOscuro.SetActive(true);

    float duration = 0.3f;
    float time = 0f;

    // Zoom IN
    panelCreditos.transform.localScale = Vector3.zero;

    while (time < duration)
    {
        float t = time / duration;
        canvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
        panelCreditos.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
        time += Time.deltaTime;
        yield return null;
    }

    canvasGroup.alpha = 1f;
    panelCreditos.transform.localScale = Vector3.one;
}

private IEnumerator FadeOutCreditos()
{
    canvasGroup.interactable = false;
    canvasGroup.blocksRaycasts = false;
    fondoOscuro.SetActive(false);

    float duration = 0.3f;
    float time = 0f;

    Vector3 initialScale = Vector3.one;

    while (time < duration)
    {
        float t = time / duration;
        canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);
        panelCreditos.transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, t);
        time += Time.deltaTime;
        yield return null;
    }

    canvasGroup.alpha = 0f;
    panelCreditos.transform.localScale = Vector3.zero;
}


private IEnumerator FadeInOpciones()
{
    canvasGroupOpciones.interactable = true;
    canvasGroupOpciones.blocksRaycasts = true;
    fondoOscuro.SetActive(true); // Reutilizamos el fondo oscuro

    float duration = 0.3f;
    float time = 0f;

    panelOpciones.transform.localScale = Vector3.zero;

    while (time < duration)
    {
        float t = time / duration;
        canvasGroupOpciones.alpha = Mathf.Lerp(0f, 1f, t);
        panelOpciones.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
        time += Time.deltaTime;
        yield return null;
    }

    canvasGroupOpciones.alpha = 1f;
    panelOpciones.transform.localScale = Vector3.one;
}

private IEnumerator FadeOutOpciones()
{
    canvasGroupOpciones.interactable = false;
    canvasGroupOpciones.blocksRaycasts = false;
    fondoOscuro.SetActive(false);

    float duration = 0.3f;
    float time = 0f;
    Vector3 initialScale = Vector3.one;

    while (time < duration)
    {
        float t = time / duration;
        canvasGroupOpciones.alpha = Mathf.Lerp(1f, 0f, t);
        panelOpciones.transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, t);
        time += Time.deltaTime;
        yield return null;
    }

    canvasGroupOpciones.alpha = 0f;
    panelOpciones.transform.localScale = Vector3.zero;
}


}
