using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Tools : MonoBehaviour
{

    [SerializeField] private Transform ToolCanvas;
    [SerializeField] private GameManager game;
    [SerializeField] private Transform BulldozePreview;
    [SerializeField] private Transform ToolButtonHolder;
    [SerializeField] private Color ToolButtonSelected;
    [SerializeField] private Color ToolButtonDefault;

    private const int Tool_None = 0, Tool_Road = 1, Tool_Buildings = 2, Tool_Manage = 3, Tool_Bulldoze = 4;
    private List<int> ToolsWithMenu = new List<int>() { 1, 2, 3 };
    private int ActiveTool = 0;
    private int CurrentRoadRotation = 0;
    private int CurrentRoadType = 0;
    private List<Transform> RoadPreviews;

    private void Start()
    {
        RoadPreviews = new List<Transform>();
        game.RoadPrefabs.ToList().ForEach(r =>
        {
            var rp = Instantiate(r, BulldozePreview.parent);
            rp.name = r.name + "-Preview";
            rp.GetChildrenList().ForEach(child => child.tag = "RoadPreview");
            RoadPreviews.Add(rp);
        });
    }

    public void OnToolPress(int ToolID)
    {
        if (ActiveTool == ToolID)
        {
            CloseTools();
            return;
        }

        ActiveTool = ToolID;
        ToolButtonHolder.GetChild(ToolID - 1).GetComponent<Image>().color = ToolButtonSelected;

        if (ToolsWithMenu.Contains(ToolID))
        {
            ToolCanvas.GetChild(ToolID - 1).gameObject.SetActive(true);

            // Show tool background
            Color c = ToolCanvas.GetComponent<Image>().color;
            c.a = 0.5f;
            ToolCanvas.GetComponent<Image>().color = c;
        }
    }

    private void CloseTools()
    {
        // reset tool button background
        for (var i=0; i<ToolButtonHolder.childCount; i++)
        {
            ToolButtonHolder.GetChild(i).GetComponent<Image>().color = ToolButtonDefault;
        }

        // Hide menu background
        Color c = ToolCanvas.GetComponent<Image>().color;
        c.a = 0;
        ToolCanvas.GetComponent<Image>().color = c;

        // close the tools
        for (var i=0; i<ToolCanvas.childCount; i++)
        {
            ToolCanvas.GetChild(i).gameObject.SetActive(false);
        }

        ActiveTool = 0;
    }

    private void Update()
    {
        switch (ActiveTool)
        {
            case Tool_Bulldoze:
                // bulldoze preview
                if (BulldozePreview.gameObject.activeSelf == false)
                {
                    BulldozePreview.gameObject.SetActive(true);
                }
                var r1 = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(r1, out var rh1))
                {
                    var roadPos = rh1.point;
                    BulldozePreview.position = new Vector3(Mathf.RoundToInt(roadPos.x), 0.51f, Mathf.RoundToInt(roadPos.z));
                }
                break;
            case Tool_Road:
                // road preview
                for(var i=0; i<RoadPreviews.Count; i++)
                {
                    if (CurrentRoadType != i && RoadPreviews[i].gameObject.activeSelf)
                    {
                        RoadPreviews[i].gameObject.SetActive(false);
                        RoadPreviews[i].rotation = Quaternion.Euler(0, 0, 0);
                    }
                }
                if (RoadPreviews[CurrentRoadType].gameObject.activeSelf == false)
                {
                    RoadPreviews[CurrentRoadType].gameObject.SetActive(true);
                }
                var r2 = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(r2, out var rh2))
                {
                    var roadPos = rh2.point;
                    RoadPreviews[CurrentRoadType].position = new Vector3(Mathf.RoundToInt(roadPos.x), 0.5f, Mathf.RoundToInt(roadPos.z));
                    switch (CurrentRoadRotation)
                    {
                        case 1:
                            RoadPreviews[CurrentRoadType].rotation = Quaternion.Euler(0, 90, 0);
                            break;
                        case 2:
                            RoadPreviews[CurrentRoadType].rotation = Quaternion.Euler(0, 180, 0);
                            break;
                        case 3:
                            RoadPreviews[CurrentRoadType].rotation = Quaternion.Euler(0, 270, 0);
                            break;
                        case 4:
                            RoadPreviews[CurrentRoadType].rotation = Quaternion.Euler(0, 360, 0);
                            break;
                    }
                }


                // road type selection
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    CurrentRoadType = 0;
                    CurrentRoadRotation = 0;
                    RoadPreviews.ForEach(r => r.rotation = Quaternion.Euler(0, 0, 0));
                } else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    CurrentRoadType = 1;
                    CurrentRoadRotation = 0;
                    RoadPreviews.ForEach(r => r.rotation = Quaternion.Euler(0, 0, 0));
                } else if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    CurrentRoadType = 2;
                    CurrentRoadRotation = 0;
                    RoadPreviews.ForEach(r => r.rotation = Quaternion.Euler(0, 0, 0));
                } else if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    CurrentRoadType = 3;
                    CurrentRoadRotation = 0;
                    RoadPreviews.ForEach(r => r.rotation = Quaternion.Euler(0, 0, 0));
                }

                break;
            case Tool_None:
                // deactivate previews, if they are still visible
                if (BulldozePreview.gameObject.activeSelf == true)
                {
                    BulldozePreview.gameObject.SetActive(false);
                }
                RoadPreviews.ForEach(previewObj =>
                {
                    if (previewObj.gameObject.activeSelf == true)
                    {
                        previewObj.gameObject.SetActive(false);
                    }
                });
                break;
        }


        if (Input.GetMouseButton(0)) // left mouse
        {
            switch(ActiveTool)
            {
                case Tool_Bulldoze:
                    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out var hit))
                    {
                        if (hit.transform.tag.Equals("Road"))
                        {
                            var hitPos = hit.point;
                            game.DeleteRoadSegment(Mathf.RoundToInt(hitPos.x), Mathf.RoundToInt(hitPos.z));
                            Destroy(hit.transform.parent.gameObject);
                        }
                    }
                    break;
                case Tool_Road:
                    var roadRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(roadRay, out var roadHit))
                    {
                        var roadPos = roadHit.point;
                        game.CreateRoad(false, Mathf.RoundToInt(roadPos.x), Mathf.RoundToInt(roadPos.z), CurrentRoadRotation, CurrentRoadType);
                    }
                    break;
            }
        } else if (Input.GetMouseButtonDown(1)) // right mouse
        {
            if (ActiveTool == Tool_Road)
            {
                CurrentRoadRotation += 1;
                if (CurrentRoadRotation >= 5)
                    CurrentRoadRotation = 0;
            }
        }
    }

}
