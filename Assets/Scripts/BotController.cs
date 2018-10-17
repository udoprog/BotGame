﻿using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class BotController : Playable
{
	[SerializeField] Transform CameraAttachment;
	[SerializeField] [Range(0f, 180)] float RotationSpeed = 90f;
	[SerializeField] [Range(0.1f, 2f)] float MovementSpeed = 1f;
	public List<Event> OnMakeActive = new List<Event>();

	/// <summary>
	/// Cameras can be rotated by using horizontal and vertical inputs.
	/// </summary>
	public override void PlayableUpdate()
	{
		Move(Input.GetAxis("Horizontal"), -Input.GetAxis("Vertical"));
	}

	/// <summary>
	/// Called when entering a controller.
	/// </summary>
	public override void Enter()
	{
		// NB: disable collider so we don't accidentally raytrace into it.
		GetComponent<Collider>().enabled = false;

		Transform CameraTransform = Camera.main.transform;
		CameraTransform.parent = CameraAttachment;
		CameraTransform.localPosition = Vector3.zero;
		CameraTransform.localRotation = Quaternion.identity;

		foreach (Event e in OnMakeActive)
		{
			StartCoroutine(e.Fire());
		}
	}

	/// <summary>
	/// Called when leaving a controller.
	/// </summary>
	public override void Leave()
	{
		GetComponent<Collider>().enabled = true;
	}

	public void Move(float h, float v)
	{
		// NB: very simple movement, do something that interacts with colission meshes.
		transform.Rotate(new Vector3(0, h * RotationSpeed * Time.deltaTime, 0), Space.World);
		transform.Translate(-new Vector3(0, 0, v * MovementSpeed * Time.deltaTime), Space.Self);
	}
}