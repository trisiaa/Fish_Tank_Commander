using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }
    
    [SerializeField] Animator transitionAnim;
    public float transitionTime = 1f;

    private void Awake()
    {
        // THE CRITICAL LOGIC:
        if (Instance == null)
        {
            Instance = this;
            // This line makes the object survive the transition to the next scene
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            // If a second manager exists in the new scene, destroy it 
            // so we don't have two wipes playing at once.
            Destroy(gameObject);
        }
    }

    public void NextLevel(string sceneName)
    {
        StartCoroutine(LoadLevel(sceneName));
    }

    IEnumerator LoadLevel(string sceneName)
    {
        // 1. Start the closing wipe
        transitionAnim.SetTrigger("End");

        // 2. Wait for it to cover the screen
        yield return new WaitForSecondsRealtime(transitionTime);

        // 3. Load the scene
        // Because of 'DontDestroyOnLoad', this script keeps running!
        SceneManager.LoadScene(sceneName);

        // 4. Wait a tiny fraction of a second for the new scene to settle
        yield return null; 

        // 5. Trigger the opening wipe in the NEW scene
        transitionAnim.SetTrigger("End");
    }
}