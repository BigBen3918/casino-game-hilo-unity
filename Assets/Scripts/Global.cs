public class GlobalVariable
{
    public string BaseUrl = "http://192.168.115.168:5001/";

    // Game Status Variable
    public float _totalBalance;
    public string _token;

    public int _cardNum;
    public int _colorNum;

    public int _bet;
    public int _earn;
    public int _ChoosePoint;

    public bool _gaming;
    public bool _spining;
    public bool _bridge;

    public float _highX;
    public int _highPercent;
    public float _lowX;
    public int _lowPercent;

    public static GlobalVariable instance = new GlobalVariable();
}