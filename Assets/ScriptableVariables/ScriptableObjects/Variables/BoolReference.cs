[System.Serializable]
public class BoolReference : BaseReference<BoolVariable, bool>
{
    public BoolReference() {}
    public BoolReference(bool defaultValue) : base(defaultValue) {}
    public static implicit operator BoolReference(bool value) {
        return new BoolReference(value);
    }
}
