using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    public BasketballAgent agent;
    public PlayerShooter player;
    public TextMeshProUGUI agentScoreText;
    public TextMeshProUGUI playerScoreText;
    public TextMeshProUGUI resultText;
    public Rigidbody ballRigidbody;

    private int agentScore = 0;
    private int playerScore = 0;
    private int shotsTaken = 0;
    private const int totalShots = 20;
    private bool isPlayerTurn = true;

    private void Start()
    {
        resultText.text = "";
        player.gameObject.SetActive(true);
        agent.gameObject.SetActive(false);
        StartNextTurn();
    }

    public void AgentShot(bool scored)
    {
        if (scored) agentScore++;
        agentScoreText.text = "Agent Score: " + agentScore;
    }

    public void PlayerShot(bool scored)
    {
        if (scored) playerScore++;
        playerScoreText.text = "Player Score: " + playerScore;
        shotsTaken++;
        if (shotsTaken < totalShots)
        {
            isPlayerTurn = false;
            StartNextTurn();
        }
        else
        {
            EndGame();
        }
    }

    private void StartNextTurn()
    {
        if (shotsTaken >= totalShots)
        {
            EndGame();
            return;
        }

        if (isPlayerTurn)
        {
            ResetBall();
            player.gameObject.SetActive(true);
            agent.gameObject.SetActive(false);
            player.EnableShooting();
        }
        else
        {
            ResetBall();
            StartCoroutine(AgentTurn());
        }
    }

    private IEnumerator AgentTurn()
    {
        player.gameObject.SetActive(false);
        agent.gameObject.SetActive(true);

        float randomPower = Random.Range(agent.baselinePower - agent.maxPowerAdjustment, agent.baselinePower + agent.maxPowerAdjustment);
        float randomAngle = Random.Range(agent.minBaselineAngle, agent.maxBaselineAngle);
        agent.ShootBall(randomPower, randomAngle);

        yield return new WaitForSeconds(4); // Agent's turn lasts for 4 seconds

        isPlayerTurn = true;
        StartNextTurn();
    }

    private void EndGame()
    {
        player.gameObject.SetActive(false);
        agent.gameObject.SetActive(false);

        if (playerScore > agentScore)
        {
            resultText.text = "Player Wins!";
        }
        else if (agentScore > playerScore)
        {
            resultText.text = "Agent Wins!";
        }
        else
        {
            resultText.text = "It's a Tie!";
        }
    }

    private void ResetBall()
    {
        ballRigidbody.useGravity = false;
        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;
        ballRigidbody.position = isPlayerTurn ? player.handPosition.position : agent.handPosition.position;
    }
}

