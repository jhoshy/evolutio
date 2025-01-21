using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LoLSDK;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour {

    public List<GameObject> lvl = new List<GameObject>();
    public int currentLevel;

    private bool canClick = true;
    
    private int partialScore;

    public Text textQuestion;
    public Text answerA;
    public Text answerB;
    public Text answerC;
    private int currentQuestion;
    public GameObject a;
    public GameObject b;
    public GameObject c;

    public Text titleInfo;
    public Text textInfo;
    public Text current;
    private int currentinfo;

    float maxScore = 600;

    public GameObject info;
    public GameObject wrong;
    public GameObject correct;

    public GameObject bar;

    public GameObject popUpInfo;
    public GameObject popUpCongratulation;

    public GameObject popUpQuiz;
    public string answerSelected;

    public GameObject fossil;
    public GameObject fossilAux;
    public bool fossilSpawned;

    public GameObject uiHit;    

    public GameObject player;
    private Vector3 offset;
    public GameObject poofParticle;

    private List<Quiz> quizList = new List<Quiz>();

    private List<Info> infoList = new List<Info>();

    public static Controller singleton;

    public bool isPlay;

    void Awake() {
        singleton = this;
    }

    // Use this for initialization
    void Start() {
        currentLevel = 0;

        if (!LOLSDK.Instance.IsInitialized) {
            LOLSDK.Init("com.baiducasoft.evolutio");
        }

        quizList = new List<Quiz>();
        fillQuestions();
        fillInfo();
        this.currentQuestion = 0;
        this.currentinfo = 0;

        LOLSDK.Instance.SubmitProgress(SharedState.score, SharedState.level, SharedState.maxLevel);

        this.isPlay = true;

        partialScore = 0;

        offset = Camera.main.transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update() {
        //Cheat to spawn fossil right away
        //if (Input.GetKeyDown(KeyCode.Tab)) {
        //    showFossil();
        //}
    }

    public void addScore(int score, bool updateBar) {
        partialScore += score;
        SharedState.score += score;
        LOLSDK.Instance.SubmitProgress(SharedState.score, SharedState.level, SharedState.maxLevel);

        float proporcao = (partialScore / maxScore) - bar.transform.localScale.x;

        if (bar.transform.localScale.x < 1) {

            if (bar.transform.localScale.x + proporcao > 1)
            {
                proporcao = 1 - bar.transform.localScale.x;
            }

            if (updateBar) {
                StopCoroutine("modificaEscala");
                StartCoroutine(EaseFunctions.modificaEscala(bar.transform, proporcao, 0, 0, 0.3f, "linear"));
            }

            if (partialScore >= this.maxScore && !this.fossilAux) {
                showFossil();
            }
        }
        
        if (partialScore >= (maxScore / 3)  && partialScore < (maxScore / 3) * 2) {
            this.info.SetActive(true);
            this.currentinfo = 0;
            if (!this.infoList[this.currentinfo].showed)
            {
                SharedState.level++;
                LOLSDK.Instance.SubmitProgress(SharedState.score, SharedState.level, SharedState.maxLevel);
                showPopUpInfo();
                this.infoList[this.currentinfo].showed = true;
            }
        } else if (partialScore >= (maxScore / 3) * 2) {
            this.currentinfo = 1;
            if (!this.infoList[this.currentinfo].showed)
            {
                SharedState.level++;
                LOLSDK.Instance.SubmitProgress(SharedState.score, SharedState.level, SharedState.maxLevel);
                showPopUpInfo();
                this.infoList[this.currentinfo].showed = true;
            }
        }
    }

    public void removeScore(int score, bool updateBar) {
        partialScore -= score;

        if (partialScore < 0)
        {   
            partialScore = 0;
        }
        
        SharedState.score -= score;

        if (SharedState.score < 0)
        {
            SharedState.score = 0;
        }

        if (score > 2) {
            Debug.LogWarning("Lost " + score);
            LOLSDK.Instance.SubmitProgress(SharedState.score, SharedState.level, SharedState.maxLevel);
        }

        if (this.isPlay) {
            float proporcao = (partialScore / maxScore) - bar.transform.localScale.x;

            if (updateBar && proporcao < 0f && proporcao > -1f)
            {
                StopCoroutine("EaseFunctions.modificaEscala");
                StartCoroutine(EaseFunctions.modificaEscala(bar.transform, proporcao, 0, 0, 0.3f, "linear"));
            }
        }
    }

    public void showPopUpInfo() {
        LOLSDK.Instance.PlaySound("FX/sfx_popup_info.mp3", false, false);
        
        popUpInfo.transform.localScale = new Vector3(0, 0, 1);

        Info info = infoList[currentinfo];
        this.titleInfo.text = info.title;
        this.textInfo.text = info.info;

        if (partialScore < (maxScore / 3) * 2) {
            current.text = (currentinfo + 1) + " / 1";
        } else {
            current.text = (currentinfo + 1) + " / 2";
        }

        popUpInfo.transform.localPosition = new Vector3(-1000, 0, 0);

        popUpInfo.SetActive(true);

        StartCoroutine(EaseFunctions.modificaEscala(popUpInfo.transform, 1, 1, 0, 1f, "easeBackOut"));
        StartCoroutine(EaseFunctions.movimenta(popUpInfo.transform, 1000, 0, 0, 1f, "easeBackOut", false));
    }

    public void hidePopUpInfo() {
        LOLSDK.Instance.PlaySound("FX/sfx_ui_click.mp3", false, false);
        
        StartCoroutine(EaseFunctions.movimenta(popUpInfo.transform, 0, -2000, 0, 0.2f, "easeBounceOut", true));
    }

    public void showPopUpQuiz() {
        LOLSDK.Instance.PlaySound("FX/sfx_question_popup.mp3", false, false);

        currentQuestion = 0;

        Quiz quiz = quizList[currentQuestion];
        this.textQuestion.text = quiz.question;
        this.answerA.text = quiz.answerA;
        this.answerB.text = quiz.answerB;
        this.answerC.text = quiz.answerC;

        popUpQuiz.transform.localScale = new Vector3(0, 0, 1);
        
        popUpQuiz.transform.localPosition = new Vector3(-1000, 0, 0);

        popUpQuiz.SetActive(true);

        StartCoroutine(EaseFunctions.modificaEscala(popUpQuiz.transform, 1, 1, 0, 1f, "easeBackOut"));
        StartCoroutine(EaseFunctions.movimenta(popUpQuiz.transform, 1000, 0, 0, 1f, "easeBackOut", false));
    }

    public void hidePopUpQuiz() {
        StartCoroutine(EaseFunctions.movimenta(popUpQuiz.transform, 0, -2000, 0, 0.5f, "easeBounceOut", true));
    }

    public void selectAnswer(string selected) {
        LOLSDK.Instance.PlaySound("FX/sfx_ui_click.mp3", false, false);

        answerSelected = selected;
        if (this.canClick) {
            switch (selected) {
                case "A":
                    a.SetActive(true);
                    b.SetActive(false);
                    c.SetActive(false);
                    break;
                case "B":
                    a.SetActive(false);
                    b.SetActive(true);
                    c.SetActive(false);
                    break;
                case "C":
                    a.SetActive(false);
                    b.SetActive(false);
                    c.SetActive(true);
                    break;
            }
        }
    }

    public void showPopUpCongratulations() {
        LOLSDK.Instance.PlaySound("FX/sfx_level_success.mp3", false, false);
        
        this.canClick = true;

        this.popUpCongratulation.transform.localScale = new Vector3(0, 0, 1);

        this.popUpCongratulation.transform.localPosition = new Vector3(-1000, 0, 0);

        this.popUpCongratulation.SetActive(true);

        StartCoroutine(EaseFunctions.modificaEscala(this.popUpCongratulation.transform, 1, 1, 0, 1f, "easeBackOut"));
        StartCoroutine(EaseFunctions.movimenta(this.popUpCongratulation.transform, 1000, 0, 0, 1f, "easeBackOut", false));
    }

    public void answerQuestion() {
        if (this.answerSelected != "" && this.canClick) {
            wrong.SetActive(false);
            correct.SetActive(false);
            if (this.answerSelected.Equals(quizList[currentQuestion].correctAnswer)) {
                LOLSDK.Instance.PlaySound("FX/sfx_right_answer.mp3", false, false);
                addScore(1500, false);
                showCorrect();
                this.canClick = false;

                if ((this.quizList.Count - 1) == this.currentQuestion) {
                    if (this.currentLevel == 2) {
                        hidePopUpQuiz();
                        SharedState.level = SharedState.maxLevel;//SharedState.level++;
                        LOLSDK.Instance.SubmitProgress(SharedState.score, SharedState.level, SharedState.maxLevel);
                        Invoke("showPopUpCongratulations", 1f);
                    } else { 
                        Invoke("nextLevel", 1.5f);
                        Invoke("hidePopUpQuiz", 1.5f);
                    }
                } else {
                    this.quizList[this.currentQuestion].answered = true;

                    Invoke("nextQuestion", 1f);
                    this.answerSelected = "";
                }
            } else {
                LOLSDK.Instance.PlaySound("FX/sfx_wrong_answer.mp3", false, false);
                removeScore(500, false);
                showWrong();
            }
        }
    }

    public void showWrong() {
        wrong.SetActive(true);

        Vector3 scale = wrong.transform.localScale;
        scale.x = 0;

        wrong.transform.localScale = scale;

        StartCoroutine(EaseFunctions.modificaEscala(wrong.transform, 1, 0, 0, 0.2f, "easeBounceOut"));
    }

    public void showCorrect() {
        correct.SetActive(true);

        Vector3 scale = correct.transform.localScale;
        scale.x = 0;

        correct.transform.localScale = scale;

        StartCoroutine(EaseFunctions.modificaEscala(correct.transform, 1, 0, 0, 0.2f, "easeBounceOut"));
    }

    public void nextLevel() {
        SharedState.level++;
        this.maxScore += 300;
        LOLSDK.Instance.SubmitProgress(SharedState.score, SharedState.level, SharedState.maxLevel);

        //Resets fossil boolean, player and camera position
        Controller.singleton.fossilSpawned = false;
        player.transform.position = Vector3.zero;
        Camera.main.transform.position = new Vector3(0, 0, -10);

        LOLSDK.Instance.PlaySound("FX/sfx_level_success.mp3", false, false);

        this.lvl[this.currentLevel].SetActive(false);
        this.currentLevel++;
        this.lvl[this.currentLevel].SetActive(true);


        this.info.SetActive(false);
        partialScore = 0;
        this.currentQuestion = 0;
        this.currentinfo = 0;
        Destroy(fossilAux);
        Camera.main.orthographicSize = 5;
        StartCoroutine(EaseFunctions.modificaEscala(bar.transform, -1, 0, 0, 0.2f, "linear"));

        wrong.SetActive(false);
        correct.SetActive(false);

        a.SetActive(false);
        b.SetActive(false);
        c.SetActive(false);

        fillQuestions();
        fillInfo();

        this.canClick = true;
    }

    public void nextQuestion() {
        this.currentQuestion++;
        Quiz quiz = quizList[currentQuestion];
        this.textQuestion.text = quiz.question;
        this.answerA.text = quiz.answerA;
        this.answerB.text = quiz.answerB;
        this.answerC.text = quiz.answerC;

        a.SetActive(false);
        b.SetActive(false);
        c.SetActive(false);

        wrong.SetActive(false);
        correct.SetActive(false);

        canClick = true;
    }

    public void nextInfo() {
        LOLSDK.Instance.PlaySound("FX/sfx_ui_click.mp3", false, false);
        if (this.currentinfo < infoList.Count - 1) {
            if (this.partialScore < (maxScore / 3) * 2) {
                current.text = (currentinfo + 1) + " / 1";
            } else {
                this.currentinfo++;
                Info info = infoList[currentinfo];
                this.titleInfo.text = info.title;
                this.textInfo.text = info.info;
                current.text = (currentinfo + 1) + " / 2";
            }
        }
    }

    public void previousInfo() {
        LOLSDK.Instance.PlaySound("FX/sfx_ui_click.mp3", false, false);
        if (this.currentinfo > 0) {
            this.currentinfo--;
            Info info = infoList[currentinfo];
            this.titleInfo.text = info.title;
            this.textInfo.text = info.info;

            if (SharedState.score < (maxScore / 3) * 2) {
                current.text = (currentinfo + 1) + " / 1";
            } else {
                current.text = (currentinfo + 1) + " / 2";
            }
        }
    }

    public void showFossil() {
        Controller.singleton.fossilSpawned = true;

        LOLSDK.Instance.PlaySound("FX/sfx_fossil_appear.mp3", false, false);

        Vector3 fossilPosition = new Vector3(Random.Range(Camera.main.transform.position.x - 7f, Camera.main.transform.position.x + 7f), Random.Range(Camera.main.transform.position.y + 7f, Camera.main.transform.position.y + 8f), 1f);

        this.fossil.transform.position = fossilPosition;

        this.fossilAux = Instantiate(this.fossil);

        float distance = Vector2.Distance(this.player.transform.position, this.fossil.transform.position) / 2;

        Vector3 pontoMedio = (this.player.transform.position + this.fossil.transform.position) / 2;
        pontoMedio.z = -10;

        //Camera.main.transform.position = pontoMedio;
        //Camera.main.orthographicSize = distance;
    }

    private void fillQuestions() {
        quizList = new List<Quiz>();
        Quiz quizObj = new Quiz();

        if (this.currentLevel == 0) {
            quizObj.question = "Which of the answers below no longer has a function?";
            quizObj.answerA = "Hawk eyes.";
            quizObj.answerB = "Dog lungs.";
            quizObj.answerC = "Human appendix.";
            quizObj.correctAnswer = "C";

            quizList.Add(quizObj);

            quizObj = new Quiz();

            quizObj.question = "The wings of both insects and birds have a similar function, but their structures are different. What does this suggest?";
            quizObj.answerA = "Birds and insects have a common evolutionary ancestor.";
            quizObj.answerB = "Birds and insects are not inherited from a common ancestor.";
            quizObj.answerC = "Insects are oviparous because they are related to birds.";
            quizObj.correctAnswer = "B";

            quizList.Add(quizObj);
        } else if (this.currentLevel == 1) {
            quizObj.question = "In humans the embryo passes through a stage in which it has a gill structure similar to that of fish. What does this suggest?";
            quizObj.answerA = "They do not share a common origin.";
            quizObj.answerB = "They are single-celled organisms.";
            quizObj.answerC = "They share a common origin.";
            quizObj.correctAnswer = "C";

            quizList.Add(quizObj);

            quizObj = new Quiz();

            quizObj.question = "If a fossil is found on a rock that is 200 years old, what is it going to look more like?";
            quizObj.answerA = "The animals we observe today.";
            quizObj.answerB = "Single-celled organisms.";
            quizObj.answerC = "The dinosaurs.";
            quizObj.correctAnswer = "A";

            quizList.Add(quizObj);
        } else if (this.currentLevel == 2) {
            quizObj.question = "Which of the following is correct?";
            quizObj.answerA = "Analogous structures have the same form in organisms.";
            quizObj.answerB = "Analogous structures perform similar functions in organisms.";
            quizObj.answerC = "Analogous structures have the same structure in organisms.";
            quizObj.correctAnswer = "B";

            quizList.Add(quizObj);

            quizObj = new Quiz();

            quizObj.question = "If you sequence a gene from a horse, a donkey, and a cow, the horse and donkey will have more similar sequences. What does this suggest?";
            quizObj.answerA = "The cow and the donkey are more closely related.";
            quizObj.answerB = "The horse and the donkey are more distantly related.";
            quizObj.answerC = "The horse and the donkey are more closely related.";
            quizObj.correctAnswer = "C";

            quizList.Add(quizObj);
        }
    }

    private void fillInfo() {
        Info infoObj = new Info();
        infoList = new List<Info>();

        if (this.currentLevel == 0) {
            infoObj.title = "Comparative Anatomy";
            infoObj.info = "Evolutionary theory predicts that features of ancestors that no longer have a function for that species will become smaller over time until they are lost.";
            infoList.Add(infoObj);
            infoObj = new Info();

            infoObj.title = "Comparative Anatomy";
            infoObj.info = "Analogous structures can be used for the same purpose and be similar in construction, but they are not inherited from a common ancestor.";
            infoList.Add(infoObj);
        } else if (this.currentLevel == 1) {
            infoObj.title = "Comparative Embryology";
            infoObj.info = "Scientists have found that the embryos of many different species show similarities, which implies they share a common origin. For example, in humans the embryo passes through a stage in which it has a gill structure similar to that of fish.";
            infoList.Add(infoObj);
            infoObj = new Info();

            infoObj.title = "Evidence of Gradual Change";
            infoObj.info = "Organisms have changed significantly over time. In rocks more than 1 billion years old, only fossils of single-celled organisms are found. Moving to rocks that are about 550 million years old, fossils of simple, multi-cellular animals can be found. As the rocks become more and more recent, the fossils look increasingly like the animals we observe today.";
            infoList.Add(infoObj);
        } else if (this.currentLevel == 2) {
            infoObj.title = "Analogous Structures";
            infoObj.info = "Analogous structures are similar features of different animals that have evolved due to convergent evolution. When two different species live in similar environments, they often evolve in a similar way. This causes the bodies of the two different species to develop similar structures even though they may have started with very different bodies.";
            infoList.Add(infoObj);
            infoObj = new Info();

            infoObj.title = "Molecular Biology";
            infoObj.info = "Comparing the biochemistry or the molecular biology of organisms can give us insights into evolution. One early discovery was that the more closely related two organisms are, the more similar their DNA sequences are.";
            infoList.Add(infoObj);
        }
    }

    public const float cameraRange = 12;
    public List<GameObject> foodPrefabs;
    public GameObject foodEnemyPrefab, chaserEnemyPrefab, redEnemyPrefab;
    public List<GameObject> spawnedFoods = new List<GameObject>();
    public List<GameObject> spawnedFoodEnemies = new List<GameObject>();
    public List<GameObject> spawnedRedEnemies = new List<GameObject>();

    int foodLimit = 30, foodEnemyLimit = 10, redEnemyLimit = 2;

    public void foodSpawn(bool spawnAsDisk) {
        //If fossil has appeared, stops spawning system
        if (Controller.singleton.fossilSpawned)
            return;

        if (spawnedFoods != null) {
            while (spawnedFoods.Count < foodLimit) {
                //Randomize asset and position
                int randomId = Random.Range(0, foodPrefabs.Count);
                Vector3 randomPos;
                if (spawnAsDisk) {
                    //This is Random inside a "disk" (so it doesnt spawn within camera view)
                    randomPos = Random.insideUnitCircle;
                    randomPos = randomPos.normalized * Random.Range(8, 12);
                } else {
                    //First spawn is Totally random so it covers the screen and it doesnt matter to be inside the camera view
                    randomPos = Random.insideUnitCircle * (9);
                }
                randomPos += Camera.main.transform.position;
                randomPos.z = foodPrefabs[randomId].transform.position.z;

                //Instantiates the prefab on a random place inside the camera range
                GameObject spawnedGO = (GameObject)Instantiate(foodPrefabs[randomId], randomPos, foodPrefabs[randomId].transform.rotation);
                //Adds the object to its spawn list
                Controller.singleton.spawnedFoods.Add(spawnedGO);
            }
        }
    }

    public void foodEnemySpawn(bool spawnAsDisk) {
        //If fossil has appeared, stops spawning system
        if (Controller.singleton.fossilSpawned)
            return;

        if (spawnedFoodEnemies != null) {
            while (spawnedFoodEnemies.Count < foodEnemyLimit) {
                //Randomize position                
                Vector3 randomPos;
                if (spawnAsDisk) {
                    //This is Random inside a "disk" (so it doesnt spawn within camera view)
                    randomPos = Random.insideUnitCircle;
                    randomPos = randomPos.normalized * Random.Range(8, 12);
                } else {
                    //First spawn is Totally random so it covers the screen and it doesnt matter to be inside the camera view
                    randomPos = Random.insideUnitCircle * (9);
                }
                randomPos += Camera.main.transform.position;
                randomPos.z = foodEnemyPrefab.transform.position.z;

                //Instantiates the prefab on a random place inside the camera range
                GameObject spawnedGO = (GameObject)Instantiate(foodEnemyPrefab, randomPos, foodEnemyPrefab.transform.rotation);
                //Adds the object to its spawn list
                Controller.singleton.spawnedFoodEnemies.Add(spawnedGO);
            }
        }
    }

    public void redEnemySpawn(bool spawnAsDisk) {
        //If fossil has appeared, stops spawning system
        if (Controller.singleton.fossilSpawned)
            return;

        if (spawnedRedEnemies != null) {
            int i = 0;
            while (spawnedRedEnemies.Count < redEnemyLimit) {
                i++;
                //Randomize position                
                Vector3 randomPos;
                if (spawnAsDisk) {
                    //This is Random inside a "disk" (so it doesnt spawn within camera view)
                    randomPos = Random.insideUnitCircle;
                    randomPos = randomPos.normalized * Random.Range(10, 14);
                } else {
                    //First spawn is Totally random so it covers the screen and it doesnt matter to be inside the camera view
                    randomPos = Random.insideUnitCircle * (9);
                }
                randomPos += Camera.main.transform.position;
                randomPos.z = redEnemyPrefab.transform.position.z-(i-1);

                //Instantiates the prefab on a random place inside the camera range
                GameObject spawnedGO = (GameObject)Instantiate(redEnemyPrefab, randomPos, redEnemyPrefab.transform.rotation);
                //Adds the object to its spawn list
                Controller.singleton.spawnedRedEnemies.Add(spawnedGO);
            }
        }
    }

    public void clearEnemies() {
        //Removes all foods from the scene
        for (int i = 0; i < spawnedFoods.Count; i++) {
            GameObject item = spawnedFoods[i];
            spawnedFoods.Remove(item);
            Destroy(item);
        }

        //Removes all food enemies from the scene
        for (int i = 0; i < spawnedFoodEnemies.Count; i++) {
            GameObject item = spawnedFoodEnemies[i];
            spawnedFoodEnemies.Remove(item);
            Destroy(item);
        }

        //Removes all red enemies from the scene
        for (int i = 0; i < spawnedRedEnemies.Count; i++) {
            GameObject item = spawnedRedEnemies[i];
            spawnedRedEnemies.Remove(item);
            Destroy(item);
        }
    }

    public void loadScene(string name) {
        SceneManager.LoadScene(name);
    }

    public void complete()
    {
        LOLSDK.Instance.CompleteGame();
    }
}
