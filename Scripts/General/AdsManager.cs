using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour
{
	[SerializeField]
    private string gameId = "1234567";
    private static string bannerPlacementId = "bannerPlacement";
	private static string newLevelPlacementID = "newLevelVideo";
	private static string loseLevelPlacementID = "loseLevelVideo";
	[SerializeField]
    private bool testMode = true;

    private void Start() {
        Advertisement.Initialize(gameId, testMode);
        StartCoroutine(ShowBannerWhenReady());
    }

    private IEnumerator ShowBannerWhenReady() {
        while (!Advertisement.IsReady(bannerPlacementId)) {
            yield return new WaitForSeconds(0.5f);
        }
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Show(bannerPlacementId);
    }

	public static void ShowNextLevelVideo() {
		if (Advertisement.IsReady(newLevelPlacementID)) {
			Advertisement.Show(newLevelPlacementID);
		}
	}

	public static void ShowLoseLevelVideo() {
		if (Advertisement.IsReady(loseLevelPlacementID)) {
			ShowOptions options = new ShowOptions
			{
				resultCallback = LoseLevelVideoResult
			};
			Advertisement.Show(loseLevelPlacementID, options);
		}
	}

	private static void LoseLevelVideoResult(ShowResult result) {
		if (result == ShowResult.Finished) {
			Debug.Log("Video completed - Offer a reward to the player");
		}
		else if (result == ShowResult.Skipped) {
			Debug.LogWarning("Video was skipped - Do NOT reward the player");
		}
		else if (result == ShowResult.Failed) {
			Debug.LogError("Video failed to show");
		}
	}
}
