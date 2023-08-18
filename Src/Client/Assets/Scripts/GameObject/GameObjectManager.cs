﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Entities;
using Services;
using SkillBridge.Message;
using Models;

public class GameObjectManager : MonoBehaviour
{
    Dictionary<int, GameObject> Characters = new Dictionary<int, GameObject>();

    private void Start()
    {
        StartCoroutine(InitGameObjects());
        CharacterManager.Instance.OnCharacterEnter = OnCharacterEnter;
    }


    private void OnDestroy()
    {
        CharacterManager.Instance.OnCharacterEnter = null;
    }

    void OnCharacterEnter(Character cha)
    {
        CreateCharacterObject(cha);
    }

    IEnumerator InitGameObjects()
    {
        foreach (var cha in CharacterManager.Instance.Characters.Values)
        {
            CreateCharacterObject(cha);
            yield return null;
        }
    }

    private void CreateCharacterObject(Character character)
    {
        if (!Characters.ContainsKey(character.Info.Id) || Characters[character.Info.Id] == null)
        {
            Object obj = Resloader.Load<Object>(character.Define.Resource);
            if (obj == null)
            {
                Debug.LogFormat($"Character:[{character.Define.TID}] Resource:[{character.Define.Resource}] not existed");
                return;
            }
            GameObject go = (GameObject)Instantiate(obj);
            go.name = "Character_" + character.Info.Id + "_" + character.Info.Name;
            go.transform.position = GameObjectTool.LogicToWorld(character.position);
            go.transform.forward = GameObjectTool.LogicToWorld(character.direction);
            Characters[character.Info.Id] = go;

            EntityController entityController = go.GetComponent<EntityController>();
            if (entityController != null)
            {
                entityController.entity = character;
                entityController.isPlayer = character.IsPlayer;
            }

            PlayerInputController playerInputController = go.GetComponent<PlayerInputController>();
            if (playerInputController != null)
            {
                if (character.Info.Id == Models.User.Instance.CurrentCharacter.Id)
                {
                    MainPlayerCamera.Instance.player = go;
                    playerInputController.enabled = true;
                    playerInputController.character = character;
                    playerInputController.entityController = entityController;
                }
                else
                {
                    playerInputController.enabled = false;
                }
            }

            UIWorldElementManager.Instance.AddCharacterNameBar(go.transform, character);
        }
    }

    #region 原始代码
    //Dictionary<int, GameObject> Characters = new Dictionary<int, GameObject>();

    /*    void Start()
        {
            StartCoroutine(InitGameObjects());
            CharacterManager.Instance.OnCharacterEnter = OnCharacterEnter;
        }*/

    /*    private void OnDestroy()
        {
            CharacterManager.Instance.OnCharacterEnter = null;
        }*/

    /*    void OnCharacterEnter(Character cha)
        {
            CreateCharacterObject(cha);
        }*/

    /*    IEnumerator InitGameObjects()
        {
            foreach (var cha in CharacterManager.Instance.Characters.Values)
            {
                CreateCharacterObject(cha);
                yield return null;
            }
        }*/

    /*    private void CreateCharacterObject(Character character)
        {
            if (!Characters.ContainsKey(character.Info.Id) || Characters[character.Info.Id] == null)
            {
                Object obj = Resloader.Load<Object>(character.Define.Resource);
                if (obj == null)
                {
                    Debug.LogErrorFormat("Character[{0}] Resource[{1}] not existed.", character.Define.TID, character.Define.Resource);
                    return;
                }
                GameObject go = (GameObject)Instantiate(obj);
                go.name = "Character_" + character.Info.Id + "_" + character.Info.Name;

                go.transform.position = GameObjectTool.LogicToWorld(character.position);
                go.transform.forward = GameObjectTool.LogicToWorld(character.direction);
                Characters[character.Info.Id] = go;

                EntityController ec = go.GetComponent<EntityController>();
                if (ec != null)
                {
                    ec.entity = character;
                    ec.isPlayer = character.IsPlayer;
                }

                PlayerInputController pc = go.GetComponent<PlayerInputController>();
                if (pc != null)
                {
                    if (character.Info.Id == Models.User.Instance.CurrentCharacter.Id)
                    {
                        MainPlayerCamera.Instance.player = go;
                        pc.enabled = true;
                        pc.character = character;
                        pc.entityController = ec;
                    }
                    else
                    {
                        pc.enabled = false;
                    }
                }
                //UIWorldElementManager.Instance.AddCharacterNameBar(go.transform, character);
            }
        }
    */
    #endregion
}

