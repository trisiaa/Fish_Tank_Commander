using UnityEngine;
using UnityEngine.Audio; 

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio Clips")]
    public AudioClip background;
    public AudioClip buttons;
    public AudioClip dialogue;
    public AudioClip gameOver;
    public AudioClip gameComplete;
    public AudioClip food;
    public AudioClip tutorial;

    [Header("Mixer Settings")]
    [SerializeField] private AudioMixerSnapshot normalSnapshot;
    [SerializeField] private AudioMixerSnapshot cutsceneSnapshot;
    [SerializeField] private float transitionTime = 1.0f; // Durasi fade in/out
    
    public static AudioManager instance;

    private void Awake ()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
    
    private void Start ()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    // Fungsi langsung untuk pindah ke snapshot Cutscene
    public void GoToCutscene()
    {
        cutsceneSnapshot.TransitionTo(0.5f); // 0.5 detik transisi agar halus
    }

    // Fungsi langsung untuk kembali ke Normal
    public void GoToNormal()
    {
        normalSnapshot.TransitionTo(0.5f);
    }

}