using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Ojanck.AudioSetup
{
    [AddComponentMenu(" Ojanck/Audio Manager")]
    public class AudioManager : MonoBehaviour
    {
        public const string Addressables_BGM = "Audios/BGM";
        public const string Addressables_SFX = "Audios/SFX";

        [Header("Audio Sources")]
        [SerializeField] private AudioSource audioSourceBGM;
        [SerializeField] private AudioSource audioSourceSFX;

        [Header("Settings")]
        [SerializeField] private bool loadResourcesOnAwake;
        [SerializeField] private bool loadAddressablesOnAwake;
        [SerializeField][Range(0, 100)] private int volumeBGM = 100;
        [SerializeField][Range(0, 100)] private int volumeSFX = 100;
        private float currentAudioPowerBGM = 1;
        private float currentAudioPowerSFX = 1;
        private const string BGMVolume = "Ojanck_BGMVolume";
        private const string SFXVolume = "Ojanck_SFXVolume";

        [SerializeField] private string resourcesLoadPath = "Audios";
        [SerializeField] private AudioObject[] audioResourcesBGM;
        [SerializeField] private AudioObject[] audioResourcesSFX;

        [SerializeField] private string addressablesFolderPath = "Assets/Addressables/Audios";
        [SerializeField] private string[] audioNamesBGM;
        [SerializeField] private string[] audioNamesSFX;

        [SerializeField] private AudioObject[] audioAddressablesBGM;
        [SerializeField] private AudioObject[] audioAddressablesSFX;

        private Dictionary<string, AudioObject> dictBGM = new Dictionary<string, AudioObject>();
        private Dictionary<string, AudioObject> dictSFX = new Dictionary<string, AudioObject>();

        private async void Awake()
        {
            RefreshDictionaries();

            if (loadResourcesOnAwake) {
                ResourcesLoadAudios();
                RefreshDictionaries();
            }

            if (loadAddressablesOnAwake) {
                await LoadAddressablesBGM();
                await LoadAddressablesSFX();

                RefreshDictionaries();
            }
        }

        private int _volumeBGM;
        private int _volumeSFX;
        private void Update()
        {
            if (_volumeBGM != volumeBGM) {
                SetVolumeBGM(volumeBGM / 100f);
                _volumeBGM = volumeBGM;
            }

            if (_volumeSFX != volumeSFX) {
                SetVolumeSFX(volumeSFX / 100f);
                _volumeSFX = volumeSFX;
            }
        }

        public void PlayBGM(string id)
        {
            var hasAudio = dictBGM.ContainsKey(id);
            if (hasAudio == false) {
                Debug.LogError($"No Audio with the ID: {id}");
                return;
            }

            if (audioSourceBGM.isPlaying)
                StopBGM();

            var audio = dictBGM[id];
            currentAudioPowerBGM = audio.AudioPower;

            var current_volume = PlayerPrefs.GetFloat(BGMVolume, 1);
            SetVolumeBGM(current_volume);

            audioSourceBGM.clip = audio.AudioClip;
            audioSourceBGM.Play();
        }

        public void SetVolumeBGM(float volume)
        {
            audioSourceBGM.volume = currentAudioPowerBGM * volume;
            PlayerPrefs.SetFloat(BGMVolume, volume);
        }

        public void StopBGM()
        {
            audioSourceBGM.Stop();
        }

        public void PlaySFX(string id)
        {
            var hasAudio = dictSFX.ContainsKey(id);
            if (hasAudio == false) {
                Debug.LogError($"No Audio with the ID: {id}");
                return;
            }

            if (audioSourceSFX.isPlaying)
                audioSourceSFX.Stop();

            var audio = dictSFX[id];
            currentAudioPowerSFX = audio.AudioPower;

            var current_volume = PlayerPrefs.GetFloat(SFXVolume, 1);
            SetVolumeSFX(current_volume);

            audioSourceSFX.clip = audio.AudioClip;
            audioSourceSFX.Play();
        }

        public void SetVolumeSFX(float volume)
        {
            audioSourceSFX.volume = currentAudioPowerSFX * volume;
            PlayerPrefs.SetFloat(SFXVolume, volume);
        }

        public void RefreshDictionaries()
        {
            try {
                var bgmList = new List<AudioObject>();
                var sfxList = new List<AudioObject>();

                if (audioResourcesBGM != null)
                    bgmList.AddRange(audioResourcesBGM);
                if (audioAddressablesBGM != null)
                    bgmList.AddRange(audioAddressablesBGM);
                
                if (audioResourcesSFX != null)
                    sfxList.AddRange(audioResourcesSFX);
                if (audioAddressablesSFX != null)
                    sfxList.AddRange(audioAddressablesSFX);
                
                dictBGM = bgmList.ToDictionary(x => x.AudioID, x => x);
                dictSFX = sfxList.ToDictionary(x => x.AudioID, x => x);
            } catch (Exception e) {
                Debug.LogError(e.Message);
            }
        }

#region RESOURCES
        /// <summary>
        /// Load all AudioObjects from the Resources.
        /// </summary>
        public void ResourcesLoadAudios()
        {
            audioResourcesBGM = Resources.LoadAll<AudioObject>($"{resourcesLoadPath}/BGM");
            audioResourcesSFX = Resources.LoadAll<AudioObject>($"{resourcesLoadPath}/SFX");
        }
#endregion

#region ADDRESSABLES
        private float loadProgressBGM;
        private float loadProgressSFX;
        private static float downloadProgressBGM;
        private static float downloadProgressSFX;

        private static bool isDownloadingBGM;
        private static bool isDownloadingSFX;
        public static bool IsDownloadingBGM => isDownloadingBGM;
        public static bool IsDownloadingSFX => isDownloadingSFX;

        /// <summary>
        /// Get the current BGM load progress.
        /// </summary>
        public float GetLoadProgressBGM() => loadProgressBGM;

        /// <summary>
        /// Get the current SFX load progress.
        /// </summary>
        public float GetLoadProgressSFX() => loadProgressSFX;

        /// <summary>
        /// Get the current BGM download progress.
        /// </summary>
        /// <returns>float: 0f if no download in progress.</returns>
        public static float GetDownloadProgressBGM() => downloadProgressBGM;

        /// <summary>
        /// Get the current SFX download progress.
        /// </summary>
        /// <returns>float: 0f if no download in progress.</returns>
        public static float GetDownloadProgressSFX() => downloadProgressSFX;

        /// <summary>
        /// Get the current BGM total download size.
        /// </summary>
        /// <returns>float: 0f if no download available.</returns>
        public async Task<float> GetDownloadSizeBGM() => await GetDownloadSize(Addressables_BGM);

        /// <summary>
        /// Get the current SFX total download size.
        /// </summary>
        /// <returns>float: 0f if no download available.</returns>
        public async Task<float> GetDownloadSizeSFX() => await GetDownloadSize(Addressables_SFX);

        private async static Task<float> GetDownloadSize(string key)
        {
            var sizeRequest = Addressables.GetDownloadSizeAsync(key);
            var size = await sizeRequest.Task;
            return size / 1024f / 1024f;
        }

    #region DOWNLOAD TASKS
        /// <summary>
        /// Download BGM Addressables independently.
        /// </summary>
        public async static Task DownloadAddressablesBGM(IEnumerable<string> audioNamesBGM)
        {
            if (isDownloadingBGM) {
                Debug.LogError("Currently Downloading BGM!");
                return;
            }

            Debug.Log("Downloading BGM...");

            isDownloadingBGM = true;
            var needDownload = new List<string>();
            
            foreach (var bgmName in audioNamesBGM) {
                var bgmKey = $"{Addressables_BGM}/{bgmName}.asset";
                var bgmSize = await GetDownloadSize(bgmKey);
                var isDownloading = bgmSize > 0;

                if (isDownloading)
                    needDownload.Add(bgmName);
            }

            foreach (var bgmName in needDownload) {
                var bgmKey = $"{Addressables_BGM}/{bgmName}.asset";
                await TryDownloadBGM(bgmKey);
            }

            isDownloadingBGM = false;

            async Task<bool> TryDownloadBGM(string bgmKey) {
                try {
                    var prevTotalDownloadProgress = downloadProgressBGM;
                    var bgmSize = await GetDownloadSize(bgmKey);
                    var bgmRequest = Addressables.DownloadDependenciesAsync(bgmKey);

                    while (bgmRequest.IsDone == false) {
                        var downloadProgress = bgmRequest.GetDownloadStatus().DownloadedBytes / 1024f / 1024f;
                        downloadProgressBGM += downloadProgress;

                        await Task.Yield();
                    }

                    downloadProgressBGM = prevTotalDownloadProgress + bgmSize;
                    return true;
                } catch (Exception e) {
                    Debug.LogError($"Error Downloading {bgmKey}...\nReason:\n{e.Message}");
                    return false;
                }
            }
        }

        /// <summary>
        /// Download SFX Addressables independently.
        /// </summary>
        public static async Task DownloadAddressablesSFX(IEnumerable<string> audioNamesSFX)
        {
            if (isDownloadingSFX) {
                Debug.LogError("Currently Downloading SFX!");
                return;
            }

            Debug.Log("Loading SFX...");
            isDownloadingSFX = true;
            var needDownload = new List<string>();
            
            foreach (var sfxName in audioNamesSFX) {
                var sfxKey = $"{Addressables_SFX}/{sfxName}.asset";
                var sfxSize = await GetDownloadSize(sfxKey);
                var isDownloading = sfxSize > 0;

                if (isDownloading)
                    needDownload.Add(sfxName);
            }

            foreach (var sfxName in needDownload) {
                var sfxKey = $"{Addressables_SFX}/{sfxName}.asset";
                await TryDownloadSFX(sfxKey);
            }

            isDownloadingSFX = false;

            async Task<bool> TryDownloadSFX(string sfxKey) {
                try {
                    var prevTotalDownloadProgress = downloadProgressSFX;
                    var sfxSize = await GetDownloadSize(sfxKey);
                    var sfxRequest = Addressables.DownloadDependenciesAsync(sfxKey);

                    while (sfxRequest.IsDone == false) {
                        var downloadProgress = sfxRequest.GetDownloadStatus().DownloadedBytes / 1024f / 1024f;
                        downloadProgressSFX += downloadProgress;

                        await Task.Yield();
                    }

                    downloadProgressSFX = prevTotalDownloadProgress + sfxSize;
                    return true;
                } catch (Exception e) {
                    Debug.LogError($"Error Downloading {sfxKey}...\nReason:\n{e.Message}");
                    return false;
                }
            }
        }

        /// <summary>
        /// Load BGM Addressables.
        /// </summary>
        public async Task LoadAddressablesBGM()
        {
            if (isDownloadingBGM) {
                Debug.LogError("Currently Downloading BGM!");
                return;
            }

            Debug.Log("Loading BGM...");
            isDownloadingBGM = true;

            var bgms = new List<AudioObject>();
            var needDownload = new List<string>();
            var needLoad = new List<string>();

            var currentIndex = 1;
            foreach (var bgmName in audioNamesBGM) {
                var bgmKey = $"{Addressables_BGM}/{bgmName}.asset";
                var bgmSize = await GetDownloadSize(bgmKey);
                var isDownloading = bgmSize > 0;

                if (isDownloading)
                    needDownload.Add(bgmName);
                else
                    needLoad.Add(bgmName);
            }

            foreach (var bgmName in needDownload) {
                var bgmKey = $"{Addressables_BGM}/{bgmName}.asset";
                await TryDownloadBGM(bgmKey, currentIndex);

                currentIndex++;
            }

            foreach (var bgmName in needLoad) {
                var bgmKey = $"{Addressables_BGM}/{bgmName}.asset";
                await TryDownloadBGM(bgmKey, currentIndex);

                currentIndex++;
            }

            audioAddressablesBGM = bgms.ToArray();
            isDownloadingBGM = false;

            async Task<bool> TryDownloadBGM(string bgmKey, int currentIndex) {
                try {
                    var prevTotalDownloadProgress = downloadProgressBGM;
                    var bgmSize = await GetDownloadSize(bgmKey);
                    var isDownloading = bgmSize > 0;

                    var bgmRequest = Addressables.LoadAssetAsync<AudioObject>(bgmKey);
                    while (bgmRequest.IsDone == false) {
                        if (isDownloading) {
                            var downloadProgress = bgmRequest.GetDownloadStatus().DownloadedBytes / 1024f / 1024f;
                            downloadProgressBGM += downloadProgress;
                        }

                        await Task.Yield();
                    }

                    if (isDownloading) {
                        downloadProgressBGM = prevTotalDownloadProgress + bgmSize;
                    }
                    loadProgressBGM = currentIndex / audioNamesBGM.Length;

                    bgms.Add(bgmRequest.Result);
                    return true;
                } catch (Exception e) {
                    loadProgressBGM = currentIndex / audioNamesBGM.Length;

                    Debug.LogError($"Error Downloading {bgmKey}...\nReason:\n{e.Message}");
                    return false;
                }
            }
        }

        /// <summary>
        /// Load SFX Addressables.
        /// </summary>
        public async Task LoadAddressablesSFX()
        {
            if (isDownloadingSFX) {
                Debug.LogError("Currently Downloading SFX!");
                return;
            }

            Debug.Log("Downloading SFX...");
            isDownloadingSFX = true;

            var sfxs = new List<AudioObject>();
            var needDownload = new List<string>();
            var needLoad = new List<string>();

            var currentIndex = 1;
            foreach (var sfxName in audioNamesSFX) {
                var sfxKey = $"{Addressables_SFX}/{sfxName}.asset";
                var sfxSize = await GetDownloadSize(sfxKey);
                var isDownloading = sfxSize > 0;

                if (isDownloading)
                    needDownload.Add(sfxName);
                else
                    needLoad.Add(sfxName);
            }

            foreach (var sfxName in needDownload) {
                var sfxKey = $"{Addressables_SFX}/{sfxName}.asset";
                await TryDownloadSFX(sfxKey, currentIndex);

                currentIndex++;
            }

            foreach (var sfxName in needLoad) {
                var sfxKey = $"{Addressables_SFX}/{sfxName}.asset";
                await TryDownloadSFX(sfxKey, currentIndex);

                currentIndex++;
            }

            audioAddressablesSFX = sfxs.ToArray();
            isDownloadingSFX = false;

            async Task<bool> TryDownloadSFX(string sfxKey, int currentIndex) {
                try {
                    var prevTotalDownloadProgress = downloadProgressSFX;
                    var sfxSize = await GetDownloadSize(sfxKey);
                    var isDownloading = sfxSize > 0;

                    var bgmRequest = Addressables.LoadAssetAsync<AudioObject>(sfxKey);
                    while (bgmRequest.IsDone == false) {
                        if (isDownloading) {
                            var downloadProgress = bgmRequest.GetDownloadStatus().DownloadedBytes / 1024f / 1024f;
                            downloadProgressSFX += downloadProgress;
                        }

                        await Task.Yield();
                    }

                    if (isDownloading) {
                        downloadProgressSFX = prevTotalDownloadProgress + sfxSize;
                    }
                    loadProgressSFX = currentIndex / audioNamesSFX.Length;

                    sfxs.Add(bgmRequest.Result);
                    return true;
                } catch (Exception e) {
                    loadProgressSFX = currentIndex / audioNamesSFX.Length;

                    Debug.LogError($"Error Downloading {sfxKey}...\nReason:\n{e.Message}");
                    return false;
                }
            }
        }
    #endregion
#endregion

#region UNITY EDITOR
    #if UNITY_EDITOR
        public void GetAddressablesBGMNames()
        {
            audioNamesBGM = GetAssetNames($"{addressablesFolderPath}/BGM");
        }

        public void GetAddressablesSFXNames()
        {
            audioNamesSFX = GetAssetNames($"{addressablesFolderPath}/SFX");
        }

        private string[] GetAssetNames(string folderPath)
        {
            Debug.Log($"Getting Audios From: {folderPath}...");
            string[] guids = UnityEditor.AssetDatabase.FindAssets("t:AudioObject", new string[] { folderPath });
            Debug.Log($"Audios Found: {guids.Length}");
            
            List<string> audioNames = new List<string>();
            foreach (string guid in guids)
            {
                string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                AudioObject obj = UnityEditor.AssetDatabase.LoadAssetAtPath<AudioObject>(path);
                audioNames.Add(obj.name);
            }

            return audioNames.ToArray();
        }
    #endif
#endregion
    }
}