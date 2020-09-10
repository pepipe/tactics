namespace Tactics.Paths
{
  public class PathNode 
  {
    public int XPos;
    public int YPos;
    public bool IsWalkable;

    public int Weight = 0;
    public int GCost = 0;
    public int HCost;
    public int FCost;    
    public PathNode CameFromNode;    

    public PathNode(int x, int y, bool isWalkable = true)
    {
      XPos = x;
      YPos = y;
      IsWalkable = isWalkable;
    }

    public void CalculateFCost()
    {
      FCost = GCost + HCost;
    }

    public override string ToString()
    {
      return XPos + "," + YPos;
    }
  }
}