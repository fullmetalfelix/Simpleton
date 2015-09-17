using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Logic : MonoBehaviour
{



	public GameObject molecule;


	protected Molecule[,] molecules;
	protected Molecule[,] molecules2;


	public bool inited = false;

	public int gridSize = 9;

	// Use this for initialization
	void Start ()
	{

		Vector3 p = Vector3.zero;

		molecules = new Molecule[10, 10];
		molecules2 = new Molecule[10, 10];


		List<Molecule> allMols = new List<Molecule> ();

		List<Molecule> firstdowns = new List<Molecule> ();



		for (int j = 0; j < gridSize / 2; j++) {
			
			float dy = -j * 2 * (3.8f + 1.8f);

			// make a line of downward
			List<Molecule> downs = new List<Molecule> ();
			for (int i = 0; i < gridSize; i++) {
				p.x = 2 * i * 3.11769f;
				p.y = dy;
				GameObject g = (GameObject)Instantiate (molecule, p, Quaternion.identity);
				Molecule m = g.GetComponent<Molecule> ();
				m.Init ();
				downs.Add (m);
			}
			allMols.AddRange (downs);

			// make a line of downward
			List<Molecule> ups = new List<Molecule> ();
			for (int i = 0; i < gridSize - 1; i++) {
				p.x = 2 * i * 3.11769f + 3.11769f;
				p.y = 1.8f + dy; //j * -2.7f
				GameObject g = (GameObject)Instantiate (molecule, p, Quaternion.AngleAxis (60, Vector3.forward));
				Molecule m = g.GetComponent<Molecule> ();
				m.upward = true;
				m.Init ();
				ups.Add (m);

				if (firstdowns.Count > 0)
					firstdowns [i].Connect (m, 2);
				

			}
			allMols.AddRange (ups);

			// connect them
			for (int i = 0; i < gridSize - 1; i++) {

				downs [i].Connect (ups [i], 0);
				downs [i + 1].Connect (ups [i], 1);

			}

			ups.Clear ();//remake the ups in the lower row
			for (int i = 0; i < gridSize; i++) {
				
				p.x = 2 * i * 3.11769f;
				p.y = -3.8f + dy; //j * -2.7f
				GameObject g = (GameObject)Instantiate (molecule, p, Quaternion.AngleAxis (60, Vector3.forward));
				Molecule m = g.GetComponent<Molecule> ();
				m.upward = true;
				m.Init ();
				ups.Add (m);
				//connect with downs
				downs [i].Connect (m, 2);
			}
			allMols.AddRange (ups);

			downs.Clear (); //make more downs
			for (int i = 0; i < gridSize - 1; i++) {
				p.x = 2 * i * 3.11769f + 3.11769f;
				p.y = -3.8f - 1.8f + dy; //j * -2.7f
				GameObject g = (GameObject)Instantiate (molecule, p, Quaternion.identity);
				Molecule m = g.GetComponent<Molecule> ();
				m.Init ();
				downs.Add (m);
				m.Connect (ups [i], 1);
				if (i < 9)
					m.Connect (ups [i + 1], 0);
			
			}
			allMols.AddRange (downs);

			firstdowns.Clear ();
			firstdowns.AddRange (downs);

		}


		foreach (Molecule item in allMols) {

			item.Restrain ();
			item.physics.isKinematic = false;
			p = item.transform.position;
			p += 0.8f * (Vector3)Random.insideUnitCircle;
			item.transform.position = p;

		}

		/*
		for (int j = 0; j < gridSize; j++) {			
			for (int i = 0; i < gridSize; i++) {
				p.x = 2 * (i * 3.11769f + (j % 2) * 1.55885f);
				p.y = 2 * j * (-2.7f);
				GameObject g = (GameObject)Instantiate (molecule, p, Quaternion.identity);
				molecules [i, j] = g.GetComponent<Molecule> ();

				p.x = 2 * ((i - (j % 2)) * 3.11769f + (j % 2) * 1.55885f) + 3.11769f;
				p.y = 2 * j * (-2.7f) + 1.8f;
				g = (GameObject)Instantiate (molecule, p, Quaternion.AngleAxis (60, Vector3.forward));

				molecules2 [i, j] = g.GetComponent<Molecule> ();
				molecules2 [i, j].upward = true;


				molecules [i, j].right = molecules2 [i, j];
				molecules2 [i, j].left = molecules [i, j];


				/*
				if (i > 0)
					molecules [i - 1, j].right = molecules [i, j];
				if (j > 0)
					molecules [i, j - 1].bottom = molecules [i, j];
				* /

			}
		}
*/
		inited = true;

	}

	void StartTEST ()
	{

		Vector3 p = Vector3.zero;


		GameObject g = (GameObject)Instantiate (molecule, p, Quaternion.identity);
		Molecule m1 = g.GetComponent<Molecule> ();
		m1.Init ();


		p.x = 3.1f;
		p.y = 1.8f;
		g = (GameObject)Instantiate (molecule, p, Quaternion.AngleAxis (60, Vector3.forward));
		Molecule m2 = g.GetComponent<Molecule> ();
		m2.upward = true;
		m2.Init ();

		m1.Connect (m2, 0);


		p.x = 2 * 3.1f;
		p.y = 0;
		g = (GameObject)Instantiate (molecule, p, Quaternion.identity);
		Molecule m3 = g.GetComponent<Molecule> ();
		m3.Init ();

		m3.Connect (m2, 1);

		p.x = 0;
		p.y = -3.8f;
		g = (GameObject)Instantiate (molecule, p, Quaternion.AngleAxis (60, Vector3.forward));
		Molecule m4 = g.GetComponent<Molecule> ();
		m4.upward = true;
		m4.Init ();

		m1.Connect (m4, 2);

		inited = true;

	}



	void FixedUpdate ()
	{

		if (!inited)
			return;

		Vector3 dist;

		//foreach (Molecule item in molecules) {
		
		//if (item.right != null) {

		//	dist = item.cRight.position - item.right.cLeft.position;

		//	}

		//}

	}

}
