using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonMenuNavigation : MonoBehaviour
{
    [SerializeField] private Button[] buttonsArray;
    private int selectedIndex = 0;
    void Start()
    {
        SelectButton(selectedIndex);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            this.selectedIndex = (this.selectedIndex + 1) % this.buttonsArray.Length;
            SelectButton(this.selectedIndex);
        }
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            this.selectedIndex = (this.selectedIndex - 1 + this.buttonsArray.Length) % this.buttonsArray.Length;
            SelectButton(this.selectedIndex);
        }
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            this.buttonsArray[selectedIndex].onClick.Invoke();
        }
    }

    void SelectButton(int index)
    {
        EventSystem.current.SetSelectedGameObject(this.buttonsArray[this.selectedIndex].gameObject);
    }
}
