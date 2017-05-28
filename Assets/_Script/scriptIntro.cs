using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class scriptIntro : MonoBehaviour
{
    public string m_sceneToLoad = "Scn_niveau_deux";
    public float m_timeBetweenImages = 5f;
    public float m_currentTimer;
    public int m_currentIndex = 0;
    public Image m_backgroundImage;
    public Sprite[] m_introImages;

	// Update is called once per frame
	void Update ()
    {
        m_currentTimer += Time.deltaTime;

        if(m_currentTimer >= m_timeBetweenImages || Input.GetButtonDown("Action"))
        {
            NextImage();
        }
	}

    public void NextImage()
    {
        m_currentTimer = 0f;
        m_currentIndex++;

        if (m_currentIndex == m_introImages.Length)
        {
            SceneManager.LoadScene(m_sceneToLoad);
        }
        else
        {
            m_backgroundImage.sprite = m_introImages[m_currentIndex];
        }
    }
}
