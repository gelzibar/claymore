using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trailHandler : MonoBehaviour {

	private Rigidbody myRigidbody;

	// Trail
	const int trailMaxSize = 10;
	public GameObject myLineRendererObject;
	private LineRenderer myLineRenderer;
	public List<Vector3> trails;
	public List<GameObject> slickTrails;
	public GameObject pSlickTrail;
	private int trailIndex, prevTrailIndex;
	private float minDistance;
	private trail_maker sTrails;
	private GameObject exhaust;


	// Use this for initialization
	void Start () {

		myRigidbody = GetComponent<Rigidbody> ();

		sTrails = GetComponent<trail_maker> ();
		myLineRenderer = GetComponentInChildren<LineRenderer> ();
		exhaust = transform.Find ("exhaust").gameObject;
		sTrails.trailEnabled = false;
		sTrails.trailToggle = false;

		trails = new List<Vector3> ();
		trailIndex = 1;
		prevTrailIndex = 0;
		minDistance = 10.0f;
		slickTrails = new List<GameObject> ();
		SetInitialLine ();
		TrailsToLinePositions ();
	}
	
	// Update is called once per frame
	void Update () {
		// Disable Butter Trails functionality.
//		UpdateLinePositions ();
//		TrailsToLinePositions ();
		
	}

	void TrailInbounds ()
	{
		int maxIndex = trailMaxSize - 1;

		if (trailIndex > maxIndex) {
			trails.RemoveAt (0);
			trails.Add (new Vector3 (exhaust.transform.position.x, exhaust.transform.position.y, exhaust.transform.position.z));

			trailIndex--;
			prevTrailIndex--;
			//trailIndex = 0;
		}
	}

	bool CompareLinePositions ()
	{
		bool returnValue = false;
		float curDistance = Vector3.Distance (myRigidbody.position, trails [prevTrailIndex]);

		if (curDistance > minDistance) {
			returnValue = true;
		}

		return returnValue;
	}

	void UpdateLinePositions ()
	{
		Vector3 curPosition = new Vector3 (exhaust.transform.position.x, exhaust.transform.position.y, exhaust.transform.position.z);

		if (sTrails.trailEnabled == false) {
			if (sTrails.trailToggle == true) {
				trails.Clear ();
				SetInitialLine ();
				trailIndex = 1;
				prevTrailIndex = 0;

				sTrails.trailToggle = false;
			}
			trails [trailIndex] = curPosition;

			if (CompareLinePositions ()) {
				// Create trail/line objects for trailIndex.
				if (trails.Capacity < trailMaxSize) {
					trails.Add (curPosition);
					LineRendererAdd (curPosition);
				} else if (trails.Count < trails.Capacity && trails.Count < trailMaxSize) {
					trails.Add (curPosition);
					LineRendererAdd (curPosition);
				}

				trailIndex++;
				prevTrailIndex++;
				TrailInbounds ();
				trails [prevTrailIndex] = curPosition;

				slickTrails.Add (Instantiate (pSlickTrail, curPosition, myRigidbody.rotation));

			}
		}
	}

	void SetInitialLine ()
	{
		Vector3 initialLine = new Vector3 (exhaust.transform.position.x, exhaust.transform.position.y, exhaust.transform.position.z);
		trails.Add (initialLine);
		trails.Add (initialLine);
		myLineRenderer.numPositions = trails.Count;
	}

	void TrailsToLinePositions ()
	{
		for (int i = 0; i < myLineRenderer.numPositions; i++) {
			myLineRenderer.SetPosition (i, trails [i]);
		}
	}

	void LineRendererAdd (Vector3 vector)
	{
		myLineRenderer.numPositions++;
		myLineRenderer.SetPosition (myLineRenderer.numPositions - 1, vector);
	}
}
