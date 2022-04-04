using UnityEngine;

[System.Serializable]
public class ColorReference : BaseReference<ColorVariable, Color>
{
    public ColorReference() {}
    public ColorReference(Color defaultValue) : base(defaultValue) {}
    public static implicit operator ColorReference(Color value) {
        return new ColorReference(value);
    }
}
