using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class KeyRebinder : MonoBehaviour
{

	[Header("Input")]
	public InputActionReference actionReference; // Gán Move tại đây (VD: PlayerControls/Move)

	[Header("Binding")]
	public int compositePartIndex; // 1 = Up, 2 = Down, 3 = Left, 4 = Right

	[Header("UI")]
	public Button rebindButton;
	public TextMeshProUGUI bindingText;

	private InputAction action;
	private int bindingIndex;

	void Start()
	{
		action = actionReference.action;
		string rebinds = PlayerPrefs.GetString("Rebinds", "");
		if (!string.IsNullOrEmpty(rebinds))
		{
			action.actionMap.asset.LoadBindingOverridesFromJson(rebinds);
		}
		// Tìm composite 2D Vector đầu tiên
		int compositeIndex = action.bindings.IndexOf(b => b.isComposite && b.name == "2D Vector");
		Debug.Log($"Composite Index: {compositeIndex}");	

		
		if (compositeIndex == -1)
		{	
			
			Debug.LogError("No 2D Vector composite found!");
		
		}

		bindingIndex = compositeIndex + compositePartIndex; // Xác định part binding

		UpdateBindingDisplay();

		// Gắn sự kiện bấm nút
		rebindButton.onClick.AddListener(() => StartRebinding());
	}

	void UpdateBindingDisplay()
	{
		if (bindingText != null)
		{

			//bindingText.text = InputControlPath.ToHumanReadableString(
			//	action.bindings[bindingIndex].effectivePath,
			//	InputControlPath.HumanReadableStringOptions.UseShortNames
			//);
			string text = InputControlPath.ToHumanReadableString(
				action.bindings[bindingIndex].effectivePath,
				InputControlPath.HumanReadableStringOptions.UseShortNames
			);
			text = text.Replace("[Keyboard]", "").Replace("[Mouse]" , ""); // Loại bỏ "Keyboard/" nếu có
			bindingText.text = text;
			bindingText.fontSize = 24; // Đặt kích thước chữ nhỏ hơn nếu cần	
		}
	}

	void StartRebinding()
	{
		var action = actionReference.action;

		// Tắt Action trước khi rebind
		if (action.enabled)
			action.Disable();
		
		rebindButton.interactable = false;
		bindingText.fontSize = 24; 
		bindingText.text = "Press a key...";

		action.PerformInteractiveRebinding(bindingIndex)
			.WithTargetBinding(bindingIndex)
			.OnComplete(operation =>
			{
				operation.Dispose();
				rebindButton.interactable = true;
				PlayerPrefs.SetString("Rebinds", action.actionMap.asset.SaveBindingOverridesAsJson());
				PlayerPrefs.Save();
				UpdateBindingDisplay();
			})
			.OnCancel(operation =>
			{
				operation.Dispose();
				rebindButton.interactable = true;
				UpdateBindingDisplay();
				
			})
			.Start();
	}
}