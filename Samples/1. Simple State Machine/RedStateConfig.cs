using UnityEngine;

[CreateAssetMenu(fileName = "RedStateConfig", menuName = "Scriptable Objects/RedStateConfig")]
public class RedStateConfig : ScriptableObject
{
    [SerializeField] public Color Color = Color.red;
}
