using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCharacterSystem : MonoBehaviour
{
    // Start is called before the first frame update
    private static MyCharacterSystem instance;
    public static MyCharacterSystem Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<MyCharacterSystem>();
            }
            return instance;
        }
    }
    [Header("Character Kit---------------")]
    public Renderer playerKitmesh;
    public Renderer AIKitmesh;
    public Material[] CharacterKitsmats;

    [Header("Character Hairs-------------")]
    public GameObject[] PlayerHairmesh;
    public GameObject[] AIHairmesh;

    [Header("Character Skins-------------")]
    public Renderer PlayerSkinmesh;
    public Renderer AISkinmesh;
    public Material[] CharacterSkinmats;



    #region // Player System Manager

    // Changing Player Kit
    public void ChangePlayerKit(Material selectMat)
    {
        Material Newkit = playerKitmesh.material;
        Newkit = selectMat;
        playerKitmesh.material = Newkit;
    }
    // Changing Player Hairs

    void ChangePlayerHair(GameObject SelectHair)
    {
        for (int i = 0; i<PlayerHairmesh.Length; i++)
        {
            PlayerHairmesh[i].gameObject.SetActive(false);
        }
        SelectHair.gameObject.SetActive(true);
    }
    // Changing Player Skins
    void ChangePlayerSkin(Material Selectskin)
    {
        Material NewSkin = PlayerSkinmesh.material;
        NewSkin = Selectskin;
        PlayerSkinmesh.material = NewSkin;
    }

    public void BrazilPlayerActive()
    {
        // Kit
        ChangePlayerKit(CharacterKitsmats[2]); // Yellow
        // Hair
        ChangePlayerHair(PlayerHairmesh[5]);
        // Skin
        ChangePlayerSkin(CharacterSkinmats[0]);
    }
    public void FrancePlayerActive()
    {
        // Kit
        ChangePlayerKit(CharacterKitsmats[0]); // Blue
        // Hair
        ChangePlayerHair(PlayerHairmesh[2]);
        // Skin
        ChangePlayerSkin(CharacterSkinmats[10]);
    }
    public void ArgentinaPlayerActive()
    {
        // Kit
        ChangePlayerKit(CharacterKitsmats[4]); // BlueLine
        // Hair
        ChangePlayerHair(PlayerHairmesh[6]);
        // Skin
        ChangePlayerSkin(CharacterSkinmats[6]);
    }
    public void EnglandPlayerActive()
    {
        // Kit
        ChangePlayerKit(CharacterKitsmats[3]); // White
        // Hair
        ChangePlayerHair(PlayerHairmesh[0]);
        // Skin
        ChangePlayerSkin(CharacterSkinmats[8]);
    }
    public void SpainPlayerActive()
    {
        // Kit
        ChangePlayerKit(CharacterKitsmats[1]); // Red
        // Hair
        ChangePlayerHair(PlayerHairmesh[9]);
        // Skin
        ChangePlayerSkin(CharacterSkinmats[10]);
    }
    public void GermanyPlayerActive()
    {
        // Kit
        ChangePlayerKit(CharacterKitsmats[1]); // Red
        // Hair
        ChangePlayerHair(PlayerHairmesh[8]);
        // Skin
        ChangePlayerSkin(CharacterSkinmats[8]);
    }
    public void NetherlandsPlayerActive()
    {
        // Kit
        ChangePlayerKit(CharacterKitsmats[2]); // Yellow
        // Hair
        ChangePlayerHair(PlayerHairmesh[7]);
        // Skin
        ChangePlayerSkin(CharacterSkinmats[7]);
    }
    public void SenegalPlayerActive()
    {
        // Kit
        ChangePlayerKit(CharacterKitsmats[3]); // White
        // Hair
        ChangePlayerHair(PlayerHairmesh[10]);
        // Skin
        ChangePlayerSkin(CharacterSkinmats[3]);
    }
    public void PortugalPlayerActive()
    {
        // Kit
        ChangePlayerKit(CharacterKitsmats[5]); // Redline
        // Hair
        ChangePlayerHair(PlayerHairmesh[5]);
        // Skin
        ChangePlayerSkin(CharacterSkinmats[10]);
    }
    public void USAPlayerActive()
    {
        // Kit
        ChangePlayerKit(CharacterKitsmats[3]); // White
        // Hair
        ChangePlayerHair(PlayerHairmesh[4]);
        // Skin
        ChangePlayerSkin(CharacterSkinmats[0]);
    }
    public void UruguayPlayerActive()
    {
        // Kit
        ChangePlayerKit(CharacterKitsmats[3]); // White
        // Hair
        ChangePlayerHair(PlayerHairmesh[2]);
        // Skin
        ChangePlayerSkin(CharacterSkinmats[2]);
    }
    public void AustraliaPlayerActive()
    {
        // Kit
        ChangePlayerKit(CharacterKitsmats[2]); // Yellow
        // Hair
        ChangePlayerHair(PlayerHairmesh[9]);
        // Skin
        ChangePlayerSkin(CharacterSkinmats[8]);
    }
    public void PolandPlayerActive()
    {
        // Kit
        ChangePlayerKit(CharacterKitsmats[3]); // White
        // Hair
        ChangePlayerHair(PlayerHairmesh[2]);
        // Skin
        ChangePlayerSkin(CharacterSkinmats[10]);
    }
    public void MoroccoPlayerActive()
    {
        // Kit
        ChangePlayerKit(CharacterKitsmats[3]); // White
        // Hair
        ChangePlayerHair(PlayerHairmesh[5]);
        // Skin
        ChangePlayerSkin(CharacterSkinmats[11]);
    }
    public void CroatiaPlayerActive()
    {
        // Kit
        ChangePlayerKit(CharacterKitsmats[5]); // Redline
        // Hair
        ChangePlayerHair(PlayerHairmesh[11]);
        // Skin
        ChangePlayerSkin(CharacterSkinmats[4]);
    }
    public void JapanPlayerActive()
    {
        // Kit
        ChangePlayerKit(CharacterKitsmats[0]); // Blue
        // Hair
        ChangePlayerHair(PlayerHairmesh[10]);
        // Skin
        ChangePlayerSkin(CharacterSkinmats[10]);
    }
    public void SwitzerlandPlayerActive()
    { 
        // Kit
        ChangePlayerKit(CharacterKitsmats[3]); // Blue
        // Hair
        ChangePlayerHair(PlayerHairmesh[9]);
        // Skin
        ChangePlayerSkin(CharacterSkinmats[8]);
    }
    public void GhanaPlayerActive()
    {
        // Kit
        ChangePlayerKit(CharacterKitsmats[1]); // Red
        // Hair
        ChangePlayerHair(PlayerHairmesh[13]);
        // Skin
        ChangePlayerSkin(CharacterSkinmats[14]);
    }
    public void QatarPlayerActive()
    {
        // Kit
        ChangePlayerKit(CharacterKitsmats[1]); // Red
        // Hair
        ChangePlayerHair(PlayerHairmesh[10]);
        // Skin
        ChangePlayerSkin(CharacterSkinmats[7]);
    }
    public void SaudiPlayerActive()
    {
        // Kit
        ChangePlayerKit(CharacterKitsmats[6]); // GreenLine
        // Hair
        ChangePlayerHair(PlayerHairmesh[9]);
        // Skin
        ChangePlayerSkin(CharacterSkinmats[7]);
    }
    public void ItalyPlayerActive()
    {
        // Kit
        ChangePlayerKit(CharacterKitsmats[3]); // White
        // Hair
        ChangePlayerHair(PlayerHairmesh[2]);
        // Skin
        ChangePlayerSkin(CharacterSkinmats[10]);
    }
    public void PakistanPlayerActive()
    {
        // Kit
        ChangePlayerKit(CharacterKitsmats[6]); // GreenLine
        // Hair
        ChangePlayerHair(PlayerHairmesh[0]);
        // Skin
        ChangePlayerSkin(CharacterSkinmats[7]);
    }
    public void IndiaPlayerActive()
    {
        // Kit
        ChangePlayerKit(CharacterKitsmats[3]); // White
        // Hair
        ChangePlayerHair(PlayerHairmesh[6]);
        // Skin
        ChangePlayerSkin(CharacterSkinmats[7]);
    }


    #endregion

    ////////////////////////////////////////////////////////// AI ///////////////////////////////////////

    #region // AI System Manager

    // Changing AI Kit
    public void ChangeAIKit(Material selectMat)
    {
        Material Newkit = AIKitmesh.material;
        Newkit = selectMat;
        AIKitmesh.material = Newkit;
    }
    // Changing AI Hairs

    void ChangeAIHair(GameObject SelectHair)
    {
        for (int i = 0; i < AIHairmesh.Length; i++)
        {
            AIHairmesh[i].gameObject.SetActive(false);
        }
        SelectHair.gameObject.SetActive(true);
    }
    // Changing AI Skins
    void ChangeAISkin(Material Selectskin)
    {
        Material NewSkin = AISkinmesh.material;
        NewSkin = Selectskin;
        AISkinmesh.material = NewSkin;
    }

    public void BrazilAIActive()
    {
        // Kit
        ChangeAIKit(CharacterKitsmats[2]); // Yellow
        // Hair
        ChangeAIHair(AIHairmesh[5]);
        // Skin
        ChangeAISkin(CharacterSkinmats[0]);
    }
    public void FranceAIActive()
    {
        // Kit
        ChangeAIKit(CharacterKitsmats[0]); // Blue
        // Hair
        ChangeAIHair(AIHairmesh[2]);
        // Skin
        ChangeAISkin(CharacterSkinmats[10]);
    }
    public void ArgentinaAIActive()
    {
        // Kit
        ChangeAIKit(CharacterKitsmats[4]); // BlueLine
        // Hair
        ChangeAIHair(AIHairmesh[6]);
        // Skin
        ChangeAISkin(CharacterSkinmats[6]);
    }
    public void EnglandAIActive()
    {
        // Kit
        ChangeAIKit(CharacterKitsmats[3]); // White
        // Hair
        ChangeAIHair(AIHairmesh[0]);
        // Skin
        ChangeAISkin(CharacterSkinmats[8]);
    }
    public void SpainAIActive()
    {
        // Kit
        ChangeAIKit(CharacterKitsmats[1]); // Red
        // Hair
        ChangeAIHair(AIHairmesh[9]);
        // Skin
        ChangeAISkin(CharacterSkinmats[10]);
    }
    public void GermanyAIActive()
    {
        // Kit
        ChangeAIKit(CharacterKitsmats[1]); // Red
        // Hair
        ChangeAIHair(AIHairmesh[8]);
        // Skin
        ChangeAISkin(CharacterSkinmats[8]);
    }
    public void NetherlandsAIActive()
    {
        // Kit
        ChangeAIKit(CharacterKitsmats[2]); // Yellow
        // Hair
        ChangeAIHair(AIHairmesh[7]);
        // Skin
        ChangeAISkin(CharacterSkinmats[7]);
    }
    public void SenegalAIActive()
    {
        // Kit
        ChangeAIKit(CharacterKitsmats[3]); // White
        // Hair
        ChangeAIHair(AIHairmesh[10]);
        // Skin
        ChangeAISkin(CharacterSkinmats[3]);
    }
    public void PortugalAIActive()
    {
        // Kit
        ChangeAIKit(CharacterKitsmats[5]); // Redline
        // Hair
        ChangeAIHair(AIHairmesh[5]);
        // Skin
        ChangeAISkin(CharacterSkinmats[10]);
    }
    public void USAAIActive()
    {
        // Kit
        ChangeAIKit(CharacterKitsmats[3]); // White
        // Hair
        ChangeAIHair(AIHairmesh[4]);
        // Skin
        ChangeAISkin(CharacterSkinmats[0]);
    }
    public void UruguayAIActive()
    {
        // Kit
        ChangeAIKit(CharacterKitsmats[3]); // White
        // Hair
        ChangeAIHair(AIHairmesh[2]);
        // Skin
        ChangeAISkin(CharacterSkinmats[2]);
    }
    public void AustraliaAIActive()
    {
        // Kit
        ChangeAIKit(CharacterKitsmats[2]); // Yellow
        // Hair
        ChangeAIHair(AIHairmesh[9]);
        // Skin
        ChangeAISkin(CharacterSkinmats[8]);
    }
    public void PolandAIActive()
    {
        // Kit
        ChangeAIKit(CharacterKitsmats[3]); // White
        // Hair
        ChangeAIHair(AIHairmesh[2]);
        // Skin
        ChangeAISkin(CharacterSkinmats[10]);
    }
    public void MoroccoAIActive()
    {
        // Kit
        ChangeAIKit(CharacterKitsmats[3]); // White
        // Hair
        ChangeAIHair(AIHairmesh[5]);
        // Skin
        ChangeAISkin(CharacterSkinmats[11]);
    }
    public void CroatiaAIActive()
    {
        // Kit
        ChangeAIKit(CharacterKitsmats[5]); // Redline
        // Hair
        ChangeAIHair(AIHairmesh[11]);
        // Skin
        ChangeAISkin(CharacterSkinmats[4]);
    }
    public void JapanAIActive()
    {
        // Kit
        ChangeAIKit(CharacterKitsmats[0]); // Blue
        // Hair
        ChangeAIHair(AIHairmesh[10]);
        // Skin
        ChangeAISkin(CharacterSkinmats[10]);
    }
    public void SwitzerlandAIActive()
    {
        // Kit
        ChangeAIKit(CharacterKitsmats[3]); // Blue
        // Hair
        ChangeAIHair(AIHairmesh[9]);
        // Skin
        ChangeAISkin(CharacterSkinmats[8]);
    }
    public void GhanaAIActive()
    {
        // Kit
        ChangeAIKit(CharacterKitsmats[1]); // Red
        // Hair
        ChangeAIHair(AIHairmesh[13]);
        // Skin
        ChangeAISkin(CharacterSkinmats[14]);
    }
    public void QatarAIActive()
    {
        // Kit
        ChangeAIKit(CharacterKitsmats[1]); // Red
        // Hair
        ChangeAIHair(AIHairmesh[10]);
        // Skin
        ChangeAISkin(CharacterSkinmats[7]);
    }
    public void SaudiAIActive()
    {
        // Kit
        ChangeAIKit(CharacterKitsmats[6]); // GreenLine
        // Hair
        ChangeAIHair(AIHairmesh[9]);
        // Skin
        ChangeAISkin(CharacterSkinmats[7]);
    }
    public void ItalyAIActive()
    {
        // Kit
        ChangeAIKit(CharacterKitsmats[3]); // White
        // Hair
        ChangeAIHair(AIHairmesh[2]);
        // Skin
        ChangeAISkin(CharacterSkinmats[10]);
    }
    public void PakistanAIActive()
    {
        // Kit
        ChangeAIKit(CharacterKitsmats[6]); // GreenLine
        // Hair
        ChangeAIHair(AIHairmesh[0]);
        // Skin
        ChangeAISkin(CharacterSkinmats[7]);
    }
    public void IndiaAIActive()
    {
        // Kit
        ChangeAIKit(CharacterKitsmats[3]); // White
        // Hair
        ChangeAIHair(AIHairmesh[6]);
        // Skin
        ChangeAISkin(CharacterSkinmats[7]);
    }

    #endregion
}
