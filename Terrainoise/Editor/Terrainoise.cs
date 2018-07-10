using UnityEditor;
using UnityEngine;
using System.Collections;

// Creates an instance of a primitive depending on the option selected by the user.


public enum OPTIONS
{

    Billow ,
    Checker ,
    Cylinders ,
    Perlin ,
    RidgedMultifractal ,
    Spheres ,
    Voronoi 

}

public class Terrainoise : EditorWindow
{

    public OPTIONS op;

    public Terrain terr;

    public int Seed = 123456;
    public int OctaveCount = 3 ;
    public double Persistence = 0.5;
    public double Frequency = 1;
    public double Lacunarity = 2;
    public double Displacement = 1;
    public bool UseDis = false;
    





    [MenuItem("Terrainoise/Terrainoise")]
    static void Init()
    {
        UnityEditor.EditorWindow window = GetWindow(typeof(Terrainoise));
        window.Show();


    }

    public void OnGUI()
    {
          op = (OPTIONS)EditorGUILayout.EnumPopup("Choose Noise:", op);


        switch (op)
        {
            case OPTIONS.Billow:
                Seed = EditorGUILayout.IntField("Seed", Seed);
                OctaveCount = EditorGUILayout.IntField("Octaves", OctaveCount);
                Persistence = EditorGUILayout.DoubleField("Persistence", Persistence);
                Frequency = EditorGUILayout.DoubleField("Frequency", Frequency);
                Lacunarity = EditorGUILayout.DoubleField("Lacunarity", Lacunarity);
                break;
            case OPTIONS.Perlin:
                Seed = EditorGUILayout.IntField("Seed", Seed);
                OctaveCount = EditorGUILayout.IntField("Octaves", OctaveCount);
                Persistence = EditorGUILayout.DoubleField("Persistence", Persistence);
                Frequency = EditorGUILayout.DoubleField("Frequency", Frequency);
                Lacunarity = EditorGUILayout.DoubleField("Lacunarity", Lacunarity);
                break;
            case OPTIONS.RidgedMultifractal:
                Seed = EditorGUILayout.IntField("Seed", Seed);
                OctaveCount = EditorGUILayout.IntField("Octaves", OctaveCount);
                Frequency = EditorGUILayout.DoubleField("Frequency", Frequency);
                Lacunarity = EditorGUILayout.DoubleField("Lacunarity", Lacunarity);
                break;
            case OPTIONS.Spheres:
                Frequency = EditorGUILayout.DoubleField("Frequency", Frequency);
                break;
            case OPTIONS.Voronoi:
                Seed = EditorGUILayout.IntField("Seed", Seed);
                Frequency = EditorGUILayout.DoubleField("Frequency", Frequency);
                Displacement = EditorGUILayout.DoubleField("Displacement", Displacement);
                UseDis = EditorGUILayout.Toggle("Use Distance", UseDis);
                break;
            case OPTIONS.Cylinders:
                Frequency = EditorGUILayout.DoubleField("Frequency", Frequency);
                break;
            default:
               
                break;
        }

        

        if (GUILayout.Button("Create"))
        {
            GameObject obj = GameObject.Find("Terrain");
            if (obj != null)
            {
                terr = obj.GetComponent<Terrain>();
                Gen(op);

            }
            else
            {
                Debug.Log("Please Add Terrain Object To The Scene. Terrain Name Must Be 'Terrain'.");
                Debug.Log("PS: Uncheck 'Lightmap Static' In The Terrain Inspector For Better Performance While Editing.");
                return;
            }
        }

      



    }

    void Gen(OPTIONS op)
    {
        switch (op)
        {
            case OPTIONS.Billow:
                Bill();
                break;
            case OPTIONS.Perlin:
                Perlin();
                break;
            case OPTIONS.RidgedMultifractal:
                RidgedMultifractal();
                break;
            case OPTIONS.Spheres:
                Spheres();
                break;
            case OPTIONS.Voronoi:
                Vornoi();
                break;
            case OPTIONS.Cylinders:
                Cylinders();
                break;
            default:
                Debug.LogError("Unrecognized Option");
                break;
        }


       

    }

    public void Bill()
    {

        LibNoise.Generator.Billow _noise = new LibNoise.Generator.Billow();
        _noise.Seed = Seed;
        _noise.OctaveCount = OctaveCount;
        _noise.Persistence = Persistence;
        _noise.Frequency = Frequency;
        _noise.Lacunarity = Lacunarity;
        Heights(terr, _noise);
        
    }

  
    public void Checker()
    {

        LibNoise.Generator.Checker _noise = new LibNoise.Generator.Checker();
        Heights(terr, _noise);

    }

    public void Perlin()
    {
        LibNoise.Generator.Perlin _noise = new LibNoise.Generator.Perlin();
        _noise.Seed = Seed;
        _noise.OctaveCount = OctaveCount;
        _noise.Persistence = Persistence;
        _noise.Frequency = Frequency;
        _noise.Lacunarity = Lacunarity;
        Heights(terr, _noise);
    }

    public void RidgedMultifractal()
    {
        LibNoise.Generator.RidgedMultifractal _noise = new LibNoise.Generator.RidgedMultifractal();
        _noise.Seed = Seed;
        _noise.OctaveCount = OctaveCount;
        _noise.Frequency = Frequency;
        _noise.Lacunarity = Lacunarity;
        Heights(terr, _noise);
    }

    public void Vornoi()
    {
        LibNoise.Generator.Voronoi _noise = new LibNoise.Generator.Voronoi();
        _noise.Seed = Seed;
        _noise.Frequency = Frequency;
        _noise.Displacement = Displacement;
        _noise.UseDistance = UseDis;
        Heights(terr, _noise);
    }

    public void Spheres()
    {

        LibNoise.Generator.Spheres _noise = new LibNoise.Generator.Spheres();
       _noise.Frequency = Frequency;
        Heights(terr, _noise);
    }

    public void Cylinders()
    {
        LibNoise.Generator.Cylinders _noise = new LibNoise.Generator.Cylinders();
        _noise.Frequency = Frequency;
        Heights(terr, _noise);
    }

    public void Heights(Terrain terrain, LibNoise.ModuleBase _noise)
    {
        float[,] heights = new float[terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight];
        for (int i = 0; i < terrain.terrainData.heightmapWidth; i++)
        {
            for (int k = 0; k < terrain.terrainData.heightmapHeight; k++)
            {

                var point = new Vector3(i, k, 0f) / 100;
                float value = (float)_noise.GetValue(point);

                value = Mathf.Clamp01((value + 1) / 2f);

                heights[i, k] = value / 10f;

            }
        }

        
        terrain.terrainData.SetHeights(0, 0, heights);
        
    }
}