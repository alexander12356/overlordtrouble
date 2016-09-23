using UnityEngine;
using System.Collections;

public class HardPorn : MonoBehaviour
{
    string a  = ".................../´¯/)            ";
    string a1 = ".................,/¯../             ";
    string a2 = "................./..../              ";
    string a3 = "............./´¯/'...'/´¯¯`·¸          ";
    string a4 = "........../'/.../..../......./¨¯\n     ";
    string a5 = "........('(...´...´.... ¯~/'...')      ";
    string a6 = ".........\n................'...../     ";
    string a7 = "..........\n................ _.·´      ";
    string a8 = "............\n..............(          ";
    string a9 = "..............\n.............\n        ";

    string m_MessageForPeopleWithLongNoses = "Yes, i can did it :)";

    public void Awake()
    {
        a += a1 + a2 + a3 + a4 + a5 + a6 + a7 + a8 + a9;
        m_MessageForPeopleWithLongNoses += a;
    }
}
