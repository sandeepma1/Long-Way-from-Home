public class UiFurnace : DragDropBase
{
    protected override void Start()
    {
        base.Start();
        InitUiSlots();
    }

    private void InitUiSlots()
    {
        for (int i = 0; i < uiSlots.Length; i++)
        {
            uiSlots[i].OnSlotDrop += OnSlotDrop;
            uiSlots[i].id = i;
            uiSlots[i].name = "UiSlot" + i.ToString();
        }
    }
}