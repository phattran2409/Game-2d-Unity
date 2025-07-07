using System;
using UnityEngine;

public class PlayerArrowIventory : MonoBehaviour
{
	public int maxArrows = 10;
	private int currentArrows;

	public event Action<int> OnArrowCountChanged;

	void Start()
	{
		currentArrows = maxArrows;
		OnArrowCountChanged?.Invoke(currentArrows);
	}

	public bool TryUseArrow()
	{
		if (currentArrows > 0)
		{
			currentArrows--;
			OnArrowCountChanged?.Invoke(currentArrows);
			return true;
		}
		return false;
	}

	public void AddArrow(int amount)
	{
		currentArrows = Mathf.Min(currentArrows + amount, maxArrows);
		OnArrowCountChanged?.Invoke(currentArrows);
	}

	public int GetCurrentArrow() => currentArrows;

}
