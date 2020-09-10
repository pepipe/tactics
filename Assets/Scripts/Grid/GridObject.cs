using Tactics.Paths;
using UnityEngine;
using TMPro;

namespace Tactics.Grid
{
  public class GridObject : MonoBehaviour
  {
    [SerializeField]
    private bool m_IsWalkable = true;
    [SerializeField]
    private GameObject m_Visual = null;
    [SerializeField]
    private GameObject m_DebugText = null;

    public PathNode Node { get => m_PathNode; }
    private PathNode m_PathNode = null;

    private Grid<GridObject> m_Grid;

    public GridObject Init(Grid<GridObject> grid, int x, int y, bool isDebugActive)
    {
      m_Grid = grid;
      m_PathNode = new PathNode(x, y, m_IsWalkable);
      InitGridObject();
      SetDebugObjects(isDebugActive);

      return this;
    }

    public void ChangeVisual(Material mat)
    {
      if (m_Visual != null)
        m_Visual.GetComponent<MeshRenderer>().material = mat;
    }

    public void SetWalkable(bool isWalkable)
    {
      m_IsWalkable = m_PathNode.IsWalkable = isWalkable;      
    }

    public void SetDebugObjects(bool active)
    {
      if(active)
      {
        //Anchor is bottom left
        var objAnchor = m_Grid.GetWorldPosition(m_PathNode.XPos, m_PathNode.YPos);

        //draw left line
        Debug.DrawLine(objAnchor, 
                        new Vector3(objAnchor.x, objAnchor.y + m_Grid.GridCellSize, objAnchor.z),
                        Color.white, 
                        100f);
        //draw right line
        Debug.DrawLine(new Vector3(objAnchor.x + m_Grid.GridCellSize, objAnchor.y, objAnchor.z),
                        new Vector3(objAnchor.x + m_Grid.GridCellSize, objAnchor.y + m_Grid.GridCellSize, objAnchor.z),
                        Color.white,
                        100f);

        //draw bottom line
        Debug.DrawLine(new Vector3(objAnchor.x, objAnchor.y, objAnchor.z),
                        new Vector3(objAnchor.x + m_Grid.GridCellSize, objAnchor.y, objAnchor.z),
                        Color.white,
                        100f);

        //draw top line
        Debug.DrawLine(new Vector3(objAnchor.x, objAnchor.y + m_Grid.GridCellSize, objAnchor.z),
                        new Vector3(objAnchor.x + m_Grid.GridCellSize, objAnchor.y + m_Grid.GridCellSize, objAnchor.z),
                        Color.white,
                        100f);
      }

      if (m_DebugText)
      {
        m_DebugText.GetComponent<TextMeshPro>().text = m_PathNode.ToString();
        m_DebugText.SetActive(active);
      }
    }

    public override string ToString()
    {
      return m_PathNode.ToString();
    }

    private void InitGridObject()
    {
      name = "GridObject[" + m_PathNode.ToString() + "]";
      var griPos = m_Grid.GetWorldPosition(m_PathNode.XPos, m_PathNode.YPos);
      transform.position = new Vector3(griPos.x + m_Grid.GridCellSize / 2f, griPos.y + m_Grid.GridCellSize / 2f);
      //Setting the grid object at the same size of the GridCell | 1 scale = 10 cell size
      transform.localScale *= m_Grid.GridCellSize / 10f;
    }
  }
}