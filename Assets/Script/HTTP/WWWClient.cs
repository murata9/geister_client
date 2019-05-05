using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WWWKit
{
	public class WWWClient
	{
		
		public delegate void FinishedDelegate(WWW www);
		
		public delegate void DisposedDelegate();
		
		private MonoBehaviour mMonoBehaviour;
		
		private string mUrl;
		
		private WWW mWww;
		
		private WWWForm mForm;

		private byte[] mData;
		
		private Dictionary<string, string> mHeaders;
		
		private float mTimeout;
		
		private FinishedDelegate mOnDone;
		
		private FinishedDelegate mOnFail;
		
		private DisposedDelegate mOnDisposed;
		
		private bool mDisposed;
		
		public Dictionary<string, string> Headers
		{
			set { mHeaders = value; }
			get { return mHeaders; }
		}
		
		public float Timeout
		{
			set { mTimeout = value; }
			get { return mTimeout; }
		}

		public string URL
		{
			set{ mUrl = value; }
		}

		public FinishedDelegate OnDone
		{
			set { mOnDone = value; }
		}
		
		public FinishedDelegate OnFail
		{
			set { mOnFail = value; }
		}
		
		public DisposedDelegate OnDisposed
		{
			set { mOnDisposed = value; }
		}
		
		public WWWClient(MonoBehaviour monoBehaviour)
		{
			mMonoBehaviour = monoBehaviour;
			mHeaders = new Dictionary<string, string>();
			mForm = new WWWForm();
			mTimeout = -1;
			mDisposed = false;
		}
		
		public void AddHeader(string headerName, string value)
		{
			mHeaders.Add(headerName, value);
		}

		public void AddData(byte[] data)
		{
			mData = data;
		}
		
		public void AddData(string fieldName, string value)
		{
			mForm.AddField(fieldName, value);
		}
		
		public void AddBinaryData(string fieldName, byte[] contents)
		{
			mForm.AddBinaryData(fieldName, contents);
		}
		
		public void AddBinaryData(string fieldName, byte[] contents, string fileName)
		{
			mForm.AddBinaryData(fieldName, contents, fileName);
		}
		
		public void AddBinaryData(string fieldName, byte[] contents, string fileName, string mimeType)
		{
			mForm.AddBinaryData(fieldName, contents, fileName, mimeType);
		}

		public void CleanData()
		{
			// うーん、微妙・・・
			mForm = new WWWForm ();
		}
		
		public void Request()
		{
			mMonoBehaviour.StartCoroutine(RequestCoroutine());
		}
		
		public void Dispose()
		{
			if (mWww != null && !mDisposed)
			{
				mWww.Dispose();
				Debug.Log ( "TimeOut:" + mTimeout.ToString() + "second" );
				mDisposed = true;
			}
		}
		
		private IEnumerator RequestCoroutine()
		{
			if (mForm.data.Length > 0) {
				foreach (var entry in mForm.headers) {
					mHeaders [System.Convert.ToString (entry.Key)] = System.Convert.ToString (entry.Value);
				}
				
				// POST request
				mWww = new WWW (mUrl, mForm.data, mHeaders);
			}
			else
			{
				// GET request
				mWww = new WWW(mUrl, mData, mHeaders);
			}
			
			yield return mMonoBehaviour.StartCoroutine(CheckTimeout());
			
			if (mDisposed)
			{
				if (mOnDisposed != null)
				{
					mOnDisposed();
				}
			}
			else if (System.String.IsNullOrEmpty(mWww.error))
			{
				if (mOnDone != null)
				{
					mOnDone(mWww);
				}
			}
			else
			{
				if (mOnFail != null)
				{
					mOnFail(mWww);
				}
			}
		}
		
		private IEnumerator CheckTimeout()
		{
			float startTime = Time.time;
			
			while (!mDisposed && !mWww.isDone)
			{
				if (mTimeout > 0 && (Time.time - startTime) >= mTimeout)
				{
					Dispose();
					Debug.LogError (string.Format("Request is timeout.{0}", mForm.data.ToString ()));
					break;
				}
				else
				{
					yield return null;
				}
			}
			
			yield return null;
		}
		
	}
	
}