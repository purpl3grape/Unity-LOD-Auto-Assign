using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LODAssignment : MonoBehaviour
{
    LODGroup lodGroup;
    LOD[] lods = new LOD[2];

    public List<Renderer> lodRenderer0 = new List<Renderer>();
    public List<Renderer> lodRenderer1 = new List<Renderer>();

    Renderer renderer;
    
    Transform childTr;

    [ContextMenu("Adding LOD child Objects")]
    private void LODUpdate()
    {
        lodGroup = GetComponent<LODGroup>();
        lodRenderer0 = new List<Renderer>();
        lodRenderer1 = new List<Renderer>();

        AssignRenderersToRendererList();
        AssignRenderListToLOD();
        AddingLODToLODGroup();
    }

    [ContextMenu("CLEARING LODs")]
    private void ClearLODs()
    {
        lodGroup.SetLODs(nulllods);
    }

    LOD[] nulllods;
    private void CreateEmptyLods()
    {
        nulllods = new LOD[2];
        nulllods[0].screenRelativeTransitionHeight = 0.3f;
        nulllods[1].screenRelativeTransitionHeight = 0.6f;
        nulllods[0].renderers = null;
        nulllods[1].renderers = null;
        lodGroup.RecalculateBounds();
    }

    [ContextMenu("Init Renderers to Lod List")]
    private void AssignRenderersToRendererList()
    {
        SearchChildOf(transform);
    }

    private void SearchChildOf(Transform parent)
    {
        for(int i = 0; i < parent.childCount; i++)
        {
            childTr = parent.GetChild(i);

            Debug.Log("Iterating through: " + childTr.name);
            if (childTr.name.Contains("LOD1"))
            {
                if (childTr.GetComponent<MeshRenderer>())
                {
                    Debug.Log("LOD 1 Adding Child: " + childTr.name);
                    lodRenderer1.Add(childTr.GetComponent<Renderer>());
                }
            }
            else if (childTr.name.Contains("LOD0"))
            {
                if (childTr.GetComponent<MeshRenderer>())
                {
                    Debug.Log("LOD 0 Adding Child: " + childTr.name);
                    lodRenderer0.Add(childTr.GetComponent<Renderer>());
                }
            }

            //Recursive
            if (childTr.childCount > 0)
            {
                SearchChildOf(childTr);
            }

        }
    }


    private void AssignRenderListToLOD()
    {
        if (lods.Length > 0)
        {
            Debug.Log("There are " + lods.Length + " lods");
            lods[0] = new LOD(1.0F / (10), lodRenderer0.ToArray());
            lods[1] = new LOD(1.0F / (20), lodRenderer1.ToArray());
        }
        else
        {
            Debug.Log("No lods found");
        }
    }

    private void AddingLODToLODGroup()
    {
        if (lodGroup != null)
        {
            lodGroup.SetLODs(lods);
            lodGroup.RecalculateBounds();
        }
        else
        {
            Debug.Log("lodGroup is null");
        }
    }


    LODGroup[] allLODGroups;

    [ContextMenu("Assign This Script to all LOD Group Objects")]
    private void AssignThisScriptToAllLODGroups()
    {
        allLODGroups = FindObjectsOfType<LODGroup>();

        foreach(LODGroup _lodgroup in allLODGroups)
        {
            if (!_lodgroup.GetComponent<LODAssignment>())
            {
                _lodgroup.gameObject.AddComponent<LODAssignment>();
            }
        }
    }


}
