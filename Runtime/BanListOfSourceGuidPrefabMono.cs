using System.Collections.Generic;
using UnityEngine;

public class BanListOfSourceGuidPrefabMono :MonoBehaviour {

    [SerializeField] List<string> m_bannedGuid= new List<string>();

    public void GetBannedGuid(out List<string> bannedGuid)
    {
        bannedGuid = m_bannedGuid;
    }
    public void GetBannedGuid(out string[] bannedGuid)
    {
        bannedGuid = m_bannedGuid.ToArray();
    }
    public void AddBannedGuid(string guid)
    {
        if (!m_bannedGuid.Contains(guid))
        {
            m_bannedGuid.Add(guid);
        }
    }
    public void RemoveBannedGuid(string guid)
    {
        if (m_bannedGuid.Contains(guid))
        {
            m_bannedGuid.Remove(guid);
        }
    }
    public void ClearBannedGuid()
    {
        m_bannedGuid.Clear();
    }

    public bool IsBanned(string guid)
    {
        return m_bannedGuid.Contains(guid);
    }
}
