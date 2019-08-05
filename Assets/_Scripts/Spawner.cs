using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float xGap = 1f;
    [SerializeField] private float yGap = 1f;
    [SerializeField] private Block[] blockPrefabs;
    List<int[]> blocks = new List<int[]>(6);
    private StreamReader sr;
    private int x;

    void Start()
    {
        ReadFile();
        Spawn();
    }

    void ReadFile()
    {
        // using, um StreamReader Ressource automatisch wieder zurückzugeben
        using (StreamReader reader = new StreamReader($"{Application.streamingAssetsPath}/Levels/level1.txt"))
        {
            string line = "";
            // versuchen Zeile zu lesen und in line zu speichern. 
            // Wenn keine Zeile mehr vorhanden ist,
            // wird null zurückgegeben und die while-Schleife endet.
            while ((line = reader.ReadLine()) != null)
            {
                // einzelne Zahlen einer Zeile als string-Array
                string[] tokens = line.Split(' ');
                // umwandeln in int-Array mit int.Parse als Converter
                int[] blockRow = Array.ConvertAll<string, int>(tokens, int.Parse);
                // die gesamte Reihe, der blocks-Liste anhängen
                blocks.Add(blockRow);
            }
        }
    }

    private void Spawn()
    {
        for (int y = 0; y < blocks.Count; y++)
        {
            for (int x = 0; x < blocks[y].Length; x++)
            {
                int prefabIdx = blocks[y][x];
                if (prefabIdx == -1) { continue; }
                Block block = Instantiate<Block>(blockPrefabs[prefabIdx], transform);
                block.transform.localPosition = new Vector3(x * xGap, -y * yGap, 0f);
            }
        }
    }
}
