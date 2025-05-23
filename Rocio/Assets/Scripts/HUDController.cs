using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TMP_Text itemLabel;

    [Header("Icono del doble salto")]
    public Sprite dobleSaltoSprite;
    [Header("Icono del dash")]
    public Sprite dashSprite;

    public void MostrarIconoDobleSalto()
    {
        itemIcon.sprite = dobleSaltoSprite;
        itemIcon.enabled = true;
        itemLabel.text = "Doble Salto";
    }

    public void MostrarIconoDash()
    {
        itemIcon.sprite = dashSprite;
        itemIcon.enabled = true;
        itemLabel.text = "Dash";
    }

    public void OcultarIcono()
    {
        itemIcon.sprite = null;
        itemIcon.enabled = false;
        itemLabel.text = "";
    }
}
