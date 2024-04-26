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
    public string ear { get; set; }
    public string nose { get; set; }
    public string throat { get; set; }

}

public static class DiseaseDictionary
{
    private static readonly Dictionary<Disease, DiseaseInfo> diseaseInfoDict = new Dictionary<Disease, DiseaseInfo>()
    {
        { Disease.Otitisexterna, new DiseaseInfo { koreanDiseaseName = "���̿�", description = "���̿��� ���� ����",
            ear = "�� �ֺ��� �ξ���� ���� �߰�", nose = "�̻� �Ұ� ����", throat = "������ ���� ��⵵ ���� ���� �߰�" } },

        { Disease.Rhinitis, new DiseaseInfo { koreanDiseaseName = "��", description = "�񿰿� ���� ����",
        ear = "�ٷ��� cerumen �߰�", nose = "�˷����� ���� ���⿡ ���� ���� ��ġ : Total lge 198.76", throat = "�ĺ��� ��⵵�� �ٷ��� Rhinorrhea �߰�"} },

        { Disease.NasalSeptumDeviation, new DiseaseInfo { koreanDiseaseName = "���߰� ������", description = "���߰� ��Ż�� ���� ����",
        ear = "�̻� �Ұ� ����", nose = "�˷����� ���� ���⿡ ���� ���� ��ġ : Total lge 88.43,\nCT �Կ� ��� : �������� ���ϰ� Ʋ���� ���߰� �Ұ�", throat = "�̻� �Ұ� ����"} },

        { Disease.OtitisMedia, new DiseaseInfo { koreanDiseaseName = "���̿�", description = "���̿��� ���� ����",
        ear = "�� �ֺ��� �ξ���� ���� �߰�,\n �ٷ��� ȭ�� ����� �߰�", nose = "�ҷ��� Rhinorrhea �߰�", throat = "������ ���� ��⵵ ���� ���� �߰�"} },

        { Disease.Tinnitus, new DiseaseInfo { koreanDiseaseName = "�̸�", description = "�̸� ���� ����",
        ear = "�̻� �Ұ� ����", nose = "�̻� �Ұ� ����", throat = "�̻� �Ұ� ����"} },

        { Disease.NasalPolyps, new DiseaseInfo { koreanDiseaseName = "�����", description = "������� ���� ����",
        ear = "�̻� �Ұ� ����", nose = "�˷����� ���� ���⿡ ���� ���� ��ġ : Total lge 92.83", throat = "�̻� �Ұ� ����"} },

        { Disease.VocalCordPolyps, new DiseaseInfo { koreanDiseaseName = "���� ����", description = "���� ������ ���� ����",
        ear = "�̻� �Ұ� ����", nose = "�̻� �Ұ� ����", throat = "�ĵ� ���ð� �˻� ��� : ���� ���� �߰� ���� �Ͼ�� �β����� ���� ���� �߰�"} },

        { Disease.Laryngitis, new DiseaseInfo { koreanDiseaseName = "�ĵο�", description = "�ĵο��� ���� ����",
        ear = "�ٷ��� cerumen �߰�", nose = "�˷����� ���� ���⿡ ���� ���� ��ġ : Total lge 130.26", throat = "�ĵ� ������ ��ü������ �ξ����. �ĵ� ���� ����."} },

        { Disease.LaryngealCancer, new DiseaseInfo { koreanDiseaseName = "�ĵξ�", description = "�ĵξϿ� ���� ����",
        ear = "�̻� �Ұ� ����", nose = "��κ� �� �� 10mm ũ���� ���� �߰�.\n�ش� ���� �����˻� ��� : Benign ", throat = "�ĵ� ���ð� ��� ū ���� �߰�.\n�ش� ���� �����˻� ��� : Malignant\n������ ũ�� ��ȭ ���� ���� ���"} }
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
