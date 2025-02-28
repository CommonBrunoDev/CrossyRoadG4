using UnityEngine;

public class RowTrain : Row
{
    private TrainState state = TrainState.Waiting;
    [SerializeField] private Car trainRef;
    private Car train;
    [SerializeField] private MeshRenderer signalRed;
    [SerializeField] private MeshRenderer signalGreen;
    [SerializeField] private MeshRenderer signalRedNightmare;
    [SerializeField] private MeshRenderer signalGreenNightmare;

    [SerializeField] private float waitingTime = 10.0f;
    private float waitingTimer = 0;

    [SerializeField] private float warningTime = 3.0f;
    private float warningTimer = 0;

    [SerializeField] private float passingTime = 2.0f;
    private float passingTimer = 0;

    [SerializeField] private float flashingSpeed = 2.0f;
    private float flashingTimer = 0;


    private void Start()
    {
        waitingTimer = Random.Range(1,waitingTime);
        signalRed.gameObject.SetActive(false);
        signalRedNightmare.gameObject.SetActive(false);
        train = Instantiate(trainRef);
        train.gameObject.SetActive(false);
    }

    private void Update()
    {
        HandleState();

        if (train.gameObject.activeSelf)
        {
            train.transform.position = new Vector3(train.transform.position.x + 2, 1, transform.position.z);

            if (train.transform.position.x < -24 || train.transform.position.x > 24)
            { train.gameObject.SetActive(false); }
        }

        if (state == TrainState.Warning)
        {
            if (flashingTimer > 0) { flashingTimer -= Time.deltaTime; }
            if (flashingTimer < 0)
            {
                if (GM.Instance.nightmareMode)
                {
                    signalRedNightmare.gameObject.SetActive(!signalRedNightmare.gameObject.activeSelf);
                    signalGreenNightmare.gameObject.SetActive(!signalGreenNightmare.gameObject.activeSelf);
                }
                else
                {
                    signalRed.gameObject.SetActive(!signalRed.gameObject.activeSelf);
                    signalGreen.gameObject.SetActive(!signalGreen.gameObject.activeSelf);
                }
                flashingTimer = 1 / flashingSpeed * Time.deltaTime;
            }
        }
    }

    private void HandleState()
    {
        switch (state)
        {
            case TrainState.Waiting:
                waitingTimer -= Time.deltaTime;
                if (waitingTimer <= 0)
                {
                    state = TrainState.Warning;
                    warningTimer = warningTime;

                    flashingTimer = 1 / flashingSpeed * Time.deltaTime;
                }
                break;

            case TrainState.Warning:
                warningTimer -= Time.deltaTime;
                if (warningTimer <= 0)
                {
                    state = TrainState.Passing;
                    passingTimer = passingTime;
                    var t = Instantiate(train);

                    TrainSetActive(true);
                }
                break;

            case TrainState.Passing:
                passingTimer -= Time.deltaTime;
                if (passingTimer <= 0)
                {
                    state = TrainState.Waiting;
                    waitingTimer = waitingTime;

                    TrainSetActive(false);
                }
                break;
        }
    }

    private void TrainSetActive(bool active)
    {
        train.gameObject.SetActive(active);
        if (active)
        {train.transform.position = new Vector3(-22,1,transform.position.z);}
        if (GM.Instance.nightmareMode)
        {
            signalRedNightmare.gameObject.SetActive(active);
            signalGreenNightmare.gameObject.SetActive(!active);
        }
        else
        {
            signalRed.gameObject.SetActive(active);
            signalGreen.gameObject.SetActive(!active);
        }
    }
}
