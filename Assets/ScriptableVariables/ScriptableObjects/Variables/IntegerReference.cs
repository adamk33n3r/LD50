[System.Serializable]
public class IntegerReference : BaseReference<IntegerVariable, int>
{
    public IntegerReference() {}
    public IntegerReference(int defaultValue) : base(defaultValue) {}
    public static implicit operator IntegerReference(int value) {
        return new IntegerReference(value);
    }
}
