using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenager : MonoBehaviour
{
    private PlayerValuesManager Save;

    [SerializeField] private GameObject uniqueObject;
    public bool start;
    private Image image;

    [SerializeField] private List<int> FirstScenes;
    [SerializeField] private List<int> SecondScenes;
    [SerializeField] private List<int> FinalScenes;

    [SerializeField] private int finishScene;


    private void Awake()
    {
        Save = GetComponent<PlayerValuesManager>();

        Save.LoadPlayerValues();
    }

    private void Start()
    {
        if(uniqueObject == null)
        {
            Debug.LogError("uniqueObject atamadin");
        }
    }

    private void Update()
    {
        if (start)
        {
            Save.ResetValue();

            SceneManager.LoadScene(levelSelect(FirstScenes));

        }
        //uniqueObject yok olduðunda yani bölüm bittiðinde
        else if (uniqueObject == null)
        {
            Save.SavePlayerValues();

            StartCoroutine(nextScene());
        }
    }

    private int levelSelect(List<int> list)
    {
        if (list == null || list.Count == 0)
        {
            Debug.LogError("Level listesi bos");
            return 0;
        }

        int randomIndex = UnityEngine.Random.Range(0, list.Count);

        return list[randomIndex];
    }

    IEnumerator nextScene()
    {
        float startAlpha = image.color.a;

        float elapsedTime = 0f;
        while (elapsedTime < 2f)
        {
            float alpha = Mathf.Lerp(startAlpha, 1f, elapsedTime / 2f);
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);

        yield return new WaitForSeconds(1f);

        if(GetComponent<PlayerValuesManager>().PlayerValues.level == 2) SceneManager.LoadScene(levelSelect(SecondScenes));
        else if(GetComponent<PlayerValuesManager>().PlayerValues.level == 3) SceneManager.LoadScene(levelSelect(FinalScenes));
        else
        {
            Debug.LogError("Tanýmlanamayan level!!!");
        }
    }
}
