using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OverheadTextHandler : MonoBehaviour
{
    public Text textComponent;
    public float displayTime = 5f;

    private Coroutine currentRoutine;

    public void ShowText(string text)
    {
        if (textComponent == null) return;

        textComponent.text = text;
        gameObject.SetActive(true);

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(HideAfterTime());
    }

    IEnumerator HideAfterTime()
    {
        yield return new WaitForSeconds(displayTime);
        gameObject.SetActive(false);
    }

    void LateUpdate()
    {
        // Always face the camera
        if (Camera.main != null)
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}