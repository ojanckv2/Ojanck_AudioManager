# API Documentation

## Overview
This API provides functionality for Ojanck - Audio Manager

---

## Table of Contents
1. [Classes](#classes)
   - [AudioManager](#AudioManager)
   - [AudioObject](#AudioObject)

---

## Classes

### AudioManager
Ojanck's Simple Audio Manager

#### Properties
| Property         | Type          | Description                                       |
|------------------|---------------|---------------------------------------------------|
| Addressables_BGM | `string`      | Returns the BGM Addressable Key Property          |
| Addressables_SFX | `string`      | Returns the SFX Addressable Key Property          |
| IsDownloadingBGM | `bool`        | Returns the true if in process of downloading BGM |
| IsDownloadingSFX | `bool`        | Returns the true if in process of downloading SFX |

#### Methods
| Method                   | Return Type   | Parameters    | Description                                                  |
|--------------------------|---------------|---------------|--------------------------------------------------------------|
| PlayBGM()                | `void`        | `void`        | Play BGM based on its ID                                     |
| SetVolumeBGM()           | `void`        | `float volume`| Set BGM volume based                                         |
| StopBGM()                | `void`        | `void`        | Stop BGM                                                     |
| PlaySFX()                | `void`        | `void`        | Play SFX based on its ID                                     |
| SetVolumeSFX()           | `void`        | `float volume`| Set SFX volume based                                         |
| RefreshDictionaries()    | `void`        | `void`        | Refresh the Audio Dictionaries. Use After Loading the Audios |
| ResourcesLoadAudios()    | `void`        | `void`        | Load All Audio From Resources                                |
| GetLoadProgressBGM()     | `float`       | `void`        | Get Load Progress BGM                                        |
| GetLoadProgressSFX()     | `float`       | `void`        | Get Load Progress SFX                                        |
| GetDownloadProgressBGM() | `float`       | `void`        | Get Download Progress BGM                                    |
| GetDownloadProgressSFX() | `float`       | `void`        | Get Download Progress SFX                                    |
| GetDownloadSizeBGM()     | `float`       | `void`        | Get Download Size BGM                                        |
| GetDownloadSizeSFX()     | `float`       | `void`        | Get Download Size SFX                                        |
| DownloadAddressablesBGM()| `void`        | `void`        | Download BGM Audios                                          |
| DownloadAddressablesSFX()| `void`        | `void`        | Download SFX Audios                                          |
| LoadAddressablesBGM()    | `void`        | `void`        | Load BGM Addressables. Download if asset's not downloaded    |
| LoadAddressablesSFX()    | `void`        | `void`        | Load SFX Addressables. Download if asset's not downloaded    |
| GetAddressablesBGMNames()| `void`        | `void`        | Editor Only. Get Required BGM Addressable Names              |
| GetAddressablesSFXNames()| `void`        | `void`        | Editor Only. Get Required SFX Addressable Names              |

---

### AudioObject
Audio Object for loading the Audio Clips into the Audio Manager.

#### Properties
| Property     | Type          | Description                           |
|--------------|---------------|---------------------------------------|
| AudioID      | `string`      | Return the Audio Object's ID.         |
| AudioClip    | `AudioClip`   | Return the Audio Object's Clip.       |
| AudioPower   | `float`       | Return the Audio Object's Audio Power.|

---
