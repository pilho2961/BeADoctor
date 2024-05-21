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
    public string symptom0 { get; set; }
    public string symptom1 { get; set; }
    public string chat {  get; set; }
}

public static class DiseaseDictionary
{
    private static readonly Dictionary<Disease, DiseaseInfo> diseaseInfoDict = new Dictionary<Disease, DiseaseInfo>()
    {
        { Disease.Otitisexterna, new DiseaseInfo { koreanDiseaseName = "���̿�", description = "���̿��� ���� ����",
            ear = "�� �ֺ��� �ξ���� ���� �߰�", nose = "�̻� �Ұ� ����", throat = "������ ���� ��⵵ ���� ���� �߰�",
        symptom0 = "�Ͱ� �Ը��ϰ� �� �� ��ó�� �����. �ε� ���� �����׿�.", symptom1 = "���� �� �Ϳ� ����뵵 ���� �־��.", chat = "�� ������ �̾����� �ް� ���� �ϴµ� �װ� �����ϱ��?"} },

        { Disease.Rhinitis, new DiseaseInfo { koreanDiseaseName = "��", description = "�񿰿� ���� ����",
        ear = "�ٷ��� cerumen �߰�", nose = "�˷����� ���� ���⿡ ���� ���� ��ġ : Total lge 198.76", throat = "�ĺ��� ��⵵�� �ٷ��� Rhinorrhea �߰�",
        symptom0 = "�ڰ� ������ ������ ������ �ż� �����ؿ�.", symptom1 = "�๰�� ���� ���� Ư�� ȯ���⿡ ������ �������� �� ���ƿ�", chat = "(�� �Դ� �Ҹ�)"} },

        { Disease.NasalSeptumDeviation, new DiseaseInfo { koreanDiseaseName = "���߰� ������", description = "���߰� ��Ż�� ���� ����",
        ear = "�̻� �Ұ� ����", nose = "�˷����� ���� ���⿡ ���� ���� ��ġ : Total lge 88.43,\nCT �Կ� ��� : �������� ���ϰ� Ʋ���� ���߰� �Ұ�", throat = "�̻� �Ұ� ����",
        symptom0 = "�ڱ� ���� �ڰ� ������ �Ҹ������� ������.", symptom1 = "���߿� �Ĵ� �� �Ծ�ôµ��� ȿ���� ��������. �� �̷��ǰ���?", chat = "����� ������ ���� �׷����."} },

        { Disease.OtitisMedia, new DiseaseInfo { koreanDiseaseName = "���̿�", description = "���̿��� ���� ����",
        ear = "�� �ֺ��� �ξ���� ���� �߰�,\n �ٷ��� ȭ�� ����� �߰�", nose = "�ҷ��� Rhinorrhea �߰�", throat = "������ ���� ��⵵ ���� ���� �߰�",
        symptom0 = "��������� ���ڱ� ���� �ʹ� ���Ŀ�. �󱼵� ���� �� ���ƿ�.", symptom1 = "������� �ְ� �׳� ������ ���� ��ü������ ���Ŀ�.", chat = "�ֱٿ� ���⸦ ���ϰ� �ξҴµ�, ������ �������?"} },

        { Disease.Tinnitus, new DiseaseInfo { koreanDiseaseName = "�̸�", description = "�̸� ���� ����",
        ear = "�̻� �Ұ� ����", nose = "�̻� �Ұ� ����", throat = "�̻� �Ұ� ����",
        symptom0 = "���� �Ϳ��� '��-'�ϰ� �Ҹ��� ����.", symptom1 = "���� ������ °����. �ֱٿ� ���� �� �� ����.", chat = "ȸ�� ���� �ٺ��� ������ �ܿ� �Գ׿�."} },

        { Disease.NasalPolyps, new DiseaseInfo { koreanDiseaseName = "�����", description = "������� ���� ����",
        ear = "�̻� �Ұ� ����", nose = "�˷����� ���� ���⿡ ���� ���� ��ġ : Total lge 92.83", throat = "�̻� �Ұ� ����",
        symptom0 = "���� �����µ� �ڰ� ������ �Խ��ϴ�.", symptom1 = "�и� ���� �����µ�...����� �� ���� ������ �������ϴ�. ", chat = "�ֱ� 1�� ���̿� ���� ������ �� ���ƿ�."} },

        { Disease.VocalCordPolyps, new DiseaseInfo { koreanDiseaseName = "���� ����", description = "���� ������ ���� ����",
        ear = "�̻� �Ұ� ����", nose = "�̻� �Ұ� ����", throat = "�ĵ� ���ð� �˻� ��� : ���� ���� �߰� ���� �Ͼ�� �β����� ���� ���� �߰�",
        symptom0 = "(�㽺Ű�� ��Ҹ���) �ȳ��ϼ���, ��� ��ôٽ��� �� ��Ҹ��� �� ��ٷ� �㽺Ű��������...�̻��� �ֳ��ϰ� ���� �ѹ� �޾ƺ��� �Խ��ϴ�.", symptom1 = "���� �������� �� �ɸ� ������ ���⵵ �մϴ�.", chat = "��ҿ� ���� ���� ���ñ� �ϴµ�, ���� �񿡵� �� �������?"} },

        { Disease.Laryngitis, new DiseaseInfo { koreanDiseaseName = "�ĵο�", description = "�ĵο��� ���� ����",
        ear = "�ٷ��� cerumen �߰�", nose = "�˷����� ���� ���⿡ ���� ���� ��ġ : Total lge 130.26", throat = "�ĵ� ������ ��ü������ �ξ����. �ĵ� ���� ����.",
        symptom0 = "���� ���Ŀ�. �񰨱䰡....", symptom1 = "���� ������ �ĵ������� �� ����� ���� �־��.", chat = "����? ���� �̴ϴ�."} },

        { Disease.LaryngealCancer, new DiseaseInfo { koreanDiseaseName = "�ĵξ�", description = "�ĵξϿ� ���� ����",
        ear = "�̻� �Ұ� ����", nose = "��κ� �� �� 10mm ũ���� ���� �߰�.\n�ش� ���� �����˻� ��� : Benign ", throat = "�ĵ� ���ð� ��� ���� �߰�.\n�ش� ���� �����˻� ��� : Malignant\n������ ũ�� ��ȭ ���� ���� ���",
        symptom0 = "���� ��,��踦 �����ؼ� ���� ���� �ѹ� ������ �Խ��ϴ�.", symptom1 = "���� ģô �߿� �� �������� �־ �����ǳ׿�.", chat = "����� �Ǵµ� ������ �ʳ׿�, ������"} }
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
