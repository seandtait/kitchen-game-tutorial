using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class KitchenObjectSO : ScriptableObject 
{
    [SerializeField]
    private Transform prefab;
    public Transform Prefab { get { return prefab; } }

    [SerializeField]
    private Sprite sprite;
    public Sprite Sprite { get { return sprite; } }

    [SerializeField]
    private string objectName;
    public string ObjectName { get {  return objectName; } }

}
