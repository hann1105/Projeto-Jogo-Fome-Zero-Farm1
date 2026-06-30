using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlantedCrop : MonoBehaviour
{
    private CropData cropData;
    private Sprite fallbackSprite;
    private SpriteRenderer spriteRenderer;
    private float plantedAt;
    private int currentStage = -1;

    public Vector3Int CellPosition { get; private set; }

    public bool IsReadyToHarvest
    {
        get { return currentStage >= cropData.StageCount - 1; }
    }

    public void Initialize(CropData data, Vector3Int cellPosition, Sprite seedSprite)
    {
        cropData = data;
        CellPosition = cellPosition;
        fallbackSprite = seedSprite;
        plantedAt = Time.time;

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = cropData.sortingOrder;

        UpdateStage(force: true);
    }

    private void Update()
    {
        UpdateStage(force: false);
    }

    public bool Harvest(Inventario inventario)
    {
        if (!IsReadyToHarvest || cropData.harvestItem == null)
        {
            return false;
        }

        int amount = Mathf.Max(1, cropData.harvestAmount);
        for (int i = 0; i < amount; i++)
        {
            inventario.Add(cropData.harvestItem);
        }

        if (cropData.bonusSeedItem != null)
        {
            int seedAmount = Mathf.Max(0, cropData.bonusSeedAmount);
            for (int i = 0; i < seedAmount; i++)
            {
                inventario.Add(cropData.bonusSeedItem);
            }
        }

        Destroy(gameObject);
        return true;
    }

    private void UpdateStage(bool force)
    {
        if (cropData == null)
        {
            return;
        }

        int stage = Mathf.FloorToInt((Time.time - plantedAt) / Mathf.Max(0.1f, cropData.secondsPerStage));
        stage = Mathf.Clamp(stage, 0, cropData.StageCount - 1);

        if (!force && stage == currentStage)
        {
            return;
        }

        currentStage = stage;
        spriteRenderer.sprite = GetSpriteForStage(stage);
    }

    private Sprite GetSpriteForStage(int stage)
    {
        if (cropData.growthStageSprites == null || cropData.growthStageSprites.Length == 0)
        {
            return fallbackSprite;
        }

        return cropData.growthStageSprites[Mathf.Clamp(stage, 0, cropData.growthStageSprites.Length - 1)];
    }
}
