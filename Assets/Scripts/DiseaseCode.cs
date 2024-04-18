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
        { Disease.Otitisexterna, new DiseaseInfo { koreanDiseaseName = "외이염", description = "외이염에 대한 설명" } },
        { Disease.Rhinitis, new DiseaseInfo { koreanDiseaseName = "비염", description = "비염에 대한 설명" } },
        { Disease.NasalSeptumDeviation, new DiseaseInfo { koreanDiseaseName = "비중격 이탈", description = "비중격 이탈에 대한 설명" } },
        { Disease.OtitisMedia, new DiseaseInfo { koreanDiseaseName = "중이염", description = "중이염에 대한 설명" } },
        { Disease.Tinnitus, new DiseaseInfo { koreanDiseaseName = "이명", description = "이명에 대한 설명" } },
        { Disease.NasalPolyps, new DiseaseInfo { koreanDiseaseName = "비용종", description = "비용종에 대한 설명" } },
        { Disease.VocalCordPolyps, new DiseaseInfo { koreanDiseaseName = "성대종", description = "성대종에 대한 설명" } },
        { Disease.Laryngitis, new DiseaseInfo { koreanDiseaseName = "후두염", description = "후두염에 대한 설명" } },
        { Disease.LaryngealCancer, new DiseaseInfo { koreanDiseaseName = "후두암", description = "후두암에 대한 설명" } }
    };

    public static DiseaseInfo GetDiseaseInfo(Disease disease)
    {
        if (diseaseInfoDict.ContainsKey(disease))
        {
            return diseaseInfoDict[disease];
        }
        else
        {
            throw new ArgumentException($"질병 '{disease}' 에 대한 정보가 없습니다.");
        }
    }
}
