using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource musicaSource;
    public AudioSource lluviaSource;
    public AudioSource efectosSource;

    public AudioClip clickSound;

    private void Awake()
    {
        // Singleton para acceder desde cualquier parte
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persistente entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        musicaSource.Play();
        lluviaSource.Play();
    }

    public void PlayClick()
    {
        if (clickSound != null)
            efectosSource.PlayOneShot(clickSound);
    }
}
