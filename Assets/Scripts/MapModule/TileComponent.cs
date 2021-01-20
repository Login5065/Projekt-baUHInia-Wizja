using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TileComponent : MonoBehaviour
{
    public Tile tile;
    public GameMap map;
    public MeshRenderer meshRendererr;

    public Material matWater;
    public Material matGrass;
    public Material matEarth;
    public Material matTemperature;
    public Gradient gradient;

    private MaterialPropertyBlock _propBlock;
    private MaterialPropertyBlock _originalPropBlock;

    public bool displayTemperature = true;
    private void Start()
    {
        displayTemperature = true;
        _propBlock = new MaterialPropertyBlock();
        _originalPropBlock = new MaterialPropertyBlock();
        meshRendererr.GetPropertyBlock(_propBlock);
        meshRendererr.GetPropertyBlock(_originalPropBlock);
    }

    public void SwitchView()
    {
        if (displayTemperature)
        {
            meshRendererr.material = matTemperature;
            float gradientValue = (float) ( (tile.tileHeat.localTemperature - MapHeat.Instance.heatData.tempMin) / ( MapHeat.Instance.heatData.tempMax - MapHeat.Instance.heatData.tempMin) );
           // Debug.Log("Gradient value" + gradientValue.ToString());
            _propBlock.SetColor("_Color", gradient.Evaluate(gradientValue));
            //matTemperature.color = gradient.Evaluate(gradientValue);
            meshRendererr.SetPropertyBlock(_propBlock);


        }
        else
        {
            UpdateTerrain();
            meshRendererr.SetPropertyBlock(_originalPropBlock);
        }

        displayTemperature = !displayTemperature;
    }


    public void UpdateTerrain()
    {
        switch (tile.TerrainType)
        {
            case Tile.TERRAIN_TYPE.WATER:
                meshRendererr.material = matWater;
                break;
            case Tile.TERRAIN_TYPE.GRASS:
                meshRendererr.material = matGrass;
                break;
            case Tile.TERRAIN_TYPE.EARTH:
                meshRendererr.material = matEarth;
                break;

        }
    }
}
