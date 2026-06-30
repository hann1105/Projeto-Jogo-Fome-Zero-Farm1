using UnityEngine;

[CreateAssetMenu(fileName = "Crop Data", menuName = "Plantacao/Crop Data", order = 51)]
public class CropData : ScriptableObject
{
    public string cropName = "Crop";
    public Sprite[] growthStageSprites;
    public float secondsPerStage = 5f;
    public ItemData harvestItem;
    public int harvestAmount = 1;
    public ItemData bonusSeedItem;
    public int bonusSeedAmount = 1;
    public Vector3 plantedOffset = new Vector3(0.5f, 0.5f, 0f);
    public Vector3 plantedScale = Vector3.one;
    public int sortingOrder = 1;

    public int StageCount
    {
        get
        {
            if (growthStageSprites == null || growthStageSprites.Length == 0)
            {
                return 1;
            }

            return growthStageSprites.Length;
        }
    }
}
