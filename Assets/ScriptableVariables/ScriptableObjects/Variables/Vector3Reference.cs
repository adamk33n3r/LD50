using UnityEngine;

[System.Serializable]
public class Vector3Reference : BaseReference<Vector3Variable, Vector3>
{
    public Vector3Reference() {}
    public Vector3Reference(Vector3 defaultValue) : base(defaultValue) {}
    public static implicit operator Vector3Reference(Vector3 value) {
        return new Vector3Reference(value);
    }
}
