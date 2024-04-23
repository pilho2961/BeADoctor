VAR playerName = "플레이어이름"
VAR playerMajor = ""

-> firstDialog

=== firstDialog ===
{playerName}!! 어제 잘 잤냐? 난 긴장돼서 한숨도 못 잤다...
* 왜?
    왜냐니!!! 오늘 전공 결정하는 날이잖아!!
    **아 오늘이었나 깜빡 잊고 있었네.
    얼른 너도 정해 오늘 안에 선택해야 되니까
    ->END
    **뭘 그렇게 호들갑이야 어차피 점수대로 가는 거 아니야?
    뭐 그렇게 시니컬하냐 그래도 눈치봐서 잘 넣으면 달라질 수 있다고.
    ->END
* 어떻게 해야할지 고민하느라 나도 거의 못 잤어... 
    나도 진짜 가고 싶은 곳은 정신과인데, 경쟁률 박터질 것 같아서 못 넣겠어. 너는?
    ** [정형외과...]
        ~ playerMajor = "정형외과"
    ** [이비인후과...]
        ~ playerMajor = "이비인후과"
    ** [소아과...]
        ~ playerMajor = "소아과"
    ** [성형외과...]
        ~ playerMajor = "성형외과"
    ** [내과...]
        ~ playerMajor = "내과"
    ** [안과...]
        ~ playerMajor = "안과"
    ** [비뇨기과...]
        ~ playerMajor = "비뇨기과"

- 나는 {playerMajor} 지원해보고 싶어!
- {playerMajor} 좋지! 너 정도 성적이면 가능할 것 같은데?

-> END