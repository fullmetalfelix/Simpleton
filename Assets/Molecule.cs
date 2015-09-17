using UnityEngine;
using System.Collections;
using System.Net;
using System.Collections.Generic;

public class Molecule : MonoBehaviour
{

	/// <summary>
	/// Interaction centers for other molecules. Set in inspector.
	/// </summary>
	public Transform cLeft, cRight, cBottom;

	public float armLength = 3.8f;
	public float pinDistance = 0.1f;

	public Molecule left, right, bottom;


	public Rigidbody2D physics;


	public SpringJoint2D jointLeft, jointRight, jointBottom;


	public bool upward = false;

	[HideInInspector]
	public Vector2 pinRight, pinLeft, pinBottom;
	[HideInInspector]
	public Vector2 pinRightW, pinLeftW, pinBottomW;
	[HideInInspector]
	public Vector2 plugRight, plugLeft, plugBottom;

	public void Init ()
	{

		//displacement vector for right
		Vector2 e = new Vector2 (Mathf.Cos (Mathf.PI / 3), -Mathf.Sin (Mathf.PI / 3));

		//pin positions in local space
		pinRight = new Vector2 (Mathf.Sqrt (3) * 0.5f, Mathf.Sin (Mathf.PI / 6));
		pinRight *= armLength * 0.5f;
		plugRight = pinRight - e * pinDistance;
		pinRight += e * pinDistance;

		e = new Vector2 (-Mathf.Cos (Mathf.PI / 3), -Mathf.Sin (Mathf.PI / 3));
		pinLeft = new Vector2 (-Mathf.Sqrt (3) * 0.5f, Mathf.Sin (Mathf.PI / 6));
		pinLeft *= armLength * 0.5f;
		plugLeft = pinLeft + e * pinDistance;
		pinLeft -= e * pinDistance;

		e = new Vector2 (1, 0);
		pinBottom = new Vector2 (0, -1);
		pinBottom *= armLength * 0.5f;
		plugBottom = pinBottom + e * pinDistance;
		pinBottom -= e * pinDistance;

		if (upward) {

			Vector2 tmp = pinBottom;
			pinBottom = pinRight;
			pinRight = tmp;

			tmp = pinLeft;
			pinLeft = pinRight;
			pinRight = tmp;

		}



		//convert to world space
		pinRightW = transform.TransformPoint ((Vector3)pinRight);
		pinLeftW = transform.TransformPoint ((Vector3)pinLeft);
		pinBottomW = transform.TransformPoint ((Vector3)pinBottom);

		Debug.DrawLine (transform.position, (Vector3)pinRightW, Color.red, 20.0f);
		Debug.DrawLine (transform.position, (Vector3)pinLeftW, Color.green, 20.0f);
		Debug.DrawLine (transform.position, (Vector3)pinBottomW, Color.blue, 20.0f);

	}



	public void Connect (Molecule other, int channel)
	{
		//seems to work only if this is downward and other is upward
		if (upward || !other.upward) {

			Debug.LogError ("wrong connection!");
			return;
		}


		// channel code 0 = R, 1 = L, 2 = B
		if (channel == 0) {
			//right --> right of other
			jointRight.connectedBody = other.physics;
			jointRight.anchor = pinRight;

			jointRight.connectedAnchor = plugLeft;
			jointRight.enabled = true;

			other.jointRight.connectedBody = physics;
			other.jointRight.anchor = other.pinRight;

			other.jointRight.connectedAnchor = plugRight;
			other.jointRight.enabled = true;


		}
		if (channel == 1) {
			//right --> right of other
			jointLeft.connectedBody = other.physics;
			jointLeft.anchor = pinLeft;

			jointLeft.connectedAnchor = plugBottom;
			jointLeft.enabled = true;

			other.jointLeft.connectedBody = physics;
			other.jointLeft.anchor = other.pinLeft;

			other.jointLeft.connectedAnchor = plugLeft;
			other.jointLeft.enabled = true;

		}
		if (channel == 2) {
			//right --> right of other
			jointBottom.connectedBody = other.physics;
			jointBottom.anchor = pinBottom;

			jointBottom.connectedAnchor = plugRight;
			jointBottom.enabled = true;

			other.jointBottom.connectedBody = physics;
			other.jointBottom.anchor = other.pinBottom;

			other.jointBottom.connectedAnchor = plugBottom;
			other.jointBottom.enabled = true;

		}


	}


	public void Restrain ()
	{
		List<SpringJoint2D> joints = new List<SpringJoint2D> ();
		joints.Add (jointLeft);
		joints.Add (jointRight);
		joints.Add (jointBottom);

		foreach (SpringJoint2D item in joints) {
			
			if (!item.enabled) {

				item.anchor = Vector2.zero;
				item.connectedAnchor = transform.position;
				item.connectedBody = null;
				item.enabled = true;
			}

		}

	}


}
