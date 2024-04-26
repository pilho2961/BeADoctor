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
        { Disease.Otitisexterna, new DiseaseInfo { koreanDiseaseName = "외이염", description = "외이염에 대한 설명",
            ear = "고막 주변부 부어오른 현상 발견", nose = "이상 소견 없음", throat = "심하지 않은 상기도 염증 반응 발견" } },

        { Disease.Rhinitis, new DiseaseInfo { koreanDiseaseName = "비염", description = "비염에 대한 설명",
        ear = "다량의 cerumen 발견", nose = "알레르기 물질 노출에 대한 반응 수치 : Total lge 198.76", throat = "후비루와 상기도에 다량의 Rhinorrhea 발견"} },

        { Disease.NasalSeptumDeviation, new DiseaseInfo { koreanDiseaseName = "비중격 만곡증", description = "비중격 이탈에 대한 설명",
        ear = "이상 소견 없음", nose = "알레르기 물질 노출에 대한 반응 수치 : Total lge 88.43,\nCT 촬영 결과 : 좌측으로 심하게 틀어진 비중격 소견", throat = "이상 소견 없음"} },

        { Disease.OtitisMedia, new DiseaseInfo { koreanDiseaseName = "중이염", description = "중이염에 대한 설명",
        ear = "고막 주변부 부어오른 현상 발견,\n 다량의 화농성 삼출액 발견", nose = "소량의 Rhinorrhea 발견", throat = "심하지 않은 상기도 염증 반응 발견"} },

        { Disease.Tinnitus, new DiseaseInfo { koreanDiseaseName = "이명", description = "이명에 대한 설명",
        ear = "이상 소견 없음", nose = "이상 소견 없음", throat = "이상 소견 없음"} },

        { Disease.NasalPolyps, new DiseaseInfo { koreanDiseaseName = "비용종", description = "비용종에 대한 설명",
        ear = "이상 소견 없음", nose = "알레르기 물질 노출에 대한 반응 수치 : Total lge 92.83", throat = "이상 소견 없음"} },

        { Disease.VocalCordPolyps, new DiseaseInfo { koreanDiseaseName = "성대 결절", description = "성대 결절에 대한 설명",
        ear = "이상 소견 없음", nose = "이상 소견 없음", throat = "후두 내시경 검사 결과 : 막성 성대 중간 지점 하얗고 두꺼워진 성대 점막 발견"} },

        { Disease.Laryngitis, new DiseaseInfo { koreanDiseaseName = "후두염", description = "후두염에 대한 설명",
        ear = "다량의 cerumen 발견", nose = "알레르기 물질 노출에 대한 반응 수치 : Total lge 130.26", throat = "후두 내벽이 전체적으로 부어오름. 후두 용종 없음."} },

        { Disease.LaryngealCancer, new DiseaseInfo { koreanDiseaseName = "후두암", description = "후두암에 대한 설명",
        ear = "이상 소견 없음", nose = "비부비동 내 약 10mm 크기의 종양 발견.\n해당 종양 조직검사 결과 : Benign ", throat = "후두 내시경 결과 큰 종양 발견.\n해당 종양 조직검사 결과 : Malignant\n종양의 크기 변화 추이 추적 요망"} }
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
