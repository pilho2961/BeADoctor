using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class dialog_info
{
    public string name;
    [TextArea(3, 5)]
    public string content;
    public bool check_read;
}

[System.Serializable]
public class Dialog_cycle
{
    public string cycle_name;
    public List<dialog_info> info = new List<dialog_info>();
    public int cycle_index;
    public bool check_cycle_read;
}


public class dialog : MonoBehaviour
{
    [SerializeField]
    public static dialog instance;
    public List<Dialog_cycle> dialog_cycles = new List<Dialog_cycle>(); //대화 지문 그룹
    public Queue<string> text_seq = new Queue<string>();                //대화 지문들의 내용을 큐로 저장한다.(끝점을 쉽게 판단하기 위해)
    public string name_;                                                //임시로 저장할 대화 지문의 이름
    public string text_;                                                //임시로 저장할 대화 지문의 내용

    public Text nameing;                                                //대화 지문 오브젝트에 있는 것을 표시할 오브젝트
    public Text DialogT;                                                //대화 지문 내용 오브젝트
    public Text Next_T;                                               //다음 버튼
    public GameObject dialog_obj;                                       //대화 지문 오브젝트

    IEnumerator seq_;
    IEnumerator skip_seq;

    public float delay;
    public bool running = false;

    void Awake()    //싱글톤 패턴으로 어느 씬에서든 접근 가능하게 한다.
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);


        DontDestroyOnLoad(gameObject);

        dialog_obj = GameObject.Find("CanvasWork").transform.Find("Dialog").gameObject;

        for (int i = 0; i < (Enum.GetValues(typeof(DiseaseCode.Disease)).Length * 3) + 5; i++)
        {
            dialog_cycles.Add(new Dialog_cycle());
            dialog_cycles[dialog_cycles.Count - 1].cycle_index = dialog_cycles.Count - 1;
            dialog_info de = new dialog_info();
            dialog_cycles[i].info.Add(de);

            int addContent = UnityEngine.Random.Range(0, 2);

            if (addContent == 1 && i < 9)
            {
                dialog_info de2 = new dialog_info();
                dialog_cycles[i].info.Add(de2);
            }
        }
    }

    private void Start()
    {
        dialog_cycles[27].info[0].name = "환자";
        dialog_cycles[28].info[0].name = "신봉하는 환자";
        dialog_cycles[29].info[0].name = "짜증난 환자";
        dialog_cycles[30].info[0].name = "화난 환자";
        dialog_cycles[31].info[0].name = "의심하는 환자";

        dialog_cycles[27].info[0].content = "감사합니다. 안녕히 계세요.";
        dialog_cycles[28].info[0].content = "아유, 선생님만 믿겠습니다. 벌써부터 다 나은 것 같아요!";
        dialog_cycles[29].info[0].content = "너무 오래 걸리네요. 대학 병원이 원래 이런가요? 오늘 유독 심한 것 같네";
        dialog_cycles[30].info[0].content = "내가 여기 다시 오나 봐라! 환자를 보겠다는거야 말겠다는거야!";
        dialog_cycles[31].info[0].content = "진짜 그게 맞아요? 인터넷에서 찾아보니까 아닌 것 같은데...";
    }

    public IEnumerator dialog_system_start(int index, Action callBack = null)//다이얼로그 출력 시작
    {
        nameing = dialog_obj.GetComponent<parameter>().name_text;   //다이얼로그 오브젝트에서 각 변수 받아오기
        DialogT = dialog_obj.GetComponent<parameter>().content;
        Next_T = dialog_obj.GetComponent<parameter>().next_text;

        running = true;
        foreach (dialog_info dialog_temp in dialog_cycles[index].info)  //대화 단위를 큐로 관리하기 위해 넣는다.
        {
            text_seq.Enqueue(dialog_temp.content);
        }

        dialog_obj.gameObject.SetActive(true);
        for (int i = 0; i < dialog_cycles[index].info.Count; i++) //대화 단위를 순서대로 출력
        {

            nameing.text = dialog_cycles[index].info[i].name;

            text_ = text_seq.Dequeue();                                  //대화 지문을 pop
            
            seq_ = seq_sentence(index, i);                               //대화 지문 출력 코루틴
            StartCoroutine(seq_);                                        //코루틴 실행


            yield return new WaitUntil(() =>
            {
                if (dialog_cycles[index].info[i].check_read)            //현재 대화를 읽었는지 아닌지
                {
                    return true;                                        //읽었다면 진행
                }
                else
                {
                    return false;                                       //읽지 않았다면 다시 검사
                }
            });
        }

        //dialog_cycles[index].check_cycle_read = true;                   //해당 대화 그룹 읽음 -> 난 대화는 반복해야돼서 주석처리
        running = false;

        callBack?.Invoke();                                             // 코루틴이 끝날 때 callBack 함수 실행
    }

    public void DisplayNext(int index, int number)                      //다음 지문으로 넘어가기
    {
        Next_T.text = "";
        Next_T.gameObject.SetActive(false);

        if (text_seq.Count == 0)                                        //다음 지문이 없다면
        {
            dialog_obj.gameObject.SetActive(false);                     //다이얼로그 끄기
        }
        StopCoroutine(seq_);                                            //실행중인 코루틴 종료

        dialog_cycles[index].info[number].check_read = true;            //현재 지문 읽음으로 표시
    }

    public IEnumerator seq_sentence(int index, int number)              //지문 텍스트 한글자식 연속 출력
    {
        skip_seq = touch_wait(seq_, index, number);                     //터치 스킵을 위한 터치 대기 코루틴 할당
        StartCoroutine(skip_seq);                                       //터치 대기 코루틴 시작
        DialogT.text = "";                                              //대화 지문 초기화
        foreach (char letter in text_.ToCharArray())                    //대화 지문 한글자씩 뽑아내기
        {
            DialogT.text += letter;                                     //한글자씩 출력
            yield return new WaitForSeconds(delay);                     //출력 딜레이
        }
        Next_T.gameObject.SetActive(true);
        Next_T.text = "계속하려면 아무 버튼이나 누르세요";
        StopCoroutine(skip_seq);                                        //지문 출력이 끝나면 터치 대기 코루틴 해제
        IEnumerator next = next_touch(index, number);                   //버튼 이외에 부분을 터치해도 넘어가는 코루틴 시작
        StartCoroutine(next);
    }

    public IEnumerator touch_wait(IEnumerator seq, int index, int number)//터치 대기 코루틴
    {
        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButton(0));
        StopCoroutine(seq);                                              //대화 지문 코루틴 해제
        DialogT.text = text_;                                            //스킵시 모든 지문 한번에 출력
        Next_T.gameObject.SetActive(true);
        Next_T.text = "계속하려면 아무 버튼이나 누르세요";
        IEnumerator next = next_touch(index, number);                    //대화 지문 코루틴 해제 됬기 때문에 다음 지문으로 가는 코루틴 시작
        StartCoroutine(next);                                                   
    }

    public IEnumerator next_touch(int index, int number)    //터치로 다음 지문 넘어가는 코루틴
    {
        StopCoroutine(seq_);
        StopCoroutine(skip_seq);
        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButton(0));
        DisplayNext(index, number);
    }

    public bool dialog_read(int check_index)          //index의 부분을 읽었는지 확인하는 함수
    {
        if (!dialog_cycles[check_index].check_cycle_read)
        {
            return true;
        }
        
        return false;
    }
}
