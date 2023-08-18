using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor.UI;
using UnityEditor.Tilemaps;
using UnityEditor.UIElements;
using UnityEditor;
#endif
#if UNITY_EDITOR
//테스트 용도의 확장 에디터 윈도우
//플레이 타임중 테스트가 필요한 것들을 버튼에 연동해서 사용할 예정
public class TestWindow : EditorWindow
{
    /// <summary>
    /// Legacy코드
    /// </summary>
    //public EnemyCommon_Legacy obj;
    //Queue<GameObject> testList = new Queue<GameObject>();
    //===========================================================================================

    string[] groundTileNames;

    //메뉴에 추가
    [MenuItem("CustomMenu/BuildTest")]
    static void OpenTestWindow()
    {
        TestWindow window = (TestWindow)EditorWindow.GetWindow(typeof(TestWindow));
        window.Show();
    }

    GameObject basTilePalette;
    TileBase baseTile;
    TileBase escapeTile;

    private void OnEnable()
    {
        string stage01Path = "/Game/TilePalette/Stage01";
        string folderPath = Application.dataPath + stage01Path;
        string[] groundTilePalette = System.IO.Directory.GetFiles(folderPath, "*.prefab");

        groundTileNames = new string[groundTilePalette.Length];

        for (int i = 0; i < groundTilePalette.Length; i++)
        {
            string filename = System.IO.Path.GetFileNameWithoutExtension(groundTilePalette[i]);
            groundTileNames[i] = filename;
            if (filename.Equals("Stage01_BaseTile_RuleTile"))
            {
                basTilePalette = AssetDatabase.LoadAssetAtPath("Assets/Game/TilePalette/Stage01/Stage01_BaseTile_RuleTile.prefab", typeof(GameObject)) as GameObject;
                Tilemap tmp = basTilePalette.GetComponentInChildren<Tilemap>();
                baseTile = tmp.GetTile(new Vector3Int(1, 1, 0));


                basTilePalette = AssetDatabase.LoadAssetAtPath("Assets/Game/TilePalette/Stage01/Stage01_Others_SpriteTile.prefab", typeof(GameObject)) as GameObject;
                tmp = basTilePalette.GetComponentInChildren<Tilemap>();
                escapeTile = tmp.GetTile(new Vector3Int(-1, 0, 0));

            }
        }

    }

    GameObject mapParent;

    [Range(1, 2)]
    public int type = 1;

    void OnGUI()
    {

        if (GUILayout.Button("MosterHit_Weak"))
        {
            List<EnemyBase> tmp = GameObject.FindObjectsOfType<EnemyBase>().ToList();

            foreach (EnemyBase enemy in tmp)
            {
                enemy.Hit(1,1);
            }
        }
        if (GUILayout.Button("MosterHit_Strong"))
        {
            List<EnemyBase> tmp = GameObject.FindObjectsOfType<EnemyBase>().ToList();

            foreach (EnemyBase enemy in tmp)
            {
                enemy.Hit(2,1);
            }
        }

        mapParent = EditorGUILayout.ObjectField("부모 필드", mapParent, typeof(GameObject), true) as GameObject;

        if (mapParent!=null)
        {

            int currSize = mapParent.transform.childCount==0?0: mapParent.transform.childCount-1;

            Transform before = mapParent.transform;
            
            if (currSize > 0)
            {
                type = mapParent.GetComponent<MapData>().type;
                before = mapParent.transform.GetChild(currSize);
            }

            if (GUILayout.Button("CreateMap"))
            {
                GameObject tmpTileMapObject;
                Tilemap tmpTilemap;
                Rigidbody2D rb;
                TilemapCollider2D tilemapCollider;
                CompositeCollider2D compositeCollider;
                PlatformEffector2D platformEffector;
                EscapeTile tmpTile;

                if (currSize >0)
                {
                    if (type == 1)
                    {
                        Transform a = before.Find("Grid").Find("EscapeRight");
                        a.GetComponent<CompositeCollider2D>().isTrigger = true;
                    }
                    else if (type == 2)
                    {
                        Transform a = before.Find("Grid").Find("EscapeUp");
                        a.GetComponent<CompositeCollider2D>().isTrigger = true;

                    }
                }
                else if (currSize == 0)
                {
                    mapParent.AddComponent<MapData>().type = type;
                }

                //기본세팅 1 ,1
                int height = 7;
                int width = 13;

                Vector2Int CenterPos;
                if (type == 1)
                {
                    CenterPos = new Vector2Int((currSize) * width * 2, 0);
                }
                else
                {
                    CenterPos = new Vector2Int(0, -(currSize) * height * 2);

                }
                Debug.Log("CenterPos"+CenterPos);

                //스테이지, 추후 enum으로 관리
                int stageIndex = 1;
                int mapIndex = 1;

                mapParent.name = "Stage" + stageIndex + "Map" + mapIndex;

                GameObject map = new GameObject();
                map.transform.parent = mapParent.transform;
                map.name = "Field" + (currSize+1);
                map.AddComponent<FieldData>().depth = currSize+1;

                GameObject tileGrid = new GameObject();
                tileGrid.AddComponent<Grid>();
                tileGrid.transform.parent = map.transform;
                tileGrid.name = "Grid";


                tmpTileMapObject = new GameObject();
                tmpTileMapObject.transform.position = tmpTileMapObject.transform.position;
                tmpTilemap = tmpTileMapObject.AddComponent<Tilemap>();
                tmpTilemap.tileAnchor = new Vector3(0, 1, 0);
                tmpTilemap.color = new Color(0f, 0f, 0f, 0f);
                tmpTileMapObject.AddComponent<TilemapRenderer>();
                tmpTile = tmpTileMapObject.AddComponent<EscapeTile>();
                tmpTile.fieldIndex = currSize + 1;
                tmpTile.escapeIndex = 3;
                tilemapCollider = tmpTileMapObject.AddComponent<TilemapCollider2D>();
                tilemapCollider.usedByComposite = true;
                rb = tmpTileMapObject.AddComponent<Rigidbody2D>();
                rb.bodyType = RigidbodyType2D.Static;
                compositeCollider = tmpTileMapObject.AddComponent<CompositeCollider2D>();
                compositeCollider.isTrigger = true;
                compositeCollider.generationType = CompositeCollider2D.GenerationType.Manual;
                compositeCollider.GenerateGeometry();

                tmpTileMapObject.transform.parent = tileGrid.transform;
                tmpTileMapObject.name = "EscapeUp";


                TileChangeData tileChangeData = new TileChangeData()
                {
                    tile = escapeTile,
                    transform = Matrix4x4.Translate(new Vector3(.5f, -.5f, 0))
                };
                for (int dx = CenterPos.x - (width); dx < CenterPos.x + width; dx++)
                {
                    Debug.Log("x " + height + "/" + dx);
                    tileChangeData.position = new Vector3Int(dx, height, 0);
                    tmpTilemap.SetTile(tileChangeData, false);
                }


                if (type != 2 || (type == 2 && currSize == 0))
                {
                    tmpTileMapObject = new GameObject();
                    tmpTileMapObject.transform.position = tmpTileMapObject.transform.position;
                    tmpTilemap = tmpTileMapObject.AddComponent<Tilemap>();
                    tmpTilemap.tileAnchor = new Vector3(0, 1, 0);
                    tmpTilemap.color = new Color(0f, 0f, 0f, 0f);
                    tmpTileMapObject.AddComponent<TilemapRenderer>();
                    tmpTile = tmpTileMapObject.AddComponent<EscapeTile>();
                    tmpTile.fieldIndex = currSize + 1;
                    tmpTile.escapeIndex = 2;
                    tilemapCollider = tmpTileMapObject.AddComponent<TilemapCollider2D>();
                    tilemapCollider.usedByComposite = true;
                    rb = tmpTileMapObject.AddComponent<Rigidbody2D>();
                    rb.bodyType = RigidbodyType2D.Static;
                    compositeCollider = tmpTileMapObject.AddComponent<CompositeCollider2D>();
                    compositeCollider.isTrigger = true;
                    compositeCollider.generationType = CompositeCollider2D.GenerationType.Manual;
                    compositeCollider.GenerateGeometry();

                    tmpTileMapObject.transform.parent = tileGrid.transform;
                    tmpTileMapObject.name = "EscapeDown";


                    tileChangeData = new TileChangeData()
                    {
                        tile = escapeTile,
                        transform = Matrix4x4.Translate(new Vector3(.5f, -.5f, 0))
                    };
                    for (int dx = CenterPos.x - (width); dx < CenterPos.x + width; dx++)
                    {
                        Debug.Log("x " + height + "/" + dx);
                        tileChangeData.position = new Vector3Int(dx, -(height + 1), 0);
                        tmpTilemap.SetTile(tileChangeData, false);
                    }
                }

                if (type != 1 || (type == 1 && currSize == 0))
                {
                    tmpTileMapObject = new GameObject();
                    tmpTileMapObject.transform.position = tmpTileMapObject.transform.position;
                    tmpTilemap = tmpTileMapObject.AddComponent<Tilemap>();
                    tmpTilemap.tileAnchor = new Vector3(0, 1, 0);
                    tmpTilemap.color = new Color(0f, 0f, 0f, 0f);
                    tmpTileMapObject.AddComponent<TilemapRenderer>();
                    tmpTile = tmpTileMapObject.AddComponent<EscapeTile>();
                    tmpTile.fieldIndex = currSize + 1;
                    tmpTile.escapeIndex = 1;
                    tilemapCollider = tmpTileMapObject.AddComponent<TilemapCollider2D>();
                    tilemapCollider.usedByComposite = true;
                    rb = tmpTileMapObject.AddComponent<Rigidbody2D>();
                    rb.bodyType = RigidbodyType2D.Static;
                    compositeCollider = tmpTileMapObject.AddComponent<CompositeCollider2D>();
                    compositeCollider.isTrigger = true;
                    compositeCollider.generationType = CompositeCollider2D.GenerationType.Manual;
                    compositeCollider.GenerateGeometry();

                    tmpTileMapObject.transform.parent = tileGrid.transform;
                    tmpTileMapObject.name = "EscapeLeft";


                    tileChangeData = new TileChangeData()
                    {
                        tile = escapeTile,
                        transform = Matrix4x4.Translate(new Vector3(.5f, -.5f, 0))
                    };
                    for (int dy = CenterPos.y - (height); dy < CenterPos.y + height; dy++)
                    {
                        Debug.Log("y " + width + "/" + dy);
                        tileChangeData.position = new Vector3Int(+CenterPos.x - (width + 1), dy, 0);
                        tmpTilemap.SetTile(tileChangeData, false);
                    }
                }

                tmpTileMapObject = new GameObject();
                tmpTileMapObject.transform.position = tmpTileMapObject.transform.position;
                tmpTilemap = tmpTileMapObject.AddComponent<Tilemap>();
                tmpTilemap.tileAnchor = new Vector3(0, 1, 0);
                tmpTilemap.color = new Color(0f, 0f, 0f, 0f);
                tmpTileMapObject.AddComponent<TilemapRenderer>();
                tmpTile = tmpTileMapObject.AddComponent<EscapeTile>();
                tmpTile.fieldIndex = currSize + 1;
                tmpTile.escapeIndex = 0;
                tilemapCollider = tmpTileMapObject.AddComponent<TilemapCollider2D>();
                tilemapCollider.usedByComposite = true;
                rb = tmpTileMapObject.AddComponent<Rigidbody2D>();
                rb.bodyType = RigidbodyType2D.Static;
                compositeCollider = tmpTileMapObject.AddComponent<CompositeCollider2D>();
                compositeCollider.isTrigger = true;
                compositeCollider.generationType = CompositeCollider2D.GenerationType.Manual;
                compositeCollider.GenerateGeometry();

                tmpTileMapObject.transform.parent = tileGrid.transform;
                tmpTileMapObject.name = "EscapeRight";


                tileChangeData = new TileChangeData()
                {
                    tile = escapeTile,
                    transform = Matrix4x4.Translate(new Vector3(.5f, -.5f, 0))
                };
                for (int dy = CenterPos.y - (height); dy < CenterPos.y + height; dy++)
                {
                    Debug.Log("y " + width + "/" + dy);
                    tileChangeData.position = new Vector3Int(+CenterPos.x + width, dy, 0);
                    tmpTilemap.SetTile(tileChangeData, false);
                }


                if (currSize == 0)
                {
                    map = new GameObject();
                    map.transform.parent = mapParent.transform;
                    map.name = "Common";

                    tileGrid = new GameObject();
                    tileGrid.AddComponent<Grid>();
                    tileGrid.transform.parent = map.transform;
                    tileGrid.name = "Grid";


                     tmpTileMapObject = new GameObject();
                    tmpTileMapObject.tag = "Floor";
                    tmpTileMapObject.transform.position = tmpTileMapObject.transform.position;
                     tmpTilemap = tmpTileMapObject.AddComponent<Tilemap>();
                    tmpTilemap.tileAnchor = new Vector3(0, 1, 0);
                    tmpTileMapObject.AddComponent<TilemapRenderer>();
                     rb = tmpTileMapObject.AddComponent<Rigidbody2D>();
                    rb.bodyType = RigidbodyType2D.Static;
                     tilemapCollider = tmpTileMapObject.AddComponent<TilemapCollider2D>();
                    tilemapCollider.usedByComposite = true;
                     compositeCollider = tmpTileMapObject.AddComponent<CompositeCollider2D>();
                    compositeCollider.usedByEffector = true;
                     platformEffector = tmpTileMapObject.AddComponent<PlatformEffector2D>();
                    platformEffector.useOneWay = false;
                    compositeCollider.generationType = CompositeCollider2D.GenerationType.Manual;
                    compositeCollider.GenerateGeometry();

                    tmpTileMapObject.transform.parent = tileGrid.transform;
                    tmpTileMapObject.name = "BaseTile";


                    for (int dy = CenterPos.y - (height); dy < CenterPos.y + height; dy++)
                    {
                        for (int dx = CenterPos.x - (width); dx < CenterPos.x + width; dx++)
                        {
                            tmpTilemap.SetTile(new Vector3Int(dx, dy, 0), baseTile);
                        }
                    }

                    tmpTileMapObject = new GameObject();
                    tmpTileMapObject.transform.position = tmpTileMapObject.transform.position;
                    tmpTilemap = tmpTileMapObject.AddComponent<Tilemap>();
                    tmpTilemap.tileAnchor = new Vector3(0, 1, 0);
                    tmpTileMapObject.AddComponent<TilemapRenderer>();

                    tmpTileMapObject.transform.parent = tileGrid.transform;
                    tmpTileMapObject.name = "ObjectTile";



                    tmpTileMapObject = new GameObject();
                    tmpTileMapObject.transform.position = tmpTileMapObject.transform.position;
                    tmpTilemap = tmpTileMapObject.AddComponent<Tilemap>();
                    tmpTilemap.tileAnchor = new Vector3(0, 1, 0);
                    tmpTileMapObject.AddComponent<TilemapRenderer>();

                    tmpTileMapObject.transform.parent = tileGrid.transform;
                    tmpTileMapObject.name = "ScriptTile";


                    tmpTileMapObject = new GameObject();
                    tmpTileMapObject.tag = "Floor";
                    tmpTileMapObject.transform.position = tmpTileMapObject.transform.position;
                    tmpTilemap = tmpTileMapObject.AddComponent<Tilemap>();
                    tmpTilemap.tileAnchor = new Vector3(0, 1, 0);
                    tmpTileMapObject.AddComponent<TilemapRenderer>();
                    rb = tmpTileMapObject.AddComponent<Rigidbody2D>();
                    rb.bodyType = RigidbodyType2D.Static;
                    tilemapCollider = tmpTileMapObject.AddComponent<TilemapCollider2D>();
                    tilemapCollider.usedByComposite = true;
                    compositeCollider = tmpTileMapObject.AddComponent<CompositeCollider2D>();
                    compositeCollider.isTrigger = true;
                    tmpTileMapObject.AddComponent<SpikeTile>();
                    compositeCollider.generationType = CompositeCollider2D.GenerationType.Manual;
                    compositeCollider.GenerateGeometry();

                    tmpTileMapObject.transform.parent = tileGrid.transform;
                    tmpTileMapObject.name = "DamagerTile";


                    tmpTileMapObject = new GameObject();
                    tmpTileMapObject.tag = "Floor";
                    tmpTileMapObject.transform.position = tmpTileMapObject.transform.position;
                    tmpTilemap = tmpTileMapObject.AddComponent<Tilemap>();
                    tmpTilemap.tileAnchor = new Vector3(0, 1, 0);
                    tmpTileMapObject.AddComponent<TilemapRenderer>();
                    rb = tmpTileMapObject.AddComponent<Rigidbody2D>();
                    rb.bodyType = RigidbodyType2D.Static;
                    tilemapCollider = tmpTileMapObject.AddComponent<TilemapCollider2D>();
                    tilemapCollider.usedByComposite = true;
                    compositeCollider = tmpTileMapObject.AddComponent<CompositeCollider2D>();
                    compositeCollider.usedByEffector = true;
                    platformEffector = tmpTileMapObject.AddComponent<PlatformEffector2D>();
                    platformEffector.useOneWay = true;
                    platformEffector.surfaceArc = 170f;
                    compositeCollider.generationType = CompositeCollider2D.GenerationType.Manual;
                    compositeCollider.GenerateGeometry();

                    tmpTileMapObject.transform.parent = tileGrid.transform;
                    tmpTileMapObject.name = "ThineTile";


                    tmpTileMapObject = new GameObject();
                    tmpTileMapObject.transform.position = tmpTileMapObject.transform.position;
                    tmpTilemap = tmpTileMapObject.AddComponent<Tilemap>();
                    tmpTilemap.tileAnchor = new Vector3(0, 1, 0);
                    tmpTileMapObject.AddComponent<TilemapRenderer>();

                    tmpTileMapObject.transform.parent = tileGrid.transform;
                    tmpTileMapObject.name = "LadderTile";


                    tmpTileMapObject = new GameObject();
                    tmpTileMapObject.transform.position = tmpTileMapObject.transform.position;
                    tmpTilemap = tmpTileMapObject.AddComponent<Tilemap>();
                    tmpTilemap.tileAnchor = new Vector3(0, 1, 0);
                    tmpTileMapObject.AddComponent<TilemapRenderer>();

                    tmpTileMapObject.transform.parent = tileGrid.transform;
                    tmpTileMapObject.name = "PaintTile";



                    tmpTileMapObject = new GameObject();
                    tmpTileMapObject.transform.position = tmpTileMapObject.transform.position;
                    tmpTilemap = tmpTileMapObject.AddComponent<Tilemap>();
                    tmpTilemap.tileAnchor = new Vector3(0, 1, 0);
                    tmpTileMapObject.AddComponent<TilemapRenderer>();

                    tmpTileMapObject.transform.parent = tileGrid.transform;
                    tmpTileMapObject.name = "WaterArea";
                }
                else
                {
                    map = mapParent.transform.Find("Common").gameObject;

                    tileGrid = map.transform.GetChild(0).gameObject;


                    tmpTileMapObject = tileGrid.transform.Find("BaseTile").gameObject;
                    tmpTilemap = tmpTileMapObject.GetComponent<Tilemap>();

                    for (int dy = CenterPos.y - (height); dy < CenterPos.y + height; dy++)
                    {
                        for (int dx = CenterPos.x - (width); dx < CenterPos.x + width; dx++)
                        {
                            tmpTilemap.SetTile(new Vector3Int(dx, dy, 0), baseTile);
                        }
                    }

                }


                mapParent.transform.Find("Common").SetAsFirstSibling();




                //MapPalette 

                //TileBase tileBase = 
                //tmpTilemap.SetTile(new Vector3Int(0, 0, 0),   );

            }
        }



        ///<summary>
        /// Legacy코드
        ///</summary>
            /* if (GUILayout.Button("RandomEnemy"))
             {
                 string[] enemiesName = EnemyPool_Legacy.instance.GetEnemiesName();
                 GameObject tmp = EnemyPool_Legacy.instance.GetEnemy(enemiesName[Random.Range(0, enemiesName.Length)]);
                 tmp.transform.position = Vector3.zero;
                 testList.Enqueue(tmp);
             }

             if (GUILayout.Button("RemoveObject"))
             {
                 GameObject tmp = testList.Dequeue();
                 EnemyPool_Legacy.instance.ReturnEnemy(tmp);
             }


             if (GUILayout.Button("Shoot"))
             {
                 EnemyCommon_Legacy tmp = AssetDatabase.LoadAssetAtPath<EnemyCommon_Legacy>("Assets/Game/Prefabs/Enemies/BigTomata.prefab");

                 Debug.Log(tmp);

                 obj = Instantiate(tmp, new Vector3(0,0,0), Quaternion.identity);
                 obj.Attack();
             }*/
    }
}
#endif