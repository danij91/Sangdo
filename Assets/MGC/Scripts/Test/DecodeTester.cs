using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DecodeTester : MonoBehaviour
{
    public TMP_InputField inputField;
    public InputField outputField;
    [Range(16, 200)]
    public int maxBytesToShow = 64;
    public Button decodeButton;

    private void Start()
    {
        decodeButton.onClick.AddListener(SerTest);
    }

    void Run()
    {
        byte[] bytes;
        try
        {
            bytes = Convert.FromBase64String(inputField.text);
        }
        catch (Exception e)
        {
            Debug.LogError("Base64 디코딩 실패: " + e.Message);
            return;
        }

        Debug.Log("프리셋 길이: " + bytes.Length + " 바이트");
        
        // 바이트 앞 16바이트 출력
        string prefix = BitConverter.ToString(bytes, 0, Math.Min(16, bytes.Length));
        Debug.Log("프리셋 시작 바이트: " + prefix);

        // BinaryFormatter 역직렬화 시도
        try
        {
            using (var stream = new MemoryStream(bytes))
            {
                BinaryFormatter bf = new BinaryFormatter();
                var result = bf.Deserialize(stream);
                Debug.Log("BinaryFormatter 역직렬화 성공: " + result);
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning("BinaryFormatter 역직렬화 실패: " + e.Message);
        }

        // 다른 포맷 (추후 추가 시도 가능)
    }
    
    void SerTest()
    {
        try
        {
            byte[] bytes = Convert.FromBase64String(inputField.text);
            Debug.Log($"[Inspector] 전체 길이: {bytes.Length} bytes");

            StringBuilder sb = new StringBuilder();
            using (MemoryStream stream = new MemoryStream(bytes))
            using (BinaryReader reader = new BinaryReader(stream))
            {
                int index = 0;
                while (reader.BaseStream.Position < reader.BaseStream.Length && index < maxBytesToShow)
                {
                    byte b = reader.ReadByte();
                    sb.AppendFormat("[{0:D3}] 0x{1:X2} ({2})\n", index, b, b);
                    index++;
                }

                if (index >= maxBytesToShow)
                {
                    sb.AppendLine($"... (중략: {bytes.Length - maxBytesToShow} 바이트 더 있음)");
                }
            }

            Debug.Log(sb.ToString());
        }
        catch (Exception e)
        {
            Debug.LogError("디코딩 실패: " + e.Message);
        }
    }
}
