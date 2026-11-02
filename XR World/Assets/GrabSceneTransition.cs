using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
public class GrabSceneTransition : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string sceneToLoad = "NextScene";
    [SerializeField] private float transitionDelay = 0.5f;

    [Header("Optional Transition Effects")]
    [SerializeField] private bool fadeTransition = true;
    [SerializeField] private float fadeTime = 1f;

    private XRGrabInteractable grabInteractable;
    private bool hasTriggered = false;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrab);
    }

    void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        if (hasTriggered)
            return;

        if (string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.LogError("Scene name is empty! Please set the scene name in the inspector.");
            return;
        }

        hasTriggered = true;
        Debug.Log("Object grabbed! Transitioning to scene: " + sceneToLoad);

        if (transitionDelay > 0)
            Invoke(nameof(LoadScene), transitionDelay);
        else
            LoadScene();
    }

    private void LoadScene()
    {
        if (fadeTransition)
            StartCoroutine(FadeAndLoadScene());
        else
            SceneManager.LoadScene(sceneToLoad);
    }

    private System.Collections.IEnumerator FadeAndLoadScene()
    {
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(sceneToLoad);
    }
}