using System.Collections.Generic;
using UnityEngine;

public class GerenciadorDePlantacao : MonoBehaviour
{
    private readonly Dictionary<Vector3Int, PlantedCrop> crops = new Dictionary<Vector3Int, PlantedCrop>();

    public bool HasCrop(Vector3Int position)
    {
        return crops.ContainsKey(position);
    }

    public bool TryPlant(Vector3Int position, CropData cropData, Sprite seedSprite)
    {
        if (cropData == null || HasCrop(position))
        {
            return false;
        }

        Vector3 worldPosition = GetPlantWorldPosition(position, cropData);
        GameObject cropObject = new GameObject(cropData.cropName);
        cropObject.transform.position = worldPosition;
        cropObject.transform.localScale = cropData.plantedScale;
        cropObject.transform.SetParent(transform);

        PlantedCrop crop = cropObject.AddComponent<PlantedCrop>();
        crop.Initialize(cropData, position, seedSprite);

        crops.Add(position, crop);
        return true;
    }

    public bool TryHarvest(Vector3Int position, Inventario inventario)
    {
        if (!crops.TryGetValue(position, out PlantedCrop crop))
        {
            return false;
        }

        if (!crop.Harvest(inventario))
        {
            return false;
        }

        crops.Remove(position);
        return true;
    }

    private Vector3 GetPlantWorldPosition(Vector3Int position, CropData cropData)
    {
        if (GerenciadorJogo.instance != null && GerenciadorJogo.instance.gerenciadorDeBlocos != null)
        {
            return GerenciadorJogo.instance.gerenciadorDeBlocos.GetCellOriginWorld(position) + cropData.plantedOffset;
        }

        return position + cropData.plantedOffset;
    }
}
