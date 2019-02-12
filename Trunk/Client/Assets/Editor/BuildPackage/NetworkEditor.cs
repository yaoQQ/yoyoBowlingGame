using UnityEditor;

public class NetworkEditor
{
    [MenuItem("Tools/断开连接/断开MainLogin", false, 0x500)]
    public static void DisconnectMainLogin()
    {
        NetworkManager.Instance.Disconnect("MainLogin");
    }

    [MenuItem("Tools/断开连接/断开MainGateway", false, 0x501)]
    public static void DisconnectMainGateway()
    {
        NetworkManager.Instance.Disconnect("MainGateway");
    }

    [MenuItem("Tools/断开连接/断开BusinessLogin", false, 0x502)]
    public static void DisconnectBusinessLogin()
    {
        NetworkManager.Instance.Disconnect("BusinessLogin");
    }

    [MenuItem("Tools/断开连接/断开BusinessGateway", false, 0x503)]
    public static void DisconnectBusinessGateway()
    {
        NetworkManager.Instance.Disconnect("BusinessGateway");
    }
}