using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    public string SwitchOff;
    public string SwitchOn;

    Text Text;
    Toggle Toggle;

    public bool IsMouse { get => Toggle.isOn; }

    void Awake()
    {
        Text = GetComponent<Text>();
        Toggle = GetComponent<Toggle>();
    }

    public void OnControlsToggle() => Text.text = Toggle.isOn ? SwitchOn : SwitchOff;
}