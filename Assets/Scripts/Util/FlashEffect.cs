using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashEffect : MonoBehaviour
{
    private const string colorPropertyName = "_Color";
    private YieldInstruction flashDuration = new WaitForSeconds(0.066f);
    private new Renderer renderer;
    private Material baseMaterial;
    private Color baseColor;

    #region Inspector

    [SerializeField]
    private Material hitMaterial = null;

    #endregion

    void Awake()
    {
        renderer = GetComponentInChildren<Renderer>();
        baseMaterial = renderer.material;
        baseColor = baseMaterial.GetColor(colorPropertyName);
    }

    public void Flash()
    {
        StartCoroutine(FlashRoutine());
    }

    public void Flash(float normalizedHP)
    {
        StartCoroutine(EnemyFlashRoutine(normalizedHP));
    }

    private IEnumerator FlashRoutine()
    {
        renderer.material = hitMaterial;

        yield return flashDuration;

        renderer.material = baseMaterial;
    }

    private IEnumerator EnemyFlashRoutine(float normalizedHP)
    {
        renderer.material = hitMaterial;

        yield return flashDuration;

        renderer.material = baseMaterial;

        float colorFadeRate = 1.0f - normalizedHP;
        Color newColor = Color.Lerp(baseColor, Color.gray, colorFadeRate);
        renderer.material.SetColor(colorPropertyName, newColor);
    }
}
