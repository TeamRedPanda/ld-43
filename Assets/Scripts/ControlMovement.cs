using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ControlMovement : MonoBehaviour {

	public float m_StartPoint = 1f;
	public float m_EndPoint = 34f;

	private CinemachineDollyCart m_PathMovement;

	private bool foward = true;

	// Use this for initialization
	void Start () {
		m_PathMovement = this.GetComponent<CinemachineDollyCart>();
		m_PathMovement.m_Position = m_StartPoint;
	}
	
	// Update is called once per frame
	void Update () {
		if (foward) {
			if (m_PathMovement.m_Position <= m_EndPoint) {
				m_PathMovement.m_Position += 0.1f;
				Debug.Log("Moving foward, currently at: " + m_PathMovement.m_Position);
			}
			else {
				Debug.Log("Arrived at the end point!");
				foward = false;
			}
		}
		else {
			if (m_PathMovement.m_Position >= m_StartPoint) {
				m_PathMovement.m_Position -= 0.1f;
				Debug.Log("Moving backward, currently at: " + m_PathMovement.m_Position);
			}
			else {
				Debug.Log("Arrive at the start point!");
				foward = true;
			}
		}
	}
}
