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
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
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
