using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryEntry : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Text nameText;
    public Image image;

    private EntityDescription entity;
    private bool showTooltip = false;

    private Vector3 targetPos;

    // Sets up the UI item
    public void Init(EntityDescription e, float yPos)
    {
        entity = e;
        nameText.text = entity.name;
        image.sprite = entity.sprite;

        // Start off the side of the screen
        float width = GetComponent<RectTransform>().rect.width;
        transform.localPosition = new Vector3(width, yPos, 0);
        targetPos = new Vector3(0, yPos, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // If its off the screen, lerp to position
        if (transform.localPosition.x > 0)
		{
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, 0.5f);
		}
    }

    // Draw UI tooltip
    public void OnGUI()
    {
        if (showTooltip)
        {
            Rect pos = new Rect(Event.current.mousePosition, new Vector2(50, 50));
            GUI.Label(pos, entity.description);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        showTooltip = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        showTooltip = false;
    }
}
