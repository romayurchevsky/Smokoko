using Scripts.CommonCode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    private void Awake()
    {
		DependencyStorage.Joystick = this;
    }

    private void OnEnable()
	{
		background.gameObject.SetActive(false);
		LevelStartReaction();
	}

	private void OnDisable()
	{
		GameManager.LevelStartAction -= LevelStartReaction;
	}

	protected override void Start()
	{
		base.Start();
		base.Clear();
	}

	public override void OnPointerDown(PointerEventData _eventData)
	{
		PointerDownAction?.Invoke();
		background.anchoredPosition = ScreenPointToAnchoredPosition(_eventData.position);
		background.gameObject.SetActive(true);
		base.OnPointerDown(_eventData);
	}

	public override void OnPointerUp(PointerEventData _eventData)
	{
		PointerUpAction?.Invoke();
		background.gameObject.SetActive(false);
		base.OnPointerUp(_eventData);
	}

	private void LevelStartReaction()
	{
		base.Clear();
	}
}