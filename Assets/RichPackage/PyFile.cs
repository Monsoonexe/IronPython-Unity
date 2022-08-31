using UnityEngine;

[CreateAssetMenu(fileName = "PyFile",
	menuName = "ScriptableObjects/PyFile")]
public class PyFile : ScriptableObject
{
    [SerializeField]
    private string tag;

    [SerializeField]
    private TextAsset textAsset;

    public LayerMask layerMask;

}
