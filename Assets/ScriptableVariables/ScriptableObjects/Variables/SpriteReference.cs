using UnityEngine;

[System.Serializable]
public class SpriteReference : BaseReference<SpriteVariable, Sprite>
{
    public SpriteReference() {}
    public SpriteReference(Sprite defaultValue) : base(defaultValue) {}
    public static implicit operator SpriteReference(Sprite value) {
        return new SpriteReference(value);
    }
}
