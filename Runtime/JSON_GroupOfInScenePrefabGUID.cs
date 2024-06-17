using System.Collections.Generic;

[System.Serializable]
public class JSON_GroupOfInScenePrefabGUID { 


    public List<JSON_LocalPositionPrefabGUID> m_prefabs = new List<JSON_LocalPositionPrefabGUID>();

    public IEnumerable<JSON_LocalPositionPrefabGUID> GetItems() { return m_prefabs;}

}
