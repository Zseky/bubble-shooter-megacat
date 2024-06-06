using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public float BubbleRadius = 0.46875f;
    public float BubbleDiameter;

    private Dictionary<Vector3Int, Bubble> bubbleGrid = new Dictionary<Vector3Int, Bubble>();

    private static Vector3Int[] directions = new Vector3Int[]
    {
        new Vector3Int(1, 0, -1), new Vector3Int(-1, 0, 1),
        new Vector3Int(0, -1, 1), new Vector3Int(1, -1, 0),
        new Vector3Int(0, 1, -1), new Vector3Int(-1, 1, 0)
    };

   
    void Start()
    {
        BubbleDiameter = BubbleRadius * 2f;
    }

    public Vector3Int WorldToCell(Vector3 worldPosition)
    {
        float q = (worldPosition.x * Mathf.Sqrt(3) / 3 - worldPosition.y / 3) / BubbleRadius;
        float r = worldPosition.y * 2 / 3 / BubbleRadius;
        return HexRound(q, r);
    }

    public Vector3 GetCellCenterWorld(Vector3Int cellPosition)
    {
        float x = BubbleRadius * Mathf.Sqrt(3) * (cellPosition.x + cellPosition.z / 2f);
        float y = BubbleRadius * 3 / 2 * cellPosition.z;
        return new Vector3(x, y, 0);
    }

    private Vector3Int HexRound(float q, float r)
    {
        float x = q;
        float z = r;
        float y = -x - z;

        int rx = Mathf.RoundToInt(x);
        int ry = Mathf.RoundToInt(y);
        int rz = Mathf.RoundToInt(z);

        float x_diff = Mathf.Abs(rx - x);
        float y_diff = Mathf.Abs(ry - y);
        float z_diff = Mathf.Abs(rz - z);

        if (x_diff > y_diff && x_diff > z_diff)
        {
            rx = -ry - rz;
        }
        else if (y_diff > z_diff)
        {
            ry = -rx - rz;
        }
        else
        {
            rz = -rx - ry;
        }

        return new Vector3Int(rx, ry, rz);
    }


    public void AddBubbleToGrid(Bubble bubble, Vector3Int cellPosition)
    {
        bubble.GridPos = cellPosition;
        bubbleGrid[cellPosition] = bubble; 
    }

    public void FindAndHandleMatches(Bubble startBubble)
    {
        List<Bubble> matches = new List<Bubble>();
        FindMatches(startBubble, startBubble.ColorIndex, matches);

        if (matches.Count >= 3)
        {
            foreach (Bubble bubble in matches)
            {
                bubble.CurrentState = Bubble.State.Die;
                bubbleGrid.Remove(bubble.GridPos);
                Destroy(bubble.gameObject);
            }
        }
       
    }

    public void FindMatches(Bubble currentBubble, int targetIndex, List<Bubble> matches)
    {
        matches.Add(currentBubble);

        foreach(Vector3Int direction in directions)
        {
            Vector3Int adjPos = currentBubble.GridPos + direction;
            if (bubbleGrid.TryGetValue(adjPos, out Bubble adjacentBubble))
            {
                if(adjacentBubble.ColorIndex== targetIndex && !matches.Contains(adjacentBubble))
                    FindMatches(adjacentBubble, targetIndex,  matches);
            }
        }

        
    }
}
