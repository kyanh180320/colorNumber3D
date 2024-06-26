using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



public class Cube : MonoBehaviour
{
    // Start is called before the first frame update
    public Material material;
    public int ID;
    public Vector3 position;
    public bool isFill;
    public List<Cube> neighbors = new List<Cube>();

    // Phương thức để lấy các Cube kết nối

    private static readonly Vector3[] directions = new Vector3[]
   {
        Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back
   };

    // Thêm phương thức này để tìm kiếm các cube kết nối bằng raycast
    public List<Cube> GetConnectedCubesWithRaycast()
    {
        List<Cube> connectedCubes = new List<Cube>();
        Queue<Cube> queue = new Queue<Cube>();
        HashSet<Cube> visited = new HashSet<Cube>();

        queue.Enqueue(this);
        visited.Add(this);

        while (queue.Count > 0)
        {
            Cube current = queue.Dequeue();
            connectedCubes.Add(current);

            foreach (Vector3 direction in directions)
            {
                Ray ray = new Ray(current.transform.position, direction);
                if (Physics.Raycast(ray, out RaycastHit hit, 1.0f))
                {
                    Cube neighbor = hit.collider.gameObject.GetComponent<Cube>();
                    if (neighbor != null && neighbor.ID == this.ID && !visited.Contains(neighbor) && neighbor.isFill == false )
                    {
                        queue.Enqueue(neighbor);
                        visited.Add(neighbor);
                    }
                }
            }
        }

        return connectedCubes;
    }
}



