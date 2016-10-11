using UnityEngine;

public class HardPorn : MonoBehaviour
{
    string a  = ".................../´¯/)               \n";
    string a1 = ".................,/¯../                \n";
    string a2 = "................./..../                \n";
    string a3 = "............./´¯/'...'/´¯¯`·¸          \n";
    string a4 = "........../'/.../..../......./¨¯\\     \n";
    string a5 = "........('(...´...´.... ¯~/'...')      \n";
    string a6 = ".........\\................'...../     \n";
    string a7 = "..........\\................ _.·´      \n";
    string a8 = "............\\..............(          \n";
    string a9 = "..............\\.............\\        \n";

    string m_MessageForPeopleWithLongNoses = "Yes, i can did it :)";

    public void Awake()
    {
        a += a1 + a2 + a3 + a4 + a5 + a6 + a7 + a8 + a9;
        a += m_MessageForPeopleWithLongNoses;
        Debug.Log(a);
    }
}
