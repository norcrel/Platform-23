using UnityEngine;
using System.IO;
using System;

public class TelemetryManager : MonoBehaviour {
	private const string fileName = "telemetry";
	private StreamWriter m_file;

	private static TelemetryManager _instance;
	public static TelemetryManager Instance { get { return _instance; } }
	// Use this for initialization
	void Awake () {
		_instance = this;
	}

	public void StartNewSession()
	{
		if (m_file != null)
		{
			m_file.Close();
		}
		TimeSpan t = DateTime.UtcNow - EPOCH;
		int secondsSinceEpoch = (int)t.TotalSeconds;
		string path = fileName + "-" + secondsSinceEpoch + ".txt";
		if (File.Exists(path))
		{
			int dupe = 0;
			while (File.Exists(path + "("+dupe+")"))
			{
				dupe++;
			}
			path += "("+dupe+")";
		}
		
		m_file = File.CreateText(path);
		m_file.WriteLine("timestamp,data");

		Debug.Log ("Telemetry written to: "+path);
	}

	readonly DateTime EPOCH = new DateTime(1970, 1, 1);
	public void LogEvent(string data)
	{
		if (m_file != null)
		{
			TimeSpan t = DateTime.UtcNow - EPOCH;
			int secondsSinceEpoch = (int)t.TotalSeconds;
			m_file.WriteLine(secondsSinceEpoch + "," + data);
		}
		else
		{
			Debug.Log ("No Session, Ignored data " + data);
		}
	}

	public void EndSession()
	{
		if (m_file != null)
		{
			m_file.Close();
			m_file = null;
		}
	}


	void OnDestroy()
	{
		if (m_file != null)
		{
			m_file.Close();
		}
	}
}
