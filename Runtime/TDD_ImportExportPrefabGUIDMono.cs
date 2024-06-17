
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TDD_ImportExportPrefabGUIDMono : MonoBehaviour
{

    public BanListOfSourceGuidPrefabMono m_banList;
    

    public List<GameObject> m_toUsePrefab = new List<GameObject>();
    public List<GameObject> m_toUseResourcesPrefab = new List<GameObject>();


    public GameObject m_toMirror;
    public PrefabSourceGUIDMono[] m_inRootToMirror;

    [TextArea(2,10)]
    public string m_jsonExport;
    [TextArea(2, 10)]
    public string m_lineExport;


    public JSON_GroupOfInScenePrefabGUID m_jsonImport;

    public GameObject m_whereToCreate;
    public List<GameObject> m_created = new List<GameObject>();

    [ContextMenu("Reload")]
    public void Reload()
    {
        DestroyCreated();
        LoadAllPrefabSourceGUID();

        RemoveFromBanList();

        JSON_GroupOfInScenePrefabGUID group = new JSON_GroupOfInScenePrefabGUID();
        m_inRootToMirror = m_toMirror.GetComponentsInChildren<PrefabSourceGUIDMono>(true);
        foreach (var item in m_inRootToMirror)
        {
            RelocationPrefabGUID.GetWorldToLocal_DirectionalPoint(
                item.transform.position,
                item.transform.rotation,
                m_toMirror.transform, 
                out Vector3 localPosition, out Quaternion localRotation);

            group.m_prefabs.Add(new JSON_LocalPositionPrefabGUID() { 
            m_guid = item.GetPrefabGUID(),
            m_localPosition = localPosition,
            m_localRotation = localRotation,
            m_localScale = item.transform.localScale
            });
           
        }

        m_jsonExport = JsonUtility.ToJson(group, true);
        SourceGuidPrefabExport.ExportAsLines(group, out m_lineExport);
        m_jsonImport = JsonUtility.FromJson<JSON_GroupOfInScenePrefabGUID>(m_jsonExport);

        foreach (var item2 in m_jsonImport.m_prefabs)
        {

            GameObject obj = TryToFindInPrefabList(item2.m_guid, m_toUsePrefab);
            if (obj == null)
                obj = TryToFindInPrefabList(item2.m_guid, m_toUseResourcesPrefab);
            if (obj != null)
            {

                GameObject newObj = Instantiate(obj, m_whereToCreate.transform);
                newObj.transform.localPosition = item2.m_localPosition;
                newObj.transform.localRotation = item2.m_localRotation;
                newObj.transform.localScale = item2.m_localScale;
                m_created.Add(newObj);
            }
            else
            {

                if(m_banList.IsBanned(item2.m_guid))
                    continue;

                GameObject notFound = new GameObject("NOT FOUND:" + item2.m_guid);
                notFound.transform.SetParent(m_whereToCreate.transform);
                notFound.transform.localPosition = item2.m_localPosition;
                notFound.transform.localRotation = item2.m_localRotation;
                notFound.transform.localScale = item2.m_localScale;
                m_created.Add(notFound);


            }
        }
    }

    private void RemoveFromBanList()    
    {
       
        if (m_banList != null) { 
            m_banList.GetBannedGuid(out List<string> bannedGuid);
            foreach (var item in bannedGuid)
            {
                m_toUsePrefab.RemoveAll(k => k.GetComponent<PrefabSourceGUIDMono>().GetPrefabGUID() == item);
                m_toUseResourcesPrefab.RemoveAll(k => k.GetComponent<PrefabSourceGUIDMono>().GetPrefabGUID() == item);
            }
        }
    }

    private void DestroyCreated()
    {
        for  (int i = 0; i < m_created.Count; i++)
        {
            DestroyImmediate(m_created[i]);
        }
        m_created.Clear();
    }

    private GameObject TryToFindInPrefabList(string guidToFind, List<GameObject> prefab)
    {
        foreach (var item in prefab)
        {
            PrefabSourceGUIDMono p = item.GetComponent<PrefabSourceGUIDMono>();
            if (p != null && p.GetPrefabGUID() == guidToFind)
                return item;
        }
        return null;
        
    }

    public void LoadAllPrefabSourceGUID() {
        m_toUseResourcesPrefab.Clear();

        PrefabSourceGUIDMono[] g = Resources.LoadAll<PrefabSourceGUIDMono>("");
        m_toUseResourcesPrefab.AddRange(g.Select(k => k.gameObject));

    }
}
