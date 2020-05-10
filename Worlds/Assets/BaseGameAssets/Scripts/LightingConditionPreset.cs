using UnityEngine;

[CreateAssetMenu(menuName = "LightingPreset")]
public class LightingConditionPreset : ScriptableObject
{
    public Gradient ambientColor;
    public Gradient directionalColor;
    public Gradient fogColor;
    public AnimationCurve fogIntensity;
}