using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ControlMovement : MonoBehaviour {

	public GameObject ObjectPath;

	private CinemachineDollyCart m_PathMovement;
	private CinemachineSmoothPath m_SmoothPath;

	private bool foward = true;

	// Use this for initialization
	void Start () {
		m_PathMovement = this.GetComponent<CinemachineDollyCart>();
		m_SmoothPath =  ObjectPath.GetComponent<CinemachineSmoothPath>();

		m_PathMovement.m_PositionUnits = CinemachinePathBase.PositionUnits.PathUnits;
		m_PathMovement.m_Position = m_SmoothPath.MinPos;
	}
	
	// Update is called once per frame
	void Update () {
		if (foward) {
			if (m_PathMovement.m_Position <= m_SmoothPath.MaxPos - 0.01f ) {
				m_PathMovement.m_Position += 0.01f;
			}
			else {
				foward = false;
			}
		}
		else {
			if (m_PathMovement.m_Position >= m_SmoothPath.MinPos + 0.01f) {
				m_PathMovement.m_Position -= 0.01f;
			}
			else {
				foward = true;
			}
		}
	}
}
