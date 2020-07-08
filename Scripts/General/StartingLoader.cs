using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class StartingLoader : MonoBehaviour
{
	public FieldStorage fieldStorageSO;
	public string webAddress;
	public Image progressBar;
	public float maxDownloadingTime = 15f;
	public float minDownloadingTime = 5f;
	public string sceneName;
	private Coroutine controllCoroutine = null;
	private Coroutine downloadCoroutine = null;

	private void Awake() {
		progressBar.fillAmount = 0f;
	}

	private void OnEnable() {
		DownloadFields();
	}

	private void DownloadFields() {
		progressBar.DOFillAmount(1f, minDownloadingTime);

		if (controllCoroutine != null) {
			StopCoroutine(controllCoroutine);
		}
		controllCoroutine = StartCoroutine(ControllCoroutine());
	}

	private IEnumerator ControllCoroutine() {
		if (downloadCoroutine != null) {
			StopCoroutine(downloadCoroutine);
		}
		downloadCoroutine = StartCoroutine(DownloadCoroutine());

		float timer = 0f;

		while (timer < maxDownloadingTime || progressBar.fillAmount < 1f) {
			if (downloadCoroutine == null) {
				timer = maxDownloadingTime;
			}
			timer += Time.deltaTime;
			yield return null;
		}

		if (downloadCoroutine != null) {
			StopCoroutine(downloadCoroutine);
		}

		SceneManager.LoadScene(sceneName);
	}

	private IEnumerator DownloadCoroutine() {

		UnityWebRequest unityWebRequest = UnityWebRequest.Get(webAddress);
		yield return unityWebRequest.SendWebRequest();

		if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError) {
			Debug.Log("networkError");
		}
		else {
			fieldStorageSO.Init(unityWebRequest.downloadHandler.text);
		}

		downloadCoroutine = null;
	}
}
