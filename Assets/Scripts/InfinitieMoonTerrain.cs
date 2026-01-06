using UnityEngine;
using System.Collections.Generic;

public class InfiniteMoonTerrain : MonoBehaviour
{
    [Header("Chunk Settings")]
    public GameObject terrainPrefab;
    public float chunkSize = 50f;
    public int viewDistance = 3;

    [Header("Variation Settings")]
    public bool randomRotation = true;
    public bool randomScale = false;
    public float scaleMin = 0.9f;
    public float scaleMax = 1.1f;
    public int seed = 0;

    [Header("Rock Decoration")]
    public GameObject[] rockPrefabs;
    public int rocksPerChunk = 5;
    public float rockScaleMin = 0.5f;
    public float rockScaleMax = 2f;

    private Transform viewer;
    private Vector2 viewerPosition;
    private Vector2 viewerChunkCoord;
    private Dictionary<Vector2, TerrainChunk> terrainChunks = new Dictionary<Vector2, TerrainChunk>();
    private List<Vector2> chunksToRemove = new List<Vector2>();

    void Start()
    {
        viewer = FindFirstObjectByType<MoonExplorer>()?.transform;
        if (viewer == null)
        {
            Debug.LogError("InfiniteMoonTerrain: MoonExplorer not found in scene!");
            enabled = false;
            return;
        }

        if (terrainPrefab == null)
        {
            Debug.LogError("InfiniteMoonTerrain: Terrain prefab not assigned. Please assign it in the Inspector.");
            enabled = false;
            return;
        }

        MeshFilter mf = terrainPrefab.GetComponentInChildren<MeshFilter>();
        if (mf != null && mf.sharedMesh != null)
        {
            Vector3 meshSize = mf.sharedMesh.bounds.size;
            Vector3 scale = terrainPrefab.transform.localScale;
            float detectedSize = Mathf.Max(meshSize.x * scale.x, meshSize.z * scale.z);

            if (Mathf.Abs(detectedSize - chunkSize) > 1f)
            {
                Debug.Log($"InfiniteMoonTerrain: Auto-adjusting Chunk Size from {chunkSize} to {detectedSize} based on prefab mesh bounds.");
                chunkSize = detectedSize;
            }
        }

        UpdateVisibleChunks();

        AdjustPlayerHeight();
    }

    void Update()
    {
        if (viewer == null) return;

        viewerPosition = new Vector2(viewer.position.x, viewer.position.z);
        Vector2 currentChunkCoord = new Vector2(
            Mathf.Floor(viewerPosition.x / chunkSize),
            Mathf.Floor(viewerPosition.y / chunkSize)
        );

        if (currentChunkCoord != viewerChunkCoord)
        {
            viewerChunkCoord = currentChunkCoord;
            UpdateVisibleChunks();
        }
    }

    void UpdateVisibleChunks()
    {
        HashSet<Vector2> currentChunks = new HashSet<Vector2>(terrainChunks.Keys);

        HashSet<Vector2> requiredChunks = new HashSet<Vector2>();
        for (int yOffset = -viewDistance; yOffset <= viewDistance; yOffset++)
        {
            for (int xOffset = -viewDistance; xOffset <= viewDistance; xOffset++)
            {
                Vector2 chunkCoord = new Vector2(
                    viewerChunkCoord.x + xOffset,
                    viewerChunkCoord.y + yOffset
                );
                requiredChunks.Add(chunkCoord);
            }
        }

        chunksToRemove.Clear();
        foreach (var coord in currentChunks)
        {
            if (!requiredChunks.Contains(coord))
            {
                chunksToRemove.Add(coord);
            }
        }

        foreach (var coord in chunksToRemove)
        {
            terrainChunks[coord].Destroy();
            terrainChunks.Remove(coord);
        }

        foreach (var chunkCoord in requiredChunks)
        {
            if (!terrainChunks.ContainsKey(chunkCoord))
            {
                terrainChunks.Add(chunkCoord, new TerrainChunk(chunkCoord, chunkSize, transform, terrainPrefab, this));
            }
        }
    }

    void AdjustPlayerHeight()
    {
        if (viewer == null) return;

        Vector3 rayOrigin = new Vector3(viewer.position.x, 500f, viewer.position.z);
        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, 1000f))
        {
            viewer.position = hit.point + Vector3.up * 2f;
            Debug.Log($"InfiniteMoonTerrain: Adjusted player height to {viewer.position.y} (Ground at {hit.point.y})");
        }
        else
        {
            viewer.position = new Vector3(viewer.position.x, 50f, viewer.position.z);
            Debug.Log("InfiniteMoonTerrain: Ground not found below player, moving to height 50f");
        }
    }

    public System.Random GetChunkRandom(Vector2 chunkCoord)
    {
        return new System.Random((int)(chunkCoord.x * 10000 + chunkCoord.y + seed));
    }
}

public class TerrainChunk
{
    private GameObject chunkObject;
    private List<GameObject> decorations = new List<GameObject>();
    private Vector2 position;

    public TerrainChunk(Vector2 coord, float size, Transform parent, GameObject terrainPrefab, InfiniteMoonTerrain generator)
    {
        position = coord * size;
        System.Random rand = generator.GetChunkRandom(coord);

        chunkObject = new GameObject($"Chunk_{coord.x}_{coord.y}");
        chunkObject.transform.parent = parent;
        chunkObject.transform.position = new Vector3(position.x, 0, position.y);

        if (terrainPrefab != null)
        {
            GameObject terrainInstance = Object.Instantiate(terrainPrefab, chunkObject.transform);
            terrainInstance.transform.localPosition = Vector3.zero;

            if (generator.randomRotation)
            {
                int rotations = rand.Next(0, 4);
                terrainInstance.transform.localRotation = Quaternion.Euler(0, rotations * 90f, 0);
            }

            if (generator.randomScale)
            {
                float scale = Mathf.Lerp(generator.scaleMin, generator.scaleMax, (float)rand.NextDouble());
                terrainInstance.transform.localScale = new Vector3(scale, 1f, scale);
            }
        }

        if (generator.rockPrefabs != null && generator.rockPrefabs.Length > 0 && terrainPrefab != null)
        {
            for (int i = 0; i < generator.rocksPerChunk; i++)
            {
                Vector3 localPos = new Vector3(
                    (float)rand.NextDouble() * size - size * 0.5f,
                    0,
                    (float)rand.NextDouble() * size - size * 0.5f
                );

                RaycastHit hit;
                Vector3 worldPos = chunkObject.transform.position + localPos + Vector3.up * 100f;
                if (Physics.Raycast(worldPos, Vector3.down, out hit, 200f))
                {
                    localPos.y = hit.point.y - chunkObject.transform.position.y;
                }

                GameObject rockPrefab = generator.rockPrefabs[rand.Next(0, generator.rockPrefabs.Length)];
                GameObject rock = Object.Instantiate(rockPrefab, chunkObject.transform);
                rock.transform.position = chunkObject.transform.position + localPos;
                rock.transform.rotation = Quaternion.Euler(
                    (float)rand.NextDouble() * 30f - 15f,
                    (float)rand.NextDouble() * 360f,
                    (float)rand.NextDouble() * 30f - 15f
                );

                float rockScale = Mathf.Lerp(generator.rockScaleMin, generator.rockScaleMax, (float)rand.NextDouble());
                rock.transform.localScale = Vector3.one * rockScale;

                decorations.Add(rock);
            }
        }
    }

    public void Destroy()
    {
        Object.Destroy(chunkObject);
    }
}