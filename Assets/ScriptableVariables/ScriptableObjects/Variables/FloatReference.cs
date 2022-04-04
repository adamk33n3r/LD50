[System.Serializable]
public class FloatReference : BaseReference<FloatVariable, float>
{
    public FloatReference() {}
    public FloatReference(float defaultValue) : base(defaultValue) {}
    public static implicit operator FloatReference(float value) {
        return new FloatReference(value);
    }
}
