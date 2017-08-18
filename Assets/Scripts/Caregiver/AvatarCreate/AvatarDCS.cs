using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UMA.CharacterSystem;
using SocketIO;
using SimpleJSON;
using System;
using UMA;
using UMA.CharacterSystem.Examples;

/// NOTE: TAKEN FROM UMA EXAMPLES and modified slightly
public class AvatarDCS : MonoBehaviour {

    public DynamicCharacterAvatar Avatar;
    public GameObject SlotPrefab;
    public GameObject WardrobePrefab;
    public GameObject SlotPanel;
    public GameObject WardrobePanel;
    public GameObject ColorPrefab;
    public GameObject DnaPrefab;
    public GameObject LabelPrefab;
    public GameObject AvatarPrefab;
    public SharedColorTable HairColor;
    public SharedColorTable SkinColor;
    public SharedColorTable EyesColor;
    public SharedColorTable ClothingColor;
    public SocketIOComponent _socket;
    public UIManager _ui;
    public TextAsset _outFile;

    /// <summary>
    /// Remove any controls from the panels
    /// </summary>
    private void Cleanup()
    {

        foreach (Transform t in SlotPanel.transform)
        {
            GameObject.Destroy(t.gameObject);
        }
        foreach (Transform t in WardrobePanel.transform)
        {
            GameObject.Destroy(t.gameObject);
        }
    }

    /// <summary>
    /// DNA Button event Handler
    /// </summary>
    public void DnaClick()
    {
        Cleanup();
        Dictionary<string,DnaSetter> AllDNA = Avatar.GetDNA();
        foreach( string dnaSetName in Constants.AVATAR_DNA)
        {
            // create a button. 
            // set set the dna setter on it.
            GameObject go = GameObject.Instantiate(DnaPrefab);
            DNAHandler ch = go.GetComponent<DNAHandler>();
            ch.Setup(Avatar, AllDNA[dnaSetName], WardrobePanel);

            Text txt = go.GetComponentInChildren<Text>();
            txt.text = AllDNA[dnaSetName].Name;
            go.transform.SetParent(SlotPanel.transform);
        }
    }

    /// <summary>
    /// Colors Button event handler
    /// </summary>
    public void ColorsClick()
    {
        Cleanup();

        foreach(UMA.OverlayColorData ocd in Avatar.CurrentSharedColors )
        {
            GameObject go = GameObject.Instantiate(ColorPrefab);
            AvailableColorsHandler ch = go.GetComponent<AvailableColorsHandler>();

            SharedColorTable currColors = ClothingColor;

            if (ocd.name.ToLower() == "skin")
                currColors = SkinColor;
            else if (ocd.name.ToLower() == "hair")
                currColors = HairColor;
            else if (ocd.name.ToLower() == "eyes")
                currColors = EyesColor;

            ch.Setup(Avatar, ocd.name, WardrobePanel,currColors);

            Text txt = go.GetComponentInChildren<Text>();
            txt.text = ocd.name;
            go.transform.SetParent(SlotPanel.transform);
        }
    }

    /// <summary>
    /// Wardrobe Button event handler
    /// </summary>
    public void WardrobeClick()
    {
        Cleanup();

        Dictionary<string, List<UMATextRecipe>> recipes = Avatar.AvailableRecipes;

        foreach (string s in recipes.Keys)
        {
            GameObject go = GameObject.Instantiate(SlotPrefab);
            SlotHandler sh = go.GetComponent<SlotHandler>();
            sh.Setup(Avatar, s,WardrobePanel);
            Text txt = go.GetComponentInChildren<Text>();
            txt.text = s;
            go.transform.SetParent(SlotPanel.transform);
        }
    }

    public SharedColorTable SkinColors;
    public SharedColorTable HairColors;

    public void DynamicCreateClick()
    {
        string[] files = { "Fram", "Bob", "Gobs" };
        float x = UnityEngine.Random.Range(-8.0f, 8.0f);
        float z = UnityEngine.Random.Range(1.0f, 12.0f);
        GameObject go = GameObject.Instantiate(AvatarPrefab);
        DynamicCharacterAvatar dca = go.GetComponent<DynamicCharacterAvatar>();
#if false
        // this shows how to load it from a string at initialization
        TextAsset t = Resources.Load<TextAsset>("CharacterRecipes/Bob");
        dca.Preload(t.text);
#else
        // this shows how to load it from a resource file at initialization
        dca.loadPathType = DynamicCharacterAvatar.loadPathTypes.CharacterSystem;
        dca.loadFilename = files[UnityEngine.Random.Range(0, 3)];
#endif
        go.transform.localPosition = new Vector3(x, 0, z);
        go.SetActive(true);
    }


    public void ToggleUpdateBounds()
    {
        SkinnedMeshRenderer[] sm = FindObjectsOfType<SkinnedMeshRenderer>();
        foreach(SkinnedMeshRenderer smr in sm)
        {
            smr.updateWhenOffscreen = !smr.updateWhenOffscreen;
        }
    }

    public void RandomClick()
    {
        RandomizeAvatar(Avatar);
    }

    private void RandomizeAvatar(DynamicCharacterAvatar Avatar)
    {
        Dictionary<string, List<UMATextRecipe>> recipes = Avatar.AvailableRecipes;

        // Set random wardrobe slots.
        foreach (string SlotName in recipes.Keys)
        {
            int cnt = recipes[SlotName].Count;
            if (cnt > 0)
            {
                //Get a random recipe from the slot, and apply it
                int min = -1;
                if (SlotName == "Legs") min = 0; // Don't allow pants removal in random test
                int rnd = UnityEngine.Random.Range(min, cnt);
                if (rnd == -1)
                {
                    Avatar.ClearSlot(SlotName);
                }
                else
                {
                    Avatar.SetSlot(recipes[SlotName][rnd]);
                }
            }
        }

        // Set Random DNA 
        Dictionary<string, DnaSetter> setters = Avatar.GetDNA();
        foreach (KeyValuePair<string, DnaSetter> dna in setters)
        {
            dna.Value.Set(0.35f + (UnityEngine.Random.value * 0.3f));
        }

        // Set Random Colors for Skin and Hair
        int RandHair = UnityEngine.Random.Range(0, HairColors.colors.Length);
        int RandSkin = UnityEngine.Random.Range(0, SkinColors.colors.Length);

        Avatar.SetColor("Hair", HairColors.colors[RandHair]);
        Avatar.SetColor("Skin", SkinColors.colors[RandSkin]);
        Avatar.BuildCharacter(true);
        Avatar.ForceUpdate(true, true, true);
    }

    public void LinkToAssets()
    {
        Application.OpenURL("https://www.assetstore.unity3d.com/en/#!/search/page=1/sortby=popularity/query=publisher:5619");
    }

    public void ToggleAnimation()
    {
    // RuntimeAnimatorController rac = Avatar.gameObject.GetComponentInChildren<>
    }

    public void setRace(string race)
    {
        Avatar.ChangeRace(race);
    }

    public void saveAvatar()
    {
        Avatar.UpdateUMA();
        Avatar.savePathType = DynamicCharacterAvatar.savePathTypes.Resources;
        Avatar.DoSave(false,Constants.UMA_OUT_FILE);
        // _outFile = Resources.Load(Constants.UMA_OUT_FILE) as TextAsset;
        Avatar.BuildCharacter(true);
        Avatar.ForceUpdate(true, true, true);
        string recipeString = Avatar.GetCurrentRecipe();
        JSONNode data = JSON.Parse(recipeString);
        _socket.Emit(Constants.CAREGIVER_SEND_AVATAR_MESSAGE, data);
        string DNARecipe = Avatar.GetCurrentDNARecipe();
        data = JSON.Parse(DNARecipe);
        _socket.Emit(Constants.CAREGIVER_SEND_AVATAR_DNA_MESSAGE, data);
        string ColorsRecipe = Avatar.GetCurrentColorsRecipe();
        data = JSON.Parse(ColorsRecipe);
        _socket.Emit(Constants.CAREGIVER_SEND_AVATAR_COLORS_MESSAGE, data);
        //string recFileData = Convert.ToBase64String(_outFile.bytes);
        // string[] recFileDatabundle = new string[1]{recFileData};
        // JSONObject fileData = ServerCommsUtility.Instance.serializeData(Constants.CAREGIVER_SEND_AVATAR_FILE_FIELDS, recFileDatabundle);
        // _socket.Emit(Constants.CAREGIVER_SEND_AVATAR_FILE_MESSAGE, fileData);
        _ui.nextUI();
    }
} 

