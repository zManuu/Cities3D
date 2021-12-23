using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tools : MonoBehaviour
{

    [SerializeField] private Transform ToolCanvas;

    private int Tool_None = 0, Tool_Road = 1, Tool_Buildings = 2, Tool_Manage = 3;
    private int ActiveTool = 0;

    public void OnToolPress(int ToolID)
    {
        if (ActiveTool == ToolID)
        {
            CloseTools();
            return;
        }

        ActiveTool = ToolID;
        ToolCanvas.GetChild(ToolID - 1).gameObject.SetActive(true);

        // Show tool background
        Color c = ToolCanvas.GetComponent<Image>().color;
        c.a = 0.5f;
        ToolCanvas.GetComponent<Image>().color = c;
    }

    private void CloseTools()
    {
        // Hide tool background
        Color c = ToolCanvas.GetComponent<Image>().color;
        c.a = 0;
        ToolCanvas.GetComponent<Image>().color = c;

        // close the tools
        for (var i=0; i<ToolCanvas.childCount; i++)
        {
            ToolCanvas.GetChild(i).gameObject.SetActive(false);
        }

        // misc
        ActiveTool = 0;
    }

}
