using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class PanelManager : Singleton<PanelManager>
{ 
    [SerializeField] GameObject clone = null;
    
    Dictionary<Panel, GameObject> dictionary = new Dictionary<Panel, GameObject>();
    
    public void Open(Panel panel)
    {
        if (dictionary.TryGetValue(panel, out clone) == false)
        {
            clone = (GameObject)Instantiate(Resources.Load(panel.ToString()));
    
            clone.name = clone.name.Replace("(Clone)", "");
    
            dictionary.Add(panel, clone);
    
            DontDestroyOnLoad(clone);
        }
        else
        {
            clone = dictionary[panel];
    
            clone.SetActive(true);
        }
    }
}
