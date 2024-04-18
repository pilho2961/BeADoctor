using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DiseaseCode;

[System.Serializable]
public class DiseaseCode
{
    public enum Disease
    {
        Otitisexterna,
        Rhinitis,
        NasalSeptumDeviation,
        OtitisMedia,
        Tinnitus,
        NasalPolyps,
        VocalCordPolyps,
        Laryngitis,
        LaryngealCancer
    }   

    public Disease disease;
}

public class DiseaseInfo
{
    public string koreanDiseaseName { get; set; }
    public string description { get; set; }
}

public static class DiseaseDictionary
{
    private static readonly Dictionary<Disease, DiseaseInfo> diseaseInfoDict = new Dictionary<Disease, DiseaseInfo>()
    {
        { Disease.Otitisexterna, new DiseaseInfo { koreanDiseaseName = "���̿�", description = "���̿��� ���� ����" } },
        { Disease.Rhinitis, new DiseaseInfo { koreanDiseaseName = "��", description = "�񿰿� ���� ����" } },
        { Disease.NasalSeptumDeviation, new DiseaseInfo { koreanDiseaseName = "���߰� ��Ż", description = "���߰� ��Ż�� ���� ����" } },
        { Disease.OtitisMedia, new DiseaseInfo { koreanDiseaseName = "���̿�", description = "���̿��� ���� ����" } },
        { Disease.Tinnitus, new DiseaseInfo { koreanDiseaseName = "�̸�", description = "�̸� ���� ����" } },
        { Disease.NasalPolyps, new DiseaseInfo { koreanDiseaseName = "�����", description = "������� ���� ����" } },
        { Disease.VocalCordPolyps, new DiseaseInfo { koreanDiseaseName = "������", description = "�������� ���� ����" } },
        { Disease.Laryngitis, new DiseaseInfo { koreanDiseaseName = "�ĵο�", description = "�ĵο��� ���� ����" } },
        { Disease.LaryngealCancer, new DiseaseInfo { koreanDiseaseName = "�ĵξ�", description = "�ĵξϿ� ���� ����" } }
    };

    public static DiseaseInfo GetDiseaseInfo(Disease disease)
    {
        if (diseaseInfoDict.ContainsKey(disease))
        {
            return diseaseInfoDict[disease];
        }
        else
        {
            throw new ArgumentException($"���� '{disease}' �� ���� ������ �����ϴ�.");
        }
    }
}
