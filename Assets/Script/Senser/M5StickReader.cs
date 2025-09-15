using UnityEngine;
using System;
using System.IO.Ports;
using System.Threading;
using System.Collections.Concurrent;
using System.IO;

public class M5StickReader : MonoBehaviour
{
    // シングルトン実装
    public static M5StickReader Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 既に存在するなら新しい方を破棄
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // シーン遷移でも残す
    }

    public struct DisplaySize
    {
        public float width;
        public float height;

        public DisplaySize(float w, float h)
        {
            width = w;
            height = h;
        }
    }

    [Header("Serial Settings")]
    [SerializeField] string portPower = "/dev/cu.usbserial-DD526B6C1E"; // power & flag 用
    [SerializeField] string portYaw = "/dev/cu.usbserial-31DA55BFAB"; // yaw 用
    [SerializeField] int baudRate = 115200;

    private SerialPort serialPower;
    private SerialPort serialYaw;

    private Thread threadPower;
    private Thread threadYaw;
    private bool running = false;

    private ConcurrentQueue<string> queuePower = new ConcurrentQueue<string>();
    private ConcurrentQueue<string> queueYaw = new ConcurrentQueue<string>();

    private float roll;
    private bool tflag;
    private bool buttonFlag;
    private float power;
    private float yaw;
    private bool throwActionFlag;
    private bool throwedActionFlag;
    private bool pushedbutton;
    private Vector2 throwPoint;
    private DisplaySize displaySize;


    void Start()
    {
        displaySize = new DisplaySize(Screen.currentResolution.width, Screen.currentResolution.height);
        throwPoint = new Vector2(0, 0);
        power = 0f;
        yaw = 0f;
        throwPoint = Vector2.zero;
        throwActionFlag = false;
        throwedActionFlag = false;
        tflag = false;
        buttonFlag = false;
        pushedbutton = false;

        OpenSerialPort(portPower, queuePower, out serialPower, out threadPower);
        OpenSerialPort(portYaw, queueYaw, out serialYaw, out threadYaw);
    }

    void OpenSerialPort(string portName, ConcurrentQueue<string> queue, out SerialPort port, out Thread thread)
    {
        port = null;
        thread = null;

        try
        {
            SerialPort sp = new SerialPort(portName, baudRate);
            sp.NewLine = "\n";
            sp.ReadTimeout = 50; // 少し短めに設定
            sp.Open();
            port = sp;

            running = true;
            thread = new Thread(() => SerialReadLoop(sp, queue)) { IsBackground = true };
            thread.Start();

            Debug.Log($"[Serial] Connected: {portName}");
        }
        catch (Exception e)
        {
            Debug.LogWarning($"[Serial] Failed to open port {portName}: {e.Message}");
        }
    }

    void SerialReadLoop(SerialPort port, ConcurrentQueue<string> queue)
    {
        while (running && port != null && port.IsOpen)
        {
            try
            {
                string line = port.ReadLine();
                if (!string.IsNullOrEmpty(line))
                    queue.Enqueue(line);
            }
            catch (TimeoutException)
            {
                // データが来なかった場合は無視
            }
            catch (IOException)
            {
                // 一時的にリソースが使えない場合は無視して少し待機
                Debug.Log("一時的にリソースが使えない");
                Thread.Sleep(50);
            }
            catch (Exception)
            {
                // その他の例外もゲームを止めずに待機
                Thread.Sleep(50);
                Debug.Log("その他の例外");
            }
        }
    }

    void Update()
    {
        // Power & Flag の処理
        while (queuePower.TryDequeue(out string lineR))
        {

            /*
            if (float.TryParse(lineR, out float r))
                roll = r;

            // roll に応じて power を加算
            if (roll <= -15)
            {
                power += 0.2f * (roll / -10f);
                tflag = true;
                if (power >= 100) power = 100;
            }
            else if (roll > -5f && tflag)
            {
                tflag = false;

                // throwActionFlag はここで立てるが、
                // ゲーム側で取得後すぐに false に戻すことを推奨
                throwActionFlag = true;
            }

            throwPoint.y = 8f * (power / 100f) - 4f;
            */
           　// roll,flag の形式だけ通す (-12.34,0 や 45,1 など)
            lineR = lineR.Trim();

            if (System.Text.RegularExpressions.Regex.IsMatch(lineR, @"^-?\d+(\.\d+)?,[01]$"))
            {
                string[] parts = lineR.Split(',');
                roll = float.Parse(parts[0]);
                buttonFlag = (parts[1] == "1");
                Debug.Log(roll + "," + buttonFlag);
            }
            else
            {
                // それ以外のログやゴミは完全無視
                continue;
            }


            throwPoint.y = 8f * (-roll / 60.0f) - 4f;

            
        }

        // --- yaw 処理 ---
        while (queueYaw.TryDequeue(out string lineY))
        {
            if (float.TryParse(lineY, out float y))
                yaw = y;

            throwPoint.x = 6.0f * yaw / 30.0f;
        }
    }

    void OnDestroy()
    {
        running = false;

        if (threadPower != null && threadPower.IsAlive) threadPower.Join(500);
        if (threadYaw != null && threadYaw.IsAlive) threadYaw.Join(500);

        if (serialPower != null && serialPower.IsOpen) serialPower.Close();
        if (serialYaw != null && serialYaw.IsOpen) serialYaw.Close();

        Debug.Log("[Serial] Closed on destroy");
    }

    public void SendFlag(bool flag)
    {
        if (serialPower != null && serialPower.IsOpen)
        {
            string msg = flag ? "1\n" : "0\n";
            serialPower.Write(msg);
        }
        else
        {
            Debug.LogWarning("SendFlag: serialPower が未接続です");
        }
    }

    public float getTarget_y()
    {
        return 8f * (-roll / 60.0f) - 4f;

    }
    public float getTarget_x()
    {
        return 6 * yaw / 30.0f;
    }
    public bool getThrowActionflag()
    {
        return throwActionFlag;
    }
    public bool getThrowedActionFlag()
    {
        return throwedActionFlag;
    }
    public Vector2 getThrowPos()
    {
        return throwPoint;
    }
    public float getRoll()
    {
        return roll;
    }
    public bool getButtonFlag()
    {
        return buttonFlag;
    }

    public void setPower(float pow)
    {
        power = pow;
    }
    public void setThrowedActionFlag(bool flag)
    {
        throwedActionFlag = flag;
    }
    public void setThrowActionFlag(bool flag)
    {
        throwActionFlag = flag;
    }
    public void setPushedButton(bool flag)
    {
        pushedbutton = flag;
    }

    public void resetThrowFlag()
    {
        // フラグをリセット
        throwedActionFlag = true;
        throwActionFlag = false;
    }

    public bool ConsumeThrowActionFlag()
    {
        if (throwActionFlag)
        {
            throwActionFlag = false; // 一度読んだらリセット
            return true;
        }
        return false;
    }



    public bool Consumepushedbutton()
    {
        if (pushedbutton)
        { 
            return true;
        }
        return false;
    }


    void OnApplicationQuit()
    {
        running = false;

        if (threadPower != null && threadPower.IsAlive) threadPower.Join(500);
        if (threadYaw != null && threadYaw.IsAlive) threadYaw.Join(500);

        if (serialPower != null && serialPower.IsOpen) serialPower.Close();
        if (serialYaw != null && serialYaw.IsOpen) serialYaw.Close();
    }
}