using System.Collections;
using UnityEngine;
using TMPro;

public class ReneDialogo : MonoBehaviour
{
    [Header("Referencia UI")]
    public TMP_Text textoUI;

    [Header("Configuración")]
    [TextArea(2, 4)]
    public string[] lineasDialogo;

    public float tiempoEntreLetras = 0.05f;
    public float tiempoEntreLineas = 3f;

    private void Start()
    {
        StartCoroutine(MostrarDialogoSecuencial());
    }

    IEnumerator MostrarDialogoSecuencial()
    {
        foreach (string linea in lineasDialogo)
        {
            textoUI.text = "";
            foreach (char letra in linea)
            {
                textoUI.text += letra;
                yield return new WaitForSeconds(tiempoEntreLetras);
            }


            yield return new WaitForSeconds(tiempoEntreLineas);
        }

        // Al final puedes limpiar el texto o dejarlo en la última frase
        // textoUI.text = "";
    }
}
