using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager _gameManager;

    private void Awake() {
        _gameManager = this;
    }

    public bool left_right_elimination = true;
    public float drop_speed = 0.5f;

    public static int width = 16;
    public static int height = 16;
    public static int elinum = 8;
    public int down_score = 0;
    public int left_and_right_score = 0;

    public GameObject[] cube;
    public bool isGameOver = false;
    public bool isPause = false;

    private GameObject[] tempobj = new GameObject[4];
    private bool isDrop;
    private bool isTouch;
    private bool[] isOver = new bool[2] { false, false };
    private int[] cubeNum = new int[4];
    private int[] cubeNum_next = new int[4];
    private GameObject[] cubePrefab = new GameObject[4];
    private Vector3 cubePrefabCenter;
    private int[,] cubeArray = new int[width, height];
    GameObject[,] cubeObject = new GameObject[width, height];

    // Use this for initialization
    void Start() {
        InitNewGame();
    }

    // Update is called once per frame
    void Update() {
        if (cubePrefab != null && isTouch && isPause == false) {
            GetTouch(cubePrefab);
        }
    }

    void InitNewGame() {
        isGameOver = false;
        isTouch = true;
        InitMap();
        InitCubeArray();
        InitCubePrefab();
        StartCoroutine(DropCube(drop_speed));
        GameOverInterface._gameOverInterface.InitNewInterface();
    }

    void InitCubeArray() {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                cubeArray[i, j] = -1;
            }
        }
    }

    void InitMap() {
        for (float x = -width / 2 - 0.5f; x <= width / 2 + 0.5f; x++) {
            Instantiate(cube[0], new Vector3(x, -height / 2 - 0.5f, 0), Quaternion.identity);
        }

        int counter = 0;
        for (float y = -height / 2 + 0.5f; y <= height / 2 + 0.5f; y++) {
            GameObject left_obj = Instantiate(cube[0], new Vector3(-width / 2 - 0.5f, y, 0), Quaternion.identity);
            GameObject right_obj = Instantiate(cube[0], new Vector3(width / 2 + 0.5f, y, 0), Quaternion.identity);
            counter++;
            if (counter <= elinum) {
                left_obj.GetComponent<SpriteRenderer>().color = Color.gray;
                right_obj.GetComponent<SpriteRenderer>().color = Color.gray;
            }
        }
    }

    int[] random_num = new int[2] { -1, -1 };
    void InitCubePrefab() {
        cubePrefab = new GameObject[4];
        if (random_num[0] == -1) {
            random_num[0] = Random.Range(0, 6);
        } else {
            random_num[0] = random_num[1];
            for (int i = 0; i < 4; i++)
                Destroy(tempobj[i]);
        }
        random_num[1] = Random.Range(0, 6);
        //int random_num = 2;
        int[,] offset;
        switch (random_num[0]) {
            case 0:
                cubeNum = new int[4] { 0, 3, 0, 1 };
                offset = new int[4, 2] { { 0, 0 }, { -1, 0 }, { 1, 0 }, { 2, 0 } };
                cubePrefabCenter = CenterPosVector(width / 2, height);
                for (int i = 0; i < 4; i++) {
                    if (cubeArray[width / 2 + offset[i, 0] - 1, height + offset[i, 1] - 1] != -1) {
                        isGameOver = true;
                        GameOverInterface._gameOverInterface.TurnBlack();
                    }
                }
                if (isGameOver == false) {
                    for (int i = 0; i < 4; i++) {
                        cubePrefab[i] = Instantiate(cube[cubeNum[i]], PosVector(width / 2 + offset[i, 0], height + offset[i, 1]), Quaternion.identity);
                        cubePrefab[i].GetComponent<SpriteRenderer>().color = Color.red;
                    }
                } else
                    print("GameOver!");
                break;
            case 1:
                cubeNum = new int[4] { 0, 4, 0, 2 };
                offset = new int[4, 2] { { 0, 0 }, { -1, 0 }, { 1, 0 }, { 2, 0 } };
                cubePrefabCenter = CenterPosVector(width / 2, height);
                for (int i = 0; i < 4; i++) {
                    if (cubeArray[width / 2 + offset[i, 0] - 1, height + offset[i, 1] - 1] != -1) {
                        isGameOver = true;
                        GameOverInterface._gameOverInterface.TurnBlack();
                    }
                }
                if (isGameOver == false) {
                    for (int i = 0; i < 4; i++) {
                        cubePrefab[i] = Instantiate(cube[cubeNum[i]], PosVector(width / 2 + offset[i, 0], height + offset[i, 1]), Quaternion.identity);
                        cubePrefab[i].GetComponent<SpriteRenderer>().color = Color.blue;
                    }
                } else
                    print("GameOver!");
                break;
            case 2:
                cubeNum = new int[4] { 3, 2, 4, 1 };
                offset = new int[4, 2] { { -1, 0 }, { 0, 0 }, { -1, -1 }, { 0, -1 } };
                cubePrefabCenter = CenterPosVector(width / 2 - 0.5f, height - 0.5f);
                for (int i = 0; i < 4; i++) {
                    if (cubeArray[width / 2 + offset[i, 0] - 1, height + offset[i, 1] - 1] != -1) {
                        isGameOver = true;
                        GameOverInterface._gameOverInterface.TurnBlack();
                    }
                }
                if (isGameOver == false) {
                    for (int i = 0; i < 4; i++) {
                        cubePrefab[i] = Instantiate(cube[cubeNum[i]], PosVector(width / 2 + offset[i, 0], height + offset[i, 1]), Quaternion.identity);
                        cubePrefab[i].GetComponent<SpriteRenderer>().color = Color.yellow;
                    }
                } else
                    print("GameOver!");
                break;
            case 3:
                cubeNum = new int[4] { 0, 0, 3, 2 };
                offset = new int[4, 2] { { 0, 0 }, { 0, -1 }, { -1, -1 }, { 1, -1 } };
                cubePrefabCenter = CenterPosVector(width / 2, height - 1f);
                for (int i = 0; i < 4; i++) {
                    if (cubeArray[width / 2 + offset[i, 0] - 1, height + offset[i, 1] - 1] != -1) {
                        isGameOver = true;
                        GameOverInterface._gameOverInterface.TurnBlack();
                    }
                }
                if (isGameOver == false) {
                    for (int i = 0; i < 4; i++) {
                        cubePrefab[i] = Instantiate(cube[cubeNum[i]], PosVector(width / 2 + offset[i, 0], height + offset[i, 1]), Quaternion.identity);
                        cubePrefab[i].GetComponent<SpriteRenderer>().color = Color.green;
                    }
                } else
                    print("GameOver!");
                break;
            case 4:
                cubeNum = new int[4] { 0, 0, 3, 3 };
                offset = new int[4, 2] { { 1, -1 }, { 0, -1 }, { -1, -1 }, { 1, 0 } };
                cubePrefabCenter = CenterPosVector(width / 2 + 1f, height - 1f);
                for (int i = 0; i < 4; i++) {
                    if (cubeArray[width / 2 + offset[i, 0] - 1, height + offset[i, 1] - 1] != -1) {
                        isGameOver = true;
                        GameOverInterface._gameOverInterface.TurnBlack();
                    }
                }
                if (isGameOver == false) {
                    for (int i = 0; i < 4; i++) {
                        cubePrefab[i] = Instantiate(cube[cubeNum[i]], PosVector(width / 2 + offset[i, 0], height + offset[i, 1]), Quaternion.identity);
                        cubePrefab[i].GetComponent<SpriteRenderer>().color = Color.cyan;
                    }
                } else
                    print("GameOver!");
                break;
            case 5:
                cubeNum = new int[4] { 0, 0, 2, 2 };
                offset = new int[4, 2] { { -1, -1 }, { 0, -1 }, { 1, -1 }, { -1, 0 } };
                cubePrefabCenter = CenterPosVector(width / 2 - 1f, height - 1f);
                for (int i = 0; i < 4; i++) {
                    if (cubeArray[width / 2 + offset[i, 0] - 1, height + offset[i, 1] - 1] != -1) {
                        isGameOver = true;
                        GameOverInterface._gameOverInterface.TurnBlack();
                    }
                }
                if (isGameOver == false) {
                    for (int i = 0; i < 4; i++) {
                        cubePrefab[i] = Instantiate(cube[cubeNum[i]], PosVector(width / 2 + offset[i, 0], height + offset[i, 1]), Quaternion.identity);
                        cubePrefab[i].GetComponent<SpriteRenderer>().color = Color.magenta;
                    }
                } else
                    print("GameOver!");
                break;
            default:
                print("error");
                break;
        }

        if (isGameOver) {
            print(ShowScoreInterface._showScoreInterface.score);
            RankManager._rankManager.UpdateRank(ShowScoreInterface._showScoreInterface.score);
        }

        int[,] offset_next;
        switch (random_num[1]) {
            case 0:
                cubeNum_next = new int[4] { 0, 3, 0, 1 };
                offset_next = new int[4, 2] { { 0, 0 }, { -1, 0 }, { 1, 0 }, { 2, 0 } };
                for (int i = 0; i < 4; i++) {
                    tempobj[i] = Instantiate(cube[cubeNum_next[i]], PosVector(width + 4 + offset_next[i, 0], height + offset_next[i, 1]), Quaternion.identity);
                    tempobj[i].GetComponent<SpriteRenderer>().color = Color.red;
                }
                break;
            case 1:
                cubeNum_next = new int[4] { 0, 4, 0, 2 };
                offset_next = new int[4, 2] { { 0, 0 }, { -1, 0 }, { 1, 0 }, { 2, 0 } };
                for (int i = 0; i < 4; i++) {
                    tempobj[i] = Instantiate(cube[cubeNum_next[i]], PosVector(width + 4 + offset_next[i, 0], height + offset_next[i, 1]), Quaternion.identity);
                    tempobj[i].GetComponent<SpriteRenderer>().color = Color.blue;
                }
                break;
            case 2:
                cubeNum_next = new int[4] { 3, 2, 4, 1 };
                offset_next = new int[4, 2] { { -1, 0 }, { 0, 0 }, { -1, -1 }, { 0, -1 } };
                for (int i = 0; i < 4; i++) {
                    tempobj[i] = Instantiate(cube[cubeNum_next[i]], PosVector(width + 4 + offset_next[i, 0], height + offset_next[i, 1]), Quaternion.identity);
                    tempobj[i].GetComponent<SpriteRenderer>().color = Color.yellow;
                }
                break;
            case 3:
                cubeNum_next = new int[4] { 0, 0, 3, 2 };
                offset_next = new int[4, 2] { { 0, 0 }, { 0, -1 }, { -1, -1 }, { 1, -1 } };
                for (int i = 0; i < 4; i++) {
                    tempobj[i] = Instantiate(cube[cubeNum_next[i]], PosVector(width + 4 + offset_next[i, 0], height + offset_next[i, 1]), Quaternion.identity);
                    tempobj[i].GetComponent<SpriteRenderer>().color = Color.green;
                }
                break;
            case 4:
                cubeNum_next = new int[4] { 0, 0, 3, 3 };
                offset_next = new int[4, 2] { { 1, -1 }, { 0, -1 }, { -1, -1 }, { 1, 0 } };
                for (int i = 0; i < 4; i++) {
                    tempobj[i] = Instantiate(cube[cubeNum_next[i]], PosVector(width + 4 + offset_next[i, 0], height + offset_next[i, 1]), Quaternion.identity);
                    tempobj[i].GetComponent<SpriteRenderer>().color = Color.cyan;
                }
                break;
            case 5:
                cubeNum_next = new int[4] { 0, 0, 2, 2 };
                offset_next = new int[4, 2] { { -1, -1 }, { 0, -1 }, { 1, -1 }, { -1, 0 } };
                for (int i = 0; i < 4; i++) {
                    tempobj[i] = Instantiate(cube[cubeNum_next[i]], PosVector(width + 4 + offset_next[i, 0], height + offset_next[i, 1]), Quaternion.identity);
                    tempobj[i].GetComponent<SpriteRenderer>().color = Color.magenta;
                }
                break;
            default:
                print("error");
                break;
        }
    }

    void GetTouch(GameObject[] cube) {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            bool isLeft = true;
            for (int i = 0; i < 4; i++) {
                int x = GetPointX(cube[i].transform.position);
                int y = GetPointY(cube[i].transform.position);
                if (!CheckLeftAndRight(x, y, cubeNum[i], 0)[0]) {
                    isLeft = false;
                }
            }
            if (isLeft) {
                cubePrefabCenter = new Vector3(cubePrefabCenter.x - 1, cubePrefabCenter.y, 0);
                for (int i = 0; i < 4; i++) {
                    float posX = cube[i].transform.position.x;
                    float posY = cube[i].transform.position.y;
                    cube[i].transform.position = new Vector3(posX - 1, posY, 0);
                }
                isDrop = true;
                for (int i = 0; i < 4; i++) {
                    int x = GetPointX(cube[i].transform.position);
                    int y = GetPointY(cube[i].transform.position);
                    if (!CheckDropCube(x, y, cubeNum[i])) {
                        isDrop = false;
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            bool isRight = true;
            if (cube != null)
                for (int i = 0; i < 4; i++) {
                    if (cube[i] != null) {
                        int x = GetPointX(cube[i].transform.position);
                        int y = GetPointY(cube[i].transform.position);
                        if (!CheckLeftAndRight(x, y, cubeNum[i], 1)[0]) {
                            isRight = false;
                        }
                    }
                }
            if (isRight) {
                cubePrefabCenter = new Vector3(cubePrefabCenter.x + 1, cubePrefabCenter.y, 0);
                for (int i = 0; i < 4; i++) {
                    float posX = cube[i].transform.position.x;
                    float posY = cube[i].transform.position.y;
                    cube[i].transform.position = new Vector3(posX + 1, posY, 0);
                }
                isDrop = true;
                for (int i = 0; i < 4; i++) {
                    int x = GetPointX(cube[i].transform.position);
                    int y = GetPointY(cube[i].transform.position);
                    if (!CheckDropCube(x, y, cubeNum[i])) {
                        isDrop = false;
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            RotateCube(cube);
        }

        if (Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Ended) {
            if (Input.touches[0].position.x < 0.33 * Screen.width) {
                bool isLeft = true;
                for (int i = 0; i < 4; i++) {
                    int x = GetPointX(cube[i].transform.position);
                    int y = GetPointY(cube[i].transform.position);
                    if (!CheckLeftAndRight(x, y, cubeNum[i], 0)[0]) {
                        isLeft = false;
                    }
                }
                if (isLeft) {
                    cubePrefabCenter = new Vector3(cubePrefabCenter.x - 1, cubePrefabCenter.y, 0);
                    for (int i = 0; i < 4; i++) {
                        float posX = cube[i].transform.position.x;
                        float posY = cube[i].transform.position.y;
                        cube[i].transform.position = new Vector3(posX - 1, posY, 0);
                    }
                    isDrop = true;
                    for (int i = 0; i < 4; i++) {
                        int x = GetPointX(cube[i].transform.position);
                        int y = GetPointY(cube[i].transform.position);
                        if (!CheckDropCube(x, y, cubeNum[i])) {
                            isDrop = false;
                        }
                    }
                }
            }

            if (Input.touches[0].position.x > 0.67 * Screen.width) {
                bool isRight = true;
                if (cube != null)
                    for (int i = 0; i < 4; i++) {
                        int x = GetPointX(cube[i].transform.position);
                        int y = GetPointY(cube[i].transform.position);
                        if (!CheckLeftAndRight(x, y, cubeNum[i], 1)[0]) {
                            isRight = false;
                        }
                    }
                if (isRight) {
                    cubePrefabCenter = new Vector3(cubePrefabCenter.x + 1, cubePrefabCenter.y, 0);
                    for (int i = 0; i < 4; i++) {
                        float posX = cube[i].transform.position.x;
                        float posY = cube[i].transform.position.y;
                        cube[i].transform.position = new Vector3(posX + 1, posY, 0);
                    }
                    isDrop = true;
                    for (int i = 0; i < 4; i++) {
                        int x = GetPointX(cube[i].transform.position);
                        int y = GetPointY(cube[i].transform.position);
                        if (!CheckDropCube(x, y, cubeNum[i])) {
                            isDrop = false;
                        }
                    }
                }
            }

            if (Input.touches[0].position.x < 0.66 * Screen.width && Input.touches[0].position.x > 0.33 * Screen.width) {
                RotateCube(cube);
            }
        }
    }

    void RotateCube(GameObject[] cube) {
        if (cube != null) {
            bool isRotated = true;
            float centerX = GetCenterPointX(cubePrefabCenter);
            float centerY = GetCenterPointY(cubePrefabCenter);
            Vector2 center = new Vector2(centerX, centerY);
            for (int i = 0; i < 4; i++) {
                int x = GetPointX(cube[i].transform.position);
                int y = GetPointY(cube[i].transform.position);
                Vector2 tempCube = new Vector2(x, y);
                Vector2 tempVector = tempCube - center;
                Vector2 rotateVector = new Vector2(tempVector.y, -tempVector.x);
                Vector2 rotateCube = center + rotateVector;
                if (rotateCube.x < 0 || rotateCube.x >= width || rotateCube.y < 0 || rotateCube.y >= height) {
                    isRotated = false;
                } else {
                    if (cubeArray[(int)rotateCube.x, (int)rotateCube.y] == 0) {
                        isRotated = false;
                    } else {
                        if (cubeNum[i] > 0 && cubeArray[(int)rotateCube.x, (int)rotateCube.y] > 0
                            && Mathf.Abs(cubeArray[(int)rotateCube.x, (int)rotateCube.y] - cubeNum[i]) != 2) {
                            isRotated = false;
                        }
                    }
                }
            }
            if (isRotated) {
                for (int i = 0; i < 4; i++) {
                    int x = GetPointX(cube[i].transform.position);
                    int y = GetPointY(cube[i].transform.position);
                    Vector2 tempCube = new Vector2(x, y);
                    Vector2 tempVector = tempCube - center;
                    Vector2 rotateVector = new Vector2(tempVector.y, -tempVector.x);
                    Vector2 rotateCube = center + rotateVector;
                    cube[i].transform.position = PosVector((int)rotateCube.x, (int)rotateCube.y);
                    //cube[i].transform.position = CenterPosVector(rotateCube.x, rotateCube.y);
                    switch (cubeNum[i]) {
                        case 0:
                            break;
                        case 1:
                            cubeNum[i] = 4;
                            cube[i].transform.Rotate(new Vector3(0, 0, -90));
                            break;
                        case 2:
                            cubeNum[i] = 1;
                            cube[i].transform.Rotate(new Vector3(0, 0, -90));
                            break;
                        case 3:
                            cubeNum[i] = 2;
                            cube[i].transform.Rotate(new Vector3(0, 0, -90));
                            break;
                        case 4:
                            cubeNum[i] = 3;
                            cube[i].transform.Rotate(new Vector3(0, 0, -90));
                            break;
                        default:
                            print("error");
                            break;
                    }
                }
                isDrop = true;
                for (int i = 0; i < 4; i++) {
                    int x = GetPointX(cube[i].transform.position);
                    int y = GetPointY(cube[i].transform.position);
                    if (!CheckDropCube(x, y, cubeNum[i])) {
                        isDrop = false;
                    }
                }
            }
        }
    }

    IEnumerator DropCube(float t) {
        //GameObject cubePrefab = Instantiate(cube, new Vector3(0.5f, height / 2 - 0.5f, 0), Quaternion.identity);
        while (isGameOver == false) {
            isDrop = true;
            isOver = new bool[2] { false, false };
            while (isDrop) {
                isOver[1] = isOver[0];
                cubePrefabCenter = new Vector3(cubePrefabCenter.x, cubePrefabCenter.y - 1, 0);
                for (int i = 0; i < 4; i++) {
                    float posX = cubePrefab[i].transform.position.x;
                    float posY = cubePrefab[i].transform.position.y;
                    cubePrefab[i].transform.position = new Vector3(posX, posY - 1, 0);
                    int x = GetPointX(cubePrefab[i].transform.position);
                    int y = GetPointY(cubePrefab[i].transform.position);
                    if (!CheckDropCube(x, y, cubeNum[i])) {
                        isDrop = false;
                    }
                }
                CheckDelete();
                yield return new WaitForSeconds(t);
            }
            //UpdateCubeArray(cubePrefab);
            //CheckDelete();
            isTouch = false;
            yield return new WaitForSeconds(t);
            CheckDelete();
            UpdateCubeArray(cubePrefab);
            InitCubePrefab();
            isTouch = true;
        }
    }

    bool CheckDropCube(int x, int y, int kind) {
        if (!(x >= 0 && x < width && y > 0 && y <= height))
            return false;
        else {
            if ((cubeArray[x, y] == 3 && kind == 1) || (cubeArray[x, y] == 2 && kind == 4)) {
                return false;
            }
            bool isrepeated;
            if (cubeArray[x, y - 1] == -1) {
                isrepeated = false;
            } else if (cubeArray[x, y - 1] == 0 || cubeArray[x, y - 1] == 1 || cubeArray[x, y - 1] == 4) {
                isrepeated = true;
            } else {
                if (kind > 0 && Mathf.Abs(cubeArray[x, y - 1] - kind) == 2) {
                    isrepeated = false;
                } else {
                    isrepeated = true;
                }
            }
            if (!isrepeated) {
                return true;
            }
            return false;
        }
    }

    void UpdateCubeArray(GameObject[] cubePrefab) {
        for (int i = 0; i < 4; i++) {
            int x = GetPointX(cubePrefab[i].transform.position);
            int y = GetPointY(cubePrefab[i].transform.position);
            //cubeObject[x, y] = cubePrefab[i];
            //cubeArray[x, y] = cubeNum[i];
            if (cubeArray[x, y] == -1) {
                cubeObject[x, y] = cubePrefab[i];
                cubeArray[x, y] = cubeNum[i];
            } else {
                //cubeObject[x, y] = cubePrefab[i];
                GameObject obj = Instantiate(cube[5], PosVector(x, y), Quaternion.identity);
                cubeObject[x, y].transform.parent = obj.transform;
                cubePrefab[i].transform.parent = obj.transform;
                cubeObject[x, y] = obj;
                cubeArray[x, y] = 0;
            }
            //print(GetPointX(cubePrefab[i].transform.position) + "  " + GetPointY(cubePrefab[i].transform.position));
        }
    }

    void CheckDelete() {
        //down
        bool flag = false;
        do {
            flag = false;
            for (int k = 0; k < height; k++) {
                bool isDelete = true;
                for (int i = 0; i < width; i++) {
                    if (cubeArray[i, k] != 0) {
                        isDelete = false;
                    }
                }
                if (isDelete) {
                    ShowScoreInterface._showScoreInterface.AddScore(down_score);
                    ShowScoreInterface._showScoreInterface.ShowScore();
                    for (int i = 0; i < width; i++) {
                        Destroy(cubeObject[i, k]);
                    }
                    for (int i = 0; i < width; i++) {
                        for (int j = k + 1; j < height; j++) {
                            if (cubeObject[i, j] != null) {
                                float posX = cubeObject[i, j].transform.position.x;
                                float posY = cubeObject[i, j].transform.position.y;
                                cubeObject[i, j].transform.position = new Vector3(posX, posY - 1, 0);
                            }
                            cubeObject[i, j - 1] = cubeObject[i, j];
                            cubeArray[i, j - 1] = cubeArray[i, j];
                        }
                        cubeObject[i, height - 1] = null;
                        cubeArray[i, height - 1] = -1;
                    }
                    flag = true;
                    break;
                }
            }
        } while (flag);

        if (left_right_elimination) {
            //left
            flag = false;
            do {
                flag = false;
                for (int k = 0; k < width / 2; k++) {
                    bool isDelete = true;
                    for (int i = 0; i < elinum; i++) {
                        if (cubeArray[k, i] != 0) {
                            isDelete = false;
                        }
                    }
                    if (isDelete) {
                        ShowScoreInterface._showScoreInterface.AddScore(left_and_right_score);
                        ShowScoreInterface._showScoreInterface.ShowScore();
                        for (int i = 0; i < height; i++) {
                            Destroy(cubeObject[k, i]);
                        }
                        for (int i = 0; i < height; i++) {
                            for (int j = k + 1; j < width; j++) {
                                if (cubeObject[j, i] != null) {
                                    float posX = cubeObject[j, i].transform.position.x;
                                    float posY = cubeObject[j, i].transform.position.y;
                                    cubeObject[j, i].transform.position = new Vector3(posX - 1, posY, 0);
                                }
                                cubeObject[j - 1, i] = cubeObject[j, i];
                                cubeArray[j - 1, i] = cubeArray[j, i];
                            }
                            cubeObject[width - 1, i] = null;
                            cubeArray[width - 1, i] = -1;
                        }
                        flag = true;
                        break;
                    }
                }
            } while (flag);

            //right
            flag = false;
            do {
                flag = false;
                for (int k = width - 1; k >= width / 2; k--) {
                    bool isDelete = true;
                    for (int i = 0; i < elinum; i++) {
                        if (cubeArray[k, i] != 0) {
                            isDelete = false;
                        }
                    }
                    if (isDelete) {
                        ShowScoreInterface._showScoreInterface.AddScore(left_and_right_score);
                        ShowScoreInterface._showScoreInterface.ShowScore();
                        for (int i = 0; i < height; i++) {
                            Destroy(cubeObject[k, i]);
                        }
                        for (int i = 0; i < height; i++) {
                            for (int j = k - 1; j >= 0; j--) {
                                if (cubeObject[j, i] != null) {
                                    float posX = cubeObject[j, i].transform.position.x;
                                    float posY = cubeObject[j, i].transform.position.y;
                                    cubeObject[j, i].transform.position = new Vector3(posX + 1, posY, 0);
                                }
                                cubeObject[j + 1, i] = cubeObject[j, i];
                                cubeArray[j + 1, i] = cubeArray[j, i];
                            }
                            cubeObject[0, i] = null;
                            cubeArray[0, i] = -1;
                        }
                        flag = true;
                        break;
                    }
                }
            } while (flag);
        }
    }

    //flag==0,left;flag==1,right
    bool[] CheckLeftAndRight(int x, int y, int kind, int flag) {
        if (flag == 0) {
            if (!(x > 0 && x < width)) {
                if (x != 0)
                    print("error");
                return new bool[2] { false, true };
            }
            /*if (cubeArray[x - 1, y] == -1) {
                return true;
            }*/
            if ((cubeArray[x, y] == 2 && kind == 4) || (cubeArray[x, y] == 1 && kind == 3)) {
                return new bool[2] { false, false };
            }
            switch (cubeArray[x - 1, y]) {
                case -1:
                    return new bool[2] { true, false };
                case 0:
                    return new bool[2] { false, false };
                case 1:
                    if (kind == 3)
                        return new bool[2] { true, false };
                    else
                        return new bool[2] { false, false };
                case 2:
                    if (kind == 4)
                        return new bool[2] { true, false };
                    else
                        return new bool[2] { false, false };
                case 3:
                    return new bool[2] { false, false };
                case 4:
                    return new bool[2] { false, false };
            }
            print("error");
            return new bool[2] { false, false };
        } else if (flag == 1) {
            if (!(x >= 0 && x < width - 1)) {
                if (x != width - 1)
                    print("error");
                return new bool[2] { false, true };
            }
            if ((cubeArray[x, y] == 3 && kind == 1) || (cubeArray[x, y] == 4 && kind == 2)) {
                return new bool[2] { false, false };
            }
            switch (cubeArray[x + 1, y]) {
                case -1:
                    return new bool[2] { true, false };
                case 0:
                    return new bool[2] { false, false };
                case 1:
                    return new bool[2] { false, false };
                case 2:
                    return new bool[2] { false, false };
                case 3:
                    if (kind == 1)
                        return new bool[2] { true, false };
                    else
                        return new bool[2] { false, false };
                case 4:
                    if (kind == 2)
                        return new bool[2] { true, false };
                    else
                        return new bool[2] { false, false };
            }
            print("error");
            return new bool[2] { false, false };
        } else {
            print("error");
            return new bool[2] { false, false };
        }
    }

    Vector3 PosVector(int x, int y) {
        return new Vector3(x - width / 2 + 0.5f, y - height / 2 + 0.5f, 0);
    }

    Vector3 CenterPosVector(float x, float y) {
        return new Vector3(x - width / 2 + 0.5f, y - height / 2 + 0.5f, 0);
    }

    int GetPointX(Vector3 Position) {
        return (int)(Position.x - 0.5f + width / 2);
    }

    int GetPointY(Vector3 Position) {
        return (int)(Position.y - 0.5f + height / 2);
    }

    float GetCenterPointX(Vector3 Position) {
        return Position.x - 0.5f + width / 2;
    }

    float GetCenterPointY(Vector3 Position) {
        return Position.y - 0.5f + height / 2;
    }
}