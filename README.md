# Ojanck AudioManager
## Overview
This API provides functionality for Ojanck - Audio Manager

---

## Getting Started

### Prerequisites
- Unity Addressables.

### Installation
- Using package manager:
   1. Go to the package manager
   2. Press the '+' on the top left corner
   3. Choose 'Add package from git url'
   4. Fill the blank using 'https://github.com/ojanckv2/Ojanck_AudioManager.git'
- From manifest.json
   1. Go to the Packages/ folder in your project.
   2. Open the manifest.json file
   3. Add ``"com.ojanck.audiomanager": "https://github.com/ojanckv2/Ojanck_AudioManager.git"`` in ``"dependencies"``

---

## How to Use
1. Create an Empty GameObject in your scene.
2. Add AudioManager script into your gameobject.
3. Go to your Resources or Addressables folder.
4. Create a BGM and SFX folder
5. Right Click either BGM or SFX folder
6. Choose ``Ojanck => Audio => New Audio Object`` to create an Audio Object.
7. Go to your AudioManager gameobject
8. Create two AudioListeners for both BGM and SFX and put it on your AudioManager
9. Do the following
    - For Getting Audios from Resources:
        1. Specify your Resources Path. Ex: ``Audios``
        2. Press the `Get Audios From Resources` Button
    - For Getting Audios from Addressables:
        1. Specify your Addressables Folder Path. Ex: ``Assets/Samples/Simple Audio Manager/1.0.0/Examples/Addressables/Audios``
        2. Press both ``Get Addressable BGM Audio Names`` and ``Get Addressable SFX Audio Names`` to get the addressable names
        3. Add your Addressables folder to your Addressables Group
        4. Change the Addressables Key into ``Audios/BGM`` for BGM Folder and ``Audios/SFX`` for SFX Folder
        5. Press both ``Load Addressable BGM Audio`` and ``Load Addressable SFX Audio`` to load the audio addressables
    - You can also toggle the ``Load Resources On Awake`` or ``Load Addressables On Awake`` to load the audios when the game starts.
    - Press ``Play BGM`` or ``Play SFX`` button on your AudioManager to Play Test. Don't forget to do it on Play Mode!
    - P.S. Don't forget to fill the AudioObject parameters before you play them 

---

## API Documentation
API documentation can be read [here](https://github.com/ojanckv2/Ojanck_AudioManager/tree/main/Runtime/Scripts/API_Documentation.md)

---