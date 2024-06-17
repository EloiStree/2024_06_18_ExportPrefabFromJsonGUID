using System.Text;
using UnityEngine;

public class SourceGuidPrefabExport {
    public static void ExportAsLines(JSON_GroupOfInScenePrefabGUID source, out string text)
    {

        StringBuilder sb = new StringBuilder();
        foreach (var item in source.m_prefabs)
        {
            Vector3 euler = item.m_localRotation.eulerAngles;
            sb.AppendLine(string.Format("{0}:" +
                "{1:0.0000}:{2:0.0000}:{3:0.0000}:" +
                "{4:0.0000}:{5:0.0000}:{6:0.0000}:" +
                "{7:0.0000}:{8:0.0000}:{9:0.0000}",
                item.m_guid,
                item.m_localPosition.x,
                item.m_localPosition.y,
                item.m_localPosition.z,
                euler.x,
                euler.y,
                euler.z,
                item.m_localScale.x,
                item.m_localScale.y,
                item.m_localScale.z
                ));

        }
        text = sb.ToString();
    }

    public static void ImportFromLines(string text, out JSON_GroupOfInScenePrefabGUID source)
    {
        JSON_GroupOfInScenePrefabGUID group = new JSON_GroupOfInScenePrefabGUID();
        string[] lines = text.Split('\n');
        foreach (var item in lines)
        {
            string[] parts = item.Split(':');
            if (parts.Length == 10)
            {
                JSON_LocalPositionPrefabGUID local = new JSON_LocalPositionPrefabGUID();
                local.m_guid = parts[0];
                local.m_localPosition = new Vector3(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3]));
                local.m_localRotation = Quaternion.Euler(float.Parse(parts[4]), float.Parse(parts[5]), float.Parse(parts[6]));
                local.m_localScale = new Vector3(float.Parse(parts[7]), float.Parse(parts[8]), float.Parse(parts[9]));
                group.m_prefabs.Add(local);
            }
        }
        source = group;
    }


}
