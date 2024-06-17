using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSourceGUIDMono : MonoBehaviour
{

    [Tooltip("The aim is to find back what is the prefab to load from text or bytes like json xml b64 ...")]
    [SerializeField] string m_guidOfPrefab;


    public string GetPrefabGUID()
    {
        return m_guidOfPrefab;
    }

    private void Reset()
    {
        SetWithNewGUID();
    }

    [ContextMenu("Set new GUID")]
    private void SetWithNewGUID()
    {
        m_guidOfPrefab = System.Guid.NewGuid().ToString();
    }
}
