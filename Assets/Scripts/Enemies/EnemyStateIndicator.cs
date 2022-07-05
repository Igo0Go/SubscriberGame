using System.Collections;
using UnityEngine;

[System.Serializable]
public class EnemyStateIndicator
{
    public Light indicatorLight;
    public Color defaultColor = Color.green;
    public Color questionColor = Color.yellow;
    public Color agressiveColor = Color.red;
    public Color sleepColor = Color.white;
    public Renderer renderer;
    [Min(0)]
    public int targetMaterialNumber;
    public string targetColorValue = "_EmissionColor";

    public IEnumerator ChangeIndicatorState(IndicatorState state)
    {
        Color targetColor;
        Color startColor = indicatorLight.color;

        Material mat = renderer.materials[targetMaterialNumber];


        switch (state)
        {
            case IndicatorState.DefaultState:
                targetColor = defaultColor;
                break;
            case IndicatorState.Question:
                targetColor = questionColor;
                break;
            case IndicatorState.Agressive:
                targetColor = agressiveColor;
                break;
            case IndicatorState.Sleep:
                targetColor = sleepColor;
                break;
            default:
                targetColor = defaultColor;
                break;
        }

        float t = 0;

        while (t <= 1)
        {
            t += Time.deltaTime;

            indicatorLight.color = Color.Lerp(startColor, targetColor, t);
            mat.SetColor(targetColorValue, indicatorLight.color);

            yield return null;
        }
    }
}
public enum IndicatorState
{
    DefaultState,
    Question,
    Agressive,
    Sleep
}
