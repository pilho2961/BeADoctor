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
        { Disease.Otitisexterna, new DiseaseInfo { koreanDiseaseName = "외이염", description = "외이염에 대한 설명",
            ear = "고막 주변부 부어오른 현상 발견", nose = "이상 소견 없음", throat = "심하지 않은 상기도 염증 반응 발견",
        symptom0 = "귀가 먹먹하고 물 찬 것처럼 울려요. 턱도 조금 아프네요.", symptom1 = "아픈 쪽 귀에 편두통도 같이 있어요.", chat = "일 때문에 이어폰을 달고 살기는 하는데 그것 때문일까요?"} },

        { Disease.Rhinitis, new DiseaseInfo { koreanDiseaseName = "비염", description = "비염에 대한 설명",
        ear = "다량의 cerumen 발견", nose = "알레르기 물질 노출에 대한 반응 수치 : Total lge 198.76", throat = "후비루와 상기도에 다량의 Rhinorrhea 발견",
        symptom0 = "코가 막혀서 입으로 숨쉬게 돼서 불편해요.", symptom1 = "콧물이 많이 나고 특히 환절기에 증상이 심해지는 것 같아요", chat = "(코 먹는 소리)"} },

        { Disease.NasalSeptumDeviation, new DiseaseInfo { koreanDiseaseName = "비중격 만곡증", description = "비중격 이탈에 대한 설명",
        ear = "이상 소견 없음", nose = "알레르기 물질 노출에 대한 반응 수치 : Total lge 88.43,\nCT 촬영 결과 : 좌측으로 심하게 틀어진 비중격 소견", throat = "이상 소견 없음",
        symptom0 = "자기 전에 코가 막혀서 불면증까지 생겼어요.", symptom1 = "시중에 파는 약 먹어봤는데도 효과가 없던데요. 왜 이런건가요?", chat = "어렸을 때부터 자주 그랬어요."} },

        { Disease.OtitisMedia, new DiseaseInfo { koreanDiseaseName = "중이염", description = "중이염에 대한 설명",
        ear = "고막 주변부 부어오른 현상 발견,\n 다량의 화농성 삼출액 발견", nose = "소량의 Rhinorrhea 발견", throat = "심하지 않은 상기도 염증 반응 발견",
        symptom0 = "어젯밤부터 갑자기 턱이 너무 아파요. 얼굴도 부은 것 같아요.", symptom1 = "두통까지 있고 그냥 오른쪽 얼굴이 전체적으로 아파요.", chat = "최근에 감기를 심하게 앓았는데, 연관이 있을까요?"} },

        { Disease.Tinnitus, new DiseaseInfo { koreanDiseaseName = "이명", description = "이명에 대한 설명",
        ear = "이상 소견 없음", nose = "이상 소견 없음", throat = "이상 소견 없음",
        symptom0 = "왼쪽 귀에서 '삐-'하고 소리가 나요.", symptom1 = "벌써 일주일 째에요. 최근에 잠을 좀 못 잤어요.", chat = "회사 일이 바빠서 병원도 겨우 왔네요."} },

        { Disease.NasalPolyps, new DiseaseInfo { koreanDiseaseName = "비용종", description = "비용종에 대한 설명",
        ear = "이상 소견 없음", nose = "알레르기 물질 노출에 대한 반응 수치 : Total lge 92.83", throat = "이상 소견 없음",
        symptom0 = "비염이 없었는데 코가 막혀서 왔습니다.", symptom1 = "분명 비염이 없었는데...어렸을 땐 전혀 증상이 없었습니다. ", chat = "최근 1년 사이에 유독 막히는 것 같아요."} },

        { Disease.VocalCordPolyps, new DiseaseInfo { koreanDiseaseName = "성대 결절", description = "성대 결절에 대한 설명",
        ear = "이상 소견 없음", nose = "이상 소견 없음", throat = "후두 내시경 검사 결과 : 막성 성대 중간 지점 하얗고 두꺼워진 성대 점막 발견",
        symptom0 = "(허스키한 목소리로) 안녕하세요, 듣고 계시다시피 제 목소리가 좀 요근래 허스키해져서요...이상이 있나하고 진료 한번 받아보러 왔습니다.", symptom1 = "뭔가 따끔따끔 목에 걸린 느낌이 나기도 합니다.", chat = "평소에 술을 자주 마시긴 하는데, 술이 목에도 안 좋을까요?"} },

        { Disease.Laryngitis, new DiseaseInfo { koreanDiseaseName = "후두염", description = "후두염에 대한 설명",
        ear = "다량의 cerumen 발견", nose = "알레르기 물질 노출에 대한 반응 수치 : Total lge 130.26", throat = "후두 내벽이 전체적으로 부어오름. 후두 용종 없음.",
        symptom0 = "목이 아파요. 목감긴가....", symptom1 = "과거 역류성 식도염으로 좀 고생한 적이 있어요.", chat = "담배요? 가끔 핍니다."} },

        { Disease.LaryngealCancer, new DiseaseInfo { koreanDiseaseName = "후두암", description = "후두암에 대한 설명",
        ear = "이상 소견 없음", nose = "비부비동 내 약 10mm 크기의 종양 발견.\n해당 종양 조직검사 결과 : Benign ", throat = "후두 내시경 결과 종양 발견.\n해당 종양 조직검사 결과 : Malignant\n종양의 크기 변화 추이 추적 요망",
        symptom0 = "제가 술,담배를 좋아해서 정기 검진 한번 받으러 왔습니다.", symptom1 = "저희 친척 중에 암 가족력이 있어서 걱정되네요.", chat = "끊어야 되는데 쉽지가 않네요, 허허허"} }
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
