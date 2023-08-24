using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Entities;
using Services;
using SkillBridge.Message;
using Models;
using Managers;

public class GameObjectManager : MonoSingleton<GameObjectManager>
{
    Dictionary<int, GameObject> Characters = new Dictionary<int, GameObject>();

    protected override void OnStart()
    {
        StartCoroutine(InitGameObjects());
        CharacterManager.Instance.OnCharacterEnter += OnCharacterEnter;
        CharacterManager.Instance.OnCharacterLeave += OnCharacterLeave;
    }

    private void OnDestroy()
    {
        CharacterManager.Instance.OnCharacterEnter -= OnCharacterEnter;
        CharacterManager.Instance.OnCharacterLeave -= OnCharacterLeave;
    }

    void OnCharacterEnter(Character character)
    {
        CreateCharacterObject(character);
    }

    private void OnCharacterLeave(Character character)
    {
        // 判断需要销毁的角色是否存在
        if (!Characters.ContainsKey(character.entityId))
        {
            return;
        }

        // 角色没有被其他方式销毁, 将其销毁并移除
        if (Characters[character.entityId] != null)
        {
            Destroy(Characters[character.entityId]);
            Characters.Remove(character.entityId);
        }

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
        if (!Characters.ContainsKey(character.entityId) || Characters[character.entityId] == null)
        {
            Object obj = Resloader.Load<Object>(character.Define.Resource);
            if (obj == null)
            {
                Debug.LogFormat($"Character:[{character.Define.TID}] Resource:[{character.Define.Resource}] not existed");
                return;
            }
            GameObject go = (GameObject)Instantiate(obj, this.transform);
            go.name = "Character_" + character.Info.Id + "_" + character.Info.Name;

            Characters[character.entityId] = go;

            UIWorldElementManager.Instance.AddCharacterNameBar(go.transform, character);
        }
        InitGameObject(Characters[character.entityId], character);
    }

    private void InitGameObject(GameObject go, Character character)
    {
        go.transform.position = GameObjectTool.LogicToWorld(character.position);
        go.transform.forward = GameObjectTool.LogicToWorld(character.direction);
        EntityController entityController = go.GetComponent<EntityController>();
        if (entityController != null)
        {
            entityController.entity = character;
            entityController.isPlayer = character.IsPlayer;
        }

        PlayerInputController playerInputController = go.GetComponent<PlayerInputController>();
        if (playerInputController != null)
        {
            if (character.entityId == User.Instance.CurrentCharacter.Entity.Id)
            {
                User.Instance.CurrentCharacterObject = go;
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

