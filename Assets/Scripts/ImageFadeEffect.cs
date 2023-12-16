using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class ImageFadeEffect : MonoBehaviour
{
    Image image;

    [SerializeField]
    float targetAlpha = 0f;
    [SerializeField]
    float fadeSpeed = 2f;
    [SerializeField]
    bool unscaledTime = false;

    public float TargetAlpha { get => targetAlpha; set { enabled = true; targetAlpha = value; } }
    public float FadeSpeed { get => fadeSpeed; set => fadeSpeed = value; }
    public bool UnscaledTime { get => unscaledTime; set => unscaledTime = value; }

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        Color color = image.color;
        color.a = Mathf.MoveTowards(color.a, targetAlpha, fadeSpeed * (unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime));
        image.color = color;

        if (color.a == targetAlpha)
        {
            enabled = false;
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        SceneVisibilityManager.instance.Hide(gameObject, false);
    }
#endif
}