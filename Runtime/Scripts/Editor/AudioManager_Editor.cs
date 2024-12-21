using UnityEngine;
using UnityEditor;

namespace Ojanck.AudioSetup.EditorScript
{
    [CustomEditor(typeof(AudioManager))]
    public class AudioManager_Editor : Editor
    {
        private SerializedProperty audioSourceBGMProp;
        private SerializedProperty audioSourceSFXProp;
        private SerializedProperty loadResourcesOnAwakeProp;
        private SerializedProperty loadAddressablesOnAwakeProp;
        private SerializedProperty volumeBGMProp;
        private SerializedProperty volumeSFXProp;
        private SerializedProperty resourcesLoadPathProp;
        private SerializedProperty audioResourcesBGMProp;
        private SerializedProperty audioResourcesSFXProp;
        private SerializedProperty audioAddressablesBGMProp;
        private SerializedProperty audioAddressablesSFXProp;
        private SerializedProperty addressablesFolderPathProp;
        private SerializedProperty audioNamesBGMProp;
        private SerializedProperty audioNamesSFXProp;

        private void OnEnable()
        {
            audioSourceBGMProp = serializedObject.FindProperty("audioSourceBGM");
            audioSourceSFXProp = serializedObject.FindProperty("audioSourceSFX");
            loadResourcesOnAwakeProp = serializedObject.FindProperty("loadResourcesOnAwake");
            loadAddressablesOnAwakeProp = serializedObject.FindProperty("loadAddressablesOnAwake");
            volumeBGMProp = serializedObject.FindProperty("volumeBGM");
            volumeSFXProp = serializedObject.FindProperty("volumeSFX");
            resourcesLoadPathProp = serializedObject.FindProperty("resourcesLoadPath");
            audioResourcesBGMProp = serializedObject.FindProperty("audioResourcesBGM");
            audioResourcesSFXProp = serializedObject.FindProperty("audioResourcesSFX");
            audioAddressablesBGMProp = serializedObject.FindProperty("audioAddressablesBGM");
            audioAddressablesSFXProp = serializedObject.FindProperty("audioAddressablesSFX");
            addressablesFolderPathProp = serializedObject.FindProperty("addressablesFolderPath");
            audioNamesBGMProp = serializedObject.FindProperty("audioNamesBGM");
            audioNamesSFXProp = serializedObject.FindProperty("audioNamesSFX");
        }

        public override async void OnInspectorGUI()
        {
            var audioManager = serializedObject.targetObject as AudioManager;
            serializedObject.Update();

            DrawMultipleProperty(
                audioSourceBGMProp,
                audioSourceSFXProp,
                loadResourcesOnAwakeProp,
                loadAddressablesOnAwakeProp,
                volumeBGMProp,
                volumeSFXProp
            );
            EditorGUILayout.Space();
            
            ShowResourcesMenu(audioManager);
            
            EditorGUILayout.Space();

            ShowAddressablesMenu(audioManager);
            
            EditorGUILayout.Space();
            
            ShowTestingField(audioManager);

            serializedObject.ApplyModifiedProperties();
        }

        private void ShowResourcesMenu(AudioManager audioManager)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            var headerStyle = GetDefaultGUIStyle();
            GUILayout.Label("Resources", headerStyle);
            EditorGUILayout.Space();

            DrawMultipleProperty(
                resourcesLoadPathProp
            );

            EditorGUI.indentLevel++;
            DrawMultipleProperty(
                audioResourcesBGMProp,
                audioResourcesSFXProp
            );
            EditorGUI.indentLevel--;

            EditorGUILayout.Space();

            if (GUILayout.Button("Get Audio Resources")) {
                audioManager.ResourcesLoadAudios();
                
                if (EditorApplication.isPlaying)
                    audioManager.RefreshDictionaries();
                else
                    EditorUtility.SetDirty(audioManager);
            }

            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
        }

        private void ShowAddressablesMenu(AudioManager audioManager)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            var headerStyle = GetDefaultGUIStyle();
            GUILayout.Label("Addressables", headerStyle);
            EditorGUILayout.Space();

            DrawMultipleProperty(
                addressablesFolderPathProp
            );

            EditorGUI.indentLevel++;
            DrawMultipleProperty(
                audioNamesBGMProp,
                audioNamesSFXProp
            );
            EditorGUI.indentLevel--;

            if (GUILayout.Button("Get Addressable BGM Audio Names")) {
                audioManager.GetAddressablesBGMNames();
                
                if (EditorApplication.isPlaying == false)
                    EditorUtility.SetDirty(audioManager);
            }
            if (GUILayout.Button("Get Addressable SFX Audio Names")) {
                audioManager.GetAddressablesSFXNames();
                
                if (EditorApplication.isPlaying == false)
                    EditorUtility.SetDirty(audioManager);
            }
            EditorGUILayout.Space();

            EditorGUI.indentLevel++;
            DrawMultipleProperty(
                audioAddressablesBGMProp,
                audioAddressablesSFXProp
            );
            EditorGUI.indentLevel--;

            if (GUILayout.Button("Load Addressable BGM Audio")) {
                LoadAddressablesBGM(audioManager);
            }
            if (GUILayout.Button("Load Addressable SFX Audio")) {
                LoadAddressablesSFX(audioManager);
            }

            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
        }

        private async void LoadAddressablesBGM(AudioManager audioManager)
        {
            await audioManager.LoadAddressablesBGM();

            if (EditorApplication.isPlaying)
                audioManager.RefreshDictionaries();
            else
                EditorUtility.SetDirty(audioManager);
        }

        private async void LoadAddressablesSFX(AudioManager audioManager)
        {
            await audioManager.LoadAddressablesSFX();

            if (EditorApplication.isPlaying)
                audioManager.RefreshDictionaries();
            else
                EditorUtility.SetDirty(audioManager);
        }

        string bgmCodeExample = "EX_BGM_01";
        string sfxCodeExample = "EX_BGM_01";

        private void ShowTestingField(AudioManager audioManager)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            var headerStyle = GetDefaultGUIStyle();
            GUILayout.Label("Play Test", headerStyle);
            EditorGUILayout.Space();

            bgmCodeExample = EditorGUILayout.TextField("BGM Code", bgmCodeExample);
            if (GUILayout.Button("Play BGM")) {
                if (EditorApplication.isPlaying == false)
                    return;

                audioManager.PlayBGM(bgmCodeExample);
            }
            if (GUILayout.Button("Stop BGM")) {
                if (EditorApplication.isPlaying == false)
                    return;

                audioManager.StopBGM();
            }
            EditorGUILayout.Space();

            sfxCodeExample = EditorGUILayout.TextField("SFX Code", sfxCodeExample);
            if (GUILayout.Button("Play SFX")) {
                if (EditorApplication.isPlaying == false)
                    return;

                audioManager.PlaySFX(sfxCodeExample);
            }

            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
        }

        private GUIStyle GetDefaultGUIStyle()
        {
            return new GUIStyle(EditorStyles.boldLabel) {
                fontSize = 14,                       // Adjust font size
                alignment = TextAnchor.MiddleCenter, // Center-align the text
                normal = { textColor = Color.white } // Set text color
            };
        }

        private void DrawPropertyWithHorizontalPadding(SerializedProperty property, float left = 20, float right = 20)
        {
            // Add padding before the property field (left and right)
            GUILayout.BeginHorizontal();
            GUILayout.Space(left); // Left padding
            EditorGUILayout.PropertyField(property);
            GUILayout.Space(right); // Right padding
            GUILayout.EndHorizontal();
        }

        private void DrawProperty(SerializedProperty property)
        {
            EditorGUILayout.PropertyField(property);
        }

        private void DrawMultipleProperty(params SerializedProperty[] properties)
        {
            foreach (var property in properties) {
                DrawProperty(property);
            }
        }
    }
}